using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using ASCOM.Controls;


namespace ASCOM.FeatherTouchDH1
{
	partial class Focuser : IFocuserV3
	{
		/// <summary>
		///
		/// Normal Exit:
		///     String with 1st character indicating status:
		///		0 -  Controller Idle, ready for forward & backward commands
		///		F -  Busy moving fwd
		///		B -  Busy moving backward
		///		I -  Busy doing and init setting step counter to 0
		///		P -  Power has been cycled
		///		S -  A Set speed operation has just been done
		///		R -  Controller was reset - either by command or automatically
		///		H -  Halt in progress
		///
		/// Additionally, the 2nd thru 6th bytes of buffer are set to the received value of the current step counter.
		/// coded in binary coded decimal. After the Temp command, they'll represent Temperature.The 7th byte is the
		/// check byte.
		///
		/// Error Exit:
		///     An "E" is returned
		///
		/// </summary>
		/// <returns>Various stuffs</returns>
		string GetStatus()
		{
			int i;
			int Checksum = 0;
			string strStatusBCD;
			string retVal = "";
			bool fReverseInOut = Properties.Settings.Default.ReverseInOut;


			// Save where we are so we know where we went.
			m_PreviousPosition = m_lPosition;

			// Get status from the focuser.
			string strStatus = CommandString((byte)FocuserCommands.STATUS);

			s_TraceLogger.LogMessage("Focuser.GetStatus", strStatus, true);

			// Check the status for errors that indicate that we are not communicating properly
			for (i = 0; i < strStatus.Length - 1; i++)
			{
				Checksum += Asc(strStatus.Substring(i, 1));
			}

			if (Checksum % 256 != Asc(strStatus.Substring(i, 1)))
			{
				LogMessage("Focuser.GetStatus ", "Checksum is {0} expected {1}", Checksum, Asc(strStatus.Substring(i, 1)));
				goto GetStatusError;
			}

			if (Asc(strStatus.Substring(0, 1)) == 0 || strStatus.Substring(0, 1) == "S" || strStatus.Substring(0, 1) == "R")
			{
				retVal = "0";
				g_bUseCachedPosition = true;
				strStatusBCD = strStatus.Substring(1, Math.Min(strStatus.Length-1, 5));

				if (fReverseInOut)
				{
					m_lPosition = Convert.ToInt32(strStatusBCD);
					m_lPosition = MAX_STEP_LIMIT - m_lPosition;

					if (m_lPosition == MAX_STEP_LIMIT)
					{
						m_lPosition = 0;    // Special case when IN-OUT is reversed
					}

					strStatusBCD = m_lPosition.ToString();
				}
				else
				{
					m_lPosition = Convert.ToInt32(strStatusBCD);
				}
			}
			else
			{
				switch (strStatus.Substring(0, 1))
				{
					case "D":           // <-------- What is D ??
						{
							retVal = "D";
							Array array = Array.CreateInstance(typeof(int), strStatus.Length);
							Array.Copy(strStatus.ToCharArray(), array, strStatus.Length);

							HubCodeVersion = array.GetValue(5) + "." + array.GetValue(4);
							MotorControllerVersion = array.GetValue(2) + "." + array.GetValue(1);
							Properties.Settings.Default.HubCodeVersion = HubCodeVersion;
							Properties.Settings.Default.MotorControlerVersion = MotorControllerVersion;
							Properties.Settings.Default.Save();
							s_TraceLogger.LogMessage("Status returned  ", strStatus, true);
						}
						break;

					case "P":
						{
							retVal = "P";
							Array array = Array.CreateInstance(typeof(int), strStatus.Length);
							Array.Copy(strStatus.ToCharArray(), array, strStatus.Length);
							HubCodeVersion = array.GetValue(5) + "." + array.GetValue(4);
							MotorControllerVersion = array.GetValue(2) + "." + array.GetValue(1);

							if (HubCodeVersion != Properties.Settings.Default.HubCodeVersion ||
								MotorControllerVersion != Properties.Settings.Default.MotorControlerVersion)
							{
								Properties.Settings.Default.HubCodeVersion = HubCodeVersion;
								Properties.Settings.Default.MotorControlerVersion = MotorControllerVersion;
								Properties.Settings.Default.Save();
							}

							// m_lPosition = 0; <- this screws up the chance to set the focuser position to what it was.
							LogMessage("Focuser.GetStatus", "Powered on. Hub version is {0} motor controller version is {1}", HubCodeVersion, MotorControllerVersion);
						}
						break;

					case "F":
					case "B":
					case "I":
					case "H":
					case "W":
					case "N":
						retVal = strStatus.Substring(0, 1);

						g_bUseCachedPosition = false;
						strStatusBCD = strStatus.Substring(1, 5);

						if (fReverseInOut)
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
							m_lPosition = MAX_STEP_LIMIT - m_lPosition;
							strStatusBCD = Convert.ToString(m_lPosition);
						}
						else
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
						}

						// This is the return value for SetPosition.
						if (strStatus.Substring(0, 1) == "N")
						{
							retVal = strStatus.Substring(0, 1) + m_lPosition.ToString();
						}

						break;

					case "C": // New hub version temperature requested in degrees C
						retVal = strStatus.Substring(0, 1);
						logEntry("Measuring Temperature");

						int lowWord = 256 * (Asc(strStatus.Substring(4, 1))) + Asc(strStatus.Substring(5, 1));  // Low word
						int highWord = 256 * (Asc(strStatus.Substring(2, 1))) + Asc(strStatus.Substring(3, 1)); //  High word

						if (Math.Abs(highWord - lowWord) > 8)
						{
							// If low and high bytes differ too much, then don't accept this result
							m_lTemperature = 0;

							//Exit Function
							return retVal;
						}
						else
						{
							m_lTemperature = highWord + lowWord;
							LogMessage("Focuser.GetStatus", "Status returned {0}   Temperature is ", strStatus, m_lTemperature);
							return retVal;
						}

					// break;

					case "T": // Old hub version temperature requested but not available - position returned
						retVal = strStatus.Substring(0, 1);

						strStatusBCD = strStatus.Substring(1, 5);
						if (fReverseInOut)
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
							m_lPosition = MAX_STEP_LIMIT - m_lPosition;
							strStatusBCD = Convert.ToString(m_lPosition);
						}
						else
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
						}

						logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
						break;

					default:
						LogMessage("Focuser.GetStatus", "Unknown status: {0}", strStatus);
						goto GetStatusError;
				} // end switch
			}

			return retVal;

GetStatusError:
			logEntry("GetStatusError");

			if (Convert.ToDouble(HubCodeVersion) < 2)
			{
				logEntry("GetStatusError:  Hub Code Version is less than 2.0 so no reset attempted.");
				retVal = "E";
			}
			else
			{
				// An error was returned from the controller.  Attempt a reset.
				string strReset = string.Empty;
				logEntry("GetStatusError:  Attempting controller reset");
				strReset = ResetController();

				switch (strReset)
				{
					case "ER":
						retVal = "0";
						logEntry("GetStatus: Reset attempt failed. No R returned and no 0 detected.");
						break;

					case "EP":
						retVal = "0";
						logEntry("GetStatus: Reset succeeded with position mismatch.");
						break;

					case "SR":
						retVal = "0";
						logEntry("GetStatus: Reset attempt succeeded with no errors.");
						break;

					case "EH":
						retVal = "E";
						logEntry("GetStatus: Reset attempt hard failed.");
						break;
				}
			}

			return retVal;
		}
	}
}
