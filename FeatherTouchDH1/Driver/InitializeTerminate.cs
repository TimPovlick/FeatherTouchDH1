using System;


namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser
	{
		/// <summary>
		///	Initializes stuffs.
		/// </summary>
		void Initialize()
		{
			LogMessage("Focuser", "Class_Initalize");

			if (m_FocuserCreated)
			{
				logEntry("Tried to create a second instance of the focuser class");
				throw new NotSupportedException("Only one program at a time can use the focuser.");
			}

			logEntry("Focuser Class_Initalize");
			m_FocuserCreated = true;
			g_bTemperatureCompensation = false;

			for (int i = 0; i < 15; i++)
			{
				m_AvgTemperature[i] = 15;
			}

			m_rLastTemperatureRecorded = 15;
			m_TempCompIn = Properties.Settings.Default.BacklashFinalDirectionIsOut;
			m_DoOnce = true;
		}
	}
}
