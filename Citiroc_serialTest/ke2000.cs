namespace Citiroc_serialTest
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
//    using Ivi.ConfigServer.Interop;
    using Ivi.Driver.Interop;
    using Ivi.Dmm.Interop;
    using System.Text;

    [Guid("5EF2A9C0-461A-11DD-90C5-B42A56D89593")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MyDevices.Dmm.ke2000")]
    public class ke2000 : Ivi.Driver.Interop.IIviDriver, IIviDriverIdentity, IIviComponentIdentity, IIviDriverOperation, IIviDriverUtility, IIviDmm, IIviDmmAC, IIviDmmFrequency, IIviDmmAdvanced, IIviDmmTemperature, IIviDmmThermocouple, IIviDmmMeasurement, IIviDmmMultiPoint, IIviDmmTrigger, System.IDisposable
    {

        private System.IntPtr _handle;
        private bool _initialized;

        private bool _disposed = true;

        ~ke2000() { Dispose(false); }


        /// <summary>
        /// This function performs the following initialization actions:
        /// 
        /// - Creates a new IVI instrument driver session.
        /// 
        /// - Opens a session to the specified device using the interface and address you specify for the Resource Name parameter.
        /// 
        /// - If the ID Query parameter is set to VI_TRUE, this function queries the instrument ID and checks that it is valid for this instrument driver.
        /// 
        /// - If the Reset parameter is set to VI_TRUE, this function resets the instrument to a known state.
        /// 
        /// - Sends initialization commands to set the instrument to the state necessary for the operation of the instrument driver.
        /// 
        /// - Returns a ViSession handle that you use to identify the instrument in all subsequent instrument driver function calls.
        /// 
        /// Note:  This function creates a new session each time you invoke it. Although you can open more than one IVI session for the same resource, it is best not to do so.  You can use the same session in multiple program threads.  You can use the ke2000_LockSession and ke2000_UnlockSession functions to protect sections of code that require exclusive access to the resource.
        /// 
        /// 
        /// </summary>
        /// <param name="Resource_Name">
        /// Pass the resource name of the device to initialize.
        /// 
        /// You can also pass the name of a virtual instrument or logical name that you configure with the IVI Configuration utility.  The virtual instrument identifies a specific device and specifies the initial settings for the session.  A logical Name identifies a particular virtual instrument.
        /// 
        /// Refer to the following table below for the exact grammar to use for this parameter.  Optional fields are shown in square brackets ([]).
        /// 
        /// Syntax
        /// ------------------------------------------------------
        /// GPIB[board]::&lt;primary address&gt;[::secondary address]::INSTR
        /// &lt;LogicalName&gt;
        /// 
        /// If you do not specify a value for an optional field, the following values are used:
        /// 
        /// Optional Field - Value
        /// ------------------------------------------------------
        /// board - 0
        /// secondary address - none (31)
        /// 
        /// The following table contains example valid values for this parameter.
        /// 
        /// "Valid Value" - Description
        /// ------------------------------------------------------
        /// "GPIB::22::INSTR" - GPIB board 0, primary address 22 no
        ///                     secondary address
        /// "GPIB::22::5::INSTR" - GPIB board 0, primary address 22
        ///                        secondary address 5
        /// "GPIB1::22::5::INSTR" - GPIB board 1, primary address 22
        ///                         secondary address 5
        /// "SampleDmm" - Logical name "SampleDmm"
        /// 
        /// Default Value:  "GPIB0::14::INSTR"
        /// </param>
        /// <param name="ID_Query">
        /// Specify whether you want the instrument driver to perform an ID Query.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Perform ID Query (Default Value)
        /// VI_FALSE (0) - Skip ID Query
        /// 
        /// When you set this parameter to VI_TRUE, the driver verifies that the instrument you initialize is a type that this driver supports.  
        /// 
        /// Circumstances can arise where it is undesirable to send an ID Query command string to the instrument.  When you set this parameter to VI_FALSE, the function initializes the instrument without performing an ID Query.
        /// </param>
        /// <param name="Reset_Device">
        /// Specify whether you want the to reset the instrument during the initialization procedure.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Reset Device (Default Value)
        /// VI_FALSE (0) - Don't Reset
        /// 
        /// </param>
        /// <param name="Instrument_Handle">
        /// Returns a ViSession handle that you use to identify the instrument in all subsequent instrument driver function calls.
        /// 
        /// Notes:
        /// 
        /// (1) This function creates a new session each time you invoke it.  This is useful if you have multiple physical instances of the same type of instrument.  
        /// 
        /// (2) Avoid creating multiple concurrent sessions to the same physical instrument.  Although you can create more than one IVI session for the same resource, it is best not to do so.  A better approach is to use the same IVI session in multiple execution threads.  You can use functions ke2000_LockSession and ke2000_UnlockSession to protect sections of code that require exclusive access to the resource.
        /// 
        /// 
        /// </param>
        public ke2000(string Resource_Name, bool ID_Query, bool Reset_Device)
        {
            int pInvokeResult = PInvoke.init(Resource_Name, System.Convert.ToUInt16(ID_Query), System.Convert.ToUInt16(Reset_Device), out this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            this._disposed = false;

        }

        /// <summary>
        /// This function performs the following initialization actions:
        /// 
        /// - Creates a new IVI instrument driver and optionally sets the initial state of the following session attributes:
        /// 
        ///     KE2000_ATTR_RANGE_CHECK         
        ///     KE2000_ATTR_QUERY_INSTRUMENT_STATUS  
        ///     KE2000_ATTR_CACHE               
        ///     KE2000_ATTR_SIMULATE            
        ///     KE2000_ATTR_RECORD_COERCIONS    
        /// 
        /// - Opens a session to the specified device using the interface and address you specify for the Resource Name parameter.
        /// 
        /// - If the ID Query parameter is set to VI_TRUE, this function queries the instrument ID and checks that it is valid for this instrument driver.
        /// 
        /// - If the Reset parameter is set to VI_TRUE, this function resets the instrument to a known state.
        /// 
        /// - Sends initialization commands to set the instrument to the state necessary for the operation of the instrument driver.
        /// 
        /// - Returns a ViSession handle that you use to identify the instrument in all subsequent instrument driver function calls.
        /// 
        /// Note:  This function creates a new session each time you invoke it. Although you can open more than one IVI session for the same resource, it is best not to do so.  You can use the same session in multiple program threads.  You can use the ke2000_LockSession and ke2000_UnlockSession functions to protect sections of code that require exclusive access to the resource.
        /// 
        /// 
        /// </summary>
        /// <param name="Resource_Name">
        /// Pass the resource name of the device to initialize.
        /// 
        /// You can also pass the name of a virtual instrument or logical name that you configure with the IVI Configuration utility.  The virtual instrument identifies a specific device and specifies the initial settings for the session.  A logical Name identifies a particular virtual instrument.
        /// 
        /// Refer to the following table below for the exact grammar to use for this parameter.  Optional fields are shown in square brackets ([]).
        /// 
        /// Syntax
        /// ------------------------------------------------------
        /// GPIB[board]::&lt;primary address&gt;[::secondary address]::INSTR
        /// &lt;LogicalName&gt;
        /// 
        /// If you do not specify a value for an optional field, the following values are used:
        /// 
        /// Optional Field - Value
        /// ------------------------------------------------------
        /// board - 0
        /// secondary address - none (31)
        /// 
        /// The following table contains example valid values for this parameter.
        /// 
        /// "Valid Value" - Description
        /// ------------------------------------------------------
        /// "GPIB::22::INSTR" - GPIB board 0, primary address 22 no
        ///                     secondary address
        /// "GPIB::22::5::INSTR" - GPIB board 0, primary address 22
        ///                        secondary address 5
        /// "GPIB1::22::5::INSTR" - GPIB board 1, primary address 22
        ///                         secondary address 5
        /// "SampleDmm" - Logical name "SampleDmm"
        /// 
        /// Default Value:  "GPIB0::14::INSTR"
        /// </param>
        /// <param name="ID_Query">
        /// Specify whether you want the instrument driver to perform an ID Query.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Perform ID Query (Default Value)
        /// VI_FALSE (0) - Skip ID Query
        /// 
        /// When you set this parameter to VI_TRUE, the driver verifies that the instrument you initialize is a type that this driver supports.  
        /// 
        /// Circumstances can arise where it is undesirable to send an ID Query command string to the instrument.  When you set this parameter to VI_FALSE, the function initializes the instrument without performing an ID Query.
        /// </param>
        /// <param name="Reset_Device">
        /// Specify whether you want the to reset the instrument during the initialization procedure.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Reset Device (Default Value)
        /// VI_FALSE (0) - Don't Reset
        /// 
        /// 
        /// </param>
        /// <param name="Option_String">
        /// You can use this control to set the initial value of certain attributes for the session.  The following table lists the attributes and the name you use in this parameter to identify the attribute.
        /// 
        /// Name              Attribute Defined Constant   
        /// --------------------------------------------
        /// RangeCheck        KE2000_ATTR_RANGE_CHECK
        /// QueryInstrStatus  KE2000_ATTR_QUERY_INSTRUMENT_STATUS   
        /// Cache             KE2000_ATTR_CACHE   
        /// Simulate          KE2000_ATTR_SIMULATE  
        /// RecordCoercions   KE2000_ATTR_RECORD_COERCIONS
        /// 
        /// The format of this string is, "AttributeName=Value" where AttributeName is the name of the attribute and Value is the value to which the attribute will be set.  To set multiple attributes, separate their assignments with a comma.
        /// 
        /// If you pass NULL or an empty string for this parameter and a VISA resource descriptor for the Resource Name parameter, the session uses the default values for the attributes. The default values for the attributes are shown below:
        /// 
        ///     Attribute Name     Default Value
        ///     ----------------   -------------
        ///     RangeCheck         VI_TRUE
        ///     QueryInstrStatus   VI_TRUE
        ///     Cache              VI_TRUE
        ///     Simulate           VI_FALSE
        ///     RecordCoercions    VI_FALSE
        ///     
        /// 
        /// If you pass NULL or an empty string for this parameter and a virtual instrument or logical name for the Resource Name parameter, the session uses the values that you configure for virtual instrument or logical name with the IVI Configuration utility.
        /// 
        /// You can override the values of the attributes by assigning a value explicitly in a string you pass for this parameter.  You do not have to specify all of the attributes and may leave any of them out.  If you do not specify one of the attributes, its default value or the value that you configure with the IVI Configuration utility will be used.
        /// 
        /// The following are the valid values for ViBoolean attributes:
        /// 
        ///     True:     1, TRUE, or VI_TRUE
        ///     False:    0, False, or VI_FALSE
        /// 
        /// 
        /// Default Value:
        /// "Simulate=0,RangeCheck=1,QueryInstrStatus=1,Cache=1"
        /// 
        /// </param>
        /// <param name="Instrument_Handle">
        /// Returns a ViSession handle that you use to identify the instrument in all subsequent instrument driver function calls.
        /// 
        /// Notes:
        /// 
        /// (1) This function creates a new session each time you invoke it.  This is useful if you have multiple physical instances of the same type of instrument.  
        /// 
        /// (2) Avoid creating multiple concurrent sessions to the same physical instrument.  Although you can create more than one IVI session for the same resource, it is best not to do so.  A better approach is to use the same IVI session in multiple execution threads.  You can use functions ke2000_LockSession and ke2000_UnlockSession to protect sections of code that require exclusive access to the resource.
        /// 
        /// 
        /// </param>
        public ke2000(string Resource_Name, bool ID_Query, bool Reset_Device, string Option_String)
        {
            int pInvokeResult = PInvoke.InitWithOptions(Resource_Name, System.Convert.ToUInt16(ID_Query), System.Convert.ToUInt16(Reset_Device), Option_String, out this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            this._disposed = false;
        }

        public ke2000()
        {

        }

        /// <summary>
        /// This function configures the common attributes of the DMM.  These attributes include the measurement function, maximum range, and the absolute resolution.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Measurement_Function">
        /// Pass the measurement function you want the DMM to perform.  The driver sets the KE2000_ATTR_FUNCTION attribute to this value. 
        /// 
        /// Valid Values:
        /// KE2000_VAL_DC_VOLTS           - DC Volts (Default)
        /// KE2000_VAL_AC_VOLTS           - AC Volts
        /// KE2000_VAL_DC_CURRENT         - DC Current
        /// KE2000_VAL_AC_CURRENT         - AC Current
        /// KE2000_VAL_2_WIRE_RES         - 2-Wire Resistance
        /// KE2000_VAL_4_WIRE_RES         - 4-Wire Resistance
        /// KE2000_VAL_TEMPERATURE        - Temperature (C) 
        /// KE2000_VAL_FREQ               - Frequency
        /// KE2000_VAL_PERIOD             - Period
        /// KE2000_VAL_DIODE              - Diode
        /// KE2000_VAL_CONTINUITY         - Continuity
        /// 
        /// </param>
        /// <param name="Range">
        /// Pass the measurement range you want to use.  The driver sets the KE2000_ATTR_RANGE attribute to this value.  
        /// 
        /// Use positive values to represent the absolute value of the maximum expected measurement.  The value must be in units appropriate for the Measurement Function.  For example, when you set the Measurement Function to KE2000_VAL_DC_VOLTS, you must specify the Range in volts.  Setting this parameter to 10.0 configures the DMM to measure DC voltages from -10.0 to +10.0 volts.
        /// 
        /// The driver reserves special negative values for controlling the DMM's auto-ranging capability.  Some measurement functions must be set to auto-ranging.
        /// 
        /// Defined Values:
        /// KE2000_VAL_AUTO_RANGE_OFF  (-2.0) - Auto-range Off
        /// KE2000_VAL_AUTO_RANGE_ON   (-1.0) - Auto-range On
        /// 
        /// Valid Manual Range:  The valid manual range depends on the Measurement Function.  The table below shows the valid manual ranges.
        /// 
        ///  --------------------------------------------------------------
        ///  |    Function    |   Min Value   |     Max Value     | Unit  |      
        ///  --------------------------------------------------------------
        ///  | DC Volts       |  0.1          |  1010.0           | volts |
        ///  | AC Volts       |  0.1          |  757.5            | volts |
        ///  | DC Current     |  0.01         |  3.03             | amps  |
        ///  | AC Current     |  1.0          |  3.03             | amps  |
        ///  | 2-Wire Res     |  100.0        |  101.0E6          | ohms  |
        ///  | 4-Wire Res     |  100.0        |  101.0E6          | ohms  |
        ///  | Temperature (C)|  Auto-range on|  Auto-range on    | ----- |
        ///  | Frequency      |  Auto-range on|  Auto-range on    | ----- |
        ///  | Period         |  Auto-range on|  Auto-range on    | ----- |
        ///  | Diode          |  Auto-range on|  Auto-range on    | ----- |
        ///  | Continuity     |  1.0          |  1000.0           | volts | --------------------------------------------------------------
        /// 
        /// Default Value: KE2000_VAL_AUTO_RANGE_ON (-1.0)
        /// 
        /// Notes:
        /// 
        /// (1) All measurement functions, except for Continuity, support KE2000_VAL_AUTO_RANGE_ON.  Only the following functions support KE2000_VAL_AUTO_RANGE_OFF: DC Volts, AC Volts, DC Current, AC Current, 2-Wire Res, and 4-Wire Res.
        /// 
        /// (2) For Continuity, this attribute specifies the threshold resistance range of the continuity test.  Continuity occurs when the measurement is less than or equal to the specified threshold resistance range.
        /// 
        /// (3) Setting this parameter to KE2000_VAL_AUTO_RANGE_ON configures the DMM to automatically calculate the range before each measurement.
        /// 
        /// (4) Setting this parameter KE2000_VAL_AUTO_RANGE_OFF configures the DMM to stop auto-ranging and keep the range fixed at the current maximum range.
        /// 
        /// </param>
        /// <param name="Resolution__absolute_">
        /// Pass your desired measurement resolution in absolute units.  The driver sets the KE2000_ATTR_RESOLUTION_ABSOLUTE attribute to this value.
        /// 
        /// Setting this parameter to lower values increases the measurement accuracy.  Setting this parameter to higher values increases the measurement speed.      
        /// 
        /// The value must be in units appropriate for the Measurement Function as shown in the following table. 
        /// 
        ///   DC Volts           - volts
        ///   AC Volts           - volts
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Frequency          - Hertz  
        ///   Period             - seconds 
        ///   Diode              - volts
        ///   Continuity         - volts
        ///   Temperature        - centigrade
        /// 
        /// 
        /// Valid Range:  
        /// 
        /// The valid range depends on the current measurement function and the current measurement range that you select with the KE2000_ATTR_FUNCTION and the KE2000_ATTR_RANGE attributes. The table below shows the minimum and maximum resolution values for each measurement type. The minimum resolution values are based on the maximum possible range for that measurement function. The maximum resolution values are based on the minimum possible range. 
        /// 
        /// 
        ///  ---------------------------------------------------------------
        ///  |    Function    | Min Resolution|  Max Resolution   | Unit   |      
        ///  ---------------------------------------------------------------
        ///  | DC Volts       |  1            |  1E-7             | volts |
        ///  | AC Volts       |  0.1          |  1E-7             | volts |
        ///  | DC Current     |  0.001        |  1E-8             | amps  |
        ///  | AC Current     |  0.001        |  1E-6             | amps  |
        ///  | 2-Wire Res     |  10000        |  1E-4             | ohms  |
        ///  | 4-Wire Res     |  10000        |  1E-4             | ohms  |
        ///  | Temperature (C)|  1            |  1E-4             |Celsius|
        ///  | Frequency      |  100          |  0.1              | Hertz |
        ///  | Period         |  1E-4         |  1E-7             | sec.  |
        ///  | Diode          |  1E-5         |  1E-6             | volts |
        ///  | Continuity     |  0.1          |  1E-4             | volts | --------------------------------------------------------------
        /// 
        /// Default Value: 0.0001 Volts
        /// 
        /// Notes:
        /// 
        /// (1) This parameter is ignored if the 'Range' parameter is set to KE2000_VAL_AUTO_RANGE_ON.
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMeasurement(int Measurement_Function, double Range, double Resolution__absolute_)
        {
            int pInvokeResult = PInvoke.ConfigureMeasurement(this._handle, Measurement_Function, Range, Resolution__absolute_);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the AC minimum and maximum frequency for DMMs that take AC voltage or AC current measurements.
        /// 
        /// This function affects the behavior of the instrument only if the KE2000_ATTR_FUNCTION attribute is set to an AC voltage or AC current measurement.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="AC_Minimum_Frequency__Hz_">
        /// Pass the minimum expected frequency component of the input signal.  The units are hertz.  The driver sets the KE2000_ATTR_AC_MIN_FREQ attribute to this value. 
        /// 
        /// This parameter effects the DMM only when you set the Measurement Function parameter to AC measurement.
        /// 
        /// Valid Values: 3.0, 30.0, 300.0 (hertz)
        /// 
        /// Default Value: 3.0 (hertz)
        /// 
        /// </param>
        /// <param name="AC_Maximum_Frequency__Hz_">
        /// Pass the maximum expected frequency component of the input signal.  The units are hertz.  The driver sets the KE2000_ATTR_AC_MAX_FREQ attribute to this value. 
        /// 
        /// This parameter effects the DMM only when you set the Measurement Function parameter to AC measurement.
        /// 
        /// Valid Range:  The valid range depends on the AC Measurement Function.
        /// 
        /// AC Volts:   3.0 - 300000.0 (hertz)
        /// AC Current: 3.0 - 5000.0 (hertz)
        /// 
        /// Default Value: 5000.0 (hertz)
        /// 
        /// Notes:
        /// 
        /// (1) The value passed in is always coerced by the driver to the maximum frequency.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureACBandwidth(double AC_Minimum_Frequency__Hz_, double AC_Maximum_Frequency__Hz_)
        {
            int pInvokeResult = PInvoke.ConfigureACBandwidth(this._handle, AC_Minimum_Frequency__Hz_, AC_Maximum_Frequency__Hz_);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the frequency voltage range of the DMM for frequency and period measurements.
        /// 
        /// This function affects the behavior of the instrument only if the KE2000_ATTR_FUNCTION attribute is set to KE2000_VAL_FREQ or KE2000_VAL_PERIOD. 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Voltage_Range__RMS_">
        /// Pass the expected maximum value of the input signal for frequency and period measurements in volts RMS. The driver sets the KE2000_ATTR_FREQ_VOLTAGE_RANGE attribute to this value.  
        /// 
        /// The driver reserves special negative values for the auto-range mode.
        /// 
        /// 
        /// Valid Range: 0.0 - 1010.0 volts RMS
        /// Default Value: 10.0 volts RMS 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureFrequencyVoltageRange(double Voltage_Range__RMS_)
        {
            int pInvokeResult = PInvoke.ConfigureFrequencyVoltageRange(this._handle, Voltage_Range__RMS_);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the DMM to take temperature measurements from a specified transducer type. 
        /// 
        /// This function affects the behavior of the instrument only when the KE2000_ATTR_FUNCTION is set to KE2000_VAL_TEMPERATURE. 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Transducer_Type">
        /// Pass the device used to measure the temperature.
        /// 
        /// Defined Values:
        /// 
        /// KE2000_VAL_THERMOCOUPLE - Thermocouple (Default)
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureTransducerType(int Transducer_Type)
        {
            int pInvokeResult = PInvoke.ConfigureTransducerType(this._handle, Transducer_Type);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the thermocouple type and the reference junction type of the thermocouple for DMMs that take temperature measurements using a thermocouple transducer type.
        /// 
        /// This function affects the behavior of the instrument only if the KE2000_ATTR_TEMP_TRANSDUCER_TYPE is set to KE2000_VAL_THERMOCOUPLE. 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Thermocouple_Type">
        /// Pass the expected thermocopule type.  The driver sets the KE2000_ATTR_TEMP_TC_TYPE attribute to this value. 
        /// 
        /// 
        /// Valid Values: KE2000_VAL_THERMO_J 
        ///               KE2000_VAL_THERMO_K
        ///               KE2000_VAL_THERMO_T
        /// 
        /// 
        /// Default Value: KE2000_VAL_THERMO_J
        /// </param>
        /// <param name="Reference_Junction_Type">
        /// Pass the type of reference junction to be used in the reference junction compensation of a thermocouple measurement. The driver sets the KE2000_ATTR_TEMP_TC_REF_JUNC_TYPE attribute to this value. 
        /// 
        /// 
        /// Valid Values:  
        /// 
        /// KE2000_VAL_TEMP_REF_JUNC_FIXED - The DMM uses a fixed value for the reference junction. Use the ke2000_ConfigureFixedRefJunction function to specify the fixed reference junction value.
        /// 
        /// KE2000_VAL_THERMO_REAL - The DMM uses a measured temperature as a reference
        /// 
        /// Default Value: KE2000_VAL_TEMP_REF_JUNC_FIXED
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureThermocouple(int Thermocouple_Type, int Reference_Junction_Type)
        {
            int pInvokeResult = PInvoke.ConfigureThermocouple(this._handle, Thermocouple_Type, Reference_Junction_Type);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the fixed reference junction for a thermocouple with a fixed reference junction type.
        /// 
        /// This function affects the behavior of the instrument only when the KE2000_ATTR_TEMP_TC_REF_JUNC_TYPE attribute is set to KE2000_VAL_TEMP_REF_JUNC_FIXED.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Fixed_Reference_Junction">
        /// Pass the external reference junction temperature when a fixed reference junction type thermocouple is used to take the temperature measurement. The driver sets the KE2000_ATTR_TEMP_TC_FIXED_REF_JUNC attribute to this value.
        /// 
        /// 
        /// The temperature is specified in degrees Celsius.
        /// 
        /// Valid Range: 0.0 to 50.0 degree centigrade
        /// Default Value: 23.0 degrees Celsius
        /// 
        /// Notes:
        /// 
        /// 1) This attribute may also be used to specify the thermocouple junction temperature of an instrument that does not have an internal temperature sensor.
        /// 
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureFixedRefJunction(double Fixed_Reference_Junction)
        {
            int pInvokeResult = PInvoke.ConfigureFixedRefJunction(this._handle, Fixed_Reference_Junction);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the real reference junction for a thermocouple with a real reference junction type. This includes setting coefficent and offset.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions  function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Coefficent">
        /// Pass the expected coefficent. The driver sets the KE2000_ATTR_THERMO_REAL_COEFFICIENT attribute to this value. 
        /// 
        /// Valid Range: -0.09999 to 0.09999
        /// 
        /// Default Value: 0.01
        /// 
        /// </param>
        /// <param name="Offset">
        /// Pass the expected offset. The driver sets the KE2000_ATTR_THERMO_REAL_OFFSET attribute to this value. 
        /// 
        /// Valid Range: -0.09999 to 0.09999
        /// 
        /// Default Value: 0.05463
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureRealRefJunction(double Coefficent, double Offset)
        {
            int pInvokeResult = PInvoke.ConfigureRealRefJunction(this._handle, Coefficent, Offset);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the common DMM trigger attributes.  These attributes include the trigger source and trigger delay.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Trigger_Source">
        /// Specify the trigger source you want to use.  The driver sets the
        /// KE2000_ATTR_TRIGGER_SOURCE attribute to this
        /// value.
        /// 
        /// After you call either the ke2000_Read or the ke2000_Initiate function, the DMM waits for the trigger you specify in this parameter.
        /// After it receives the trigger, the DMM waits the length of time
        /// you specify in the Trigger Delay parameter.  The DMM then takes
        /// a measurement.
        /// 
        /// Valid Values:
        /// KE2000_VAL_IMMEDIATE    - Immediate (Default)
        /// KE2000_VAL_EXTERNAL     - External
        /// KE2000_VAL_SOFTWARE_TRIG- Software Trigger Function
        /// KE2000_VAL_TIMER        - Timed trigger 
        /// 
        /// Notes:
        /// 
        /// (1) KE2000_VAL_IMMEDIATE - The DMM does not wait
        /// for a trigger of any kind.
        /// 
        /// (2) KE2000_VAL_EXTERNAL - The DMM waits for a
        /// trigger on the external trigger input.
        /// 
        /// (3) KE2000_VAL_SOFTWARE_TRIG - The DMM waits until
        /// you call the ke2000_SendSoftwareTrigger function.
        /// 
        /// (4) KE2000_VAL_TIMER - The DMM generates a trigger at the beginning of the timer interval and every time it times out.  The timer interval is specified by the attribute KE2000_ATTR_TIMER_INTERVAL.  For example, if the DMM is set to timer trigger source with a 30 second timer interval, the first trigger occurs immediately after the DMM initiates.  Subsequent triggers will then occur every 30 seconds.
        /// 
        /// </param>
        /// <param name="Trigger_Delay__sec_">
        /// Pass the value you want to use for the trigger delay.  Express this value in seconds.  The driver sets the KE2000_ATTR_TRIGGER_DELAY attribute to this value. 
        /// 
        /// The trigger delay specifies the length of time the DMM waits after it receives the trigger and before it takes a measurement.
        /// 
        /// Use positive values to set the trigger delay in seconds.  The driver reserves negative values for configuring the DMM to calculate the trigger delay automatically.
        /// 
        /// Defined Constant Values:
        /// KE2000_VAL_AUTO_DELAY_OFF (-2.0)- Auto-delay off
        /// KE2000_VAL_AUTO_DELAY_ON  (-1.0)- Auto-delay on
        /// 
        /// Valid Manual Range:  0.000 - 999999.999 (seconds)
        /// 
        /// Default Value: 0.001 (seconds)
        /// 
        /// Notes:
        /// 
        /// (1) Setting this parameter to KE2000_VAL_AUTO_DELAY_ON (-1.0) configures the DMM to automatically select a delay based on the selected function and range.  See the Auto Delay table in Section 3 of the Keithley Model 2000 Multimeter User's Manual for more information.
        /// 
        /// (2) Setting this parameter to KE2000_VAL_AUTO_DELAY_OFF (-2.0) stops the DMM from calculating the trigger delay and sets the trigger delay to the last automatically calculated value.
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureTrigger(int Trigger_Source, double Trigger_Delay__sec_)
        {
            int pInvokeResult = PInvoke.ConfigureTrigger(this._handle, Trigger_Source, Trigger_Delay__sec_);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the attributes for multi-point measurements.  These attributes include the trigger count, sample count, sample trigger and sample interval.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Trigger_Count">
        /// Pass the number of triggers you want the DMM to receive before returning to the Idle state.  The driver sets the KE2000_ATTR_TRIGGER_COUNT attribute to this value.
        /// 
        /// Valid Range:  1 - 1024
        /// 
        /// Default Value: 1
        /// 
        /// Notes:
        /// 
        /// (1) The result of this value times the sample count value cannot be greater than 1024.
        /// 
        /// (2) Although the Keithley 2000 does support trigger count greater than 1024, the instrument only has a maximum buffer of 1024 for holding readings.  Therefore to standardize its triggering behavior for interchangeability, we have restricted the trigger count.
        /// 
        /// </param>
        /// <param name="Sample_Count">
        /// Pass the number of measurements you want the DMM to take each time it receives a trigger.  The driver sets the KE2000_ATTR_SAMPLE_COUNT attribute to this value. 
        /// 
        /// Valid Range:  1 - 1024
        /// 
        /// Default Value: 1
        /// 
        /// Notes:
        /// 
        /// (1) The result of this value times the trigger count value cannot be greater than 1024.
        /// 
        /// </param>
        /// <param name="Sample_Trigger">
        /// Pass the type of sample trigger you want to use.  The driver sets the KE2000_ATTR_SAMPLE_TRIGGER attribute to this value. 
        /// 
        /// When the DMM takes a measurement and the Sample Count parameter is greater than 1, the DMM does not take the next measurement until the event you specify in the Sample Trigger parameter occurs.
        /// 
        /// Valid Values:
        /// 
        /// KE2000_VAL_INTERVAL     - Interval 
        /// 
        /// Notes:
        /// 
        /// (1) KE2000_VAL_INTERVAL - The DMM takes the next measurement after waiting the length of time you specify in the Sample Interval parameter.
        /// 
        /// </param>
        /// <param name="Sample_Interval__sec_">
        /// Pass the length of time you want the DMM to wait between samples.  Express this value in seconds.  The driver sets the KE2000_ATTR_SAMPLE_INTERVAL attribute to this value. 
        /// 
        /// If the Sample Trigger parameter is set to KE2000_VAL_INTERVAL, the DMM waits between measurements for the length of time you specify in this parameter.
        /// 
        /// Use positive values to set the sample interval in seconds.  The driver reserves negative values for configuring the DMM to calculate the sample interval automatically.
        /// 
        /// Defined Constant Values:
        /// KE2000_VAL_AUTO_DELAY_OFF - Auto-delay off
        /// KE2000_VAL_AUTO_DELAY_ON  - Auto-delay on
        /// 
        /// Valid Manual Range:  0.000 - 999999.999 (seconds)
        /// 
        /// Default Value: 0.001 (seconds)
        /// 
        /// Notes:
        /// 
        /// (1) Setting this parameter to KE2000_VAL_AUTO_DELAY_ON configures the DMM to automatically select a interval based on the selected function and range.  See the Auto Delay table in Section 3 of the Keithley Model 2000 Multimeter User's Manual for more information.
        /// 
        /// (2) For the ke2000 instrument driver, the KE2000_ATTR_TRIGGER_DELAY attribute and the KE2000_ATTR_SAMPLE_INTERVAL attribute represent the same delay mechanism.  They are analogous to each other.
        /// 
        /// (3) This parameter is ignored if the value of the 'Sample Count' parameter is 1.
        /// 
        /// (4) This parameter is ignored if the value of the 'Sample Trigger' parameter is not KE2000_VAL_INTERVAL.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMultiPoint(int Trigger_Count, int Sample_Count, int Sample_Trigger, double Sample_Interval__sec_)
        {
            int pInvokeResult = PInvoke.ConfigureMultiPoint(this._handle, Trigger_Count, Sample_Count, Sample_Trigger, Sample_Interval__sec_);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// After each measurement, the DMM generates a measurement-complete signal. This function configures the destination of the measurement-complete signal. 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Meas_Complete_Destination">
        /// Specifies the measurement complete destination. The driver uses this value to set the KE2000_ATTR_MEAS_COMPLETE_DEST attribute.
        ///  
        /// Defined Values: 
        /// 
        /// KE2000_VAL_EXTERNAL (Default) - External
        /// 
        /// Notes:
        /// 
        /// (1) This signal is commonly referred to as Voltmeter Complete.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMeasCompleteDest(int Meas_Complete_Destination)
        {
            int pInvokeResult = PInvoke.ConfigureMeasCompleteDest(this._handle, Meas_Complete_Destination);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the auto zero mode of the DMM.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Auto_Zero_Mode">
        /// Specify the auto-zero mode. The driver sets the KE2000_ATTR_AUTO_ZERO attribute to this value.
        /// 
        /// When the auto-zero mode is enabled, the DMM internally disconnects the input signal and takes a Zero Reading. The DMM then subtracts the Zero Reading from the measurement. This prevents offset voltages present in the instrument's input circuitry from affecting measurement accuracy.
        /// 
        /// Defined Values:
        /// 
        /// KE2000_VAL_AUTO_ZERO_ON (Default) - Configures the DMM to take a Zero Reading for each measurement. The DMM subtracts the Zero Reading from the value it measures.
        /// 
        /// KE2000_VAL_AUTO_ZERO_OFF - Disables the auto-zero feature.
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureAutoZeroMode(int Auto_Zero_Mode)
        {
            int pInvokeResult = PInvoke.ConfigureAutoZeroMode(this._handle, Auto_Zero_Mode);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the filter attributes for the current measurement function.  These attributes include KE2000_ATTR_FILTER_COUNT, KE2000_ATTR_FILTER_TYPE, KE2000_ATTR_FILTER_ENABLE.
        /// 
        /// Filtering allows you to stabilize noisy measurements.  Filtering is not available for the following measurement functions: Frequency, Period, Continuity, and Diode.
        /// 
        /// If filtering is enabled, the ke2000_Read and the ke2000_Fetch functions will return readings that reflect the filter process.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Filter_Type">
        /// Specifies the type of filter to be used.  The driver sets the KE2000_ATTR_FILTER_TYPE attribute to this value.
        /// 
        /// Valid Values:
        /// KE2000_VAL_FILTER_MOVING    - Moving average filter
        /// KE2000_VAL_FILTER_REPEATING - Repeating filter (Default Value)
        /// 
        /// Notes:
        /// 
        /// (1) The moving average filter uses a first-in, first-out stack.  When the stack becomes full, the measurement conversions are averaged, yielding a reading.  For each subsequent conversion placed into the stack, the oldest conversion is discarded, and the stack re-averaged, yielding a new reading.
        /// 
        /// (2) For the repeating filter, the stack is filled and the conversion are averaged to yield a reading.  The stack is then cleared and the process starts over.  Choose this filter for scanning so readings form other channels are not averaged with the present channel.
        /// 
        /// </param>
        /// <param name="Count">
        /// This parameter sets the filter count.  The driver sets the KE2000_ATTR_FILTER_COUNT attribute to this value.  In general, the filter count is the number of readings that are acquired and stored in the filter buffer for the averaging calculation.  The larger the filter count, the more filtering that is performed.
        /// 
        /// Valid Range: 1 - 100
        /// 
        /// Default Value: 10
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable Filter.  The driver sets the KE2000_ATTR_FILTER_ENABLE attribute to this value.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Filter
        /// VI_FALSE (0) - Disable Filter (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureFilter(int Filter_Type, int Count, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureFilter(this._handle, Filter_Type, Count, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the attributes related to the hold feature for the current measurement function.  These attributes are KE2000_ATTR_HOLD_WINDOW, KE2000_ATTR_HOLD_COUNT, and KE2000_ATTR_HOLD_ENABLE.
        /// 
        /// When hold is enabled, the first processed reading becomes the 'seed' reading and the DMM performs another reading conversion.  After the next reading is processed, it is checked to see if it is within the selected window of the 'seed' reading.  If the reading is within the window, another reading conversion is done.  The reading conversion process repeats until the specified number of consecutive readings are within the window.  If one of the reading is not within the window, the instrument acquires a new 'seed' reading and the hold process starts over.
        /// 
        /// The hold process is performed after the filter process.  If hold operation is enabled, the ke2000_Read and ke2000_Fetch functions will return readings that reflect the hold process.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Window">
        /// This parameter sets the window for hold feature.  The driver sets the KE2000_ATTR_HOLD_WINDOW attribute to this value. The window is express as a percent of the "seed" reading for the hold process.
        /// 
        /// Valid Range: 0.01 - 20 (percent)
        /// 
        /// Default Value: 1.0
        /// 
        /// </param>
        /// <param name="Count">
        /// This parameter sets the count for hold feature.  The driver sets the KE2000_ATTR_HOLD_COUNT attribute to this value.  The count is the number of readings that are compared to the "seed" reading during the hold process.
        /// 
        /// Valid Range: 2 - 100
        /// 
        /// Default Value: 2
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable the hold feature.  The driver sets the KE2000_ATTR_HOLD_ENABLE attribute to this value.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Hold
        /// VI_FALSE (0) - Disable Hold (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureHold(double Window, int Count, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureHold(this._handle, Window, Count, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the relative operation attributes for the current measurement function.  These attributes include KE2000_ATTR_RELATIVE_REFERENCE and KE2000_ATTR_RELATIVE_ENABLE.
        /// 
        /// The relative operation can be used to null offset or subtract a baseline reading from future readings.  This operation is not available for the Continuity and Diode measurement functions.
        /// 
        /// The relative operation is performed after the hold process.  If relative operation is enabled, the ke2000_Read and ke2000_Fetch functions will return readings that reflect the relative operation.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Reference">
        /// This parameter sets the reference for the relative operation.  The driver sets the KE2000_ATTR_RELATIVE_REFERENCE attribute to this value.  
        /// 
        /// Valid Range:  The valid range depends on the Measurement Function parameter of the ke2000_ConfigureMeasurement function as shown in the following table.
        /// 
        /// --------------------------------------------------------------
        /// |    Function    | Min Value | Max Value | Units             |
        /// --------------------------------------------------------------
        /// | DC Volts       | -1010.0   |  1010.0   | volts, dB, or dBm |
        /// | AC Volts       | -757.5    |  757.5    | volts, dB, or dBm |
        /// | DC Current     | -3.03     |  3.03     | amperes           |
        /// | AC Current     | -3.03     |  3.03     | amperes           |
        /// | 2-Wire Res     | 0.0       |  101.0E6  | ohms              |
        /// | 4-Wire Res     | 0.0       |  101.0E6  | ohms              |
        /// | Temperature (C)| -328.0    |  3310.0   | celsius           |
        /// | Frequency      | 0.0       |  15.0E6   | hertz             |
        /// | Period         | 0.0       |  1.0      | seconds           |
        /// --------------------------------------------------------------
        /// 
        /// Default Value: 0.0
        /// 
        /// Notes:
        /// 
        /// (1) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable the relative operation.  The driver sets the KE2000_ATTR_RELATIVE_ENABLE attribute to this value.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Relative
        /// VI_FALSE (0) - Disable Relative (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureRelative(double Reference, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureRelative(this._handle, Reference, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures attributes for the math operation. These attributes include KE2000_ATTR_MATH_FUNCTION and KE2000_ATTR_MATH_ENABLE.
        /// 
        /// The math operation is performed after the relative operation.  If math operation is enabled, the ke2000_Read and ke2000_Fetch functions will return readings that reflect the math operation.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Function">
        /// Pass the math calculation function you want the DMM to perform.  The driver sets the KE2000_ATTR_MATH_FUNCTION attribute to this value. 
        /// 
        /// Valid Values:
        /// KE2000_VAL_MATH_NONE         - None
        /// KE2000_VAL_MATH_MXB          - mX+b calculation
        /// KE2000_VAL_MATH_PERCENT      - Percent calculation
        /// 
        /// Default Value: KE2000_VAL_MATH_PERCENT
        /// 
        /// Notes:
        /// 
        /// (1) The mX+b calculation allows you to manipulate readings mathematically according to the following calculation:
        /// 
        ///   Y = mX + b
        /// 
        /// (2) The percent calculation performs a percent deviation from a user-specified reference value.  the percentage calculation is performed as follows:
        ///   
        ///             Reading - Reference
        ///   Percent = ------------------- x 100%
        ///                   Reading 
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable the math operation.  The driver sets the KE2000_ATTR_MATH_ENABLE attribute to this value.
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Math Operation
        /// VI_FALSE (0) - Disable Math Operation (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMath(int Function, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureMath(this._handle, Function, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures attributes for the mX+b (polynomial) math function. These attributes include KE2000_ATTR_MXB_M_FACTOR and KE2000_ATTR_MXB_B_OFFSET.
        /// 
        /// The mX+b function allows you to manipulate readings mathematically according to the following calculation:
        /// 
        ///   Y = mX + b
        /// 
        /// Where: 
        ///   X - Normal reading.
        ///   m - User-specified constant for scale factor.
        ///   b - User-specified constant for offset.
        ///   Y - Math result
        /// 
        /// The math operation is performed after the relative operation.  If math operation is enabled, the ke2000_Read and ke2000_Fetch functions will return readings that reflect the math operation.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="m_Scale_Factor">
        /// Pass the value for the "m" scale factor of the mX+b math function. The driver sets the KE2000_ATTR_MXB_M_FACTOR attribute to this value. 
        /// 
        /// Valid Range: -100.0E+06 - 100.0E+06
        /// 
        /// Default Value: 1.0
        /// 
        /// </param>
        /// <param name="b_Offset">
        /// Pass the value for the "b" offset of the mX+b math function.   The driver sets the KE2000_ATTR_MXB_B_OFFSET attribute to this value.
        /// 
        /// The value must be in units appropriate for the Measurement Function parameter of the ke2000_ConfigureMeasurement function as shown in the following table.
        ///   
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Temperature (C)    - celsius
        ///   Continuity         - ohms
        ///   Diode              - volts
        /// 
        /// Valid Range: -100.0E+06 - 100.0E+06
        /// 
        /// Default Value: 0.0
        /// 
        /// Notes
        /// 
        /// (1) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMathMXB(double m_Scale_Factor, double b_Offset)
        {
            int pInvokeResult = PInvoke.ConfigureMathMXB(this._handle, m_Scale_Factor, b_Offset);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures the attribute for the percent math function.  This attribute is KE2000_ATTR_PERCENT_REFERENCE.
        /// 
        /// The percent calculation performs a percent deviation from a user-specified reference value.  The percentage calculation is performed as follows:
        ///   
        ///             Reading - Reference
        ///   Percent = ------------------- x 100%
        ///                   Reading 
        /// 
        /// Where:
        ///   Reading - Normal reading
        ///   Reference - User-specified constant
        ///   Percent - Math result
        /// 
        /// 
        /// The math operation is performed after the relative operation.  If math operation is enabled, the ke2000_Read and ke2000_Fetch functions will return readings that reflect the math operation.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Reference">
        /// Pass the value for the user-specified reference for the percent function.  The driver sets the KE2000_ATTR_PERCENT_REFERENCE attribute to this value.
        /// 
        /// The value must be in units appropriate for the Measurement Function parameter of the ke2000_ConfigureMeasurement function as shown in the following table.
        ///   
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Temperature (C)    - celsius
        ///   Continuity         - ohms
        ///   Diode              - volts
        /// 
        /// Valid Range: -100.0E+06 - 100.0E+06
        /// 
        /// Default Value: 1.0
        /// 
        /// Notes:
        /// 
        /// (1) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMathPercent(double Reference)
        {
            int pInvokeResult = PInvoke.ConfigureMathPercent(this._handle, Reference);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures attributes for the statistic calculation operation. These attributes include KE2000_ATTR_STAT_FUNCTION and KE2000_ATTR_STAT_ENABLE.
        /// 
        /// The statistic calculation operation can provide statistics about the multi-point measurements.  After a multi-point measurement is made, the DMM can be setup to provide statistics (i.e. mean value, standard deviation, maximum value, and minimum value) about the multi-point measurement values.  
        /// 
        /// The statistic calculation operation is performed after mX+b and percent math operations.  Call the ke2000_FetchMultiPointStat function to get statistic calculation result.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Function">
        /// Pass the statistic calculation function you want the DMM to perform.  The driver sets the KE2000_ATTR_STAT_FUNCTION attribute to this value. 
        /// 
        /// Valid Values:
        /// KE2000_VAL_STAT_NONE         - None
        /// KE2000_VAL_STAT_MEAN         - Mean calculation
        /// KE2000_VAL_STAT_SDEV         - Standard Deviation calculation
        /// KE2000_VAL_STAT_MAX          - Maximum value
        /// KE2000_VAL_STAT_MIN          - Minimum value
        /// 
        /// Default Value: KE2000_VAL_STAT_NONE
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable the statistic calculation function.  The driver sets the KE2000_ATTR_STAT_ENABLE attribute to this value. 
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Statistic Calculation 
        /// VI_FALSE (0) - Disable Statistic Calculation (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureMultiPointStat(int Function, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureMultiPointStat(this._handle, Function, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function configures attributes for the limit test operation. These attributes include KE2000_ATTR_LIMIT_UPPER, KE2000_ATTR_LIMIT_LOWER, and KE2000_ATTR_LIMIT_ENABLE.  Limit testing can be applied to all measurement functions except Continuity.
        /// 
        /// The limit test is performed after mX+b and percent math operations.  Call the ke2000_FetchLimitTestData function to get limit test results.  
        /// 
        /// Notes:
        /// 
        /// (1) Once limit test is enabled, you must call ke2000_FetchLimitTestData to clear any old results that may be in the limit test result register.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Upper_Limit">
        /// Pass the upper range of the limit test.  The driver sets the KE2000_ATTR_LIMIT_UPPER attribute to this value. 
        /// 
        /// The value must be in units appropriate for the Measurement Function parameter of the ke2000_ConfigureMeasurement function as shown in the following table.
        ///   
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Temperature (C)    - celsius
        ///   Diode              - volts
        /// 
        /// Valid Range: -100.0E6 - 100.0E6
        /// 
        /// Default Value: 1.0
        /// 
        /// Notes:
        /// 
        /// (1) If the math operation is enabled and is set to percentage calculation, then the units of this parameter are in percent.
        /// 
        /// (2) This value must always be greater than or equal to the  Lower Limit parameter value.
        /// 
        /// </param>
        /// <param name="Lower_Limit">
        /// Pass the lower range of the limit test.  The driver sets the KE2000_ATTR_LIMIT_LOWER attribute to this value. 
        /// 
        /// The value must be in units appropriate for the Measurement Function parameter of the ke2000_ConfigureMeasurement function as shown in the following table.
        ///   
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Temperature (C)    - celsius
        ///   Diode              - volts
        /// 
        /// Valid Range: -100.0E6 - 100.0E6
        /// 
        /// Default Value: -1.0
        /// 
        /// Notes:
        /// 
        /// (1) If the math operation is enabled and is set to percentage calculation, then the units of this parameter are in percent.
        /// 
        /// (2) This value must always be lesser than or equal to the Upper Limit parameter value.
        /// 
        /// </param>
        /// <param name="State">
        /// Specify whether to enable the limit test.  The driver sets the KE2000_ATTR_LIMIT_ENABLE attribute to this value. 
        /// 
        /// Valid Range:
        /// VI_TRUE  (1) - Enable Limit Test
        /// VI_FALSE (0) - Disable Limit Test (Default Value)
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConfigureLimitTest(double Upper_Limit, double Lower_Limit, bool State)
        {
            int pInvokeResult = PInvoke.ConfigureLimitTest(this._handle, Upper_Limit, Lower_Limit, System.Convert.ToUInt16(State));
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function acquires the last processed input signal and uses it as a new relative reference value for the relative operation.  The driver sets the KE2000_ATTR_RELATIVE_REFERENCE attribute to this value.
        /// 
        /// You must first call the ke2000_Initiate function to initiate a measurement before calling this function.
        /// 
        /// Notes:
        /// 
        /// (1) The relative or math operation is filtered out of the processed input signal before the value is acquired.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int AcquireNewRelativeReference()
        {
            int pInvokeResult = PInvoke.AcquireNewRelativeReference(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function acquires the last processed input signal and uses it as a new reference value for the percent function.  The driver sets the KE2000_ATTR_PERCENT_REFERENCE attribute to this value.
        /// 
        /// You must first call the ke2000_Initiate function to initiate a measurement before calling this function.
        /// 
        /// Notes:
        /// 
        /// (1) The relative or math operation is filtered out of the processed input signal before the value is acquired.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int AcquireNewMathPercentReference()
        {
            int pInvokeResult = PInvoke.AcquireNewMathPercentReference(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function returns the actual range the DMM is currently using, even while it is auto-ranging.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Actual_Range">
        /// Returns the value of the KE2000_ATTR_AUTO_RANGE_VALUE attribute.
        /// 
        /// The units of the returned value depend on the value of the KE2000_ATTR_FUNCTION attribute.
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int GetAutoRangeValue(out double Actual_Range)
        {
            int pInvokeResult = PInvoke.GetAutoRangeValue(this._handle, out Actual_Range);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function returns additional information about the state of the instrument. Specifically, it returns the aperture time and the aperture time units.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Aperture_Time">
        /// Returns the value of the KE2000_ATTR_APERTURE_TIME attribute. 
        /// 
        /// The units of this attribute depend on the value of the KE2000_ATTR_APERTURE_TIME_UNITS attribute.
        /// </param>
        /// <param name="Aperture_Time_Units">
        /// Returns the value of the KE2000_ATTR_APERTURE_TIME_UNITS attribute. 
        /// 
        /// valid values:  KE2000_VAL_SECONDS - Seconds         
        ///                KE2000_VAL_POWER_LINE_CYCLES - Powerline cycles
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int GetApertureTimeInfo(out double Aperture_Time, out int Aperture_Time_Units)
        {
            int pInvokeResult = PInvoke.GetApertureTimeInfo(this._handle, out Aperture_Time, out Aperture_Time_Units);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function initiates a measurement, waits until the DMM has returned to the Idle state, and returns the measured value.  
        /// 
        /// Notes:
        /// 
        /// (1) After this function executes, the Reading parameter contains  an actual reading or a value indicating that an over-range condition occurred.
        /// 
        /// (2) If an over-range condition occurs, the Reading parameter contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Maximum_Time__ms_">
        /// Pass the maximum length of time in which to allow the read operation to complete.  Express this value in milliseconds.  
        /// 
        /// If the operation does not complete within this time interval, the function returns the KE2000_ERROR_MAX_TIME_EXCEEDED error code.  When this occurs, you can call ke2000_Abort to cancel the read operation and return the instrument to the Idle state.  
        /// 
        /// Defined Values:
        /// KE2000_VAL_MAX_TIME_INFINITE             KE2000_VAL_MAX_TIME_IMMEDIATE           
        /// 
        /// Default Value: 5000 (ms)
        /// 
        /// Notes:
        /// 
        /// (1) The Maximum Time parameter affects only this function.  It has no effect on other timeout parameters or attributes.
        /// 
        /// </param>
        /// <param name="Reading">
        /// Returns the measured value.  The value you specify for the Measurement Function parameter of the ke2000_ConfigureMeasurement function determines the units of this parameter as shown in the following table.
        /// 
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Temperature (C)    - celsius
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Diode              - volts
        ///   Continuity         - ohms
        ///  
        /// Notes:
        /// 
        /// (1) If an over-range condition occurs, the Reading parameter contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (2) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (3) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// (4) If the math operation is enabled with the percent function, then the units of this parameter are always percent.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int Read(int Maximum_Time__ms_, out double Reading)
        {
            int pInvokeResult = PInvoke.Read(this._handle, Maximum_Time__ms_, out Reading);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function initiates the measurement, waits for the DMM to return to the Idle state, and returns an array of measured values.  The number of measurements the DMM takes is determined by the values you specify for the Trigger Count and Sample Count parameters of the ke2000_ConfigureMultiPoint function.
        /// 
        /// Notes:
        /// 
        /// (1) After this function executes, each element in the Reading Array parameter is an actual reading or a value indicating that an over-range condition occurred.
        /// 
        /// (2) If an over-range condition occurs, the corresponding Reading Array element contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Maximum_Time__ms_">
        /// Pass the maximum length of time in which to allow the multi-point read operation to complete.  Express this value in milliseconds.  
        /// 
        /// If the operation does not complete within this time interval, the function returns the KE2000_ERROR_MAX_TIME_EXCEEDED error code.  When this occurs, you can call ke2000_Abort to cancel the multi-point read operation and return the instrument to the Idle state.  
        /// 
        /// Defined Values:
        /// KE2000_VAL_MAX_TIME_INFINITE             KE2000_VAL_MAX_TIME_IMMEDIATE           
        /// 
        /// Default Value: 5000 (ms)
        /// 
        /// Notes:
        /// 
        /// (1) The Maximum Time parameter affects only this function.  It has no effect on other timeout parameters or attributes.
        /// 
        /// </param>
        /// <param name="Array_Size">
        /// Pass the number of elements in the Reading Array parameter.
        /// 
        /// Default Value: None
        /// 
        /// </param>
        /// <param name="Reading_Array">
        /// Returns an array of the measurement values.  The value you specify for the Measurement Function parameter determines the units of this parameter as shown in the following table.
        /// 
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Temperature (C)    - celsius
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Diode              - volts
        ///   Continuity         - ohms
        /// 
        /// Notes:
        /// 
        /// (1) The size of the Reading Array must be at least the size you specify for the Array Size parameter.
        /// 
        /// (2) If an over-range condition occurs, the corresponding Reading Array element contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (4) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// (5) If the math operation is enabled with the percent function, then the units of this parameter are always percent.
        /// 
        /// </param>
        /// <param name="Actual_Number_of_Points">
        /// Indicates the number of measured values the function places in the Reading Array parameter.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        /// 
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        /// 
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ReadMultiPoint(int Maximum_Time__ms_, int Array_Size, double[] Reading_Array, out int Actual_Number_of_Points)
        {
            int pInvokeResult = PInvoke.ReadMultiPoint(this._handle, Maximum_Time__ms_, Array_Size, Reading_Array, out Actual_Number_of_Points);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function initiates a measurement.  After you call this function, the DMM leaves the Idle state and waits for a trigger.
        /// 
        /// Notes:
        /// 
        /// (1) This function does not check the instrument status.   Typically, you call this function only in a sequence of calls to other low-level driver functions.  The sequence performs one operation.  You use the low-level functions to optimize one or more aspects of interaction with the instrument.  If you want to check the instrument status, call the ke2000_error_query function at the conclusion of the sequence.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public void Initiate()
        {
            int pInvokeResult = PInvoke.Initiate(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function sends a command to trigger the DMM.  Call this function if you pass KE2000_VAL_SOFTWARE_TRIG for the Trigger Source parameter of the ke2000_ConfigureTrigger function.
        /// 
        /// Notes:
        /// 
        /// (1) This function does not check the instrument status.   Typically, you call this function only in a sequence of calls to other low-level driver functions.  The sequence performs one operation.  You use the low-level functions to optimize one or more aspects of interaction with the instrument.  If you want to check the instrument status, call the ke2000_error_query function at the conclusion of the sequence.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public void SendSoftwareTrigger()
        {
            int pInvokeResult = PInvoke.SendSoftwareTrigger(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function returns the value from a previously initiated measurement.  
        /// 
        /// You must first call the ke2000_Initiate function to initiate a measurement before calling this function.
        /// 
        /// Notes:
        /// 
        /// (1) After this function executes, the Reading parameter contains  an actual reading or a value indicating that an over-range condition occurred.
        /// 
        /// (2) If an over-range condition occurs, the Reading parameter contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (4) This function does not check the instrument status.   Typically, you call this function only in a sequence of calls to other low-level driver functions.  The sequence performs one operation.  You use the low-level functions to optimize one or more aspects of interaction with the instrument.  If you want to check the instrument status, call the ke2000_error_query function at the conclusion of the sequence.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Maximum_Time__ms_">
        /// Pass the maximum length of time in which to allow the fetch operation to complete.  Express this value in milliseconds.  
        /// 
        /// If the operation does not complete within this time interval, the function returns the KE2000_ERROR_MAX_TIME_EXCEEDED error code.  When this occurs, you can call ke2000_Abort to cancel the fetch operation and return the instrument to the Idle state.  
        /// 
        /// Defined Values:
        /// KE2000_VAL_MAX_TIME_INFINITE             KE2000_VAL_MAX_TIME_IMMEDIATE           
        /// 
        /// Default Value: 5000 (ms)
        /// 
        /// Notes:
        /// 
        /// (1) The Maximum Time parameter affects only this function.  It has no effect on other timeout parameters or attributes.
        /// 
        /// </param>
        /// <param name="Reading">
        /// Returns the most recent measurement value.  The value you specify for the Measurement Function parameter of the ke2000_ConfigureMeasurement function determines the units of this parameter as shown in the following table.
        ///  
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Temperature (C)    - celsius
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Diode              - volts
        ///   Continuity         - ohms
        /// 
        /// Notes:
        /// 
        /// (1) If an over-range condition occurs, the Reading parameter contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (2) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (3) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// (4) If the math operation is enabled with the percent function, then the units of this parameter are always percent.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int Fetch(int Maximum_Time__ms_, out double Reading)
        {
            int pInvokeResult = PInvoke.Fetch(this._handle, Maximum_Time__ms_, out Reading);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function returns an array of values from a previously initiated multi-point measurement.  The number of measurements the DMM takes is determined by the values you specify for the Trigger Count and Sample Count parameters of the ke2000_ConfigureMultiPoint function.
        /// 
        /// You must first call the ke2000_Initiate function to initiate a measurement before calling this function.
        /// 
        /// Notes:
        /// 
        /// (1) After this function executes, each element in the Reading Array parameter is an actual reading or a value indicating that an over-range condition occurred.
        /// 
        /// (2) If an over-range condition occurs, the corresponding Reading Array element contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (4) This function does not check the instrument status.   Typically, you call this function only in a sequence of calls to other low-level driver functions.  The sequence performs one operation.  You use the low-level functions to optimize one or more aspects of interaction with the instrument.  If you want to check the instrument status, call the ke2000_error_query function at the conclusion of the sequence.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// This control accepts the Instrument Handle returned by the Initialize function to select the desired instrument driver session.
        /// 
        /// Default Value:  None
        /// </param>
        /// <param name="Maximum_Time__ms_">
        /// Pass the maximum length of time in which to allow the multi-point fetch operation to complete.  Express this value in milliseconds.  
        /// 
        /// If the operation does not complete within this time interval, the function returns the KE2000_ERROR_MAX_TIME_EXCEEDED error code.  When this occurs, you can call ke2000_Abort to cancel the multi-point fetch operation and return the instrument to the Idle state.  
        /// 
        /// Defined Values:
        /// KE2000_VAL_MAX_TIME_INFINITE             KE2000_VAL_MAX_TIME_IMMEDIATE           
        /// 
        /// Default Value: 5000 (ms)
        /// 
        /// Notes:
        /// 
        /// (1) The Maximum Time parameter affects only this function.  It has no effect on other timeout parameters or attributes.
        /// 
        /// </param>
        /// <param name="Array_Size">
        /// Pass the number of elements in the Reading Array parameter.
        /// 
        /// Default Value: None
        /// 
        /// </param>
        /// <param name="Reading_Array">
        /// Returns an array of the most recent measurement values.  The value you specify for the Measurement Function parameter of the ke2000_ConfigureMeasurement function determines the units of this parameter as shown in the following table.
        ///  
        ///   DC Volts           - volts, dB, or dBm
        ///   AC Volts           - volts, dB, or dBm
        ///   DC Current         - amperes
        ///   AC Current         - amperes
        ///   2-Wire Resistance  - ohms
        ///   4-Wire Resistance  - ohms
        ///   Temperature (C)    - celsius
        ///   Frequency          - hertz
        ///   Period             - seconds
        ///   Diode              - volts
        ///   Continuity         - ohms
        /// 
        /// Notes:
        /// 
        /// (1) The size of the Reading Array must be at least the size you specify for the Array Size parameter.
        /// 
        /// (2) If an over-range condition occurs, the corresponding Reading Array element contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (3) You can test the measurement value for over-range with the ke2000_IsOverRange function.
        /// 
        /// (4) For DC Volts and AC Volts measurement functions, the units depend on the KE2000_ATTR_DC_VOLTS_UNITS and KE2000_ATTR_AC_VOLTS_UNITS attributes respectively.
        /// 
        /// (5) If the math operation is enabled with the percent function, then the units of this parameter are always percent.
        /// 
        /// </param>
        /// <param name="Actual_Number_of_Points">
        /// Indicates the number of measured values the function places in the Reading Array parameter.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int FetchMultiPoint(int Maximum_Time__ms_, int Array_Size, double[] Reading_Array, out int Actual_Number_of_Points)
        {
            int pInvokeResult = PInvoke.FetchMultiPoint(this._handle, Maximum_Time__ms_, Array_Size, Reading_Array, out Actual_Number_of_Points);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function aborts a previously initiated measurement and returns the DMM to the Idle state.
        /// 
        /// Notes:
        /// 
        /// (1) This function does not check the instrument status.   Typically, you call this function only in a sequence of calls to other low-level driver functions.  The sequence performs one operation.  You use the low-level functions to optimize one or more aspects of interaction with the instrument.  If you want to check the instrument status, call the ke2000_error_query function at the conclusion of the sequence.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public void Abort()
        {
            int pInvokeResult = PInvoke.Abort(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function takes a measurement value that you obtain from one of the Measure, Read, or Fetch functions and determines if the value is a valid measurement value or a value indicating an over-range condition occurred.  
        /// 
        /// Notes:
        /// 
        /// (1) If an over-range condition occurs, the measurement value contains an IEEE defined NaN (Not a Number) value indicating that an over-range occurred.
        /// 
        /// (2) This function does not check the instrument status.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Measurement_Value">
        /// Pass the measurement value that you obtain from one of the Measure, Read, or Fetch functions.  The driver tests this value to determine if the value is a valid measurement value or a value indicating an over-range condition occurred.  
        /// 
        /// Default Value: None
        /// 
        /// Notes:
        /// 
        /// (1) If an over-range condition occurs, the measurement value contains an IEEE defined NaN (Not a Number) value indicating that an over-range occurred.
        /// 
        /// </param>
        /// <param name="Is_Over_Range">
        /// Returns whether the measurement value is a valid measurement or a value indicating an over-range condition.
        /// 
        /// Valid Return Values:
        /// VI_TRUE  (1) - The value indicates an over-range condition.
        /// VI_FALSE (0) - The value is a valid measurement.
        /// 
        /// Notes:
        /// 
        /// (1) If an over range condition occurs, the measurement value contains an IEEE defined NaN (Not a Number) value.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int IsOverRange(double Measurement_Value, out bool Is_Over_Range)
        {
            ushort Is_Over_RangeAsUShort;
            int pInvokeResult = PInvoke.IsOverRange(this._handle, Measurement_Value, out Is_Over_RangeAsUShort);
            Is_Over_Range = System.Convert.ToBoolean(Is_Over_RangeAsUShort);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function retrieves the result of the statistic calculation operation on the multi-point measurement.  The trigger count and/or sample count should be greater than 1 before this function is called.
        /// 
        /// You must first call the ke2000_Initiate function to initiate a multi-point measurement, before calling this function.
        /// 
        /// Notes:
        /// 
        /// (1) If an over-range condition occurs, the Data parameter contains an IEEE defined NaN (Not a Number) value and the function returns KE2000_WARN_OVER_RANGE.  
        /// 
        /// (2) You can test the data value for over-range with the ke2000_IsOverRange function.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Maximum_Time__ms_">
        /// Pass the maximum length of time in which to allow the multi-point fetch stat operation to complete.  Express this value in milliseconds.  
        /// 
        /// If the operation does not complete within this time interval, the function returns the KE2000_ERROR_MAX_TIME_EXCEEDED error code.  When this occurs, you can call ke2000_Abort to cancel the multi-point fetch stat operation and return the instrument to the Idle state.  
        /// 
        /// Defined Values:
        /// KE2000_VAL_MAX_TIME_INFINITE             KE2000_VAL_MAX_TIME_IMMEDIATE           
        /// 
        /// Default Value: 5000 (ms)
        /// 
        /// Notes:
        /// 
        /// (1) The Maximum Time parameter affects only this function.  It has no effect on other timeout parameters or attributes.
        /// 
        /// </param>
        /// <param name="Data">
        /// Returns the results of the statistic calculation operation.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int FetchMultiPointStat(int Maximum_Time__ms_, out double Data)
        {
            int pInvokeResult = PInvoke.FetchMultiPointStat(this._handle, Maximum_Time__ms_, out Data);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function retrieves the results of the limit test.  After getting the results of the limit test, this function will clear the limit test result register.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Failed_Upper_Limit">
        /// Returns whether any readings has failed the upper limit of the limit test.  Failing occurs when a reading is greater than the specified upper limit.
        /// 
        /// Valid Returns:
        /// VI_TRUE  (1) - Upper limit failed
        /// VI_FALSE (0) - Upper limit passed
        /// 
        /// </param>
        /// <param name="Failed_Lower_Limit">
        /// Returns whether any readings has failed the lower limit of the limit test.  Failing occurs when a reading is lesser than the specified lower limit.
        /// 
        /// Valid Returns:
        /// VI_TRUE  (1) - Lower limit failed
        /// VI_FALSE (0) - Lower limit passed
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int FetchLimitTestData(out bool Failed_Upper_Limit, out bool Failed_Lower_Limit)
        {
            ushort Failed_Upper_LimitAsUShort;
            ushort Failed_Lower_LimitAsUShort;
            int pInvokeResult = PInvoke.FetchLimitTestData(this._handle, out Failed_Upper_LimitAsUShort, out Failed_Lower_LimitAsUShort);
            Failed_Upper_Limit = System.Convert.ToBoolean(Failed_Upper_LimitAsUShort);
            Failed_Lower_Limit = System.Convert.ToBoolean(Failed_Lower_LimitAsUShort);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function takes a temperture measurement in Centigrade, 
        /// converts to Fahrenheit or Kelvin accordingly.
        /// 
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Temperature_Type">
        /// Determine the temperature unit for the conversion.
        /// 
        /// Defined Values:
        /// 
        /// KE2000_TEMP_TYPE_F (Default) - Fahrenheit 
        /// 
        /// KE2000_TEMP_TYPE_K - Kelvin
        /// </param>
        /// <param name="Temperature">
        /// Returns the temperature value in degree Fahrenheit or Kelvin.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ConvertTemperature(int Temperature_Type, out double Temperature)
        {
            int pInvokeResult = PInvoke.ConvertTemperature(this._handle, Temperature_Type, out Temperature);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function runs the instrument's self test routine and returns the test result(s). 
        /// 
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Self_Test_Result">
        /// This control contains the value returned from the instrument self test.  Zero means success.  For any other code, see the device's operator's manual.
        /// 
        /// Self-Test Code    Description
        /// ---------------------------------------
        ///    0              Passed self test
        ///    1              Self test failed
        /// 
        /// 
        /// </param>
        /// <param name="Self_Test_Message">
        /// Returns the self-test response string from the instrument. See the device's operation manual for an explanation of the string's contents.
        /// 
        /// You must pass a ViChar array with at least 256 bytes.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int self_test(out short Self_Test_Result, System.Text.StringBuilder Self_Test_Message)
        {
            int pInvokeResult = PInvoke.self_test(this._handle, out Self_Test_Result, Self_Test_Message);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function resets the instrument to a known state and sends initialization commands to the instrument.  The initialization commands set instrument settings such as Headers Off, Short Command form, and Data Transfer Binary to the state necessary for the operation of the instrument driver.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int reset()
        {
            int pInvokeResult = PInvoke.reset(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function resets the instrument and applies initial user specified settings from the Logical Name which was used to initialize the session.  If the session was created without a Logical Name, this function is equivalent to the ke2000_reset function.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public void ResetWithDefaults()
        {
            int pInvokeResult = PInvoke.ResetWithDefaults(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function places the instrument in a quiescent state where it has minimal or no impact on the system to which it is connected.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public void Disable()
        {
            int pInvokeResult = PInvoke.Disable(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function returns the revision numbers of the instrument driver and instrument firmware.
        /// 
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Instrument_Driver_Revision">
        /// Returns the instrument driver software revision numbers in the form of a string.
        /// 
        /// You must pass a ViChar array with at least 256 bytes.
        /// </param>
        /// <param name="Firmware_Revision">
        /// Returns the instrument firmware revision numbers in the form of a string.
        /// 
        /// You must pass a ViChar array with at least 256 bytes.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int revision_query(System.Text.StringBuilder Instrument_Driver_Revision, System.Text.StringBuilder Firmware_Revision)
        {
            int pInvokeResult = PInvoke.revision_query(this._handle, Instrument_Driver_Revision, Firmware_Revision);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function reads an error code and a message from the instrument's error queue.
        /// 
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Error_Code">
        /// Returns the error code read from the instrument's error queue.
        /// 
        /// 
        /// </param>
        /// <param name="Error_Message">
        /// Returns the error message string read from the instrument's error message queue.
        /// 
        /// You must pass a ViChar array with at least 256 bytes.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int error_query(out int Error_Code, System.Text.StringBuilder Error_Message)
        {
            int pInvokeResult = PInvoke.error_query(this._handle, out Error_Code, Error_Message);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function invalidates the cached values of all attributes for the session.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public void InvalidateAllAttributes()
        {
            int pInvokeResult = PInvoke.InvalidateAllAttributes(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function returns the highest-level channel name that corresponds to the specific driver channel string that is in the channel table at an index you specify.  By passing 0 for the buffer size, the caller can ascertain the buffer size required to get the entire channel name string and then call the function again with a sufficiently large buffer.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Index">
        /// A 1-based index into the channel table.
        /// 
        /// </param>
        /// <param name="Buffer_Size">
        /// Pass the number of bytes in the ViChar array you specify for the Channel Name parameter.
        /// 
        /// If the channel name, including the terminating NUL byte, contains more bytes than you indicate in this parameter, the function copies BufferSize - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// If you pass a negative number, the function copies the value to the buffer regardless of the number of bytes in the value.
        /// 
        /// If you pass 0, you can pass VI_NULL for the Channel Name buffer parameter.
        /// 
        /// Default Value:  None
        /// </param>
        /// <param name="Channel_Name">
        /// Returns the highest-level channel name that corresponds to the specific driver channel string that is in the channel table at an index you specify..
        /// 
        /// The buffer must contain at least as many elements as the value you specify with the Buffer Size parameter.  If the channel name description, including the terminating NUL byte, contains more bytes than you indicate with the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// If you pass 0 for the Buffer Size, you can pass VI_NULL for this parameter.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// If the current value of the return buffer, including the terminating NUL byte, is larger than the size you indicate in the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public int GetChannelName(int Index, int Buffer_Size, System.Text.StringBuilder Channel_Name)
        {
            int pInvokeResult = PInvoke.GetChannelName(this._handle, Index, Buffer_Size, Channel_Name);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function returns the coercion information associated with the IVI session.  This function retrieves and clears the oldest instance in which the instrument driver coerced a value you specified to another value.
        /// 
        /// If you set the KE2000_ATTR_RECORD_COERCIONS attribute to VI_TRUE, the instrument driver keeps a list of all coercions it makes on ViInt32 or ViReal64 values you pass to instrument driver functions.  You use this function to retrieve information from that list.
        /// 
        /// If the next coercion record string, including the terminating NUL byte, contains more bytes than you indicate in this parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// If you pass a negative number, the function copies the value to the buffer regardless of the number of bytes in the value.
        /// 
        /// If you pass 0, you can pass VI_NULL for the Coercion Record buffer parameter.
        /// 
        /// The function returns an empty string in the Coercion Record parameter if no coercion records remain for the session.
        /// 
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// </param>
        /// <param name="Buffer_Size">
        /// Pass the number of bytes in the ViChar array you specify for the Coercion Record parameter.
        /// 
        /// If the next coercion record string, including the terminating NUL byte, contains more bytes than you indicate in this parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// If you pass a negative number, the function copies the value to the buffer regardless of the number of bytes in the value.
        /// 
        /// If you pass 0, you can pass VI_NULL for the Coercion Record buffer parameter.
        /// 
        /// Default Value:  None
        /// 
        /// 
        /// </param>
        /// <param name="Coercion_Record">
        /// Returns the next coercion record for the IVI session.  If there are no coercion records, the function returns an empty string.
        /// 
        /// The buffer must contain at least as many elements as the value you specify with the Buffer Size parameter.  If the next coercion record string, including the terminating NUL byte, contains more bytes than you indicate with the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// This parameter returns an empty string if no coercion records remain for the session.
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// If the current value of the return buffer, including the terminating NUL byte, is larger than the size you indicate in the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int GetNextCoercionRecord(int Buffer_Size, System.Text.StringBuilder Coercion_Record)
        {
            int pInvokeResult = PInvoke.GetNextCoercionRecord(this._handle, Buffer_Size, Coercion_Record);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        public string GetNextCoercionRecord()
        {
            int bufferSize = 256;
            StringBuilder coerRec = new StringBuilder(bufferSize);
            GetNextCoercionRecord(bufferSize, coerRec);
            return coerRec.ToString();

        }



        /// <summary>
        /// This function returns the interchangeability warnings associated with the IVI session. It retrieves and clears the oldest instance in which the class driver recorded an interchangeability warning.  Interchangeability warnings indicate that using your application with a different instrument might cause different behavior. You use this function to retrieve interchangeability warnings.
        /// 
        /// The driver performs interchangeability checking when the KE2000_ATTR_INTERCHANGE_CHECK attribute is set to VI_TRUE.
        /// 
        /// The function returns an empty string in the Interchange Warning parameter if no interchangeability warnings remain for the session.
        /// 
        /// In general, the instrument driver generates interchangeability warnings when an attribute that affects the behavior of the instrument is in a state that you did not specify.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Buffer_Size">
        /// Pass the number of bytes in the ViChar array you specify for the Interchange Warning parameter.
        /// 
        /// If the next interchangeability warning string, including the terminating NUL byte, contains more bytes than you indicate in this parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value. For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// If you pass a negative number, the function copies the value to the buffer regardless of the number of bytes in the value.
        /// 
        /// If you pass 0, you can pass VI_NULL for the Interchange Warning buffer parameter.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Interchange_Warning">
        /// Returns the next interchange warning for the IVI session. If there are no interchange warnings, the function returns an empty  string.
        /// 
        /// The buffer must contain at least as many elements as the value you specify with the Buffer Size parameter. If the next interchangeability warning string, including the terminating NUL byte, contains more bytes than you indicate with the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// This parameter returns an empty string if no interchangeability
        /// warnings remain for the session.
        /// 
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// If the current value of the return buffer, including the terminating NUL byte, is larger than the size you indicate in the Buffer Size parameter, the function copies Buffer Size - 1 bytes into the buffer, places an ASCII NUL byte at the end of the buffer, and returns the buffer size you must pass to get the entire value.  For example, if the value is "123456" and the Buffer Size is 4, the function places "123" into the buffer and returns 7.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public int GetNextInterchangeWarning(int Buffer_Size, System.Text.StringBuilder Interchange_Warning)
        {
            int pInvokeResult = PInvoke.GetNextInterchangeWarning(this._handle, Buffer_Size, Interchange_Warning);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        public string GetNextInterchangeWarning()
        {
            int bufferSize = 256;
            StringBuilder interchangeWarning = new StringBuilder(bufferSize);
            GetNextInterchangeWarning(bufferSize, interchangeWarning);
            return interchangeWarning.ToString();

        }

        /// <summary>
        /// This function clears the list of current interchange warnings.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public void ClearInterchangeWarnings()
        {
            int pInvokeResult = PInvoke.ClearInterchangeWarnings(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// When developing a complex test system that consists of multiple test modules, it is generally a good idea to design the test modules so that they can run in any order.  To do so requires ensuring that each test module completely configures the state of each instrument it uses.  If a particular test module does not completely configure the state of an instrument, the state of the instrument depends on the configuration from a previously executed test module.  If you execute the test modules in a different order, the behavior of the instrument and therefore the entire test module is likely to change.  This change in behavior is generally instrument specific and represents an interchangeability problem.
        /// 
        /// You can use this function to test for such cases.  After you call this function, the interchangeability checking algorithms in the specific driver ignore all previous configuration operations.  By calling this function at the beginning of a test module, you can determine whether the test module has dependencies on the operation of previously executed test modules.
        /// 
        /// This function does not clear the interchangeability warnings from the list of previously recorded interchangeability warnings.  If you want to guarantee that the ke2000_GetNextInterchangeWarning function only returns those interchangeability warnings that are generated after calling this function, you must clear the list of interchangeability warnings.  You can clear the interchangeability warnings list by repeatedly calling the ke2000_GetNextInterchangeWarning function until no more interchangeability warnings are returned.  If you are not interested in the content of those warnings, you can call the ke2000_ClearInterchangeWarnings function.
        /// 
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, or if the status code is not listed below, call the ke2000_error_message function.  To obtain additional information about the error condition, use the ke2000_GetError and ke2000_ClearError functions.
        /// 
        /// Status Codes:
        /// 
        /// Status    Description
        /// -------------------------------------------------
        ///        0  No error (the call was successful).
        /// 
        /// 3FFF0005  The specified termination character was read.
        /// 3FFF0006  The specified number of bytes was read.
        /// 
        /// 3FFC0102  Reset not supported.
        /// 
        /// BFFF0000  Miscellaneous or system error occurred.
        /// BFFF000E  Invalid session handle.
        /// BFFF0015  Timeout occurred before operation could complete.
        /// BFFF0034  Violation of raw write protocol occurred.
        /// BFFF0035  Violation of raw read protocol occurred.
        /// BFFF0036  Device reported an output protocol error.
        /// BFFF0037  Device reported an input protocol error.
        /// BFFF0038  Bus error occurred during transfer.
        /// BFFF003A  Invalid setup (attributes are not consistent).
        /// BFFF005F  A "no listeners" condition was detected.
        /// BFFF0060  This interface is not the controller-in-charge.
        /// BFFF0067  Operation is not supported on this session.
        /// 
        /// </returns>
        public void ResetInterchangeCheck()
        {
            int pInvokeResult = PInvoke.ResetInterchangeCheck(this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            //return pInvokeResult;
        }

        /// <summary>
        /// This function writes a user-specified string to the instrument.
        /// 
        /// Note:  This function bypasses IVI attribute state caching.  Therefore, when you call this function, the cached values for all attributes will be invalidated.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Write_Buffer">
        /// Pass the string to be written to the instrument.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int WriteInstrData(string Write_Buffer)
        {
            int pInvokeResult = PInvoke.WriteInstrData(this._handle, Write_Buffer);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        /// <summary>
        /// This function reads data from the instrument.
        /// </summary>
        /// <param name="Instrument_Handle">
        /// The ViSession handle that you obtain from the ke2000_init or ke2000_InitWithOptions function.  The handle identifies a particular instrument session.
        /// 
        /// Default Value:  None
        /// 
        /// </param>
        /// <param name="Number_of_Bytes_To_Read">
        /// Pass the maximum number of bytes to read from the instruments.  
        /// 
        /// Valid Range:  0 to the number of elements in the Read Buffer.
        /// 
        /// Default Value:  0
        /// 
        /// 
        /// </param>
        /// <param name="Read_Buffer">
        /// After this function executes, this parameter contains the data that was read from the instrument.
        /// </param>
        /// <param name="Num_Bytes_Read">
        /// Returns the number of bytes actually read from the instrument and stored in the Read Buffer.
        /// </param>
        /// <returns>
        /// Returns the status code of this operation.  The status code  either indicates success or describes an error or warning condition.  You examine the status code from each call to an instrument driver function to determine if an error occurred.
        /// 
        /// To obtain a text description of the status code, call the ke2000_error_message function.  To obtain additional information about the error condition, call the ke2000_GetError function.  To clear the error information from the driver, call the ke2000_ClearError function.
        ///           
        /// The general meaning of the status code is as follows:
        /// 
        /// Value                  Meaning
        /// -------------------------------
        /// 0                      Success
        /// Positive Values        Warnings
        /// Negative Values        Errors
        /// 
        /// This driver defines the following status codes:
        ///           
        /// Status    Description
        /// -------------------------------------------------
        /// No defined status codes.
        ///           
        /// This instrument driver also returns errors and warnings defined by other sources.  The following table defines the ranges of additional status codes that this driver can return.  The table lists the different include files that contain the defined constants for the particular status codes:
        ///           
        /// Numeric Range (in Hex)   Status Code Types    
        /// -------------------------------------------------
        /// 3FFA2000 to 3FFA3FFF     IviDmm   Warnings
        /// 3FFA0000 to 3FFA1FFF     IVI      Warnings
        /// 3FFF0000 to 3FFFFFFF     VISA     Warnings
        /// 3FFC0000 to 3FFCFFFF     VXIPnP   Driver Warnings
        ///           
        /// BFFA2000 to BFFA3FFF     IviDmm   Errors
        /// BFFA0000 to BFFA1FFF     IVI      Errors
        /// BFFF0000 to BFFFFFFF     VISA     Errors
        /// BFFC0000 to BFFCFFFF     VXIPnP   Driver Errors
        /// 
        /// </returns>
        public int ReadInstrData(int Number_of_Bytes_To_Read, System.Text.StringBuilder Read_Buffer, out int Num_Bytes_Read)
        {
            int pInvokeResult = PInvoke.ReadInstrData(this._handle, Number_of_Bytes_To_Read, Read_Buffer, out Num_Bytes_Read);
            PInvoke.TestForError(this._handle, pInvokeResult);
            return pInvokeResult;
        }

        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if ((this._disposed == false))
            {
                PInvoke.close(this._handle);
                this._handle = System.IntPtr.Zero;
            }
            this._disposed = true;
        }

        public void SetInt32(ke2000Properties propertyId, string repeatedCapabilityOrChannel, int val)
        {
            PInvoke.TestForError(this._handle, PInvoke.SetAttributeViInt32(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), val));
        }

        public void SetInt32(ke2000Properties propertyId, int val)
        {
            this.SetInt32(propertyId, "", val);
        }

        public int GetInt32(ke2000Properties propertyId, string repeatedCapabilityOrChannel)
        {
            int val;
            PInvoke.TestForError(this._handle, PInvoke.GetAttributeViInt32(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), out val));
            return val;
        }

        public int GetInt32(ke2000Properties propertyId)
        {
            return this.GetInt32(propertyId, "");
        }

        public void SetDouble(ke2000Properties propertyId, string repeatedCapabilityOrChannel, double val)
        {
            PInvoke.TestForError(this._handle, PInvoke.SetAttributeViReal64(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), val));
        }

        public void SetDouble(ke2000Properties propertyId, double val)
        {
            this.SetDouble(propertyId, "", val);
        }

        public double GetDouble(ke2000Properties propertyId, string repeatedCapabilityOrChannel)
        {
            double val;
            PInvoke.TestForError(this._handle, PInvoke.GetAttributeViReal64(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), out val));
            return val;
        }

        public double GetDouble(ke2000Properties propertyId)
        {
            return this.GetDouble(propertyId, "");
        }

        public void SetBoolean(ke2000Properties propertyId, string repeatedCapabilityOrChannel, bool val)
        {
            PInvoke.TestForError(this._handle, PInvoke.SetAttributeViBoolean(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), System.Convert.ToUInt16(val)));
        }

        public void SetBoolean(ke2000Properties propertyId, bool val)
        {
            this.SetBoolean(propertyId, "", val);
        }

        public bool GetBoolean(ke2000Properties propertyId, string repeatedCapabilityOrChannel)
        {
            ushort val;
            PInvoke.TestForError(this._handle, PInvoke.GetAttributeViBoolean(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), out val));
            return System.Convert.ToBoolean(val);
        }

        public bool GetBoolean(ke2000Properties propertyId)
        {
            return this.GetBoolean(propertyId, "");
        }

        public void SetString(ke2000Properties propertyId, string repeatedCapabilityOrChannel, string val)
        {
            PInvoke.TestForError(this._handle, PInvoke.SetAttributeViString(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), val));
        }

        public void SetString(ke2000Properties propertyId, string val)
        {
            this.SetString(propertyId, "", val);
        }

        public string GetString(ke2000Properties propertyId, string repeatedCapabilityOrChannel)
        {
            System.Text.StringBuilder newVal = new System.Text.StringBuilder(512);
            int size = PInvoke.GetAttributeViString(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), 512, newVal);
            if ((size < 0))
            {
                PInvoke.ThrowError(this._handle, size);
            }
            else
            {
                if ((size > 0))
                {
                    newVal.Capacity = size;
                    PInvoke.TestForError(this._handle, PInvoke.GetAttributeViString(this._handle, repeatedCapabilityOrChannel, ((int)(propertyId)), size, newVal));
                }
            }
            return newVal.ToString();
        }

        public string GetString(ke2000Properties propertyId)
        {
            return this.GetString(propertyId, "");
        }

        public void Initialize(string ResourceName, bool IdQuery, bool Reset, string OptionString)
        {
            int pInvokeResult = PInvoke.InitWithOptions(ResourceName, System.Convert.ToUInt16(IdQuery), System.Convert.ToUInt16(Reset), OptionString, out this._handle);
            PInvoke.TestForError(this._handle, pInvokeResult);
            this._disposed = false;
            _initialized = true;
        }

        public void SelfTest(ref int TestResult, ref string TestMessage)
        {
            short testRes;
            StringBuilder testMessage = new StringBuilder();
            self_test(out testRes, testMessage);
            TestResult = testRes;
            TestMessage = testMessage.ToString();
        }

        public void ErrorQuery(ref int ErrorCode, ref string ErrorMessage)
        {
            StringBuilder errMess = new StringBuilder();
            error_query(out ErrorCode, errMess);
            ErrorMessage = errMess.ToString();
        }

        public string InstrumentFirmwareRevision
        {
            get
            {
                StringBuilder driverRev = new StringBuilder();
                StringBuilder firmwareRev = new StringBuilder();
                revision_query(driverRev, firmwareRev);
                return firmwareRev.ToString();
            }
        }

        public string Revision
        {
            get
            {
                StringBuilder driverRev = new StringBuilder();
                StringBuilder firmwareRev = new StringBuilder();
                revision_query(driverRev, firmwareRev);
                return driverRev.ToString();
            }
        }



        public void Close()
        {
            _initialized = false;
            this.Dispose();
        }

        public void Reset()
        {
            reset();
        }

        public string LogicalName
        {
            get
            {
                return GetString(ke2000Properties.MathEnable);
            }
        }

        public string Vendor
        {
            get
            {
                return GetString(ke2000Properties.SpecificDriverVendor);
            }
        }

        public string Description
        {
            get
            {
                return GetString(ke2000Properties.SpecificDriverDescription);
            }
        }

        public string InstrumentModel
        {
            get
            {
                return GetString(ke2000Properties.InstrumentModel);
            }
        }

        public string InstrumentManufacturer
        {
            get
            {
                return GetString(ke2000Properties.InstrumentManufacturer);
            }
        }

        public bool Simulate
        {
            get
            {
                return GetBoolean(ke2000Properties.Simulate);
            }
            set
            {
                SetBoolean(ke2000Properties.Simulate, value);
            }
        }

        public bool Initialized
        {
            get { return _initialized; }
        }

        public bool RecordCoercions
        {
            get
            {
                return GetBoolean(ke2000Properties.RecordCoercions);
            }
            set
            {
                SetBoolean(ke2000Properties.RecordCoercions, value);
            }
        }

        public bool RangeCheck
        {
            get
            {
                return GetBoolean(ke2000Properties.RangeCheck);
            }
            set
            {
                SetBoolean(ke2000Properties.RangeCheck, value);
            }
        }

        public bool QueryInstrumentStatus
        {
            get
            {
                return GetBoolean(ke2000Properties.QueryInstrumentStatus);
            }
            set
            {
                SetBoolean(ke2000Properties.QueryInstrumentStatus, value);
            }
        }

        public bool InterchangeCheck
        {
            get
            {
                return GetBoolean(ke2000Properties.InterchangeCheck);
            }
            set
            {
                SetBoolean(ke2000Properties.InterchangeCheck, value);
            }
        }

        public bool Cache
        {
            get
            {
                return GetBoolean(ke2000Properties.Cache);
            }
            set
            {
                SetBoolean(ke2000Properties.Cache, value);
            }
        }

        public string IoResourceDescriptor
        {
            get
            {
                return GetString(ke2000Properties.IoResourceDescriptor);
            }
            set
            {
                GetString(ke2000Properties.IoResourceDescriptor, value);
            }
        }

        public int SpecificationMinorVersion
        {
            get
            {
                return GetInt32(ke2000Properties.SpecificDriverClassSpecMinorVersion);
            }
            set
            {
                SetInt32(ke2000Properties.SpecificDriverClassSpecMinorVersion, value);
            }
        }

        public int SpecificationMajorVersion
        {
            get
            {
                return GetInt32(ke2000Properties.SpecificDriverClassSpecMajorVersion);
            }
            set
            {
                SetInt32(ke2000Properties.SpecificDriverClassSpecMajorVersion, value);
            }
        }

        public string GroupCapabilities
        {
            get
            {
                return GetString(ke2000Properties.GroupCapabilities);
            }
        }
        public string SupportedInstrumentModels
        {
            get
            {
                return GetString(ke2000Properties.SupportedInstrumentModels);
            }
        }

        public string Identifier
        {
            get
            {
                return GetString(ke2000Properties.IdQueryResponse);
            }
        }

        public void UnlockObject()
        {
            //iviDriverErrorQueue.Enqueue(new Error(IviDriver_ErrorCodes.E_IVI_METHOD_NOT_SUPPORTED));
            throw new Exception("E_IVI_METHOD_NOT_SUPPORTED");
        }

        public void LockObject()
        {
            //iviDriverErrorQueue.Enqueue(new Error(IviDriver_ErrorCodes.E_IVI_METHOD_NOT_SUPPORTED));
            throw new Exception("E_IVI_METHOD_NOT_SUPPORTED");
        }

        public string DriverSetup
        {
            get { return GetString(ke2000Properties.DriverSetup); }
        }

        public IIviDriverOperation DriverOperation
        {
            get { return (IIviDriverOperation)this; }
        }

        public IIviDriverIdentity Identity
        {
            get { return (IIviDriverIdentity)this; }
        }

        public IIviDriverUtility Utility
        {
            get { return (IIviDriverUtility)this; }
        }


        #region IIviDmm Members

        public IIviDmmAC AC
        {
            get { return (IIviDmmAC)this; }
        }

        public IIviDmmAdvanced Advanced
        {
            get { return (IIviDmmAdvanced)this; }
        }


        public void Configure(IviDmmFunctionEnum Function, double Range, double Resolution)
        {

            ConfigureMeasurement(CastFunction(Function), Range, Resolution);
        }


        public IIviDmmFrequency Frequency
        {
            get { return (IIviDmmFrequency)this; }
        }


        public IviDmmFunctionEnum Function
        {
            get
            {
                return CastFunction(GetInt32(ke2000Properties.Function));
            }
            set
            {
                SetInt32(ke2000Properties.Function, CastFunction(value));
            }
        }


        public IIviDmmMeasurement Measurement
        {
            get { return (IIviDmmMeasurement)this; }
        }

        public double Range
        {
            get
            {
                return GetDouble(ke2000Properties.Range);
            }
            set
            {
                SetDouble(ke2000Properties.Range, value);
            }
        }

        public double Resolution
        {
            get
            {
                return GetDouble(ke2000Properties.ResolutionAbsolute);
            }
            set
            {
                SetDouble(ke2000Properties.ResolutionAbsolute, value);
            }
        }

        public IIviDmmTemperature Temperature
        {
            get { return (IIviDmmTemperature)this; }
        }

        public IIviDmmTrigger Trigger
        {
            get { return (IIviDmmTrigger)this; }
        }

        #endregion

        #region IIviDmmAC Members

        void IIviDmmAC.ConfigureBandwidth(double MinFreq, double MaxFreq)
        {
            ConfigureACBandwidth(MinFreq, MaxFreq);
        }

        public double FrequencyMax
        {
            get
            {
                return GetDouble(ke2000Properties.AcMaxFreq);
            }
            set
            {
                SetDouble(ke2000Properties.AcMaxFreq, value);
            }
        }

        public double FrequencyMin
        {
            get
            {
                return GetDouble(ke2000Properties.AcMinFreq);
            }
            set
            {
                SetDouble(ke2000Properties.AcMinFreq, value);
            }
        }



        #endregion

        #region IIviDmmAdvanced Members

        public double ActualRange
        {
            get
            {
                double res;
                GetAutoRangeValue(out res);
                return res;
            }
        }

        public double ApertureTime
        {
            get { return GetDouble(ke2000Properties.ApertureTime); }
        }

        public IviDmmApertureTimeUnitsEnum ApertureTimeUnits
        {
            get
            {
                return CastApertureTimeUnits(GetInt32(ke2000Properties.ApertureTimeUnits));
            }
        }

        public IviDmmAutoZeroEnum AutoZero
        {
            get
            {
                return CastAutoZero(GetInt32(ke2000Properties.AutoZero));
            }
            set
            {
                SetInt32(ke2000Properties.AutoZero, CastAutoZero(value));
            }
        }

        public double PowerlineFrequency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IIviDmmFrequency Members

        public double VoltageRange
        {
            get
            {
                return GetDouble(ke2000Properties.FreqVoltageRange);
            }
            set
            {
                SetDouble(ke2000Properties.FreqVoltageRange, value);
            }
        }

        #endregion

        #region IIviDmmMeasurement Members        

        public double Fetch(int MaxTimeMilliseconds)
        {
            double reading;
            Fetch(MaxTimeMilliseconds, out reading);
            return reading;

        }

        public double[] FetchMultiPoint(int MaxTimeMilliseconds)
        {
            int size = this.Trigger.MultiPoint.Count * this.Trigger.MultiPoint.SampleCount;
            double[] readingArray = new double[size];
            int numberOfPoints;
            FetchMultiPoint(MaxTimeMilliseconds, size, readingArray, out numberOfPoints);
            return readingArray;
        }

        public bool IsOverRange(double MeasurementValue)
        {
            bool res;
            IsOverRange(MeasurementValue, out res);
            return res;
        }

        public double Read(int MaxTimeMilliseconds)
        {
            double reading;
            Read(MaxTimeMilliseconds, out reading);
            return reading;
        }

        public double[] ReadMultiPoint(int MaxTimeMilliseconds)
        {
            int size = this.Trigger.MultiPoint.Count * this.Trigger.MultiPoint.SampleCount;
            double[] readingArray = new double[size];
            int numberOfPoints;
            ReadMultiPoint(MaxTimeMilliseconds, size, readingArray, out numberOfPoints);
            return readingArray;
        }

        #endregion

        #region IIviDmmTemperature Members

        IIviDmmRTD IIviDmmTemperature.RTD
        {
            get { throw new NotImplementedException(); }
        }

        IIviDmmThermistor IIviDmmTemperature.Thermistor
        {
            get { throw new NotImplementedException(); }
        }

        IIviDmmThermocouple IIviDmmTemperature.Thermocouple
        {
            get { return (IIviDmmThermocouple)this; }
        }

        IviDmmTransducerTypeEnum IIviDmmTemperature.TransducerType
        {
            get
            {
                return CastTransducerType(GetInt32(ke2000Properties.TempTransducerType));
            }
            set
            {
                SetInt32(ke2000Properties.TempTransducerType, CastTransducerType(value));
            }
        }

        #endregion

        #region IIviDmmThermocouple Members

        void IIviDmmThermocouple.Configure(IviDmmThermocoupleTypeEnum Type, IviDmmRefJunctionTypeEnum RefJunctionType)
        {
            throw new NotImplementedException();
        }

        double IIviDmmThermocouple.FixedRefJunction
        {
            get
            {
                return GetDouble(ke2000Properties.TempTcFixedRefJunc);
            }
            set
            {
                SetDouble(ke2000Properties.TempTcFixedRefJunc, value);
            }
        }

        IviDmmRefJunctionTypeEnum IIviDmmThermocouple.RefJunctionType
        {
            get
            {
                int juncType = GetInt32(ke2000Properties.TempTcRefJuncType);
                return CastRefJunctionType(juncType);
            }
            set
            {
                SetInt32(ke2000Properties.TempTcRefJuncType, CastRefJunctionType(value));
            }
        }

        IviDmmThermocoupleTypeEnum IIviDmmThermocouple.Type
        {
            get
            {
                int TCType;
                TCType = GetInt32(ke2000Properties.TempTcType);
                return CastTCType(TCType);

            }
            set
            {
                SetInt32(ke2000Properties.TempTcType, CastTCType(value));
            }
        }

        #endregion

        #region IIviDmmMultiPoint Members

        void IIviDmmMultiPoint.Configure(int TriggerCount, int SampleCount, IviDmmSampleTriggerEnum SampleTrigger, double SampleInterval)
        {
            ConfigureMultiPoint(TriggerCount, SampleCount, CastSampleTrigger(SampleTrigger), SampleInterval);
        }

        int IIviDmmMultiPoint.Count
        {
            get
            {
                return GetInt32(ke2000Properties.TriggerCount);
            }
            set
            {
                SetInt32(ke2000Properties.TriggerCount, value);
            }
        }

        IviDmmMeasCompleteDestEnum IIviDmmMultiPoint.MeasurementComplete
        {
            get
            {

                return (IviDmmMeasCompleteDestEnum)GetInt32(ke2000Properties.MeasCompleteDest);
            }
            set
            {
                SetInt32(ke2000Properties.MeasCompleteDest, (int)value);
            }
        }

        int IIviDmmMultiPoint.SampleCount
        {
            get
            {
                return GetInt32(ke2000Properties.SampleCount);
            }
            set
            {
                SetInt32(ke2000Properties.SampleCount, value);
            }
        }

        double IIviDmmMultiPoint.SampleInterval
        {
            get
            {
                return GetDouble(ke2000Properties.SampleInterval);
            }
            set
            {
                SetDouble(ke2000Properties.SampleInterval, value);
            }
        }

        IviDmmSampleTriggerEnum IIviDmmMultiPoint.SampleTrigger
        {
            get
            {
                return CastSampleTrigger(GetInt32(ke2000Properties.SampleTrigger));
            }
            set
            {
                SetInt32(ke2000Properties.SampleTrigger, CastSampleTrigger(value));
            }
        }

        #endregion

        #region IIviDmmTrigger Members

        void IIviDmmTrigger.Configure(IviDmmTriggerSourceEnum TriggerSource, double TriggerDelay)
        {
            ConfigureTrigger(CastTrigger(TriggerSource), TriggerDelay);
        }

        double IIviDmmTrigger.Delay
        {
            get
            {
                return GetDouble(ke2000Properties.TriggerDelay);
            }
            set
            {
                SetDouble(ke2000Properties.TriggerDelay, value);
            }
        }

        IIviDmmMultiPoint IIviDmmTrigger.MultiPoint
        {
            get { return (IIviDmmMultiPoint)this; }
        }

        IviDmmTriggerSlopeEnum IIviDmmTrigger.Slope
        {
            get
            {
                throw new NotImplementedException();

            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IviDmmTriggerSourceEnum IIviDmmTrigger.Source
        {
            get
            {
                return CastTrigger(GetInt32(ke2000Properties.TriggerSource));
            }
            set
            {
                SetInt32(ke2000Properties.TriggerSource, CastTrigger(value));
            }
        }

        #endregion

        #region Casts

        private int CastSampleTrigger(IviDmmSampleTriggerEnum SampleTrigger)
        {
            switch (SampleTrigger)
            {
                case IviDmmSampleTriggerEnum.IviDmmSampleTriggerExternal:
                    return ke2000Constants.External;
                case IviDmmSampleTriggerEnum.IviDmmSampleTriggerImmediate:
                    return ke2000Constants.Immediate;
                case IviDmmSampleTriggerEnum.IviDmmSampleTriggerSwTrigFunc:
                    return ke2000Constants.SoftwareTrig;
                default:
                    return (int)SampleTrigger;

            }
        }

        private IviDmmSampleTriggerEnum CastSampleTrigger(int SampleTrigger)
        {
            switch (SampleTrigger)
            {
                case ke2000Constants.External:
                    return IviDmmSampleTriggerEnum.IviDmmSampleTriggerExternal;
                case ke2000Constants.Immediate:
                    return IviDmmSampleTriggerEnum.IviDmmSampleTriggerImmediate;
                case ke2000Constants.SoftwareTrig:
                    return IviDmmSampleTriggerEnum.IviDmmSampleTriggerSwTrigFunc;
                default:
                    return (IviDmmSampleTriggerEnum)SampleTrigger;
            }
        }

        private int CastTrigger(IviDmmTriggerSourceEnum Trigger)
        {
            switch (Trigger)
            {
                case IviDmmTriggerSourceEnum.IviDmmTriggerSourceExternal:
                    return ke2000Constants.External;
                case IviDmmTriggerSourceEnum.IviDmmTriggerSourceImmediate:
                    return ke2000Constants.Immediate;
                case IviDmmTriggerSourceEnum.IviDmmTriggerSourceSwTrigFunc:
                    return ke2000Constants.SoftwareTrig;
                default:
                    return (int)Trigger;
            }
        }

        private IviDmmTriggerSourceEnum CastTrigger(int Trigger)
        {
            switch (Trigger)
            {
                case ke2000Constants.External:
                    return IviDmmTriggerSourceEnum.IviDmmTriggerSourceExternal;
                case ke2000Constants.Immediate:
                    return IviDmmTriggerSourceEnum.IviDmmTriggerSourceImmediate;
                case ke2000Constants.SoftwareTrig:
                    return IviDmmTriggerSourceEnum.IviDmmTriggerSourceSwTrigFunc;
                default:
                    return (IviDmmTriggerSourceEnum)Trigger;
            }
        }

        private static IviDmmThermocoupleTypeEnum CastTCType(int TCType)
        {
            switch (TCType)
            {
                case ke2000Constants.ThermoJ:
                    return IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeJ;
                case ke2000Constants.ThermoK:
                    return IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeK;
                case ke2000Constants.ThermoT:
                    return IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeT;
                default:
                    return (IviDmmThermocoupleTypeEnum)TCType;
            }
        }

        private static int CastTCType(IviDmmThermocoupleTypeEnum TCType)
        {
            switch (TCType)
            {
                case IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeJ:
                    return ke2000Constants.ThermoJ;
                case IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeK:
                    return ke2000Constants.ThermoK;
                case IviDmmThermocoupleTypeEnum.IviDmmThermocoupleTypeT:
                    return ke2000Constants.ThermoT;
                default:
                    return (int)TCType;
            }
        }

        private static int CastRefJunctionType(IviDmmRefJunctionTypeEnum juncType)
        {
            switch (juncType)
            {
                case IviDmmRefJunctionTypeEnum.IviDmmRefJunctionTypeFixed:
                    return ke2000Constants.TempRefJuncFixed;
                default:
                    return (int)juncType;
            }
        }

        private static IviDmmRefJunctionTypeEnum CastRefJunctionType(int juncType)
        {
            switch (juncType)
            {
                case ke2000Constants.TempRefJuncFixed:
                    return IviDmmRefJunctionTypeEnum.IviDmmRefJunctionTypeFixed;
                default:
                    return (IviDmmRefJunctionTypeEnum)juncType;
            }
        }

        private static IviDmmTransducerTypeEnum CastTransducerType(int p)
        {
            switch (p)
            {
                case ke2000Constants.Thermocouple:
                    return IviDmmTransducerTypeEnum.IviDmmTransducerTypeThermocouple;
                default:
                    return (IviDmmTransducerTypeEnum)p;
            }
        }

        private static int CastTransducerType(IviDmmTransducerTypeEnum p)
        {
            switch (p)
            {
                case IviDmmTransducerTypeEnum.IviDmmTransducerTypeThermocouple:
                    return ke2000Constants.Thermocouple;
                default:
                    return (int)p;
            }
        }

        private static int CastFunction(IviDmmFunctionEnum Function)
        {
            int function;
            switch (Function)
            {
                case IviDmmFunctionEnum.IviDmmFunction2WireRes:
                    function = ke2000Constants._2WireRes;
                    break;
                case IviDmmFunctionEnum.IviDmmFunction4WireRes:
                    function = ke2000Constants._4WireRes;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionACCurrent:
                    function = ke2000Constants.AcCurrent;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionACVolts:
                    function = ke2000Constants.AcVolts;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionDCCurrent:
                    function = ke2000Constants.DcCurrent;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionDCVolts:
                    function = ke2000Constants.DcVolts;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionFreq:
                    function = ke2000Constants.Freq;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionPeriod:
                    function = ke2000Constants.Period;
                    break;
                case IviDmmFunctionEnum.IviDmmFunctionTemperature:
                    function = ke2000Constants.Temperature;
                    break;
                default:
                    function = (int)Function;
                    break;

            }
            return function;
        }

        private static IviDmmFunctionEnum CastFunction(int Function)
        {
            IviDmmFunctionEnum function;
            switch (Function)
            {
                case ke2000Constants._2WireRes:
                    function = IviDmmFunctionEnum.IviDmmFunction2WireRes;
                    break;
                case ke2000Constants._4WireRes:
                    function = IviDmmFunctionEnum.IviDmmFunction4WireRes;
                    break;
                case ke2000Constants.AcCurrent:
                    function = IviDmmFunctionEnum.IviDmmFunctionACCurrent;
                    break;
                case ke2000Constants.AcVolts:
                    function = IviDmmFunctionEnum.IviDmmFunctionACVolts;
                    break;
                case ke2000Constants.DcCurrent:
                    function = IviDmmFunctionEnum.IviDmmFunctionDCCurrent;
                    break;
                case ke2000Constants.DcVolts:
                    function = IviDmmFunctionEnum.IviDmmFunctionDCVolts;
                    break;
                case ke2000Constants.Freq:
                    function = IviDmmFunctionEnum.IviDmmFunctionFreq;
                    break;
                case ke2000Constants.Period:
                    function = IviDmmFunctionEnum.IviDmmFunctionPeriod;
                    break;
                case ke2000Constants.Temperature:
                    function = IviDmmFunctionEnum.IviDmmFunctionTemperature;
                    break;
                default:
                    function = (IviDmmFunctionEnum)Function;
                    break;
            }
            return function;
        }

        private static IviDmmApertureTimeUnitsEnum CastApertureTimeUnits(int p)
        {
            return (IviDmmApertureTimeUnitsEnum)p;
        }

        private static IviDmmAutoZeroEnum CastAutoZero(int p)
        {
            switch (p)
            {
                case ke2000Constants.AutoZeroOn:
                    return IviDmmAutoZeroEnum.IviDmmAutoZeroOn;
                case ke2000Constants.AutoZeroOff:
                    return IviDmmAutoZeroEnum.IviDmmAutoZeroOff;
                default:
                    return (IviDmmAutoZeroEnum)p;
            }
        }

        private static int CastAutoZero(IviDmmAutoZeroEnum p)
        {
            switch (p)
            {
                case IviDmmAutoZeroEnum.IviDmmAutoZeroOn:
                    return ke2000Constants.AutoZeroOn;
                case IviDmmAutoZeroEnum.IviDmmAutoZeroOff:
                    return ke2000Constants.AutoZeroOff;
                default:
                    return (int)p;
            }
        }

        #endregion

        private class PInvoke
        {

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_init", CallingConvention = CallingConvention.StdCall)]
            public static extern int init(string Resource_Name, ushort ID_Query, ushort Reset_Device, out System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_InitWithOptions", CallingConvention = CallingConvention.StdCall)]
            public static extern int InitWithOptions(string Resource_Name, ushort ID_Query, ushort Reset_Device, string Option_String, out System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMeasurement", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMeasurement(System.IntPtr Instrument_Handle, int Measurement_Function, double Range, double Resolution__absolute_);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureACBandwidth", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureACBandwidth(System.IntPtr Instrument_Handle, double AC_Minimum_Frequency__Hz_, double AC_Maximum_Frequency__Hz_);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureFrequencyVoltageRange", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureFrequencyVoltageRange(System.IntPtr Instrument_Handle, double Voltage_Range__RMS_);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureTransducerType", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureTransducerType(System.IntPtr Instrument_Handle, int Transducer_Type);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureThermocouple", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureThermocouple(System.IntPtr Instrument_Handle, int Thermocouple_Type, int Reference_Junction_Type);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureFixedRefJunction", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureFixedRefJunction(System.IntPtr Instrument_Handle, double Fixed_Reference_Junction);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureRealRefJunction", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureRealRefJunction(System.IntPtr Instrument_Handle, double Coefficent, double Offset);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureTrigger", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureTrigger(System.IntPtr Instrument_Handle, int Trigger_Source, double Trigger_Delay__sec_);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMultiPoint", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMultiPoint(System.IntPtr Instrument_Handle, int Trigger_Count, int Sample_Count, int Sample_Trigger, double Sample_Interval__sec_);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMeasCompleteDest", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMeasCompleteDest(System.IntPtr Instrument_Handle, int Meas_Complete_Destination);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureAutoZeroMode", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureAutoZeroMode(System.IntPtr Instrument_Handle, int Auto_Zero_Mode);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureFilter", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureFilter(System.IntPtr Instrument_Handle, int Filter_Type, int Count, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureHold", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureHold(System.IntPtr Instrument_Handle, double Window, int Count, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureRelative", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureRelative(System.IntPtr Instrument_Handle, double Reference, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMath", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMath(System.IntPtr Instrument_Handle, int Function, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMathMXB", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMathMXB(System.IntPtr Instrument_Handle, double m_Scale_Factor, double b_Offset);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMathPercent", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMathPercent(System.IntPtr Instrument_Handle, double Reference);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureMultiPointStat", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureMultiPointStat(System.IntPtr Instrument_Handle, int Function, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConfigureLimitTest", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConfigureLimitTest(System.IntPtr Instrument_Handle, double Upper_Limit, double Lower_Limit, ushort State);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_AcquireNewRelativeReference", CallingConvention = CallingConvention.StdCall)]
            public static extern int AcquireNewRelativeReference(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_AcquireNewMathPercentReference", CallingConvention = CallingConvention.StdCall)]
            public static extern int AcquireNewMathPercentReference(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAutoRangeValue", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAutoRangeValue(System.IntPtr Instrument_Handle, out double Actual_Range);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetApertureTimeInfo", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetApertureTimeInfo(System.IntPtr Instrument_Handle, out double Aperture_Time, out int Aperture_Time_Units);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_Read", CallingConvention = CallingConvention.StdCall)]
            public static extern int Read(System.IntPtr Instrument_Handle, int Maximum_Time__ms_, out double Reading);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ReadMultiPoint", CallingConvention = CallingConvention.StdCall)]
            public static extern int ReadMultiPoint(System.IntPtr Instrument_Handle, int Maximum_Time__ms_, int Array_Size, [In, Out] double[] Reading_Array, out int Actual_Number_of_Points);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_Initiate", CallingConvention = CallingConvention.StdCall)]
            public static extern int Initiate(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SendSoftwareTrigger", CallingConvention = CallingConvention.StdCall)]
            public static extern int SendSoftwareTrigger(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_Fetch", CallingConvention = CallingConvention.StdCall)]
            public static extern int Fetch(System.IntPtr Instrument_Handle, int Maximum_Time__ms_, out double Reading);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_FetchMultiPoint", CallingConvention = CallingConvention.StdCall)]
            public static extern int FetchMultiPoint(System.IntPtr Instrument_Handle, int Maximum_Time__ms_, int Array_Size, [In, Out] double[] Reading_Array, out int Actual_Number_of_Points);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_Abort", CallingConvention = CallingConvention.StdCall)]
            public static extern int Abort(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_IsOverRange", CallingConvention = CallingConvention.StdCall)]
            public static extern int IsOverRange(System.IntPtr Instrument_Handle, double Measurement_Value, out ushort Is_Over_Range);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_FetchMultiPointStat", CallingConvention = CallingConvention.StdCall)]
            public static extern int FetchMultiPointStat(System.IntPtr Instrument_Handle, int Maximum_Time__ms_, out double Data);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_FetchLimitTestData", CallingConvention = CallingConvention.StdCall)]
            public static extern int FetchLimitTestData(System.IntPtr Instrument_Handle, out ushort Failed_Upper_Limit, out ushort Failed_Lower_Limit);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ConvertTemperature", CallingConvention = CallingConvention.StdCall)]
            public static extern int ConvertTemperature(System.IntPtr Instrument_Handle, int Temperature_Type, out double Temperature);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_self_test", CallingConvention = CallingConvention.StdCall)]
            public static extern int self_test(System.IntPtr Instrument_Handle, out short Self_Test_Result, System.Text.StringBuilder Self_Test_Message);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_reset", CallingConvention = CallingConvention.StdCall)]
            public static extern int reset(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ResetWithDefaults", CallingConvention = CallingConvention.StdCall)]
            public static extern int ResetWithDefaults(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_Disable", CallingConvention = CallingConvention.StdCall)]
            public static extern int Disable(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_revision_query", CallingConvention = CallingConvention.StdCall)]
            public static extern int revision_query(System.IntPtr Instrument_Handle, System.Text.StringBuilder Instrument_Driver_Revision, System.Text.StringBuilder Firmware_Revision);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_error_query", CallingConvention = CallingConvention.StdCall)]
            public static extern int error_query(System.IntPtr Instrument_Handle, out int Error_Code, System.Text.StringBuilder Error_Message);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_InvalidateAllAttributes", CallingConvention = CallingConvention.StdCall)]
            public static extern int InvalidateAllAttributes(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetChannelName", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetChannelName(System.IntPtr Instrument_Handle, int Index, int Buffer_Size, System.Text.StringBuilder Channel_Name);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetNextCoercionRecord", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetNextCoercionRecord(System.IntPtr Instrument_Handle, int Buffer_Size, System.Text.StringBuilder Coercion_Record);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetNextInterchangeWarning", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetNextInterchangeWarning(System.IntPtr Instrument_Handle, int Buffer_Size, System.Text.StringBuilder Interchange_Warning);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ClearInterchangeWarnings", CallingConvention = CallingConvention.StdCall)]
            public static extern int ClearInterchangeWarnings(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ResetInterchangeCheck", CallingConvention = CallingConvention.StdCall)]
            public static extern int ResetInterchangeCheck(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_WriteInstrData", CallingConvention = CallingConvention.StdCall)]
            public static extern int WriteInstrData(System.IntPtr Instrument_Handle, string Write_Buffer);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_ReadInstrData", CallingConvention = CallingConvention.StdCall)]
            public static extern int ReadInstrData(System.IntPtr Instrument_Handle, int Number_of_Bytes_To_Read, System.Text.StringBuilder Read_Buffer, out int Num_Bytes_Read);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_close", CallingConvention = CallingConvention.StdCall)]
            public static extern int close(System.IntPtr Instrument_Handle);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_error_message", CallingConvention = CallingConvention.StdCall)]
            public static extern int error_message(System.IntPtr Instrument_Handle, int Error_Code, System.Text.StringBuilder Error_Message_2);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAttributeViInt32", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAttributeViInt32(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, out int Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAttributeViReal64", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAttributeViReal64(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, out double Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAttributeViString", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAttributeViString(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, int Array_Size, System.Text.StringBuilder Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAttributeViBoolean", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAttributeViBoolean(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, out ushort Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetAttributeViSession", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetAttributeViSession(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, out System.IntPtr Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SetAttributeViInt32", CallingConvention = CallingConvention.StdCall)]
            public static extern int SetAttributeViInt32(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, int Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SetAttributeViReal64", CallingConvention = CallingConvention.StdCall)]
            public static extern int SetAttributeViReal64(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, double Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SetAttributeViString", CallingConvention = CallingConvention.StdCall)]
            public static extern int SetAttributeViString(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, string Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SetAttributeViBoolean", CallingConvention = CallingConvention.StdCall)]
            public static extern int SetAttributeViBoolean(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, ushort Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_SetAttributeViSession", CallingConvention = CallingConvention.StdCall)]
            public static extern int SetAttributeViSession(System.IntPtr Instrument_Handle, string Channel_Name, int Attribute_ID, System.IntPtr Attribute_Value);

            [DllImport("ke2000_32.dll", EntryPoint = "ke2000_GetError", CallingConvention = CallingConvention.StdCall)]
            public static extern int GetError(System.IntPtr Instrument_Handle, out int Code, int BufferSize, System.Text.StringBuilder Description);


            public static int TestForError(System.IntPtr handle, int status)
            {
                if ((status < 0))
                {
                    PInvoke.ThrowError(handle, status);
                }
                return status;
            }

            public static int ThrowError(System.IntPtr handle, int code)
            {
                int status;
                int size = PInvoke.GetError(handle, out status, 0, null);
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                if ((size >= 0))
                {
                    msg.Capacity = size;
                    PInvoke.GetError(handle, out status, size, msg);
                }
                throw new System.Runtime.InteropServices.ExternalException(msg.ToString(), code);
            }
        }

    }

    public class ke2000Constants
    {

        public const int DcVolts = 1;

        public const int AcVolts = 2;

        public const int DcCurrent = 3;

        public const int AcCurrent = 4;

        public const int _2WireRes = 5;

        public const int _4WireRes = 101;

        public const int Temperature = 108;

        public const int Freq = 104;

        public const int Period = 105;

        public const int Diode = 1003;

        public const int Continuity = 1004;

        public const int Thermocouple = 1;

        public const int ThermoJ = 6;

        public const int ThermoK = 7;

        public const int ThermoT = 11;

        public const int TempRefJuncFixed = 2;

        public const int ThermoReal = 0;

        public const int Immediate = 1;

        public const int External = 2;

        public const int SoftwareTrig = 3;

        public const int Timer = 1001;

        public const int Interval = 10;

        public const int AutoZeroOn = 1;

        public const int AutoZeroOff = 0;

        public const int FilterMoving = 1;

        public const int FilterRepeating = 2;

        public const int MathNone = 1;

        public const int MathMxb = 2;

        public const int MathPercent = 3;

        public const int StatNone = 1;

        public const int StatMean = 2;

        public const int StatSdev = 3;

        public const int StatMax = 4;

        public const int StatMin = 5;

        public const int TempTypeF = 1;

        public const int TempTypeK = 2;

        public const double _10000 = 10000;

        public const double _1000 = 1000;

        public const double _100 = 100;

        public const double _10 = 10;

        public const double _1 = 1;

        public const double _0_1 = 0.1;

        public const double _0_01 = 0.01;

        public const double _0_001 = 0.001;

        public const double _0_0001 = 0.0001;

        public const double _0_00001 = 1E-05;

        public const double _0_000001 = 1E-06;

        public const double _0_0000001 = 1E-07;

        public const double _0_00000001 = 1E-08;

        public const double _0_000000001 = 1E-09;

        public const double _300_0 = 300;

        public const double _30_0 = 30;

        public const double _3_0 = 3;

        public const int ThermoSimulated = 1;

        public const int UnitsV = 1;

        public const int UnitsDb = 2;

        public const int UnitsDbm = 3;

        public const double _1_0 = 1;

        public const double _10_0 = 10;

        public const double _35Digits = 3;

        public const double _45Digits = 4;

        public const double _55Digits = 5;

        public const double _65Digits = 6;
    }

    public enum ke2000Properties
    {

        /// <summary>
        /// System.Boolean
        /// </summary>
        RangeCheck = 1050002,

        /// <summary>
        /// System.Boolean
        /// </summary>
        QueryInstrumentStatus = 1050003,

        /// <summary>
        /// System.Boolean
        /// </summary>
        Cache = 1050004,

        /// <summary>
        /// System.Boolean
        /// </summary>
        Simulate = 1050005,

        /// <summary>
        /// System.Boolean
        /// </summary>
        RecordCoercions = 1050006,

        /// <summary>
        /// System.Boolean
        /// </summary>
        InterchangeCheck = 1050021,

        /// <summary>
        /// System.String
        /// </summary>
        SpecificDriverDescription = 1050514,

        /// <summary>
        /// System.String
        /// </summary>
        SpecificDriverPrefix = 1050302,

        /// <summary>
        /// System.String
        /// </summary>
        SpecificDriverVendor = 1050513,

        /// <summary>
        /// System.String
        /// </summary>
        SpecificDriverRevision = 1050551,

        /// <summary>
        /// System.Int32
        /// </summary>
        SpecificDriverClassSpecMajorVersion = 1050515,

        /// <summary>
        /// System.Int32
        /// </summary>
        SpecificDriverClassSpecMinorVersion = 1050516,

        /// <summary>
        /// System.String
        /// </summary>
        SupportedInstrumentModels = 1050327,

        /// <summary>
        /// System.String
        /// </summary>
        GroupCapabilities = 1050401,

        /// <summary>
        /// System.Int32
        /// </summary>
        ChannelCount = 1050203,

        /// <summary>
        /// System.String
        /// </summary>
        InstrumentManufacturer = 1050511,

        /// <summary>
        /// System.String
        /// </summary>
        InstrumentModel = 1050512,

        /// <summary>
        /// System.String
        /// </summary>
        InstrumentFirmwareRevision = 1050510,

        /// <summary>
        /// System.String
        /// </summary>
        DriverSetup = 1050007,

        /// <summary>
        /// System.String
        /// </summary>
        LogicalName = 1050305,

        /// <summary>
        /// System.String
        /// </summary>
        IoResourceDescriptor = 1050304,

        /// <summary>
        /// System.Int32
        /// </summary>
        Function = 1250001,

        /// <summary>
        /// System.Double
        /// </summary>
        Range = 1250002,

        /// <summary>
        /// System.Double
        /// </summary>
        ResolutionAbsolute = 1250008,

        /// <summary>
        /// System.Int32
        /// </summary>
        TriggerSource = 1250004,

        /// <summary>
        /// System.Double
        /// </summary>
        TriggerDelay = 1250005,

        /// <summary>
        /// System.Int32
        /// </summary>
        TriggerCount = 1250304,

        /// <summary>
        /// System.Int32
        /// </summary>
        SampleCount = 1250301,

        /// <summary>
        /// System.Int32
        /// </summary>
        SampleTrigger = 1250302,

        /// <summary>
        /// System.Double
        /// </summary>
        SampleInterval = 1250303,

        /// <summary>
        /// System.Int32
        /// </summary>
        MeasCompleteDest = 1250305,

        /// <summary>
        /// System.Double
        /// </summary>
        TimerInterval = 1150005,

        /// <summary>
        /// System.Double
        /// </summary>
        AcMinFreq = 1250006,

        /// <summary>
        /// System.Double
        /// </summary>
        AcMaxFreq = 1250007,

        /// <summary>
        /// System.Double
        /// </summary>
        FreqVoltageRange = 1250101,

        /// <summary>
        /// System.Int32
        /// </summary>
        TempTransducerType = 1250201,

        /// <summary>
        /// System.Int32
        /// </summary>
        TempTcType = 1250231,

        /// <summary>
        /// System.Int32
        /// </summary>
        TempTcRefJuncType = 1250232,

        /// <summary>
        /// System.Double
        /// </summary>
        TempTcFixedRefJunc = 1250233,

        /// <summary>
        /// System.Int32
        /// </summary>
        ThermoRefJunction = 1150015,

        /// <summary>
        /// System.Double
        /// </summary>
        ThermoRealCoefficient = 1150017,

        /// <summary>
        /// System.Double
        /// </summary>
        ThermoRealOffset = 1150018,

        /// <summary>
        /// System.Int32
        /// </summary>
        AutoZero = 1250332,

        /// <summary>
        /// System.Int32
        /// </summary>
        DcVoltsUnits = 1150034,

        /// <summary>
        /// System.Double
        /// </summary>
        DcVoltsDbmImpedance = 1150032,

        /// <summary>
        /// System.Double
        /// </summary>
        DcVoltsDbReference = 1150031,

        /// <summary>
        /// System.Int32
        /// </summary>
        AcVoltsUnits = 1150033,

        /// <summary>
        /// System.Double
        /// </summary>
        AcVoltsDbmImpedance = 1150030,

        /// <summary>
        /// System.Double
        /// </summary>
        AcVoltsDbReference = 1150029,

        /// <summary>
        /// System.Boolean
        /// </summary>
        FilterEnable = 1150011,

        /// <summary>
        /// System.Int32
        /// </summary>
        FilterType = 1150012,

        /// <summary>
        /// System.Int32
        /// </summary>
        FilterCount = 1150013,

        /// <summary>
        /// System.Boolean
        /// </summary>
        HoldEnable = 1150008,

        /// <summary>
        /// System.Double
        /// </summary>
        HoldWindow = 1150009,

        /// <summary>
        /// System.Int32
        /// </summary>
        HoldCount = 1150010,

        /// <summary>
        /// System.Boolean
        /// </summary>
        RelativeEnable = 1150019,

        /// <summary>
        /// System.Double
        /// </summary>
        RelativeReference = 1150020,

        /// <summary>
        /// System.Boolean
        /// </summary>
        MathEnable = 1150022,

        /// <summary>
        /// System.Int32
        /// </summary>
        MathFunction = 1150021,

        /// <summary>
        /// System.Double
        /// </summary>
        MxbMFactor = 1150023,

        /// <summary>
        /// System.Double
        /// </summary>
        MxbBOffset = 1150024,

        /// <summary>
        /// System.Double
        /// </summary>
        PercentReference = 1150026,

        /// <summary>
        /// System.Boolean
        /// </summary>
        StatEnable = 1150028,

        /// <summary>
        /// System.Int32
        /// </summary>
        StatFunction = 1150027,

        /// <summary>
        /// System.Boolean
        /// </summary>
        LimitEnable = 1150035,

        /// <summary>
        /// System.Double
        /// </summary>
        LimitUpper = 1150036,

        /// <summary>
        /// System.Double
        /// </summary>
        LimitLower = 1150037,

        /// <summary>
        /// System.Double
        /// </summary>
        AutoRangeValue = 1250331,

        /// <summary>
        /// System.Double
        /// </summary>
        ApertureTime = 1250321,

        /// <summary>
        /// System.Int32
        /// </summary>
        ApertureTimeUnits = 1250322,

        /// <summary>
        /// System.String
        /// </summary>
        IdQueryResponse = 1150001,

        /// <summary>
        /// System.Double
        /// </summary>
        FreqThresholdVoltage = 1150003,

        /// <summary>
        /// System.Double
        /// </summary>
        PeriodThresholdVoltage = 1150004,

        /// <summary>
        /// System.Double
        /// </summary>
        DiodeTestCurrent = 1150002,

        /// <summary>
        /// System.Boolean
        /// </summary>
        BeepEnable = 1150007,

        /// <summary>
        /// System.Boolean
        /// </summary>
        DisplayEnable = 1150006,

        /// <summary>
        /// System.Double
        /// </summary>
        Resolution = 1250003,
    }
}
