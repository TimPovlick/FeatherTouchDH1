using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser
	{
		bool m_fFinalMoveDirectionIsOut;

		/// <summary>
		/// True if the serial port is connected to the focuser.
		/// </summary>
		bool g_bFocuserConnected;


		/// <summary>
		/// Port of VB Let Link property.
		/// </summary>
		/// <param name=" bConnect "></param>
		void LetLink(bool bConnect)
		{
			int lPos;

			LogMessage("Focuser.LetLink", "arg is {0}", bConnect);

			if (bConnect == g_bFocuserConnected)
			{
				return; // Nothing to do.
			}

			if (!bConnect)
			{
				g_bUseCachedPosition = false;
				lPos = GetPosition();

				if (lPos > 99900) lPos = 0;
				if (lPos > MaxStepNumber) lPos = MaxStepNumber;

				SaveFocuserPosition(lPos);

				CloseConnection();

				return; // Exit Property
			}

			// Load the maximum step number
			MaxStepNumber = Properties.Settings.Default.MaxStep;

			// Load the last saved focus position for restoration and protect against it being out of range.
			int lastSavedPosition = Properties.Settings.Default.LastFocusPosition;
			if (lastSavedPosition > 99900) lastSavedPosition = 0;
			if (lastSavedPosition > MaxStepNumber) lastSavedPosition = MaxStepNumber;

			if (lastSavedPosition != Properties.Settings.Default.LastFocusPosition)
			{
				Properties.Settings.Default.LastFocusPosition = lastSavedPosition;
				Properties.Settings.Default.Save();
			}

			// Compute steps/degree for temperature compensation.
			if (Properties.Settings.Default.DeltaT != 0)
			{
				m_dStepsPerDegree = Properties.Settings.Default.DeltaSteps / Properties.Settings.Default.DeltaT;
			}
			else
			{
				m_dStepsPerDegree = 0;
			}

			// Temperature compensation IN or OUT?
			m_TempCompIn = Properties.Settings.Default.BacklashFinalDirectionIsOut;

			// Load Step Resolution
			Focuser.ModelNumberEnum modelEnum = (Focuser.ModelNumberEnum)Properties.Settings.Default.FocuserModelIndex;
			LogMessage("Focuser.LetLink", "Focuser model index is {0}", modelEnum);
			LogMessage("Focuser.LetLink", "Focuser step resolution is {0}", Properties.Settings.Default.StepResolution);

			// Determine if focuser is short or long body based on model number.
			DetermineFocuserLength(modelEnum);

			// Determine the number of steps based on the user selected coarseness.
			DetermineStepResolution(Properties.Settings.Default.StepResolution);

			OpenConnection();

			// This is also done in the power-up code.
			SetSpeed((byte)Properties.Settings.Default.FocusSpeed, m_StepResolution);
			SaveDataToFirmware();

			return;
		}

		/// <summary>
		// This method determines the step resolution based on the user selected coarseness.
		/// </summary>
		internal void DetermineStepResolution(string userChoice)
		{
			switch (userChoice)
			{
				case "Fine":
					if (IsLongFocuser)
					{
						StepResolution = 4;
					}
					else
					{
						StepResolution = 2;
					}
					break;

				case "Medium":
					if (IsLongFocuser)
						StepResolution = 8;
					else
						StepResolution = 4;

					break;

				case "Coarse":
					if (IsLongFocuser)
						StepResolution = 16;
					else
						StepResolution = 8;
					break;

				default:
					LogMessage("Focuser.DetermineStepResolution", "Coarse {0} is invalid. Setting StepResolution to 4", userChoice);
					StepResolution = 4;
					break;
			}
		}

		void OpenConnection()
		{
			g_bFocuserConnected = true;

			// Get the focuser status.
			string status = GetStatus();

			if (status.Substring(0, 1) == "E")
			{
				// If Not (g_SharedSerial Is Nothing) Then    ' If we ever connected to a good port
				//    g_SharedSerial.ClearBuffers            ' Always clear serial status
				//    If g_SharedSerial.Connected Then g_SharedSerial.Connected = False       ' Release since there is no focuser there
				// End If
				// logEntry ("OpenConnection: GetStatus returned an E trying port: COMM" & CStr(PortNum))
				// If PortNum < 16 Then GoTo TRY_NEXT_PORT
				// GoTo FOCUSER_DETECT_FAILED:
				throw new ASCOM.DriverException("Focuser.Link: Focuser returned an 'E'rror. ", ErrorCodes.UnspecifiedError);
			}
			else
			{
				switch (status.Substring(0, 1))
				{
					case "P":
						// Focuser was power cycled since last connection.  Need to recalibrate.
						// Move in the maximum step number to reset 0.  Focuser might hit stops.
						// FocuserInit();
						SetSpeed((byte)Properties.Settings.Default.FocusSpeed, StepResolution);

						if (!Properties.Settings.Default.NoCalibrationOnPowerUp)
						{
							bool fIsMoving = false;

							// On Error Resume Next
							FocusIn((uint)MaxStepNumber);

							while (fIsMoving)
							{
								Thread.Sleep(100);
							}

							// Initialize the count to 0
							//FocuserInit();

							// Move to last saved position.
							FocusOut((uint)Properties.Settings.Default.LastFocusPosition);

							while (fIsMoving)
							{
								Thread.Sleep(100);
							}

							m_CurrentPosition = GetPosition();
						}
						else
						{
							m_CurrentPosition = Properties.Settings.Default.LastFocusPosition;
							SetFocuserPosition(m_CurrentPosition.ToString());
						}

						break;

					default:
						// Driver is reconnecting to an already initialized focuser.
						SetSpeed((byte)Properties.Settings.Default.FocusSpeed, StepResolution);
						m_CurrentPosition = GetPosition();
						break;
				}

				// if (frmSetup.Visible)
				{
					// frmSetup.lblScanStatus.Visible = False
					// frmSetup.lblScanPortNum.Visible = False
					// DoEvents
				}

				return;
			}

			//FOCUSER_DETECT_FAILED:
			//    On Error GoTo 0
			//        frmSetup.lblScanStatus.Visible = False
			//        frmSetup.lblScanPortNum.Visible = False
			//        DoEvents
			//        CloseConnection
			//        Err.Raise SCODE_NO_FOCUSER, ERR_SOURCE, MSG_NO_FOCUSER
			//    Exit Sub

			//SERIAL_PORT_FAILED:
			//    frmSetup.lblScanStatus.Visible = False
			//    frmSetup.lblScanPortNum.Visible = False
			//    DoEvents
			//    CloseConnection
			//    status = Err.Description
			//    code = Err.Number
			//    src = Err.Source
			//    Resume SER_FAIL_FIN

			//SER_FAIL_FIN:
			//    On Error GoTo 0
			//        Err.Raise code, src, status

			//End Sub
		}

		/// <summary>
		/// Disconnect from the serial port.
		/// </summary>
		void CloseConnection()
		{
			LogMessage("Connected.Set", "Disconnecting from serial port {0}", ComPort);

			g_bFocuserConnected = false;

			if (s_Serial != null)             // If we ever connected
			{
				s_Serial.ClearBuffers();        // Always clear serial buffers

				if (s_Serial.Connected)
				{
					s_Serial.Connected = false; // Release if no longer used
				}

				s_Serial.Dispose();
				s_Serial = null;
			}
		}

		/// <summary>
		/// Determines if focuser is short or long body based on model number.
		/// </summary>
		/// <param name="modelEnum">The focuser model number/name enumeration.</param>
		internal void DetermineFocuserLength(ModelNumberEnum modelEnum)
		{
			switch (modelEnum)
			{
				case Focuser.ModelNumberEnum.FTF3545:
				case Focuser.ModelNumberEnum.AP27FTMU:
				case Focuser.ModelNumberEnum.AP4FOC3E:
				case Focuser.ModelNumberEnum.AP27FOC3E:
					LogMessage("Focuser.DetermineFocuserLength", "{0} is a long Focuser.", modelEnum);
					IsLongFocuser = true;
					break;

				default:
					LogMessage("Focuser.DetermineFocuserLength", "{0} is not a long Focuser.", modelEnum);
					IsLongFocuser = false;
					break;
			}
		}
	}
}
