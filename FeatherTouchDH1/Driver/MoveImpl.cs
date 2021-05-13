using System;

namespace ASCOM.FeatherTouchDH1
{
	public partial class Focuser
	{
		void MoveImpl(int lNewPosition)
		{
			// WAS:  On Error GoTo MoveError
			// TODO: Put a try/catch in this code and throw an exception from within?

			if (g_bTemperatureCompensation)
			{
				LogMessage("Focuser.MoveImpl", "Move called while TempComp active!");
				throw new DriverException(ERR_SOURCE + MSG_FOCUSER_ERROR + " Move called while TempComp active. Move rejected.", SCODE_FOCUSER_ERROR);
			}

			// ASCOM SPEC: Step distance or absolute position, depending on the value of the Absolute property.

			g_bUseCachedPosition = false;
			g_bUseCachedIsMoving = false;

			if (lNewPosition > MaxStep)
			{
				logEntry("New position exceeds Maximum Focuser Travel");
				throw new InvalidValueException(" Maximum Focuser Travel Exceeded");
			}

			bool fReverseInOut = Properties.Settings.Default.ReverseInOut;

			if (lNewPosition < 0)
			{
				lNewPosition = 0;
			}

			m_CurrentPosition = GetPosition();

			int lSteps = lNewPosition - m_CurrentPosition;
			LogMessage("Focuser.MoveImpl", "Move to {0}  ({1} steps)", lNewPosition, lSteps);

			if (Math.Abs(lSteps) > Properties.Settings.Default.MaxIncrement)
			{
				logEntry("Number of steps exceed maximum focuser travel per move!");
				throw new ArgumentOutOfRangeException(" Maximum Focuser Travel Per Move Exceeded");
			}

			if (lSteps == 0)
			{
				return;			// Nothing to do, return to Bozo.
			}

			int backlashSteps = Properties.Settings.Default.BacklashSteps;

			if (lSteps > 0)
			{
				if (Properties.Settings.Default.FinalDirectionIsOut)
				{
					FocusOut((uint)lSteps);
				}
				else
				{
					LogMessage("Focuser.MoveImpl", "FocusOut backlash compensation {0} steps.", lSteps);
					FocusOut((uint)(lSteps + backlashSteps));
					m_fDoBacklashIn = true;
				}
			}
			else
			{
				if (!Properties.Settings.Default.FinalDirectionIsOut)
				{
					FocusIn((uint)Math.Abs(lSteps));
				}
				else
				{
					LogMessage("Focuser.MoveImpl", "FocusIn backlash compensation {0} steps. ", (Math.Abs(lSteps) + backlashSteps));
					FocusIn((uint)(Math.Abs(lSteps) + backlashSteps));
					m_fDoBacklashOut = true;
				}
			}

			return;

		// MoveError:      // <<<<---------------------------------- TODO
			LogMessage("Focuser.MoveImpl", "Error trapped in Move");
			throw new DriverException(ERR_SOURCE + MSG_FOCUSER_ERROR + " The Move command failed.", SCODE_FOCUSER_ERROR);
		}
	}
}
