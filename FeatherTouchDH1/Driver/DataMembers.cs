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
	public partial class Focuser
	{
		/// <summary>
		/// FeatherTouch Focuser specific serial port commands.
		/// </summary>
		private enum FocuserCommands
		{
			OUT = 241,      // '0xF1
			IN = 242,       // '0xF2
			STATUS = 243,   // '0xF3
			TEMP = 244,     // '0xF4
			INIT = 245,     // '0xF5
			HALT = 246,     // '0xF6
			SPEED = 247,    // '0xF7
			RESET = 248,    // '0xF8
			SETLOW = 249,   // '0xF9
			SETHI = 250,    // '0xFA
			SAVEDATA = 251, // '0xFB
			READDATA = 252, // '0xFC
		}

		/// <summary>
		/// The FeatherTouch DH1 focuser model numbers.
		/// </summary>
		internal enum ModelNumberEnum
		{
			FTF2008BCR,
			FTF2008,
			FTF2015,
			FTF2020,
			FTF2025,
			FTF3545,
			AP27FTMU,
			AP27FOC3E,
			AP4FOC3E,
			Other
		};

		/// <summary>
		/// Specific data per focuser model.
		/// </summary>
		internal struct FocuserDataType
		{
			public ModelNumberEnum ModelNumber;
			public uint StepCount;
			public double MicronsPerStep;
			public double TotalTravelInches;
			public uint MinimumStepsPerCount;

			/// <summary>
			/// A constructor for FocuserDataType that takes 5 arguments.
			/// </summary>
			/// <param name="modelNumber">The FeatherTouch model number/name.</param>
			/// <param name="stepCount">Total number of steps.</param>
			/// <param name="micronsPerStep">Microns per step, hwardware dependent.</param>
			/// <param name="totalTravelInches">Focuser travel measured in inches.</param>
			/// <param name="minimumStepsPerCount">The minimum number of steps per move command.</param>
			public FocuserDataType(ModelNumberEnum modelNumber, uint stepCount, double micronsPerStep, double totalTravelInches, uint minimumStepsPerCount)
			{
				ModelNumber = modelNumber;
				StepCount = stepCount;
				MicronsPerStep = micronsPerStep;
				TotalTravelInches = totalTravelInches;
				MinimumStepsPerCount = minimumStepsPerCount;
			}
		}

		/// <summary>
		/// ASCOM DeviceID (COM ProgID) for this driver.
		/// The DeviceID is used by ASCOM applications to load the driver at runtime.
		/// </summary>
		internal const string driverID = "ASCOM.FeatherTouchDH1.Focuser";

		/// <summary>
		/// Driver description that displays in the ASCOM Chooser.
		/// </summary>
		internal const string driverDescription = "ASCOM Focuser Driver for FeatherTouch DH1.";

		/// <summary>
		/// The one and only instance of this Singleton class.
		/// </summary>
		static Focuser s_Focuser;

		/// <summary>
		/// Collection of Feather Touch focuser models and properties.
		/// </summary>
		internal Dictionary<ModelNumberEnum, FocuserDataType> m_FocuserTypes = new Dictionary<ModelNumberEnum, FocuserDataType>();

		/// <summary>
		/// Private variable to hold an ASCOM Utilities object
		/// </summary>
		Util m_Utilities;

		/// <summary>
		/// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
		/// </summary>
		AstroUtils m_AstroUtilities;

		/// <summary>
		/// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
		/// </summary>
		static internal TraceLogger s_TraceLogger;

		/// <summary>
		/// The serial port connection to the focuser controller.
		/// </summary>
		Serial s_Serial;

		/// <summary>
		/// The current focuser position
		/// </summary>
		int m_CurrentPosition;

		int m_lPosition;

		/// <summary>
		/// The focuser's previous position.
		/// </summary>
		int m_PreviousPosition;


		bool g_bUseCachedPosition;
		bool g_bUseCachedIsMoving;

		/// <summary>
		/// Number of times we try to reset the serial port.
		/// </summary>
		int m_Retries = 5;

		/// <summary>
		///	When true this focuser is long otherwise it is short.
		/// </summary>
		bool m_IsLongFocuser;

		/// <summary>
		///
		/// </summary>
		byte m_StepResolution;


		/// <summary>
		/// Maximum number of steps this Focuser can travel.
		/// </summary>
		internal int MaxStepNumber
		{
			get
			{
				return m_MaxStepNumber;
			}

			set
			{
				m_MaxStepNumber = value;
			}

		}

		/// <summary>
		/// Absolute maximum steps allowed.
		/// </summary>
		const int MAX_STEP_LIMIT = 99999;

		/// <summary>
		/// Maximum number of steps this Focuser can travel.
		/// </summary>
		int m_MaxStepNumber;

		/// <summary>
		/// True if the focuser was moving when last queried, false otherwise.
		/// </summary>
		bool m_bCachedIsMoving;

		/// <summary>
		/// If false, query the focuser to determine if moving.
		/// </summary>

		int m_lAsyncIsMovingEndTix;

		/// <summary>
		/// If true perform a backlash IN operation.
		/// </summary>
		bool m_fDoBacklashIn;

		/// <summary>
		/// If true perform a backlash OUT operation.
		/// </summary>
		bool m_fDoBacklashOut;

		/// <summary>
		/// Set to true when the focuser is created.
		/// </summary>
		bool m_FocuserCreated;

		bool m_TempCompIn;
		bool m_DoOnce;
	}
}
