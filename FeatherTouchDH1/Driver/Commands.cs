using System;
using System.Threading;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Exceptions;

namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser : IFocuserV3
	{
		/// <summary>
		/// Transmits a command byte sequence to the focuser via the serial port.
		/// </summary>
		/// <param name="Cmd">The command byte to send.</param>
		/// <exception cref="SerialPortInUseException"> Thrown when unable to acquire the serial port. </exception>
		/// <exception cref="NotConnectedException"> Thrown when this command is used before setting Connect = True </exception>
		public void CommandByte(byte Cmd)
		{
			string buf = string.Empty;
			byte[] txData = { Cmd, 0, 0, (byte)(Cmd & 0x7F) }; // Check byte will be just command, bytes 1 & 2 don't contribute

			CheckConnected();
			CommandClear();

			s_Serial.TransmitBinary(txData);
			Thread.Sleep( Properties.Settings.Default.SerialDelay);
		}


		/// <summary>
		/// Sends a byte to the Focuser via the serial port.
		/// </summary>
		/// <param name="bCmd">The byte to send.</param>
		/// <exception cref="SerialPortInUseException"> Thrown when unable to acquire the serial port. </exception>
		/// <exception cref="NotConnectedException"> Thrown when this command is used before setting Connect = True </exception>
		public void CommandBinary(byte bCmd)
		{
			try
			{
				CheckConnected();
				CommandClear();

				// Convert to a byte array.
				byte[] byteArray = new[] { bCmd };
				s_Serial.TransmitBinary(byteArray);
				Thread.Sleep( Properties.Settings.Default.SerialDelay);

				return;
			}
			catch (Exception e)
			{
				LogMessage("CommandBinary", "Caught {0}", e.Message);
				throw e;
			}
		}

		/// <summary>
		/// Clears the serial port's transmit and receive buffers.
		/// </summary>
		public void CommandClear()
		{
			s_Serial.ClearBuffers();
		}


		/// <summary>
		/// Transmits a byte (usually "F" or "B") and number of steps to move.
		/// </summary>
		/// <param name="bCmd">Normally the letter F(orward) or B(ackward)</param>
		/// <param name="uSteps">Number of steps to move.</param>
		/// <exception cref="SerialPortInUseException"> Thrown when unable to acquire the serial port. </exception>
		/// <exception cref="NotConnectedException"> Thrown when this command is used before setting Connect = True </exception>
		public void CommandStep(byte bCmd, uint uSteps)
		{
			CheckConnected();

			// On Error GoTo CommandStepError
			CommandClear();

			if (uSteps < 0 || uSteps > 65535)
			{
				throw new InvalidValueException("CommandStep", uSteps.ToString(), "0", "65536");
			}

			byte[] bTxData = new byte[4];
			bTxData[0] = bCmd;
			bTxData[1] = (byte)(uSteps >> 8 & 0xFF); // Was: bTxData[1] = Fix(CLng(uSteps) / 256);
			bTxData[2] = (byte)(uSteps & 0xFF);     // Was: bTxData[2] = CLng(uSteps) Mod 256;

			// Add up to get check-byte, truncate to 8 bits.
			bTxData[3] = Convert.ToByte(((((int)bCmd + (int)bTxData[1] + (int)bTxData[2])) % 256) & 0x7F);

			// Send the command byte and pause.
			s_Serial.TransmitBinary(bTxData);
			Thread.Sleep( Properties.Settings.Default.SerialDelay);
		}

		/// <summary>
		/// Verifies the Serial object instance is 'Connected'.
		/// </summary>
		/// <returns>True if connected</returns>
		/// <exception cref="NotConnectedException"> Not connected to the focuser.</exception>
		bool CheckConnected()
		{
			if (s_Serial == null || !s_Serial.Connected)
			{
				LogMessage("CheckConnected", "Serial port not connected");

				throw new NotConnectedException(string.Format(
					"CheckConnected: Not connected to [0] 0x[1]", ComPort, ErrorCodes.NotConnected));
			}

			return true;
		}

		/// <summary>
		/// Transmits the 3 byte sequence to the focuser.
		/// </summary>
		/// <param name="bCmd0">A byte</param>
		/// <param name="bCmd1">Another byte.</param>
		/// <param name="bCmd2">Yet another byte.</param>
		/// <exception cref="SerialPortInUseException"> Thrown when unable to acquire the serial port. </exception>
		/// <exception cref="NotConnectedException"> Thrown when this command is used before setting Connect = True </exception>
		void CommandRaw(byte bCmd0, byte bCmd1, byte bCmd2)
		{
			LogMessage("Focuser.CommandRaw", "");

			// Once again verify the serial port is connected.
			CheckConnected();

			/// Clear serial port buffers.
			CommandClear();

			byte bCmd3 = (byte)(((bCmd0 + Convert.ToInt32(bCmd1) + Convert.ToInt32(bCmd2)) % 256) & 0x7F);
			byte[] bTxData = new[] { bCmd0, bCmd1, bCmd2, bCmd3 };

			s_Serial.TransmitBinary(bTxData);

			// Pause to allow the command to be sent and responded to.
			Thread.Sleep( Properties.Settings.Default.SerialDelay);

			return;
		}


		/// <summary>
		/// Sends a binary byte to the Focuser via the serial port.
		/// </summary>
		/// <param name="bCmd">The byte to send.</param>
		/// <returns></returns>
		string CommandString(byte bCmd)
		{
			int iRetries = 3;

			CheckConnected();

	RetryCommandString:
			// TODO:  On Error GoTo CommandStringTimeout
			// TODO: Catch exception and perform a retry.

			CommandClear();

			// Convert to a byte array.
			byte[] byteArray = new[] { bCmd };

			s_Serial.TransmitBinary(byteArray);
			Thread.Sleep( Properties.Settings.Default.SerialDelay);

			string buf = s_Serial.Receive();

			if (buf != null)
				return buf;
			else
				return string.Empty;

	// CommandStringTimeout:
			LogMessage("Focuser.CommandString", "Timed-out waiting for response.");

			if (iRetries > 0)
			{
				LogMessage("Focuser.CommandString", "Retry #", iRetries);

				iRetries--;

				CommandClear();
				goto RetryCommandString;
			}

	// CommandStringError:
			logEntry("CommandStringError");
			return "E";
		}

		/// <summary>
		/// Tries to halt (stop) the focuser.
		/// </summary>
		///<returns>true if successful, false otherwise.</returns>
		public bool HaltImpl()
		{
			string strStatus = GetStatus();
			int iRetries = 0;

			while ((strStatus == "E") && iRetries < m_Retries)
			{
				LogMessage("Focuser.HaltImpl", "Received 'E' (error). Retry #{0}", iRetries);
				iRetries++;

				Thread.Sleep(100);
				strStatus = GetStatus();
			}

			if (strStatus == "E")
			{
				LogMessage("FocuserHalt", "ERROR: Max retries exceeded, giving up!");
				//Err.Raise SCODE_CONTROLLER_ERROR, ERR_SOURCE, MSG_CONTROLLER_ERROR
				return false;
			}
			else if (strStatus != "F" && strStatus != "B")
			{
				return false;
			}

			CommandByte((byte)FocuserCommands.HALT);
			Thread.Sleep(250);
			return true;
		}

		/// <summary>
		/// Gets or sets the focuser hub firmware version stored in the profile.
		///
		/// NOTE: The actual value is retrieved in GetStatus when the focuser is first powered on.
		/// </summary>
		string HubCodeVersion
		{
			get
			{
				return Properties.Settings.Default.HubCodeVersion;
			}

			set
			{
				LogMessage("Focuser.HubCodeVersion.Set", "Hub Code Version is {0}", value);
				Properties.Settings.Default.HubCodeVersion = value;
				Properties.Settings.Default.Save();
			}
		}

		/// <summary>
		/// Gets or sets the focuser motor controller firmware version stored in the profile.
		///
		/// NOTE: The actual value is retrieved in GetStatus when the focuser is first powered on.
		/// </summary>
		string MotorControllerVersion
		{
			get
			{
				return Properties.Settings.Default.MotorControlerVersion;
			}

			set
			{
				Properties.Settings.Default.MotorControlerVersion = value;
				Properties.Settings.Default.Save();
				LogMessage("Focuser.MotorControllerVersion.Set", "Motor Controller Version is ", value);
			}
		}


		/// <summary>
		/// Performs a soft reset of the controller.
		///
		/// NOTE: Assumes the controller is alive enough to do this.
		/// </summary>
		/// <returns></returns>
		public string ResetController()
		{
			int i;
			int Checksum = 0;
			string retVal;

			CheckConnected();

			// On Error GoTo ResetControllerError

			logEntry("ResetController: Performing soft reset.");
			CommandBinary((byte)FocuserCommands.RESET);

			// Wait for 2500msec to allow reset to occur
			Thread.Sleep(2500);

			string strStatus = CommandString((byte)FocuserCommands.STATUS);

			// Check the status for errors that indicate that we are not communicating properly
			for (i = 0; i < strStatus.Length - 1; i++)
			{
				Checksum += Asc(strStatus.Substring(i, 1));
			}

			if (Checksum % 256 != Asc(strStatus.Substring(i, 1)))
			{
				// Hardware error
				LogMessage("Focuser.ResetController", "Checksum is {0}  expected {1}", Checksum, Asc(strStatus.Substring(i, 1)));
				LogMessage("Focuser.ResetController", "Reset appears to have failed.");
				retVal =  "EH";
			}

			if (strStatus.Substring(0, 1) == "R")
			{
				retVal = "SR"; //Reset success

				if (m_lPosition != m_PreviousPosition)
				{
					// Position error
					logEntry("Position mismatch after reset: Previous = " + m_PreviousPosition + " Current = " + m_lPosition);
					retVal = "EP";
				}
			}
			else
			{
				if (strStatus.Substring(0, 1) != "0")
				{
					// Reset error
					logEntry("Reset appears to have failed.  Status returned = " + strStatus.Substring(0, 1));
					retVal = "ER";
				}
				else
				{
					// We might have missed the R so this is just a soft failure - log it and continue.
					logEntry("Reset appears to have failed with no R returned from status. Status returned = " + strStatus.Substring(0, 1));
					retVal = "SR";
				}
			}

			return retVal;
		}


		/// <summary>
		/// Returns an integer value representing the character code corresponding to the FIRST character in the string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>The character code corresponding to the first character.</returns>
		static int Asc(string str)
		{
			if (string.IsNullOrEmpty(str))
				throw new ArgumentException("Asc: Invalid parameter, str cannot be null or empty");

			return Asc(str[0]); // .Substring(0, 1)[0]);
		}

		/// <summary>
		/// Returns an integer value representing the character code corresponding to a character.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.strings.asc?view=net-5.0"/>
		/// <see cref="https://stackoverflow.com/questions/721201/whats-the-equivalent-of-vbs-asc-and-chr-functions-in-c"/>
		/// <param name="c">The character to process.</param>
		/// <returns>The character code corresponding to the character.</returns>
		static int Asc(char c)
		{
			int converted = c;
			if (converted >= 0x80)
			{
				byte[] buffer = new byte[2];
				// if the resulting conversion is 1 byte in length, just use the value
				if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
				{
					converted = buffer[0];
				}
				else
				{
					// byte swap bytes 1 and 2;
					converted = buffer[0] << 16 | buffer[1];
				}
			}

			return converted;
		}

		int GetPosition()
		{
			string strStatus;

			if (g_bUseCachedPosition)
				return GetCachedPosition();

			strStatus = GetStatus();
			int iRetries = 0;

			while (strStatus == "E" && iRetries++ < m_Retries)
			{
				Thread.Sleep(100);
				strStatus = GetStatus();
			}

			if (strStatus == "E")
			{

				LogMessage("Focuser.GetPosition", "An error was returned while trying to get the position.");
				throw new DriverException(ERR_SOURCE + MSG_CONTROLLER_ERROR + " Error trying to get the position", SCODE_CONTROLLER_ERROR);
			}

			if (m_lPosition != Properties.Settings.Default.LastFocusPosition)
			{
				// Persist this updated the position .
				Properties.Settings.Default.LastFocusPosition = m_lPosition;
				Properties.Settings.Default.Save();
			}

			return m_lPosition;
		}


		int GetCachedPosition()
		{
			return m_lPosition;
		}

		internal string SetFocuserPosition(string strPosition)
		{
			LogMessage("Focuser.SetFocuserPosition", "Move to {0}", strPosition);

			// Verify we are connected to the focuser.
			CheckConnected();

			int pos = Convert.ToInt32(strPosition);
			uint uLow = (uint)(pos % 9999);
			CommandStep((byte)FocuserCommands.SETLOW, uLow);

			byte bHigh = (byte)(pos / 9999);	// Round down!
			CommandRaw((byte)FocuserCommands.SETHI, 0, bHigh);

			return GetStatus();
		}

		/// <summary>
		/// Persists the focuser's position to the store if it has changed.
		/// </summary>
		/// <param name="position"></param>
		/// <returns>The previously persisted position.</returns>
		internal int SaveFocuserPosition(int position)
		{
			int previous = Properties.Settings.Default.LastFocusPosition;

			if (position != previous)
			{
				Properties.Settings.Default.LastFocusPosition = position;
				Properties.Settings.Default.Save();
			}

			return previous;
		}


		/// <summary>
		/// Saves the focuser step resolution.
		/// </summary>
		void SaveDataToFirmware()
		{
			CheckConnected();
			CommandRaw((byte)FocuserCommands.SAVEDATA, StepResolution, 0);
		}


		/// <summary>
		/// Attempts to read the firmware version from the focuser.
		/// </summary>
		void ReadVersionFromFirmware()
		{
			CheckConnected();
			CommandRaw((byte)FocuserCommands.READDATA, 0, 1);
			GetStatus();
		}

		/// <summary>
		/// ASCOM Spec:
		/// Returns the step size in Microns for the focuser. Throws an exception if the
		/// focuser does not intrinsically know what the step size is.
		///
		/// The step size for the feather touch is nominally 3.25 micron.
		/// It will scale depending on the resolution set.
		/// </summary>
		/// <returns></returns>
		float StepSizeImpl()
		{
			float retVal;

			logEntry("Get StepSize");

			switch (StepResolution)
			{
				case 2:
					retVal = Properties.Settings.Default.StepSize;
					break;
				case 4:
					retVal = Properties.Settings.Default.StepSize * 2;
					break;
				case 8:
					retVal = Properties.Settings.Default.StepSize * 4;
					break;

				case 16:
					retVal = Properties.Settings.Default.StepSize * 8;
					break;

				default:
					retVal = 6.4F;
					break;
			}

			if (IsLongFocuser)
				retVal = retVal / 2;

			return retVal;
		}

		/// <summary>
		/// Sets the focuser's travel speed and resolution.
		/// </summary>
		/// <param name="bSpeed">A number between 1 (slow) to 255 (fastest) </param>
		/// <param name="bResolution"></param>
		void SetSpeed(byte bSpeed, byte bResolution)
		{
			// Check status and make sure that the focuser is idle, if not execute a Halt.
			// Send the speed command followed by speed (1-255) and the resolution (2, 4, 8 or 16)
			string strStatus = GetStatus();

			int iRetries = 0;
			while (strStatus == "E" && iRetries < m_Retries)
			{
				iRetries++;
				Thread.Sleep(100);
				strStatus = GetStatus();
			}

			if (strStatus == "F" || strStatus == "B")
			{
				HaltImpl();
				logEntry("SetSpeed: Focuser halted");
				strStatus = GetStatus();
				iRetries = 0;

				while (strStatus == "E" && iRetries < m_Retries)
				{
					iRetries++;
					Thread.Sleep(100);
					strStatus = GetStatus();
				}

				if (strStatus != "0")
					goto SetSpeedError;
			}

			// Note: The controller firmware treats 255 as minimum and 1 maximum speed.
			// To make it more intuitive reverse this so 255 is fast.
			bSpeed = (byte)(256 - bSpeed);
			LogMessage("Focuser.SetSpeed", "Speed is {0}  Resolution is {1}", bSpeed, bResolution);
			CommandRaw((byte)FocuserCommands.SPEED, bSpeed, bResolution);

			return;

SetSpeedError:
			logEntry("SetSpeedError" + bSpeed + " " + bResolution);
			throw new DriverException(ERR_SOURCE + MSG_CONTROLLER_ERROR + " Focuser did not report as moving.", SCODE_CONTROLLER_ERROR);

		}
	}
}
