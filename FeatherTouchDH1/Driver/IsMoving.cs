using System;

namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser
	{

		/// <summary>
		/// Determines if the focuser is moving.
		/// NOTE: May perform the backlash correction.
		/// </summary>
		/// <returns></returns>
		bool IsMovingImpl()
		{
			bool retVal = false;    // Assume we are not moving.
			string strStatus;

			try
			{
				if (Environment.TickCount > m_lAsyncIsMovingEndTix || !g_bUseCachedIsMoving)
				{
					strStatus = GetStatus();

					if (!string.IsNullOrEmpty(strStatus) & "B" == strStatus || "F" == strStatus)
					{
						m_bCachedIsMoving = true;
						retVal = m_bCachedIsMoving;
					}
					else
					{
						if (g_bUseCachedIsMoving == false)
						{
							if (m_fDoBacklashIn)
							{
								BacklashCompIn();
								m_fDoBacklashIn = false;
							}

							if (m_fDoBacklashOut)
							{
								BacklashCompOut();
								m_fDoBacklashOut = false;
							}
						}

						m_bCachedIsMoving = false;
						retVal = m_bCachedIsMoving;
						g_bUseCachedIsMoving = true;
					}

					m_lAsyncIsMovingEndTix = Environment.TickCount + Properties.Settings.Default.IsMovingCacheTime;
				}
				else
				{
					retVal = m_bCachedIsMoving;
				}

				return retVal;
			}
			catch (Exception e)
			{
				LogMessage("Focuser.IsMoving", "Caught exception! {0}", e.Message);
				throw new DriverException(ERR_SOURCE + MSG_FOCUSER_ERROR + " The IsMoving query failed.", e);
			}
		}

		/// <summary>
		/// Moves the focuser OUT m_lBacklashSteps steps.
		/// </summary>
		void BacklashCompOut()
		{
			LogMessage("Focuser.BacklashCompOut", "Backlash compensation {0} steps.", Properties.Settings.Default.BacklashSteps);

			// Move focuser outward.
			FocusOut((uint)Properties.Settings.Default.BacklashSteps);
		}

		/// <summary>
		/// Moves the focuser IN BacklashSteps steps.
		/// </summary>
		void BacklashCompIn()
		{
			LogMessage("Focuser.BacklashCompIn", "Backlash compensation {0} steps.", Properties.Settings.Default.BacklashSteps);

			// Move focuser inward.
			FocusIn((uint)Properties.Settings.Default.BacklashSteps);
		}


		/// <summary>
		/// Determines if the focuser is really moving, bypasses the caching.
		/// </summary>
		/// <returns>True if moving, false otherwise.</returns>
		bool IsReallyMoving()
		{
			logEntry("IsReallyMoving");

			string strStatus = GetStatus();

			if ("B" == strStatus || "F" == strStatus)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
