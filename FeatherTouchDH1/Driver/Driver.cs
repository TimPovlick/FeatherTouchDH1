//tabs=4
// --------------------------------------------------------------------------------------
// ASCOM Focuser driver for FeatherTouchDH1
//
// Description:	Driver for the original FeatherTouch controller model DH1.
//
// Implements:	ASCOM Focuser interface version: V5.3 I3
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who		Version		Description
// --------------------------------------------------------------------------------------
// 15-Mar-2021	Tom		5.0.0		Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define Focuser

using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ASCOM.FeatherTouchDH1
{
	//
	// Your driver's DeviceID is ASCOM.FeatherTouchDH1.Focuser
	//
	// The GUID attribute sets the CLSID for ASCOM.FeatherTouchDH1.Focuser
	// The ClassInterface/None attribute prevents an empty interface called
	// _FeatherTouchDH1 from being created and used as the [default] interface
	//

	/// <summary>
	/// ASCOM Focuser Driver for FeatherTouchDH1.
	/// </summary>
	[Guid("25f49ceb-9c1f-4c7a-b0a6-d4c1a9582d6f")]
	[ClassInterface(ClassInterfaceType.None)]
	public partial class Focuser : IFocuserV3
	{

		#region  Properties
		/// <summary>
		/// SetupDiaglogForm needs access to this.
		/// </summary>
		internal byte StepResolution
		{
			get
			{
				return m_StepResolution;
			}

			set
			{
				m_StepResolution = value;
			}
		}

		/// <summary>
		/// SetupDiaglogForm needs access to this.
		/// </summary>
		internal bool IsLongFocuser
		{
			get
			{
				return m_IsLongFocuser;
			}

			set
			{
				m_IsLongFocuser = value;
			}
		}
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="FeatherTouchDH1"/> class.
		/// Must be public for COM registration.
		/// </summary>
		public Focuser()
		{
			s_Focuser = this;
			s_TraceLogger = new TraceLogger("", "FeatherTouchDH1");

			// Load the property setting from the ASCOM store.
			Properties.Settings.Default.Reload();

			Console.WriteLine("Last position is {0}   COM Port is {1}", Properties.Settings.Default.LastFocusPosition, Properties.Settings.Default.COMPort);

			s_TraceLogger.Enabled = Properties.Settings.Default.EnableLogging;

			// Save the property settings to the log to aid debugging.
			LogProfile();

			LogMessage("Focuser", "Starting initialization");

			m_Utilities = new Util(); // Initialize utility object
			m_AstroUtilities = new AstroUtils(); // Initialize astro-utilities object

			//
			// Populate the Dictionary collection of focuser models and properties.
			//
			m_FocuserTypes.Add(ModelNumberEnum.FTF2008BCR, new FocuserDataType(ModelNumberEnum.FTF2008BCR, 6141, 3.26, 0.8, 2));
			m_FocuserTypes.Add(ModelNumberEnum.FTF2008, new FocuserDataType(ModelNumberEnum.FTF2008, 7799, 3.26, 1, 2));
			m_FocuserTypes.Add(ModelNumberEnum.FTF2015, new FocuserDataType(ModelNumberEnum.FTF2015, 12478, 3.26, 1.6, 2));
			m_FocuserTypes.Add(ModelNumberEnum.FTF2020, new FocuserDataType(ModelNumberEnum.FTF2020, 16378, 3.26, 2.1, 2));
			m_FocuserTypes.Add(ModelNumberEnum.FTF2025, new FocuserDataType(ModelNumberEnum.FTF2025, 20277, 3.26, 2.6, 2));
			m_FocuserTypes.Add(ModelNumberEnum.FTF3545, new FocuserDataType(ModelNumberEnum.FTF3545, 62856, 1.82, 4.5, 4));
			m_FocuserTypes.Add(ModelNumberEnum.AP27FTMU, new FocuserDataType(ModelNumberEnum.AP27FTMU, 61459, 1.82, 4.4, 4));

			//                                      modelNumber  stepCount  micronsPerStep  totalTravelInches  minimumStepsPerCount
			m_FocuserTypes.Add(ModelNumberEnum.AP27FOC3E, new FocuserDataType(ModelNumberEnum.AP27FOC3E, 61459, 1.82, 4.4, 4));

			m_FocuserTypes.Add(ModelNumberEnum.AP4FOC3E, new FocuserDataType(ModelNumberEnum.AP4FOC3E, 58666, 1.82, 4.2, 4));
			m_FocuserTypes.Add(ModelNumberEnum.Other, new FocuserDataType(ModelNumberEnum.Other, 58666, 1.82, 4.2, 4));

			Initialize();

			LogMessage("Focuser", "Completed initialization");
		}
		#endregion

		//
		// PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION
		//
		#region Common properties and methods.

		/// <summary>
		/// Displays the Setup Dialog form.
		/// If the user clicks the OK button to dismiss the form, then
		/// the new settings are saved, otherwise the old values are reloaded.
		/// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
		/// </summary>
		public void SetupDialog()
		{
			// Consider only showing the setup dialog if not connected
			// or call a different dialog if connected
			if (IsConnected)
			{
				System.Windows.Forms.MessageBox.Show("Already connected, just press OK");
			}

			using (SetupDialogForm form = new SetupDialogForm(s_TraceLogger, s_Focuser))
			{
				var result = form.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					Properties.Settings.Default.Save();
				}
			}
		}


		//
		// Summary:
		//     Returns the list of custom action names supported by this driver.
		//
		// Value:
		//     An ArrayList of strings (SafeArray collection) containing the names of supported
		//     actions.
		//
		// Exceptions:
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented
		//     This method must return an empty System.Collections.ArrayList if no actions are
		//     supported. Do not throw a ASCOM.PropertyNotImplementedException.
		//     This is an aid to client authors and testers who would otherwise have to repeatedly
		//     poll the driver to determine its capabilities. Returned action names may be in
		//     mixed case to enhance presentation but the ASCOM.DeviceInterface.IFocuserV3.Action(System.String,System.String)
		//     method is case insensitive.
		//     Collections have been used in the Telescope specification for a number of years
		//     and are known to be compatible with COM. Within .NET, System.Collections.ArrayList
		//     is the correct implementation to use because the .NET Generic methods are not
		//     compatible with COM.
		public ArrayList SupportedActions
		{
			get
			{
				ArrayList actions = new ArrayList
				{
					"ResetController",
					"IsReallyMoving",
					"GetHubVersion",
					"GetMotorControllerVersion",
					"SetPosition"
				};

				return actions;
			}
		}

		//
		// Summary:
		//     Invokes the specified device-specific custom action.
		//
		// Parameters:
		//   ActionName:
		//     A well known name agreed by interested parties that represents the action to
		//     be carried out.
		//
		//   ActionParameters:
		//     List of required parameters or an Empty String if none are required.
		//
		// Returns:
		//     A string response. The meaning of returned strings is set by the driver author.
		//
		// Exceptions:
		//   T:ASCOM.MethodNotImplementedException:
		//     Thrown if no actions are supported.
		//
		//   T:ASCOM.ActionNotImplementedException:
		//     It is intended that the ASCOM.DeviceInterface.IFocuserV3.SupportedActions method
		//     will inform clients of driver capabilities, but the driver must still throw an
		//     ASCOM.ActionNotImplementedException exception if it is asked to perform an action
		//     that it does not support.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the driver is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful.
		//
		// Remarks:
		//     Must be implemented.
		//     Action names are case insensitive, so SelectWheel, selectwheel and SELECTWHEEL
		//     all refer to the same action.
		//     The names of all supported actions must be returned in the ASCOM.DeviceInterface.IFocuserV3.SupportedActions
		//     property.
		public string Action(string actionName, string actionParameters)
		{
			switch (actionName.ToLower())
			{
				case "resetcontroller":
					return ResetController();

				case "isreallymoving":
					return IsReallyMoving().ToString();

				case "gethubversion":
					return HubCodeVersion;

				case "getmotorcontrollerversion":
					return MotorControllerVersion;

				case "setposition":
					return SetFocuserPosition(actionParameters);
			}

			LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
			throw new ActionNotImplementedException("Action " + actionName + " is not implemented by FeatherTouch focuser driver.");
		}

		public void CommandBlind(string command, bool raw)
		{
			CheckConnected("CommandBlind");
			// TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
			// If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

			throw new MethodNotImplementedException("CommandBlind");
		}


		//
		// Summary:
		//     Transmits an arbitrary string to the device and waits for a boolean response.
		//     Optionally, protocol framing characters may be added to the string before transmission.
		//
		// Parameters:
		//   Command:
		//     The literal command string to be transmitted.
		//
		//   Raw:
		//     if set to true the string is transmitted 'as-is'. If set to false then protocol
		//     framing characters may be added prior to transmission.
		//
		// Returns:
		//     Returns the interpreted boolean response received from the device.
		//
		// Exceptions:
		//   T:ASCOM.MethodNotImplementedException:
		//     If the method is not implemented
		//
		//   T:ASCOM.NotConnectedException:
		//     If the driver is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exception
		public bool CommandBool(string command, bool raw)
		{
			CheckConnected("CommandBool");

			// TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
			// If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

			// string retString = CommandString(command, raw); // Send the command and wait for the response
			// bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
			// return retBool; // Return the boolean value to the client

			throw new MethodNotImplementedException("Focuser.CommandBool");
		}

		//
		// Summary:
		//     Transmits an arbitrary string to the device and waits for a string response.
		//     Optionally, protocol framing characters may be added to the string before transmission.
		//
		// Parameters:
		//   Command:
		//     The literal command string to be transmitted.
		//
		//   Raw:
		//     if set to true the string is transmitted 'as-is'. If set to false then protocol
		//     framing characters may be added prior to transmission.
		//
		// Returns:
		//     Returns the string response received from the device.
		//
		// Exceptions:
		//   T:ASCOM.MethodNotImplementedException:
		//     If the method is not implemented
		//
		//   T:ASCOM.NotConnectedException:
		//     If the driver is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exception
		public string CommandString(string command, bool raw)
		{
			CheckConnected("CommandString");

			// TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
			// If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client
			throw new MethodNotImplementedException("CommandString");
		}

		/// <summary>
		/// Closes and disposes.
		/// </summary>
		public void Dispose()
		{
			// Clean up the trace logger and utility objects
			if (s_TraceLogger != null)
			{
				s_TraceLogger.Enabled = false;
				s_TraceLogger.Dispose();
				s_TraceLogger = null;
			}

			if (m_Utilities != null)
			{
				m_Utilities.Dispose();
				m_Utilities = null;
			}

			if (m_AstroUtilities != null)
			{
				m_AstroUtilities.Dispose();
				m_AstroUtilities = null;
			}

			m_FocuserCreated = false;
		}

		//
		// Summary:
		//     Set True to connect to the device hardware. Set False to disconnect from the
		//     device hardware. You can also read the property to check whether it is connected.
		//     This reports the current hardware state.
		//
		// Value:
		//     true if connected to the hardware; otherwise, false.
		//
		// Exceptions:
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implementedDo not use a NotConnectedException here, that exception is
		//     for use in other methods that require a connection in order to succeed.
		//     The Connected property sets and reports the state of connection to the device
		//     hardware. For a hub this means that Connected will be true when the first driver
		//     connects and will only be set to false when all drivers have disconnected. A
		//     second driver may find that Connected is already true and setting Connected to
		//     false does not report Connected as false. This is not an error because the physical
		//     state is that the hardware connection is still true.
		//     Multiple calls setting Connected to true or false will not cause an error.
		//     The Connected property is not implemented in Version 1 drivers; these use the
		//     ASCOM.DeviceInterface.IFocuserV3.Link property and will raise a Not Implemented
		//     exception for this property. Version 2 drivers must implement both Connected
		//     and Link. Applications should check that InterfaceVersion returns 2 or more before
		//     using Connected.
		public bool Connected
		{
			get
			{
				return IsConnected;
			}

			set
			{
				LogMessage("Focuser.Connected", "Set {0}", value);

				if (value == IsConnected)
				{
					if (IsConnected)
						LogMessage("Connected.Set", "Already connected.");
					else
						LogMessage("Connected.Set", "Already disconnected.");

					return;
				}

				if (value)
				{
					LogMessage("Connected.Set", "Connecting to serial port {0}", ComPort);

					// Configure the serial port.
					if (s_Serial == null)
					{
						s_Serial = new Serial();
					}

					s_Serial.PortName = ComPort;
					s_Serial.Parity = SerialParity.None;
					s_Serial.Speed = SerialSpeed.ps9600;
					s_Serial.Handshake = SerialHandshake.None;
					s_Serial.DataBits = 8;
					s_Serial.StopBits = SerialStopBits.One;
					s_Serial.ReceiveTimeoutMs = 1000 * 5;		// Adjust as needed.

					// Serial port is configured, proceed to open the port.
					s_Serial.Connected = true;

					// Do the 'Link' ASCOM API method call.
					LetLink(value);
				}
				else
				{
					LetLink(value);

					// Log our property setting for debug purposes.
					LogProfile();
				}
			}
		}

		public string Description
		{
			get
			{
				return driverDescription;
			}
		}

		//
		// Summary:
		//     Descriptive and version information about this ASCOM driver.
		//
		// Exceptions:
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented This string may contain line endings and may be hundreds
		//     to thousands of characters long. It is intended to display detailed information
		//     on the ASCOM driver, including version and copyright data. See the ASCOM.DeviceInterface.IFocuserV3.Description
		//     property for information on the device itself. To get the driver version in a
		//     parse-able string, use the ASCOM.DeviceInterface.IFocuserV3.DriverVersion property.
		public string DriverInfo
		{
			get
			{
				Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
				LogMessage("Focuser.DriverInfo.Get", driverInfo);
				return driverInfo;
			}
		}

		//
		// Summary:
		//     A string containing only the major and minor version of the driver.
		//
		// Exceptions:
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented This must be in the form "n.n". It should not to be confused
		//     with the ASCOM.DeviceInterface.IFocuserV3.InterfaceVersion property, which is
		//     the version of this specification supported by the driver.
		public string DriverVersion
		{
			get
			{
				Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
				string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
				LogMessage("Focuser.DriverVersion.Get", driverVersion);
				return driverVersion;
			}
		}

		//
		// Summary:
		//     The interface version number that this device supports. Should return 3 for this
		//     interface version.
		//
		// Exceptions:
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented Clients can detect legacy V1 drivers by trying to read ths
		//     property. If the driver raises an error, it is a V1 driver. V1 did not specify
		//     this property. A driver may also return a value of 1. In other words, a raised
		//     error or a return value of 1 indicates that the driver is a V1 driver.
		public short InterfaceVersion
		{
			// set by the driver wizard
			get
			{
				LogMessage("Focuser.InterfaceVersion.Get", "3");
				return Convert.ToInt16("3");
			}
		}

		/// <summary>
		/// Friendly name of this focuser.
		/// </summary>
		public string Name
		{
			get
			{
				string name = "FeatherTouch DH1";
				LogMessage("Focuser.Name.Get", name);
				return name;
			}
		}

		#endregion

		#region IFocuser Implementation


		//
		// Summary:
		//     True if the focuser is capable of absolute position; that is, being commanded
		//     to a specific step location.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the driver must be connected in order to determine the property value.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented
		public bool Absolute
		{
			get
			{
				s_TraceLogger.LogMessage("Absolute Get", true.ToString());
				return true; // This is an absolute focuser
			}
		}

		// Summary:
		//     Immediately stop any focuser motion due to a previous ASCOM.DeviceInterface.IFocuserV3.Move(System.Int32)
		//     method call.
		//
		// Exceptions:
		//   T:ASCOM.MethodNotImplementedException:
		//     Focuser does not support this method.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the driver is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exceptionSome focusers may not support this function,
		//     in which case an exception will be raised.
		//     Recommendation: Host software should call this method upon initialization and,
		//     if it fails, disable the Halt button in the user interface.

		public void Halt()
		{
			s_TraceLogger.LogMessage("Halt", "Calling s_Focuser.Halt!");
			s_Focuser.HaltImpl();
		}

		//
		// Summary:
		//     True if the focuser is currently moving to a new position. False if the focuser
		//     is stationary.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the driver is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented
		public bool IsMoving
		{
			get
			{
				return IsMovingImpl();
			}
		}

		//
		// Summary:
		//     State of the connection to the focuser.
		//
		// Remarks:
		//     Must throw an exception if the call was not successful Must be implemented Set
		//     True to start the connection to the focuser; set False to terminate the connection.
		//     The current connection status can also be read back through this property. An
		//     exception will be raised if the link fails to change state for any reason.
		//     Note
		//     The FocuserV1 interface was the only interface to name its "Connect" method "Link"
		//     all others named their "Connect" method as "Connected". All interfaces including
		//     Focuser now have a ASCOM.DeviceInterface.IFocuserV3.Connected method and this
		//     is the recommended method to use to "Connect" to Focusers exposing the V2 and
		//     later interfaces.
		//     Do not use a NotConnectedException here, that exception is for use in other methods
		//     that require a connection in order to succeed.
		public bool Link
		{
			get
			{
				return Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
			}
			set
			{
				Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
			}
		}

		//
		// Summary:
		//     Maximum step position permitted.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented The focuser can step between 0 and ASCOM.DeviceInterface.IFocuserV3.MaxStep.
		//     If an attempt is made to move the focuser beyond these limits, it will automatically
		//     stop at the limit.
		public int MaxIncrement
		{
			get
			{
				return Properties.Settings.Default.MaxIncrement;
			}
		}

		//
		// Summary:
		//     Maximum increment size allowed by the focuser; i.e. the maximum number of steps
		//     allowed in one move operation.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented For most focusers this is the same as the ASCOM.DeviceInterface.IFocuserV3.MaxStep
		//     property. This is normally used to limit the Increment display in the host software.
		public int MaxStep
		{
			get
			{
				// Maximum extent of the focuser, so position range is 0 to 10,000
				int maxSteps = Properties.Settings.Default.MaxStep;
				LogMessage("Focuser.MaxStep.Get: ", "Maximum steps is {0}", maxSteps);
				return maxSteps;
			}
		}

		//
		// Summary:
		//     Moves the focuser by the specified amount or to the specified position depending
		//     on the value of the ASCOM.DeviceInterface.IFocuserV3.Absolute property.
		//
		// Parameters:
		//   Position:
		//     Step distance or absolute position, depending on the value of the ASCOM.DeviceInterface.IFocuserV3.Absolute
		//     property.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented
		//     If the ASCOM.DeviceInterface.IFocuserV3.Absolute property is True, then this
		//     is an absolute positioning focuser. The Move command tells the focuser to move
		//     to an exact step position, and the Position parameter of the Move method is an
		//     integer between 0 and ASCOM.DeviceInterface.IFocuserV3.MaxStep.
		//     If the ASCOM.DeviceInterface.IFocuserV3.Absolute property is False, then this
		//     is a relative positioning focuser. The Move command tells the focuser to move
		//     in a relative direction, and the Position parameter of the Move method (in this
		//     case, step distance) is an integer between minus ASCOM.DeviceInterface.IFocuserV3.MaxIncrement
		//     and plus ASCOM.DeviceInterface.IFocuserV3.MaxIncrement.
		//     BEHAVIOURAL CHANGE - Platform 6.4
		//     Prior to Platform 6.4, the interface specification mandated that drivers must
		//     throw an ASCOM.InvalidOperationException if a move was attempted when ASCOM.DeviceInterface.IFocuserV3.TempComp
		//     was True, even if the focuser was able to execute the move safely without disrupting
		//     temperature compensation.
		//     Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification
		//     has been revised to IFocuserV3, removing the requrement to throw the InvalidOperationException
		//     exception. IFocuserV3 compliant drivers are expected to execute Move requests
		//     when temperature compensation is active and to hide any specific actions required
		//     by the hardware from the client. For example this could be achieved by disabling
		//     temperature compensation, moving the focuser and re-enabling temperature compensation
		//     or simply by moving the focuser with compensation enabled if the hardware supports
		//     this.
		//     Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException
		//     exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException
		//     exceptions, in line with this revised specification.

		public void Move(int Position)
		{
			s_Focuser.MoveImpl(Position);
		}

		//
		// Summary:
		//     Current focuser position, in steps.
		//
		// Exceptions:
		//   T:ASCOM.PropertyNotImplementedException:
		//     If the property is not available for this device.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exception Valid only for absolute positioning focusers
		//     (see the ASCOM.DeviceInterface.IFocuserV3.Absolute property). A PropertyNotImplementedException
		//     exception must be thrown if this device is a relative positioning focuser rather
		//     than an absolute position focuser.
		public int Position
		{
			get
			{
				return GetPosition();
			}
		}

		//
		// Summary:
		//     Step size (microns) for the focuser.
		//
		// Exceptions:
		//   T:ASCOM.PropertyNotImplementedException:
		//     If the focuser does not intrinsically know what the step size is.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exception Must throw an exception if the focuser
		//     does not intrinsically know what the step size is.
		public double StepSize
		{
			get
			{
				return StepSizeImpl();
			}
		}

		//
		// Summary:
		//     The state of temperature compensation mode (if available), else always False.
		//
		// Exceptions:
		//   T:ASCOM.PropertyNotImplementedException:
		//     If ASCOM.DeviceInterface.IFocuserV3.TempCompAvailable is False and an attempt
		//     is made to set ASCOM.DeviceInterface.IFocuserV3.TempComp to true.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     TempComp Read must be implemented and must not throw a PropertyNotImplementedException.
		//     TempComp Write can throw a PropertyNotImplementedException. If the ASCOM.DeviceInterface.IFocuserV3.TempCompAvailable
		//     property is True, then setting ASCOM.DeviceInterface.IFocuserV3.TempComp to True
		//     puts the focuser into temperature tracking mode; setting it to False will turn
		//     off temperature tracking.
		//     If temperature compensation is not available, this property must always return
		//     False.
		//     A ASCOM.PropertyNotImplementedException exception must be thrown if ASCOM.DeviceInterface.IFocuserV3.TempCompAvailable
		//     is False and an attempt is made to set ASCOM.DeviceInterface.IFocuserV3.TempComp
		//     to true.
		//     BEHAVIOURAL CHANGE - Platform 6.4
		//     Prior to Platform 6.4, the interface specification mandated that drivers must
		//     throw an ASCOM.InvalidOperationException if a move was attempted when ASCOM.DeviceInterface.IFocuserV3.TempComp
		//     was True, even if the focuser was able to execute the move safely without disrupting
		//     temperature compensation.
		//     Following discussion on ASCOM-Talk in January 2018, the Focuser interface specification
		//     has been revised to IFocuserV3, removing the requrement to throw the InvalidOperationException
		//     exception. IFocuserV3 compliant drivers are expected to execute Move requests
		//     when temperature compensation is active and to hide any specific actions required
		//     by the hardware from the client. For example this could be achieved by disabling
		//     temperature compensation, moving the focuser and re-enabling temperature compensation
		//     or simply by moving the focuser with compensation enabled if the hardware supports
		//     this.
		//     Conform will continue to pass IFocuserV2 drivers that throw InvalidOperationException
		//     exceptions. However, Conform will now fail IFocuserV3 drivers that throw InvalidOperationException
		//     exceptions, in line with this revised specification.
		public bool TempComp
		{
			get
			{
				LogMessage("Focuser.TempComp.Get", "Not supported.");
				return false;
			}
			set
			{
				LogMessage("Focuser.TempComp.Set", "Not supported! Throwing an exception.");
				throw new ASCOM.PropertyNotImplementedException("TempComp", false);
			}
		}

		//
		// Summary:
		//     True if focuser has temperature compensation available.
		//
		// Exceptions:
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Must be implemented Will be True only if the focuser's temperature compensation
		//     can be turned on and off via the ASCOM.DeviceInterface.IFocuserV3.TempComp property.
		public bool TempCompAvailable
		{
			get
			{
				LogMessage("Focuser.TempCompAvailable.Get", "Not supported");
				return false; // Temperature compensation is not available in this driver
			}
		}

		//
		// Summary:
		//     Current ambient temperature in degrees Celsius as measured by the focuser.
		//
		// Exceptions:
		//   T:ASCOM.PropertyNotImplementedException:
		//     If the property is not available for this device.
		//
		//   T:ASCOM.NotConnectedException:
		//     If the device is not connected and this information is only available when connected.
		//
		//   T:ASCOM.DriverException:
		//     Must throw an exception if the call was not successful
		//
		// Remarks:
		//     Can throw a not implemented exception
		//     Raises an exception if ambient temperature is not available. Commonly available
		//     on focusers with a built-in temperature compensation mode.
		//     Clarification - October 2019
		//     Historically no units were specified for this property. Henceforth, if applications
		//     need to process the supplied temperature, they should proceed on the basis that
		//     the units are degrees Celsius for consistency with ASCOM.DeviceInterface.IObservingConditions.Temperature.
		//     Conversion to other temperature units can be achieved through the ASCOM.Utilities.Util.ConvertUnits(System.Double,ASCOM.Utilities.Units,ASCOM.Utilities.Units)
		//     utility method.
		public double Temperature
		{
			get
			{
				throw new PropertyNotImplementedException("Focuser.Temperature", true, "Temperature not yet supported.");
			}
		}

		#endregion

		#region Private properties and methods
		// here are some useful properties and methods that can be used as required
		// to help with driver development

		#region ASCOM Registration

		// Register or unregister driver for ASCOM. This is harmless if already
		// registered or unregistered.
		//
		/// <summary>
		/// Register or unregister the driver with the ASCOM Platform.
		/// This is harmless if the driver is already registered/unregistered.
		/// </summary>
		/// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
		private static void RegUnregASCOM(bool bRegister)
		{
			using (var P = new ASCOM.Utilities.Profile())
			{
				P.DeviceType = "Focuser";
				if (bRegister)
				{
					P.Register(driverID, driverDescription);
				}
				else
				{
					P.Unregister(driverID);
				}
			}
		}

		/// <summary>
		/// This function registers the driver with the ASCOM Chooser and
		/// is called automatically whenever this class is registered for COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is successfully built.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During setup, when the installer registers the assembly for COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually register a driver with ASCOM.
		/// </remarks>
		[ComRegisterFunction]
		public static void RegisterASCOM(Type t)
		{
			RegUnregASCOM(true);
		}

		/// <summary>
		/// This function unregisters the driver from the ASCOM Chooser and
		/// is called automatically whenever this class is unregistered from COM Interop.
		/// </summary>
		/// <param name="t">Type of the class being registered, not used.</param>
		/// <remarks>
		/// This method typically runs in two distinct situations:
		/// <list type="numbered">
		/// <item>
		/// In Visual Studio, when the project is cleaned or prior to rebuilding.
		/// For this to work correctly, the option <c>Register for COM Interop</c>
		/// must be enabled in the project settings.
		/// </item>
		/// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
		/// </list>
		/// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
		/// </remarks>
		[ComUnregisterFunction]
		public static void UnregisterASCOM(Type t)
		{
			RegUnregASCOM(false);
		}

		#endregion

		/// <summary>
		/// Returns true if there is a valid connection to the driver hardware
		/// </summary>
		private bool IsConnected
		{
			get
			{
				// TODO check that the driver hardware connection exists and is connected to the hardware
				if (s_Serial != null && s_Serial.Connected)
					return true;
				else
					return false;
			}
		}

		/// <summary>
		/// A Dictionary of known and focuser models. Also "Other" for unknown/unsupported/experimental.
		/// </summary>
		internal Dictionary<ModelNumberEnum, FocuserDataType> FocuserModels
		{
			get
			{
				return m_FocuserTypes;
			}
		}

		/// <summary>
		/// This should get the COM port from Properties.Settings.
		/// </summary>
		internal static string ComPort
		{
			get
			{
				return Properties.Settings.Default.COMPort;
			}
			set
			{
				Properties.Settings.Default.COMPort = value;
			}
		}

		/// <summary>
		/// Use this function to throw an exception if we aren't connected to the hardware
		/// </summary>
		/// <param name="message"></param>
		private void CheckConnected(string message)
		{
			if (!IsConnected)
			{
				throw new ASCOM.NotConnectedException(message);
			}
		}

		/// <summary>
		/// Read the device configuration from the ASCOM Profile store
		/// </summary>
		internal void ReadProfile()
		{
			Properties.Settings.Default.Reload();
		}

		/// <summary>
		/// Write the device configuration to the  ASCOM  Profile store
		/// </summary>
		internal void WriteProfile()
		{
			Properties.Settings.Default.Save();
		}

		/// <summary>
		/// Log helper function that takes formatted strings and arguments
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		internal void LogMessage(string identifier, string message, params object[] args)
		{
			var msg = string.Format(message, args);
			s_TraceLogger.LogMessage(identifier, msg);
		}
		#endregion

		/// <summary>
		/// Logs all the property settings. Useful for debugging.
		/// </summary>
		internal static void LogProfile()
		{
			s_TraceLogger.LogStart("Focuser.LogProfile", "Property Settings vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv");
			SettingsPropertyValueCollection valCollection = Properties.Settings.Default.PropertyValues;

			foreach (SettingsPropertyValue setting in valCollection)
			{
				// Console.WriteLine("Name: {0}  Value: {1} ", setting.Name, setting.PropertyValue);
				s_TraceLogger.LogMessage("Focuser.LogProfile", string.Format("\t {0} is {1}", setting.Name, setting.PropertyValue));
			}

			s_TraceLogger.LogStart("Focuser.LogProfile", "Property Settings ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
		}
	}
}
