using System.Threading;

namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser
	{
		void FocusOut(uint uSteps)
		{
			bool fReverseInOut = Properties.Settings.Default.ReverseInOut;

			if (uSteps == 0)
			{
				return;
			}

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
				logEntry("FocusOut: Focuser halted");

				strStatus = GetStatus();
				iRetries = 0;

				while (strStatus == "E" && iRetries < m_Retries)
				{
					iRetries++;
					Thread.Sleep(100);
					strStatus = GetStatus();
				}

				if (strStatus != "0")
				{
					goto FocusOutError;
				}
			}

			if (fReverseInOut)
			{
				CommandStep((byte)FocuserCommands.IN,  uSteps);
			}
			else
			{
				CommandStep((byte)FocuserCommands.OUT, uSteps);
			}

			return;

	FocusOutError:
			LogMessage("FocusOut", "Error! Received " + strStatus);

			switch (strStatus)
			{
				case "0":
				case "S":
					throw new DriverException(ERR_SOURCE + MSG_FOCUSER_ERROR + "Focuser did not report as moving.", SCODE_FOCUSER_ERROR);

				case "F":
					throw new DriverException(ERR_SOURCE + MSG_BUSY_FOCUSER + " moving in...", SCODE_BUSY_FOCUSER);

				case "B":
					throw new DriverException(ERR_SOURCE + MSG_BUSY_FOCUSER + " moving out...", SCODE_BUSY_FOCUSER);

				default:
					throw new DriverException(ERR_SOURCE + MSG_CONTROLLER_ERROR, SCODE_CONTROLLER_ERROR);
			}
		}
	}
}
