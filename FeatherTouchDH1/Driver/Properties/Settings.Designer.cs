﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASCOM.FeatherTouchDH1.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2.6")]
        public string HubCodeVersion {
            get {
                return ((string)(this["HubCodeVersion"]));
            }
            set {
                this["HubCodeVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int BacklashSteps {
            get {
                return ((int)(this["BacklashSteps"]));
            }
            set {
                this["BacklashSteps"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM3")]
        public string COMPort {
            get {
                return ((string)(this["COMPort"]));
            }
            set {
                this["COMPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int DeltaSteps {
            get {
                return ((int)(this["DeltaSteps"]));
            }
            set {
                this["DeltaSteps"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public float DeltaT {
            get {
                return ((float)(this["DeltaT"]));
            }
            set {
                this["DeltaT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FinalDirectionIsOut {
            get {
                return ((bool)(this["FinalDirectionIsOut"]));
            }
            set {
                this["FinalDirectionIsOut"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("255")]
        public int FocusSpeed {
            get {
                return ((int)(this["FocusSpeed"]));
            }
            set {
                this["FocusSpeed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AP27FTMU")]
        public string FocuserModel {
            get {
                return ((string)(this["FocuserModel"]));
            }
            set {
                this["FocuserModel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6")]
        public int FocuserModelIndex {
            get {
                return ((int)(this["FocuserModelIndex"]));
            }
            set {
                this["FocuserModelIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BacklashFinalDirectionIsOut {
            get {
                return ((bool)(this["BacklashFinalDirectionIsOut"]));
            }
            set {
                this["BacklashFinalDirectionIsOut"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int LastFocusPosition {
            get {
                return ((int)(this["LastFocusPosition"]));
            }
            set {
                this["LastFocusPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Logging {
            get {
                return ((int)(this["Logging"]));
            }
            set {
                this["Logging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3120")]
        public int MaxIncrement {
            get {
                return ((int)(this["MaxIncrement"]));
            }
            set {
                this["MaxIncrement"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3120")]
        public int MaxStep {
            get {
                return ((int)(this["MaxStep"]));
            }
            set {
                this["MaxStep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.6")]
        public string MotorControlerVersion {
            get {
                return ((string)(this["MotorControlerVersion"]));
            }
            set {
                this["MotorControlerVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool NoCalibrationOnPowerUp {
            get {
                return ((bool)(this["NoCalibrationOnPowerUp"]));
            }
            set {
                this["NoCalibrationOnPowerUp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ReverseInOut {
            get {
                return ((bool)(this["ReverseInOut"]));
            }
            set {
                this["ReverseInOut"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int SerialDelay {
            get {
                return ((int)(this["SerialDelay"]));
            }
            set {
                this["SerialDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Medium")]
        public string StepResolution {
            get {
                return ((string)(this["StepResolution"]));
            }
            set {
                this["StepResolution"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("13.04")]
        public float StepSize {
            get {
                return ((float)(this["StepSize"]));
            }
            set {
                this["StepSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EnableLogging {
            get {
                return ((bool)(this["EnableLogging"]));
            }
            set {
                this["EnableLogging"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public int IsMovingCacheTime {
            get {
                return ((int)(this["IsMovingCacheTime"]));
            }
            set {
                this["IsMovingCacheTime"] = value;
            }
        }
    }
}
