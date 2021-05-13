// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.

#define Focuser
// remove this to bypass the code that uses the chooser to select the driver
#define UseChooser

using System;
using ASCOM.Utilities;
using ASCOM.DriverAccess;
using System.Threading;

namespace ASCOM
{
	class Program
	{
		static DriverAccess.Focuser s_Focuser;

		// [STAThreadAttribute()]
		static void Main(string[] args)
		{
			// util.SerialTraceFile = string.Format(@"C:\Users\tpovl\OneDrive\Documents\ASCOM\SerialLogs\Serial.log", DateTime.Now);
			// util.SerialTrace = false;

			try
			{
#if UseChooser
				// Choose the s_Focuser
				string id = DriverAccess.Focuser.Choose("");
				if (string.IsNullOrEmpty(id))
					return;

				// Create this focuser.
				s_Focuser = new DriverAccess.Focuser(id);
#else
			// this can be replaced by this code, it avoids the chooser and creates the driver class directly.
			DriverAccess.Focuser s_Focuser = new DriverAccess.Focuser("FeatherTouchDH1.Focuser");
#endif
				// now run some tests, adding code to your driver so that the tests will pass.
				// these first tests are common to all drivers.
				Console.WriteLine("name " + s_Focuser.Name);
				Console.WriteLine("description " + s_Focuser.Description);
				Console.WriteLine("DriverInfo " + s_Focuser.DriverInfo);
				Console.WriteLine("driverVersion " + s_Focuser.DriverVersion);


				#region Focuser
				Console.WriteLine("{0}Focuser:", Environment.NewLine);
				// progID = Focuser.Choose("ASCOM.Simulator.Focuser");        // Pre-select simulator

				if (id != "")
				{
					s_Focuser.Connected = true;
					s_Focuser.Link = true;

					Console.WriteLine("Connected to {0}  Position is {1}", id, s_Focuser.Position);

					// Verify the Halt method works.
					// HaltTest();

					// Verify we can move the focuser in and out.
					MoveTest(1000, 10);

					s_Focuser.Connected = false;
					s_Focuser.Link = false;
					s_Focuser.Dispose();
				}
				#endregion

			}
			catch (Exception ex)
			{
				Console.WriteLine("Caught Exception! ");
				Console.WriteLine(ex);
			}

			Console.WriteLine("Press Enter to finish");
			Console.ReadLine();
		}


		/// <summary>
		/// Tests
		/// </summary>
		/// <param name="moveSteps">Number of steps for each move in/out.</param>
		/// <param name="numberOfLoops">Number of test loops.</param>
		private static void MoveTest(int moveSteps, int numberOfLoops)
		{
			Console.WriteLine("{0}Move Test vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv", Environment.NewLine);

			MoveToZero();

			for (int loops = 0; loops < numberOfLoops; loops++)
			{
				int nfp = (int)(s_Focuser.Position + moveSteps);

				for (int ii = 0; ii < 5; ii++)
				{
					int moveTo = nfp + moveSteps * ii;
					Console.Write("\tMoving to {0} ",  moveTo);
					s_Focuser.Move(moveTo);

					while (s_Focuser.IsMoving)
					{
						Console.Write(".");
						Thread.Sleep(50);
					}

					Console.WriteLine("{0}\tFocuser is now at {1}", Environment.NewLine, s_Focuser.Position);
				}

				// Get position again.
				nfp = s_Focuser.Position;

				for (int ii = 0; ii <= 5; ii++)
				{
					int moveTo = nfp - moveSteps * ii;
					Console.Write("\tMoving to {0} ", moveTo);
					s_Focuser.Move(moveTo);

					while (s_Focuser.IsMoving)
					{
						Console.Write(".");
						Thread.Sleep(50);
					}

					Console.WriteLine("{0}\tFocuser is now at {1}", Environment.NewLine, s_Focuser.Position);
				}

				Console.WriteLine("Completed test loop {0}", loops + 1);
			}

			Console.WriteLine("Move Test ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ {0}", Environment.NewLine);
		}


		/// <summary>
		/// Moves the focuser to the origin (zero).
		/// </summary>
		private static void MoveToZero()
		{
			int pos = s_Focuser.Position;
			if (pos != 0)
			{
				Console.WriteLine("Current position is {0}. Moving to 0", pos);
				s_Focuser.Move(-pos);

				while (s_Focuser.IsMoving)
				{
					Thread.Sleep(100);
				}
			}
		}

		/// <summary>
		/// Tests the focuser's Halt method.
		/// </summary>
		static void HaltTest()
		{
			Console.WriteLine("{0}Halt Test vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv", Environment.NewLine);

			int pos = s_Focuser.Position;
			if (pos > 0)
			{
				Console.WriteLine("\tFocuser currently at {0}. Moving to 0", pos);
				s_Focuser.Move(-s_Focuser.Position);

				while (s_Focuser.IsMoving)
				{
					// Do nothing while the focuser moves back to zero.
					Thread.Sleep(100);
				}
			}


			int moveTo = s_Focuser.MaxStep;
			Console.WriteLine("\tMoving to {0}", s_Focuser.MaxStep);
			s_Focuser.Move(s_Focuser.MaxStep);

			// Give the focuser time to move.
			Thread.Sleep(1000 * 5);

			s_Focuser.Halt();

			Console.WriteLine("\tHalted at " + s_Focuser.Position);

			Console.WriteLine("Halt Test ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^{0}", Environment.NewLine);
		}

		/// <summary>
		/// Resets the focuser controller firmware, assuming it is able to parse the command.
		/// </summary>
		void TestHalt()
		{
			string status = s_Focuser.Action("ResetController", "");

			if (status == "SR")
			{
				Console.WriteLine("ResetController returned success.");
			}
			else
			{
				Console.WriteLine("ResetController failed!  Returned error code {0}.", status);
			}
		}
	}
}
