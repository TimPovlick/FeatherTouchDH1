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
	public partial class Focuser : IFocuserV3
	{
		public bool g_bTemperatureCompensation;
		int m_lTemperature;
		double[] m_AvgTemperature = new double[16];
		// int DeltaSteps;
		double m_dStepsPerDegree;
		float m_rLastTemperatureRecorded;

		void logEntry(string s)
		{
			LogMessage("Focuser", s);
		}

		float GetTemperature()
		{
			bool DoOnce = true;
			int DeltaSteps = 0;
			const int TemperatureDelay = 500;
			// string strTemperature;
			float CurrentTemperature;
			float DeltaT;
			int m_TemperatureMeasureCount = 0;


			LogMessage("GetFocuserTemperature", "+++");

			CommandByte((byte)FocuserCommands.TEMP);

			string strStatus = GetStatus();

			int iRetries = 0;
			bool fRetried = false;

			while (strStatus == "E" && iRetries < m_Retries)
			{
				fRetried = true;
				iRetries++;
				Thread.Sleep(100);
				strStatus = GetStatus();
			}

			if (fRetried)
			{
				Thread.Sleep(100);
				CommandByte((byte)FocuserCommands.TEMP);
				Thread.Sleep(TemperatureDelay);
				strStatus = GetStatus();

				if (strStatus == "E") goto GetTemperatureError;
			}

			if (strStatus == "C")
			{
				// The temperature probe is attached and we got a correct measurement.
				double ConvertedTemperature = m_lTemperature * 54.6 / 10813;
				ConvertedTemperature = 16000 / ConvertedTemperature - 273.15;


				for (int i = 0; i <= 14; i++)
				{
					m_AvgTemperature[i] = m_AvgTemperature[i + 1];
				}

				m_AvgTemperature[15] = ConvertedTemperature;
				m_TemperatureMeasureCount++;
			}
			else if (strStatus == "T")
			{
				// Older versions of the hub return a "T" but no temperature.
				throw new ASCOM.NotImplementedException("GetFocuserTemperature:  Older versions of the hub return a 'T' but no temperature.");
			}
			else
			{
				goto GetTemperatureError;
			}

			if (m_TemperatureMeasureCount < 1 || m_TemperatureMeasureCount > 16)
			{
				m_TemperatureMeasureCount = 16;
			}

			double TemperatureMovingAverage = 0;

			int index = 15;
			do
			{
				TemperatureMovingAverage += m_AvgTemperature[index];
				index--;
			}
			while (index < 16 - m_TemperatureMeasureCount);


			TemperatureMovingAverage /= m_TemperatureMeasureCount;
			CurrentTemperature = (float)TemperatureMovingAverage;

			if (DoOnce)
			{
				DoOnce = false;
				m_rLastTemperatureRecorded = CurrentTemperature;
			}

			if (g_bTemperatureCompensation)
			{
				DeltaT = m_rLastTemperatureRecorded - CurrentTemperature;
				DeltaSteps = Math.Abs((int)(DeltaT * m_dStepsPerDegree));
				logEntry(string.Format("Temp tracking on. Last T: {0}  Current T: {1}  DeltaT is {2}", m_rLastTemperatureRecorded, CurrentTemperature, DeltaT));

				if (Math.Abs(DeltaT) > Math.Abs(Properties.Settings.Default.DeltaT))
				{
					m_rLastTemperatureRecorded = CurrentTemperature;

					if (DeltaT > 0 && m_TempCompIn)
					{
						logEntry(string.Format("Focusing In: {0} steps", DeltaSteps));
						FocusIn((uint)DeltaSteps);
					}
					else if (DeltaT > 0 && !m_TempCompIn)
					{
						logEntry("Focusing Out: " + DeltaSteps.ToString() + " steps");
						FocusOut((uint)DeltaSteps);
					}
					else if (DeltaT < 0 && m_TempCompIn)
					{
						logEntry("Focusing Out: " + DeltaSteps.ToString() + " steps");
						FocusOut((uint)DeltaSteps);
					}
					else // DeltaT < 0 and not m_TempCompIn
					{
						logEntry("Focusing In: " + DeltaSteps.ToString() + " steps");
						FocusIn((uint)DeltaSteps);
					}
				}
			}


			return CurrentTemperature;

			GetTemperatureError:
			logEntry("GetFocuserTemperature: An error was trapped trying to get the temperature.");

			// Err.Raise(SCODE_CONTROLLER_ERROR, ERR_SOURCE, MSG_CONTROLLER_ERROR);
			return 0;
		}
	}
}
