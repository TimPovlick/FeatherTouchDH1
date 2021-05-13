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
	//public class Errorconstants
	partial class Focuser : IFocuserV3
	{
		public const int vbObjectError = -2147221504;

		public const string ERR_SOURCE = "ASCOM Feather Touch Focuser Driver ";
		public const int SCODE_NOT_IMPLEMENTED = vbObjectError + 0x400;
		public const string MSG_NOT_IMPLEMENTED = " is not implemented by this focus driver.";
		public const int SCODE_DLL_LOADFAIL = vbObjectError + 0x401;

		//' Error message for above generated at run time
		public const int SCODE_NOT_CONNECTED = vbObjectError + 0x402;
		public const string MSG_NOT_CONNECTED = "The focuser is not connected.";

		public const int SCODE_MOVE_WHILE_COMP = vbObjectError + 0x403;
		public const string MSG_MOVE_WHILE_COMP = "The focuser cannot be moved while temperature compensation running.";


		public const int SCODE_VAL_OUTOFRANGE = vbObjectError + 0x404;
		public const string MSG_VAL_OUTOFRANGE = "The value is out of range.";

		public const int SCODE_NO_FOCUSER = vbObjectError + 0x405;
		public const string MSG_NO_FOCUSER = "The focuser was not found. Check serial connection.";

		public const int SCODE_CONTROLLER_ERROR = vbObjectError + 0x406;
		public const string MSG_CONTROLLER_ERROR = "Fatal hardware error from focuser controller.";

		public const int SCODE_BUSY_FOCUSER = vbObjectError + 0x407;
		public const string MSG_BUSY_FOCUSER = "The focuser is busy ";

		public const int SCODE_FOCUSER_ERROR = vbObjectError + 0x408;
		public const string MSG_FOCUSER_ERROR = "Focuser error. ";

		public const int SCODE_INIT_FAILURE = vbObjectError + 0x409;
		public const string MSG_INIT_FAILURE = "Focuser failed to initilize to zero properly.";

		public const int SCODE_TEMPERATURE_ERROR = vbObjectError + 0x40A;
		public const string MSG_TEMPERATURE_ERROR = "Temperature measurement error. Is the temperature probe attached?";


		//
		// Serial.cls
		//
		public const string ERR_SOURCE_SERIAL = "Feather Touch Focuser Driver Serial Port Object";
		public const int SCODE_UNSUP_SPEED = vbObjectError + 0x40;
		public const string MSG_UNSUP_SPEED = "Unsupported port speed. Use the PortSpeed enumeration.";
		public const int SCODE_INVALID_TIMEOUT = vbObjectError + 0x40C;
		public const string MSG_INVALID_TIMEOUT = "Timeout must be 1 - 120 seconds.";
		public const int SCODE_RECEIVE_TIMEOUT = vbObjectError + 0x40D;
		public const string MSG_RECEIVE_TIMEOUT = "Timed out waiting for received data.";
		public const int SCODE_EMPTY_TERM = vbObjectError + 0x40E;
		public const string MSG_EMPTY_TERM = "Terminator string must have at least one character.";
		public const int SCODE_ILLEGAL_COUNT = vbObjectError + 0x40F;
		public const string MSG_ILLEGAL_COUNT = "Character count must be positive and greater than 0.";
		public const int SCODE_PORT_FAILURE = vbObjectError + 0x410;
		public const string MSG_PORT_FAILURE = "Failed to find or open COM port.";

		public const int SCODE_REGERR = vbObjectError + 0x440;
	}
}
