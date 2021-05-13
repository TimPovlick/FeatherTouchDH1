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
using ASCOM.Controls;


namespace ASCOM.FeatherTouchDH1
{
	partial class Focuser : IFocuserV3
	{
		/// <summary>
		///
		/// Normal Exit:
		///     String with 1st character indicating status:
		///		0 -  Controller Idle, ready for Fwd & Bkwd commands
		///		F -  Busy moving fwd
		///		B -  Busy moving Bkwd
		///		I -  Busy doing and init setting step counter to 0
		///		P -  Power has been cycled
		///		S -  A Setspeed operation has just been done
		///		R -  Controller was reset - either by command or automatically
		///		H -  Halt in progress
		/// Additionally, the 2nd thru 6th bytes of buffer are set to the received value of the current step counter.
		/// coded in binary coded decimal. After the Temp command, they'll represent Temperature.The 7th byte is the
		/// check byte.
		///
		/// Error Exit:
		///     An "E" is returned
		///
		/// </summary>
		/// <returns></returns>
		string GetStatus()
		{
			int i = 1;
			int Checksum = 0;
			string strStatus;
			string strStatusBCD = string.Empty;
			string retVal = "";
			bool fReverseInOut = Properties.Settings.Default.ReverseInOut;

			// Save where we are so we know where we went.
			m_lPreviousPosition = m_lPosition;

			// Get status from the focuser.
			strStatus = CommandString((byte)FocuserCommands.STATUS);

			if (string.IsNullOrEmpty(strStatus))
			{
				Console.WriteLine("GetStatus: CommandString(STATUS) returned null!");
				strStatus = string.Empty;
			}

			// Check the status for errors that indicate that we are not communicating properly
			for (i = 0; i < strStatus.Length - 1; i++)
			{
				Checksum += Asc(strStatus.Substring(i, 1));
			}

			if (Checksum % 256 != Asc(strStatus.Substring(i, 1)))
			{
				Console.WriteLine("GetStatus: Checkum is {0}  expected {1}", Checksum, Asc(strStatus.Substring(i, 1)));
				goto GetStatusError;
			}

			// WAS: if (Asc(Left(strStatus, 1)) = 0 || Left(strStatus, 1) = "S" || Left(strStatus, 1) = "R")
			if (Asc(strStatus.Substring(0, 1)) == 0 || strStatus.Substring(0, 1) == "S" || strStatus.Substring(0, 1) == "R")
			{
				retVal = "0";
				g_bUseCachedPosition = true;
				strStatusBCD = strStatus.Substring(1, Math.Min(strStatus.Length-1, 5));       // Mid(strStatus, 2, 5)

				if (fReverseInOut)
				{
					m_lPosition = Convert.ToInt32(strStatusBCD);
					m_lPosition = 99999 - m_lPosition;

					if (m_lPosition == 99999)
					{
						m_lPosition = 0;    // Special case when IN-OUT is reversed
					}

					strStatusBCD = m_lPosition.ToString();
				}
				else
				{
					m_lPosition = Convert.ToInt32(strStatusBCD);
				}

				logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
			}
			else
			{
				switch (strStatus.Substring(0, 1))
				{
					case "D":
						retVal = "D";
						// SetHubCodeVer(Asc(Mid(strStatus, 6, 1)) & "." & Asc(Mid(strStatus, 5, 1)))
						SetHubCodeVer(Asc(strStatus.Substring(5, 1)) + "." + Asc(strStatus.Substring(4, 1)));
						// SetMotorCtlrVer(Asc(Mid(strStatus, 4, 1)) & "." & Asc(Mid(strStatus, 3, 1)))
						SetMotorCtlrVer(Asc(strStatus.Substring(3, 1)) + "." + Asc(strStatus.Substring(2, 1)));
						logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
						break;

					case "P":
						retVal = "P";

						// SetHubCodeVer(Asc(Mid(strStatus, 6, 1)) & "." & Asc(Mid(strStatus, 5, 1)))
						SetHubCodeVer(Asc(strStatus.Substring(5, 1)) + "." + Asc(strStatus.Substring(4, 1)));

						//SetMotorCtlrVer(Asc(Mid(strStatus, 4, 1)) & "." & Asc(Mid(strStatus, 3, 1)))
						SetMotorCtlrVer(Asc(strStatus.Substring(3, 1)) + "." + Asc(strStatus.Substring(2, 1)));
						m_lPosition = 0;
						logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
						break;

					case "F":
					case "B":
					case "I":
					case "H":
					case "W":
					case "N":
						g_bUseCachedPosition = false;
						retVal = strStatus.Substring(0, 1);
						strStatusBCD = strStatus.Substring(2, 4);

						if (fReverseInOut)
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
							m_lPosition = 99999 - m_lPosition;
							strStatusBCD = Convert.ToString(m_lPosition);
						}
						else
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
						}

						logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
						break;

					case "C": // New hub version temperature requested in degrees C
						retVal = strStatus.Substring(0, 1);
						logEntry("Measuring Temperature");

						// lowWord = 256 * (Asc(Mid(strStatus, 5, 1))) + Asc(Mid(strStatus, 6, 1))   ' Low word
						int lowWord = 256 * (Asc(strStatus.Substring(4, 1))) + Asc(strStatus.Substring(5, 1));  // Low word

						// highWord = 256 * (Asc(Mid(strStatus, 3, 1))) + Asc(Mid(strStatus, 4, 1))  ' High word
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
							logEntry("Status returned = " + strStatus + " Temperature = " + Convert.ToString(m_lTemperature));

							//	Exit Function
							return retVal;
						}

					// break;

					case "T": // Old hub version temperature requested but not available - position returned
						retVal = strStatus.Substring(0, 1);

						// strStatusBCD = Mid(strStatus, 2, 5)
						strStatusBCD = strStatus.Substring(1, 5);
						if (fReverseInOut)
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
							m_lPosition = 99999 - m_lPosition;
							strStatusBCD = Convert.ToString(m_lPosition);
						}
						else
						{
							m_lPosition = Convert.ToInt32(strStatusBCD);
						}

						logEntry("Status returned = " + strStatus + " Position = " + strStatusBCD);
						break;

					default:
						logEntry("Status E Returned = " + strStatus);
						goto GetStatusError;
				} // end switch
			}

			// Exit Function
			return retVal;

GetStatusError:

			logEntry("GetStatusError");

			if (GetHubCodeVer() < 2)
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

			return "Not Implemented";
		}
	}
}
