/*---------------------------------------------------------------------------*/
/* Distributed by VXIplug&play Systems Alliance.                             */
/* Contains National Instruments extensions.                                 */
/* Do not modify the contents of this file.                                  */
/*---------------------------------------------------------------------------*/
/*                                                                           */
/* Title   : VISA.H                                                          */
/* Date    : 11-19-1999                                                      */
/* Purpose : Include file for the VISA Library 2.2 specification             */
/*                                                                           */
/*---------------------------------------------------------------------------*/
/* When using NI-VISA extensions, you must link with the VISA library that   */
/* comes with NI-VISA.  Currently, the extensions provided by NI-VISA are    */
/* for PXI (Compact PCI eXtensions for Instrumentation) and a fast set of    */
/* macros for viPeekXX/viPokeXX that guarantees binary compatibility with    */
/* other implementations of VISA.  To use these features you must define the */
/* macro NIVISA_PXI or NIVISA_PEEKPOKE before including this header.         */
/*---------------------------------------------------------------------------*/
/* Contains modifications by Shawn Aikey                                     */
/*---------------------------------------------------------------------------*/
/*                                                                           */
/* Title		: Visa.cs                                                        */
/* Date			: 2002.05.21                                                     */
/* Purpose	: This file now works with the C# Language from MS(TM)           */
/*                                                                           */
/*---------------------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;

namespace VISA2_2
{
	public delegate System.UInt32 viEventHandler(System.UInt32 vi, System.UInt32 eventType, System.UInt32 viEvent, System.UInt32 userHandle);
	
	public class VISA
	{
#region Basic Definitions
		public const System.Int32 _VI_ERROR = -2147483647 - 1; /*(0x80000000)*/
		// Completion and Error Codes
		public const System.Int32 VI_SUCCESS = 0;
		// Other VISA Definitions
		public const System.Int32 VI_NULL = 0;
		public const System.Int32 VI_TRUE = 1;
		public const System.Int32 VI_FALSE = 0;
		
#endregion

#region Resource Manager Functions and Operations

[DllImport("visa32.dll")] public static extern 
		System.Int32 viOpenDefaultRM (ref System.UInt32 vi);
[DllImport("visa32.dll")] public static extern 
		System.Int32 viFindRsrc      (System.UInt32 sesn, System.String expr, ref System.UInt32 vi,
                                  ref System.UInt32 retCnt, System.String desc);
[DllImport("visa32.dll")] public static extern
		System.Int32 viFindNext      (System.UInt32 vi, System.String desc);
[DllImport("visa32.dll")] public static extern
		System.Int32 viParseRsrc		 (System.UInt32 rmSesn, System.String rsrcName,
																	ref System.UInt16 intfType, ref System.UInt16 intfNum);
[DllImport("visa32.dll")] public static extern
		System.Int32 viOpen          (System.UInt32 sesn, System.String name, System.UInt32 mode,
                                  System.UInt32 timeout, ref System.UInt32 vi);
#endregion

#region Resource Template Operations

[DllImport("visa32.dll")] public static extern
		System.Int32  viClose         (System.UInt32 vi);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.UInt16 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.UInt32 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.UInt64 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.Int16 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.Int32 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.Int64 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.Single attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.Double attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viSetAttribute  (System.UInt32 vi, System.UInt32 attrName, System.String attrValue);

[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.Single attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.Double attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, [MarshalAs(UnmanagedType.VBByRefStr)] ref System.String attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.Int16 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.Int32 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.Int64 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.UInt16 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.UInt32 attrValue);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGetAttribute  (System.UInt32 vi, System.UInt32 attrName, ref System.UInt64 attrValue);

[DllImport("visa32.dll")] public static extern
		System.Int32  viStatusDesc    (System.UInt32 vi, System.UInt32 status,[MarshalAs(UnmanagedType.VBByRefStr)] ref  System.String desc);

[DllImport("visa32.dll")] public static extern
		System.Int32  viLock          (System.UInt32 vi, System.UInt32 lockType, System.UInt32 timeout,
                                   System.String requestedKey, System.String accessKey);
[DllImport("visa32.dll")] public static extern
		System.Int32  viUnlock        (System.UInt32 vi);
[DllImport("visa32.dll")] public static extern 
		System.Int32  viEnableEvent   (System.UInt32 vi, System.UInt32 eventType, System.UInt16 mechanism,
                                   System.UInt32 context);
[DllImport("visa32.dll")] public static extern
		System.Int32  viDisableEvent  (System.UInt32 vi, System.UInt32 eventType, System.UInt16 mechanism);
[DllImport("visa32.dll")] public static extern
		System.Int32  viDiscardEvents (System.UInt32 vi, System.UInt32 eventType, System.UInt16 mechanism);
[DllImport("visa32.dll")] public static extern
		System.Int32  viWaitOnEvent   (System.UInt32 vi, System.UInt32 inEventType, System.UInt32 timeout,
                                   ref System.UInt32 outEventType, System.UInt32 outContext);
[DllImport("visa32.dll")] public static extern
		System.Int32  viInstallHandler(System.UInt32 vi, System.UInt32 eventType, viEventHandler handler,
                                   System.UInt32 userHandle);
[DllImport("visa32.dll")] public static extern
		System.Int32  viUninstallHandler(System.UInt32 vi, System.UInt32 eventType, viEventHandler handler,
                                     System.UInt32 userHandle);
#endregion

#region Basic I/O Operations

[DllImport("visa32.dll")] public static extern
		System.Int32  viRead          (System.UInt32 vi, [MarshalAs(UnmanagedType.VBByRefStr)] ref System.String buf, System.UInt32 cnt, ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viReadToFile    (System.UInt32 vi, [MarshalAs(UnmanagedType.VBByRefStr)] ref System.String filename, System.UInt32 cnt,
                                   ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viWrite         (System.UInt32 vi, System.String  buf, System.UInt32 cnt, ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viWriteFromFile (System.UInt32 vi, System.String filename, System.UInt32 cnt,
                                   ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viAssertTrigger (System.UInt32 vi, System.UInt16 protocol);
[DllImport("visa32.dll")] public static extern
		System.Int32  viReadSTB       (System.UInt32 vi, ref System.UInt16 status);
[DllImport("visa32.dll")] public static extern
		System.Int32  viClear         (System.UInt32 vi);

#endregion

#region Formatted and Buffered I/O Operations

[DllImport("visa32.dll")] public static extern
		System.Int32  viSetBuf        (System.UInt32 vi, System.UInt16 mask, System.UInt32 size);
[DllImport("visa32.dll")] public static extern
		System.Int32  viFlush         (System.UInt32 vi, System.UInt16 mask);

[DllImport("visa32.dll")] public static extern
		System.Int32  viBufWrite      (System.UInt32 vi, System.String  buf, System.UInt32 cnt, ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viBufRead       (System.UInt32 vi, System.String buf, System.UInt32 cnt, ref System.UInt32 retCnt);

[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf       (System.UInt32 vi, System.String writeFmt, System.String arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf			 (System.UInt32 vi, System.String writeFmt, System.Single arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf       (System.UInt32 vi, System.String writeFmt, System.Double arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf			 (System.UInt32 vi, System.String writeFmt, System.Int16 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf       (System.UInt32 vi, System.String writeFmt, System.Int32 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viPrintf			 (System.UInt32 vi, System.String writeFmt, System.Int64 arg);
		
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, ref System.Int16 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, ref System.Int32 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, ref System.Int64 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, ref System.Single arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, ref System.Double arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viScanf        (System.UInt32 vi, System.String readFmt, [MarshalAs(UnmanagedType.VBByRefStr)] ref System.String arg);

[DllImport("visa32.dll")] public static extern
		System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	ref System.Int16 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	ref System.Int32 arg);
[DllImport("visa32.dll")] public static extern
		System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	ref System.Int64 arg);
[DllImport("visa32.dll")] public static extern
    System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	ref System.Single arg);
[DllImport("visa32.dll")] public static extern
    System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	 ref System.Double arg);
[DllImport("visa32.dll")] public static extern
	  System.Int32  viQueryf       (System.UInt32 vi, System.String writeFmt, System.String readFmt, 
																	 [MarshalAs(UnmanagedType.VBByRefStr)] ref System.String arg);
#endregion

#region Interface Specific Operations

[DllImport("visa32.dll")] public static extern
		System.Int32  viGpibControlREN(System.UInt32 vi, System.UInt16 mode);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGpibControlATN(System.UInt32 vi, System.UInt16 mode);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGpibSendIFC   (System.UInt32 vi);
[DllImport("visa32.dll")] public static extern
	System.Int32  viGpibCommand   (System.UInt32 vi, System.String cmd, System.UInt32 cnt, ref System.UInt32 retCnt);
[DllImport("visa32.dll")] public static extern
		System.Int32  viGpibPassControl(System.UInt32 vi, System.UInt16 primAddr, System.UInt16 secAddr);

[DllImport("visa32.dll")] public static extern
		System.Int32  viVxiCommandQuery(System.UInt32 vi, System.UInt16 mode, System.UInt32 cmd,
                                    ref System.UInt32 response);
[DllImport("visa32.dll")] public static extern
		System.Int32  viAssertUtilSignal(System.UInt32 vi, System.UInt16 line);
[DllImport("visa32.dll")] public static extern
		System.Int32  viAssertIntrSignal(System.UInt32 vi, System.Int16 mode, System.UInt32 statusID);
[DllImport("visa32.dll")] public static extern
		System.Int32  viMapTrigger    (System.UInt32 vi, System.Int16 trigSrc, System.Int16 trigDest, 
                                   System.UInt16 mode);
[DllImport("visa32.dll")] public static extern
		System.Int32  viUnmapTrigger  (System.UInt32 vi, System.Int16 trigSrc, System.Int16 trigDest);
#endregion

#region Attributes

public const System.UInt32 VI_ATTR_RSRC_CLASS  = (0xBFFF0001);
public const System.UInt32 VI_ATTR_RSRC_NAME  = (0xBFFF0002);
public const System.UInt32 VI_ATTR_RSRC_IMPL_VERSION  = (0x3FFF0003);
public const System.UInt32 VI_ATTR_RSRC_LOCK_STATE  = (0x3FFF0004);
public const System.UInt32 VI_ATTR_MAX_QUEUE_LENGTH  = (0x3FFF0005);
public const System.UInt32 VI_ATTR_USER_DATA  = (0x3FFF0007);
public const System.UInt32 VI_ATTR_FDC_CHNL  = (0x3FFF000D);
public const System.UInt32 VI_ATTR_FDC_MODE  = (0x3FFF000F);
public const System.UInt32 VI_ATTR_FDC_GEN_SIGNAL_EN  = (0x3FFF0011);
public const System.UInt32 VI_ATTR_FDC_USE_PAIR  = (0x3FFF0013);
public const System.UInt32 VI_ATTR_SEND_END_EN  = (0x3FFF0016);
public const System.UInt32 VI_ATTR_TERMCHAR  = (0x3FFF0018);
public const System.UInt32 VI_ATTR_TMO_VALUE  = (0x3FFF001A);
public const System.UInt32 VI_ATTR_GPIB_READDR_EN  = (0x3FFF001B);
public const System.UInt32 VI_ATTR_IO_PROT  = (0x3FFF001C);
public const System.UInt32 VI_ATTR_DMA_ALLOW_EN  = (0x3FFF001E);
public const System.UInt32 VI_ATTR_ASRL_BAUD  = (0x3FFF0021);
public const System.UInt32 VI_ATTR_ASRL_DATA_BITS  = (0x3FFF0022);
public const System.UInt32 VI_ATTR_ASRL_PARITY  = (0x3FFF0023);
public const System.UInt32 VI_ATTR_ASRL_STOP_BITS  = (0x3FFF0024);
public const System.UInt32 VI_ATTR_ASRL_FLOW_CNTRL  = (0x3FFF0025);
public const System.UInt32 VI_ATTR_RD_BUF_OPER_MODE  = (0x3FFF002A);
public const System.UInt32 VI_ATTR_WR_BUF_OPER_MODE  = (0x3FFF002D);
public const System.UInt32  VI_ATTR_SUPPRESS_END_EN  = (0x3FFF0036);
public const System.UInt32 VI_ATTR_TERMCHAR_EN  = (0x3FFF0038);
public const System.UInt32 VI_ATTR_DEST_ACCESS_PRIV  = (0x3FFF0039);
public const System.UInt32 VI_ATTR_DEST_BYTE_ORDER  = (0x3FFF003A);
public const System.UInt32 VI_ATTR_SRC_ACCESS_PRIV  = (0x3FFF003C);
public const System.UInt32 VI_ATTR_SRC_BYTE_ORDER  = (0x3FFF003D);
public const System.UInt32 VI_ATTR_SRC_INCREMENT  = (0x3FFF0040);
public const System.UInt32 VI_ATTR_DEST_INCREMENT  = (0x3FFF0041);
public const System.UInt32 VI_ATTR_WIN_ACCESS_PRIV  = (0x3FFF0045);
public const System.UInt32 VI_ATTR_WIN_BYTE_ORDER  = (0x3FFF0047);
public const System.UInt32 VI_ATTR_GPIB_ATN_STATE  = (0x3FFF0057);
public const System.UInt32 VI_ATTR_GPIB_ADDR_STATE  = (0x3FFF005C);
public const System.UInt32 VI_ATTR_GPIB_CIC_STATE  = (0x3FFF005E);
public const System.UInt32 VI_ATTR_GPIB_NDAC_STATE  = (0x3FFF0062);
public const System.UInt32 VI_ATTR_GPIB_SRQ_STATE  = (0x3FFF0067);
public const System.UInt32 VI_ATTR_GPIB_SYS_CNTRL_STATE = (0x3FFF0068);
public const System.UInt32 VI_ATTR_GPIB_HS488_CBL_LEN  = (0x3FFF0069);
public const System.UInt32 VI_ATTR_CMDR_LA  = (0x3FFF006B);
public const System.UInt32 VI_ATTR_VXI_DEV_CLASS  = (0x3FFF006C);
public const System.UInt32 VI_ATTR_MAINFRAME_LA  = (0x3FFF0070);
public const System.UInt32 VI_ATTR_MANF_NAME  = (0xBFFF0072);
public const System.UInt32 VI_ATTR_MODEL_NAME  = (0xBFFF0077);
public const System.UInt32 VI_ATTR_VXI_VME_INTR_STATUS = (0x3FFF008B);
public const System.UInt32 VI_ATTR_VXI_TRIG_STATUS  = (0x3FFF008D);
public const System.UInt32 VI_ATTR_VXI_VME_SYSFAIL_STATE = (0x3FFF0094);
public const System.UInt32 VI_ATTR_WIN_BASE_ADDR  = (0x3FFF0098);
public const System.UInt32 VI_ATTR_WIN_SIZE  = (0x3FFF009A);
public const System.UInt32 VI_ATTR_ASRL_AVAIL_NUM  = (0x3FFF00AC);
public const System.UInt32 VI_ATTR_MEM_BASE  = (0x3FFF00AD);
public const System.UInt32 VI_ATTR_ASRL_CTS_STATE  = (0x3FFF00AE);
public const System.UInt32 VI_ATTR_ASRL_DCD_STATE  = (0x3FFF00AF);
public const System.UInt32 VI_ATTR_ASRL_DSR_STATE  = (0x3FFF00B1);
public const System.UInt32 VI_ATTR_ASRL_DTR_STATE  = (0x3FFF00B2);
public const System.UInt32 VI_ATTR_ASRL_END_IN  = (0x3FFF00B3);
public const System.UInt32 VI_ATTR_ASRL_END_OUT  = (0x3FFF00B4);
public const System.UInt32 VI_ATTR_ASRL_REPLACE_CHAR  = (0x3FFF00BE);
public const System.UInt32 VI_ATTR_ASRL_RI_STATE  = (0x3FFF00BF);
public const System.UInt32 VI_ATTR_ASRL_RTS_STATE  = (0x3FFF00C0);
public const System.UInt32 VI_ATTR_ASRL_XON_CHAR  = (0x3FFF00C1);
public const System.UInt32 VI_ATTR_ASRL_XOFF_CHAR  = (0x3FFF00C2);
public const System.UInt32 VI_ATTR_WIN_ACCESS  = (0x3FFF00C3);
public const System.UInt32 VI_ATTR_RM_SESSION  = (0x3FFF00C4);
public const System.UInt32 VI_ATTR_VXI_LA  = (0x3FFF00D5);
public const System.UInt32 VI_ATTR_MANF_ID  = (0x3FFF00D9);
public const System.UInt32 VI_ATTR_MEM_SIZE  = (0x3FFF00DD);
public const System.UInt32 VI_ATTR_MEM_SPACE  = (0x3FFF00DE);
public const System.UInt32 VI_ATTR_MODEL_CODE  = (0x3FFF00DF);
public const System.UInt32 VI_ATTR_SLOT  = (0x3FFF00E8);
public const System.UInt32 VI_ATTR_INTF_INST_NAME  = (0xBFFF00E9);
public const System.UInt32 VI_ATTR_IMMEDIATE_SERV  = (0x3FFF0100);
public const System.UInt32 VI_ATTR_INTF_PARENT_NUM  = (0x3FFF0101);
public const System.UInt32 VI_ATTR_RSRC_SPEC_VERSION  = (0x3FFF0170);
public const System.UInt32 VI_ATTR_INTF_TYPE  = (0x3FFF0171);
public const System.UInt32 VI_ATTR_GPIB_PRIMARY_ADDR  = (0x3FFF0172);
public const System.UInt32 VI_ATTR_GPIB_SECONDARY_ADDR = (0x3FFF0173);
public const System.UInt32 VI_ATTR_RSRC_MANF_NAME  = (0xBFFF0174);
public const System.UInt32 VI_ATTR_RSRC_MANF_ID  = (0x3FFF0175);
public const System.UInt32 VI_ATTR_INTF_NUM  = (0x3FFF0176);
public const System.UInt32 VI_ATTR_TRIG_ID  = (0x3FFF0177);
public const System.UInt32 VI_ATTR_GPIB_REN_STATE  = (0x3FFF0181);
public const System.UInt32 VI_ATTR_GPIB_UNADDR_EN  = (0x3FFF0184);
public const System.UInt32 VI_ATTR_DEV_STATUS_BYTE  = (0x3FFF0189);
public const System.UInt32 VI_ATTR_FILE_APPEND_EN  = (0x3FFF0192);
public const System.UInt32 VI_ATTR_VXI_TRIG_SUPPORT  = (0x3FFF0194);
public const System.UInt32 VI_ATTR_TCPIP_ADDR  = (0xBFFF0195);
public const System.UInt32 VI_ATTR_TCPIP_HOSTNAME  = (0xBFFF0196);
public const System.UInt32 VI_ATTR_TCPIP_PORT  = (0x3FFF0197);
public const System.UInt32 VI_ATTR_TCPIP_DEVICE_NAME  = (0xBFFF0199);
public const System.UInt32 VI_ATTR_TCPIP_NODELAY  = (0x3FFF019A);
public const System.UInt32 VI_ATTR_TCPIP_KEEPALIVE  = (0x3FFF019B);
public const System.UInt32 VI_ATTR_JOB_ID  = (0x3FFF4006);
public const System.UInt32 VI_ATTR_EVENT_TYPE  = (0x3FFF4010);
public const System.UInt32 VI_ATTR_SIGP_STATUS_ID  = (0x3FFF4011);
public const System.UInt32 VI_ATTR_RECV_TRIG_ID  = (0x3FFF4012);
public const System.UInt32 VI_ATTR_INTR_STATUS_ID  = (0x3FFF4023);
public const System.UInt32 VI_ATTR_STATUS  = (0x3FFF4025);
public const System.UInt32 VI_ATTR_RET_COUNT  = (0x3FFF4026);
public const System.UInt32 VI_ATTR_BUFFER  = (0x3FFF4027);
public const System.UInt32 VI_ATTR_RECV_INTR_LEVEL  = (0x3FFF4041);
public const System.UInt32 VI_ATTR_OPER_NAME  = (0xBFFF4042);
public const System.UInt32 VI_ATTR_GPIB_RECV_CIC_STATE = (0x3FFF4193);
public const System.UInt32 VI_ATTR_RECV_TCPIP_ADDR  = (0xBFFF4198);

#endregion

#region Event Types

public const System.UInt32 VI_EVENT_IO_COMPLETION  = (0x3FFF2009);
public const System.UInt32 VI_EVENT_TRIG  = (0xBFFF200A);
public const System.UInt32 VI_EVENT_SERVICE_REQ  = (0x3FFF200B);
public const System.UInt32 VI_EVENT_CLEAR  = (0x3FFF200D);
public const System.UInt32 VI_EVENT_EXCEPTION  = (0xBFFF200E);
public const System.UInt32 VI_EVENT_GPIB_CIC  = (0x3FFF2012);
public const System.UInt32 VI_EVENT_GPIB_TALK  = (0x3FFF2013);
public const System.UInt32 VI_EVENT_GPIB_LISTEN  = (0x3FFF2014);
public const System.UInt32 VI_EVENT_VXI_VME_SYSFAIL  = (0x3FFF201D);
public const System.UInt32 VI_EVENT_VXI_VME_SYSRESET  = (0x3FFF201E);
public const System.UInt32 VI_EVENT_VXI_SIGP  = (0x3FFF2020);
public const System.UInt32 VI_EVENT_VXI_VME_INTR  = (0xBFFF2021);
public const System.UInt32 VI_EVENT_TCPIP_CONNECT  = (0x3FFF2036);
public const System.UInt32 VI_ALL_ENABLED_EVENTS  = (0x3FFF7FFF);

#endregion

#region Completion and Error Codes

public const System.Int32 VI_SUCCESS_EVENT_EN  = (0x3FFF0002); /* 3FFF0002,  1073676290 */
public const System.Int32 VI_SUCCESS_EVENT_DIS  = (0x3FFF0003); /* 3FFF0003,  1073676291 */
public const System.Int32 VI_SUCCESS_QUEUE_EMPTY  = (0x3FFF0004); /* 3FFF0004,  1073676292 */
public const System.Int32 VI_SUCCESS_TERM_CHAR  = (0x3FFF0005); /* 3FFF0005,  1073676293 */
public const System.Int32 VI_SUCCESS_MAX_CNT  = (0x3FFF0006); /* 3FFF0006,  1073676294 */
public const System.Int32 VI_SUCCESS_DEV_NPRESENT  = (0x3FFF007D); /* 3FFF007D,  1073676413 */
public const System.Int32 VI_SUCCESS_TRIG_MAPPED  = (0x3FFF007E); /* 3FFF007E,  1073676414 */
public const System.Int32 VI_SUCCESS_QUEUE_NEMPTY  = (0x3FFF0080); /* 3FFF0080,  1073676416 */
public const System.Int32 VI_SUCCESS_NCHAIN  = (0x3FFF0098); /* 3FFF0098,  1073676440 */
public const System.Int32 VI_SUCCESS_NESTED_SHARED  = (0x3FFF0099); /* 3FFF0099,  1073676441 */
public const System.Int32 VI_SUCCESS_NESTED_EXCLUSIVE  = (0x3FFF009A); /* 3FFF009A,  1073676442 */
public const System.Int32 VI_SUCCESS_SYNC  = (0x3FFF009B); /* 3FFF009B,  1073676443 */

public const System.Int32 VI_WARN_CONFIG_NLOADED  = (0x3FFF0077); /* 3FFF0077,  1073676407 */
public const System.Int32 VI_WARN_NULL_OBJECT  = (0x3FFF0082); /* 3FFF0082,  1073676418 */
public const System.Int32 VI_WARN_NSUP_ATTR_STATE  = (0x3FFF0084); /* 3FFF0084,  1073676420 */
public const System.Int32 VI_WARN_UNKNOWN_STATUS  = (0x3FFF0085); /* 3FFF0085,  1073676421 */
public const System.Int32 VI_WARN_NSUP_BUF  = (0x3FFF0088); /* 3FFF0088,  1073676424 */

public const System.Int32 VI_ERROR_SYSTEM_ERROR  = (_VI_ERROR+0x3FFF0000); /* BFFF0000, -1073807360 */
public const System.Int32 VI_ERROR_INV_OBJECT  = (_VI_ERROR+0x3FFF000E); /* BFFF000E, -1073807346 */
public const System.Int32 VI_ERROR_RSRC_LOCKED  = (_VI_ERROR+0x3FFF000F); /* BFFF000F, -1073807345 */
public const System.Int32 VI_ERROR_INV_EXPR  = (_VI_ERROR+0x3FFF0010); /* BFFF0010, -1073807344 */
public const System.Int32 VI_ERROR_RSRC_NFOUND  = (_VI_ERROR+0x3FFF0011); /* BFFF0011, -1073807343 */
public const System.Int32 VI_ERROR_INV_RSRC_NAME  = (_VI_ERROR+0x3FFF0012); /* BFFF0012, -1073807342 */
public const System.Int32 VI_ERROR_INV_ACC_MODE  = (_VI_ERROR+0x3FFF0013); /* BFFF0013, -1073807341 */
public const System.Int32 VI_ERROR_TMO  = (_VI_ERROR+0x3FFF0015); /* BFFF0015, -1073807339 */
public const System.Int32 VI_ERROR_CLOSING_FAILED  = (_VI_ERROR+0x3FFF0016); /* BFFF0016, -1073807338 */
public const System.Int32 VI_ERROR_INV_DEGREE  = (_VI_ERROR+0x3FFF001B); /* BFFF001B, -1073807333 */
public const System.Int32 VI_ERROR_INV_JOB_ID  = (_VI_ERROR+0x3FFF001C); /* BFFF001C, -1073807332 */
public const System.Int32 VI_ERROR_NSUP_ATTR  = (_VI_ERROR+0x3FFF001D); /* BFFF001D, -1073807331 */
public const System.Int32 VI_ERROR_NSUP_ATTR_STATE  = (_VI_ERROR+0x3FFF001E); /* BFFF001E, -1073807330 */
public const System.Int32 VI_ERROR_ATTR_READONLY  = (_VI_ERROR+0x3FFF001F); /* BFFF001F, -1073807329 */
public const System.Int32 VI_ERROR_INV_LOCK_TYPE  = (_VI_ERROR+0x3FFF0020); /* BFFF0020, -1073807328 */
public const System.Int32 VI_ERROR_INV_ACCESS_KEY  = (_VI_ERROR+0x3FFF0021); /* BFFF0021, -1073807327 */
public const System.Int32 VI_ERROR_INV_EVENT  = (_VI_ERROR+0x3FFF0026); /* BFFF0026, -1073807322 */
public const System.Int32 VI_ERROR_INV_MECH  = (_VI_ERROR+0x3FFF0027); /* BFFF0027, -1073807321 */
public const System.Int32 VI_ERROR_HNDLR_NINSTALLED  = (_VI_ERROR+0x3FFF0028); /* BFFF0028, -1073807320 */
public const System.Int32 VI_ERROR_INV_HNDLR_REF  = (_VI_ERROR+0x3FFF0029); /* BFFF0029, -1073807319 */
public const System.Int32 VI_ERROR_INV_CONTEXT  = (_VI_ERROR+0x3FFF002A); /* BFFF002A, -1073807318 */
public const System.Int32 VI_ERROR_QUEUE_OVERFLOW  = (_VI_ERROR+0x3FFF002D); /* BFFF002D, -1073807315 */
public const System.Int32 VI_ERROR_NENABLED  = (_VI_ERROR+0x3FFF002F); /* BFFF002F, -1073807313 */
public const System.Int32 VI_ERROR_ABORT  = (_VI_ERROR+0x3FFF0030); /* BFFF0030, -1073807312 */
public const System.Int32 VI_ERROR_RAW_WR_PROT_VIOL  = (_VI_ERROR+0x3FFF0034); /* BFFF0034, -1073807308 */
public const System.Int32 VI_ERROR_RAW_RD_PROT_VIOL  = (_VI_ERROR+0x3FFF0035); /* BFFF0035, -1073807307 */
public const System.Int32 VI_ERROR_OUTP_PROT_VIOL  = (_VI_ERROR+0x3FFF0036); /* BFFF0036, -1073807306 */
public const System.Int32 VI_ERROR_INP_PROT_VIOL  = (_VI_ERROR+0x3FFF0037); /* BFFF0037, -1073807305 */
public const System.Int32 VI_ERROR_BERR  = (_VI_ERROR+0x3FFF0038); /* BFFF0038, -1073807304 */
public const System.Int32 VI_ERROR_IN_PROGRESS  = (_VI_ERROR+0x3FFF0039); /* BFFF0039, -1073807303 */
public const System.Int32 VI_ERROR_INV_SETUP  = (_VI_ERROR+0x3FFF003A); /* BFFF003A, -1073807302 */
public const System.Int32 VI_ERROR_QUEUE_ERROR  = (_VI_ERROR+0x3FFF003B); /* BFFF003B, -1073807301 */
public const System.Int32 VI_ERROR_ALLOC  = (_VI_ERROR+0x3FFF003C); /* BFFF003C, -1073807300 */
public const System.Int32 VI_ERROR_INV_MASK  = (_VI_ERROR+0x3FFF003D); /* BFFF003D, -1073807299 */
public const System.Int32 VI_ERROR_IO  = (_VI_ERROR+0x3FFF003E); /* BFFF003E, -1073807298 */
public const System.Int32 VI_ERROR_INV_FMT  = (_VI_ERROR+0x3FFF003F); /* BFFF003F, -1073807297 */
public const System.Int32 VI_ERROR_NSUP_FMT  = (_VI_ERROR+0x3FFF0041); /* BFFF0041, -1073807295 */
public const System.Int32 VI_ERROR_LINE_IN_USE  = (_VI_ERROR+0x3FFF0042); /* BFFF0042, -1073807294 */
public const System.Int32 VI_ERROR_NSUP_MODE  = (_VI_ERROR+0x3FFF0046); /* BFFF0046, -1073807290 */
public const System.Int32 VI_ERROR_SRQ_NOCCURRED  = (_VI_ERROR+0x3FFF004A); /* BFFF004A, -1073807286 */
public const System.Int32 VI_ERROR_INV_SPACE  = (_VI_ERROR+0x3FFF004E); /* BFFF004E, -1073807282 */
public const System.Int32 VI_ERROR_INV_OFFSET  = (_VI_ERROR+0x3FFF0051); /* BFFF0051, -1073807279 */
public const System.Int32 VI_ERROR_INV_WIDTH  = (_VI_ERROR+0x3FFF0052); /* BFFF0052, -1073807278 */
public const System.Int32 VI_ERROR_NSUP_OFFSET  = (_VI_ERROR+0x3FFF0054); /* BFFF0054, -1073807276 */
public const System.Int32 VI_ERROR_NSUP_VAR_WIDTH  = (_VI_ERROR+0x3FFF0055); /* BFFF0055, -1073807275 */
public const System.Int32 VI_ERROR_WINDOW_NMAPPED  = (_VI_ERROR+0x3FFF0057); /* BFFF0057, -1073807273 */
public const System.Int32 VI_ERROR_RESP_PENDING  = (_VI_ERROR+0x3FFF0059); /* BFFF0059, -1073807271 */
public const System.Int32 VI_ERROR_NLISTENERS  = (_VI_ERROR+0x3FFF005F); /* BFFF005F, -1073807265 */
public const System.Int32 VI_ERROR_NCIC  = (_VI_ERROR+0x3FFF0060); /* BFFF0060, -1073807264 */
public const System.Int32 VI_ERROR_NSYS_CNTLR  = (_VI_ERROR+0x3FFF0061); /* BFFF0061, -1073807263 */
public const System.Int32 VI_ERROR_NSUP_OPER  = (_VI_ERROR+0x3FFF0067); /* BFFF0067, -1073807257 */
public const System.Int32 VI_ERROR_INTR_PENDING  = (_VI_ERROR+0x3FFF0068); /* BFFF0068, -1073807256 */
public const System.Int32 VI_ERROR_ASRL_PARITY  = (_VI_ERROR+0x3FFF006A); /* BFFF006A, -1073807254 */
public const System.Int32 VI_ERROR_ASRL_FRAMING  = (_VI_ERROR+0x3FFF006B); /* BFFF006B, -1073807253 */
public const System.Int32 VI_ERROR_ASRL_OVERRUN  = (_VI_ERROR+0x3FFF006C); /* BFFF006C, -1073807252 */
public const System.Int32 VI_ERROR_TRIG_NMAPPED  = (_VI_ERROR+0x3FFF006E); /* BFFF006E, -1073807250 */
public const System.Int32 VI_ERROR_NSUP_ALIGN_OFFSET  = (_VI_ERROR+0x3FFF0070); /* BFFF0070, -1073807248 */
public const System.Int32 VI_ERROR_USER_BUF  = (_VI_ERROR+0x3FFF0071); /* BFFF0071, -1073807247 */
public const System.Int32 VI_ERROR_RSRC_BUSY  = (_VI_ERROR+0x3FFF0072); /* BFFF0072, -1073807246 */
public const System.Int32 VI_ERROR_NSUP_WIDTH  = (_VI_ERROR+0x3FFF0076); /* BFFF0076, -1073807242 */
public const System.Int32 VI_ERROR_INV_PARAMETER  = (_VI_ERROR+0x3FFF0078); /* BFFF0078, -1073807240 */
public const System.Int32 VI_ERROR_INV_PROT  = (_VI_ERROR+0x3FFF0079); /* BFFF0079, -1073807239 */
public const System.Int32 VI_ERROR_INV_SIZE  = (_VI_ERROR+0x3FFF007B); /* BFFF007B, -1073807237 */
public const System.Int32 VI_ERROR_WINDOW_MAPPED  = (_VI_ERROR+0x3FFF0080); /* BFFF0080, -1073807232 */
public const System.Int32 VI_ERROR_NIMPL_OPER  = (_VI_ERROR+0x3FFF0081); /* BFFF0081, -1073807231 */
public const System.Int32 VI_ERROR_INV_LENGTH  = (_VI_ERROR+0x3FFF0083); /* BFFF0083, -1073807229 */
public const System.Int32 VI_ERROR_INV_MODE  = (_VI_ERROR+0x3FFF0091); /* BFFF0091, -1073807215 */
public const System.Int32 VI_ERROR_SESN_NLOCKED  = (_VI_ERROR+0x3FFF009C); /* BFFF009C, -1073807204 */
public const System.Int32 VI_ERROR_MEM_NSHARED  = (_VI_ERROR+0x3FFF009D); /* BFFF009D, -1073807203 */
public const System.Int32 VI_ERROR_LIBRARY_NFOUND  = (_VI_ERROR+0x3FFF009E); /* BFFF009E, -1073807202 */
public const System.Int32 VI_ERROR_NSUP_INTR  = (_VI_ERROR+0x3FFF009F); /* BFFF009F, -1073807201 */
public const System.Int32 VI_ERROR_INV_LINE  = (_VI_ERROR+0x3FFF00A0); /* BFFF00A0, -1073807200 */
public const System.Int32 VI_ERROR_FILE_ACCESS  = (_VI_ERROR+0x3FFF00A1); /* BFFF00A1, -1073807199 */
public const System.Int32 VI_ERROR_FILE_IO  = (_VI_ERROR+0x3FFF00A2); /* BFFF00A2, -1073807198 */
public const System.Int32 VI_ERROR_NSUP_LINE  = (_VI_ERROR+0x3FFF00A3); /* BFFF00A3, -1073807197 */
public const System.Int32 VI_ERROR_NSUP_MECH  = (_VI_ERROR+0x3FFF00A4); /* BFFF00A4, -1073807196 */
public const System.Int32 VI_ERROR_INTF_NUM_NCONFIG  = (_VI_ERROR+0x3FFF00A5); /* BFFF00A5, -1073807195 */
public const System.Int32 VI_ERROR_CONN_LOST  = (_VI_ERROR+0x3FFF00A6); /* BFFF00A6, -1073807194 */

#endregion

#region Other VISA Definitions

public const System.Int32 VI_FIND_BUFLEN  = (256);

public const System.Int32 VI_INTF_GPIB  = (1);
public const System.Int32 VI_INTF_VXI  = (2);
public const System.Int32 VI_INTF_GPIB_VXI  = (3);
public const System.Int32 VI_INTF_ASRL  = (4);
public const System.Int32 VI_INTF_TCPIP  = (6);

public const System.Int32 VI_NORMAL  = (1);
public const System.Int32 VI_FDC  = (2);
public const System.Int32 VI_HS488  = (3);
public const System.Int32 VI_PROT_4882_STRS  = (4);

public const System.Int32 VI_FDC_NORMAL  = (1);
public const System.Int32 VI_FDC_STREAM  = (2);

public const System.Int32 VI_LOCAL_SPACE  = (0);
public const System.Int32 VI_A16_SPACE  = (1);
public const System.Int32 VI_A24_SPACE  = (2);
public const System.Int32 VI_A32_SPACE  = (3);

public const System.Int32 VI_UNKNOWN_LA  = (-1);
public const System.Int32 VI_UNKNOWN_SLOT  = (-1);
public const System.Int32 VI_UNKNOWN_LEVEL  = (-1);

public const System.Int32 VI_QUEUE  = (1);
public const System.Int32 VI_HNDLR  = (2);
public const System.Int32 VI_SUSPEND_HNDLR  = (4);
public const System.Int32 VI_ALL_MECH  = (0xFFFF);

public const System.Int32 VI_ANY_HNDLR  = (0);

public const System.Int32 VI_TRIG_ALL  = (-2);
public const System.Int32 VI_TRIG_SW  = (-1);
public const System.Int32 VI_TRIG_TTL0  = (0);
public const System.Int32 VI_TRIG_TTL1  = (1);
public const System.Int32 VI_TRIG_TTL2  = (2);
public const System.Int32 VI_TRIG_TTL3  = (3);
public const System.Int32 VI_TRIG_TTL4  = (4);
public const System.Int32 VI_TRIG_TTL5  = (5);
public const System.Int32 VI_TRIG_TTL6  = (6);
public const System.Int32 VI_TRIG_TTL7  = (7);
public const System.Int32 VI_TRIG_ECL0  = (8);
public const System.Int32 VI_TRIG_ECL1  = (9);
public const System.Int32 VI_TRIG_PANEL_IN  = (27);
public const System.Int32 VI_TRIG_PANEL_OUT  = (28);

public const System.Int32 VI_TRIG_PROT_DEFAULT  = (0);
public const System.Int32 VI_TRIG_PROT_ON  = (1);
public const System.Int32 VI_TRIG_PROT_OFF  = (2);
public const System.Int32 VI_TRIG_PROT_SYNC  = (5);

public const System.Int32 VI_READ_BUF  = (1);
public const System.Int32 VI_WRITE_BUF  = (2);
public const System.Int32 VI_READ_BUF_DISCARD  = (4);
public const System.Int32 VI_WRITE_BUF_DISCARD  = (8);
public const System.Int32 VI_IO_IN_BUF  = (16);
public const System.Int32 VI_IO_OUT_BUF  = (32);
public const System.Int32 VI_IO_IN_BUF_DISCARD  = (64);
public const System.Int32 VI_IO_OUT_BUF_DISCARD  = (128);

public const System.Int32 VI_FLUSH_ON_ACCESS  = (1);
public const System.Int32 VI_FLUSH_WHEN_FULL  = (2);
public const System.Int32 VI_FLUSH_DISABLE  = (3);

public const System.Int32 VI_NMAPPED  = (1);
public const System.Int32 VI_USE_OPERS  = (2);
public const System.Int32 VI_DEREF_ADDR  = (3);

public const System.Int32 VI_TMO_IMMEDIATE  = (0);
public const System.UInt32 VI_TMO_INFINITE  = (0xFFFFFFFF);

public const System.Int32 VI_NO_LOCK  = (0);
public const System.Int32 VI_EXCLUSIVE_LOCK  = (1);
public const System.Int32 VI_SHARED_LOCK  = (2);
public const System.Int32 VI_LOAD_CONFIG  = (4);

public const System.Int32 VI_NO_SEC_ADDR  = (0xFFFF);

public const System.Int32 VI_ASRL_PAR_NONE  = (0);
public const System.Int32 VI_ASRL_PAR_ODD  = (1);
public const System.Int32 VI_ASRL_PAR_EVEN  = (2);
public const System.Int32 VI_ASRL_PAR_MARK  = (3);
public const System.Int32 VI_ASRL_PAR_SPACE  = (4);

public const System.Int32 VI_ASRL_STOP_ONE  = (10);
public const System.Int32 VI_ASRL_STOP_ONE5  = (15);
public const System.Int32 VI_ASRL_STOP_TWO  = (20);

public const System.Int32 VI_ASRL_FLOW_NONE  = (0);
public const System.Int32 VI_ASRL_FLOW_XON_XOFF  = (1);
public const System.Int32 VI_ASRL_FLOW_RTS_CTS  = (2);
public const System.Int32 VI_ASRL_FLOW_DTR_DSR  = (4);

public const System.Int32 VI_ASRL_END_NONE  = (0);
public const System.Int32 VI_ASRL_END_LAST_BIT  = (1);
public const System.Int32 VI_ASRL_END_TERMCHAR  = (2);
public const System.Int32 VI_ASRL_END_BREAK  = (3);

public const System.Int32 VI_STATE_ASSERTED  = (1);
public const System.Int32 VI_STATE_UNASSERTED  = (0);
public const System.Int32 VI_STATE_UNKNOWN  = (-1);

public const System.Int32 VI_BIG_ENDIAN  = (0);
public const System.Int32 VI_LITTLE_ENDIAN  = (1);

public const System.Int32 VI_DATA_PRIV  = (0);
public const System.Int32 VI_DATA_NPRIV  = (1);
public const System.Int32 VI_PROG_PRIV  = (2);
public const System.Int32 VI_PROG_NPRIV  = (3);
public const System.Int32 VI_BLCK_PRIV  = (4);
public const System.Int32 VI_BLCK_NPRIV  = (5);
public const System.Int32 VI_D64_PRIV  = (6);
public const System.Int32 VI_D64_NPRIV  = (7);

public const System.Int32 VI_WIDTH_8  = (1);
public const System.Int32 VI_WIDTH_16  = (2);
public const System.Int32 VI_WIDTH_32  = (4);

public const System.Int32 VI_GPIB_REN_DEASSERT  = (0);
public const System.Int32 VI_GPIB_REN_ASSERT  = (1);
public const System.Int32 VI_GPIB_REN_DEASSERT_GTL  = (2);
public const System.Int32 VI_GPIB_REN_ASSERT_ADDRESS  = (3);
public const System.Int32 VI_GPIB_REN_ASSERT_LLO  = (4);
public const System.Int32 VI_GPIB_REN_ASSERT_ADDRESS_LLO = (5);
public const System.Int32 VI_GPIB_REN_ADDRESS_GTL  = (6);

public const System.Int32 VI_GPIB_ATN_DEASSERT  = (0);
public const System.Int32 VI_GPIB_ATN_ASSERT  = (1);
public const System.Int32 VI_GPIB_ATN_DEASSERT_HANDSHAKE = (2);
public const System.Int32 VI_GPIB_ATN_ASSERT_IMMEDIATE = (3);

public const System.Int32 VI_GPIB_HS488_DISABLED  = (0);
public const System.Int32 VI_GPIB_HS488_NIMPL  = (-1);

public const System.Int32 VI_GPIB_UNADDRESSED  = (0);
public const System.Int32 VI_GPIB_TALKER  = (1);
public const System.Int32 VI_GPIB_LISTENER  = (2);

public const System.Int32 VI_VXI_CMD16  = (0x0200);
public const System.Int32 VI_VXI_CMD16_RESP16  = (0x0202);
public const System.Int32 VI_VXI_RESP16  = (0x0002);
public const System.Int32 VI_VXI_CMD32  = (0x0400);
public const System.Int32 VI_VXI_CMD32_RESP16  = (0x0402);
public const System.Int32 VI_VXI_CMD32_RESP32  = (0x0404);
public const System.Int32 VI_VXI_RESP32  = (0x0004);

public const System.Int32 VI_ASSERT_SIGNAL  = (-1);
public const System.Int32 VI_ASSERT_USE_ASSIGNED  = (0);
public const System.Int32 VI_ASSERT_IRQ1  = (1);
public const System.Int32 VI_ASSERT_IRQ2  = (2);
public const System.Int32 VI_ASSERT_IRQ3  = (3);
public const System.Int32 VI_ASSERT_IRQ4  = (4);
public const System.Int32 VI_ASSERT_IRQ5  = (5);
public const System.Int32 VI_ASSERT_IRQ6  = (6);
public const System.Int32 VI_ASSERT_IRQ7  = (7);

public const System.Int32 VI_UTIL_ASSERT_SYSRESET  = (1);
public const System.Int32 VI_UTIL_ASSERT_SYSFAIL  = (2);
public const System.Int32 VI_UTIL_DEASSERT_SYSFAIL  = (3);

public const System.Int32 VI_VXI_CLASS_MEMORY  = (0);
public const System.Int32 VI_VXI_CLASS_EXTENDED  = (1);
public const System.Int32 VI_VXI_CLASS_MESSAGE  = (2);
public const System.Int32 VI_VXI_CLASS_REGISTER  = (3);
public const System.Int32 VI_VXI_CLASS_OTHER  = (4);

#endregion

#region Backward Compatibility Stuff

public const System.Int32 VI_ERROR_INV_SESSION  = (VI_ERROR_INV_OBJECT);
public const System.UInt32 VI_INFINITE  = (VI_TMO_INFINITE);
public const System.Int32 VI_ASRL488  = (VI_PROT_4882_STRS);
public const System.Int32 VI_ASRL_IN_BUF  = (VI_IO_IN_BUF);
public const System.Int32 VI_ASRL_OUT_BUF  = (VI_IO_OUT_BUF);
public const System.Int32 VI_ASRL_IN_BUF_DISCARD  = (VI_IO_IN_BUF_DISCARD);
public const System.Int32 VI_ASRL_OUT_BUF_DISCARD  = (VI_IO_OUT_BUF_DISCARD);

#endregion

#region National Instruments

/* This specifies the behavior of viFindRsrc */
public const System.UInt32 VI_ATTR_FIND_NEEDS_REFRESH  = (0x3FFF018F); /* ViBoolean, read-only */
public const System.UInt32 VI_ATTR_FIND_RSRC_MODE  = (0x3FFF0190); /* System.UInt16, read-write */

public const System.Int32 VI_FIND_SEARCHBUS_NOALIAS  = (0);
public const System.Int32 VI_FIND_SEARCHBUS_ALIAS  = (2);
public const System.Int32 VI_FIND_NOSEARCHBUS_ALIAS  = (3);

/* This allows a user to query the size of formatted I/O buffers */
public const System.UInt32 VI_ATTR_RD_BUF_SIZE  = (0x3FFF002B);
public const System.UInt32 VI_ATTR_WR_BUF_SIZE  = (0x3FFF002E);

/* This is for VXI SERVANT resources */
public const System.UInt32 VI_EVENT_VXI_DEV_CMD  = (0xBFFF200F);
public const System.UInt32 VI_ATTR_VXI_DEV_CMD_TYPE  = (0x3FFF4037); /* ViInt16, read-only */
public const System.UInt32 VI_ATTR_VXI_DEV_CMD_VALUE  = (0x3FFF4038); /* System.UInt32, read-only */

public const System.Int32 VI_VXI_DEV_CMD_TYPE_16  = (16);
public const System.Int32 VI_VXI_DEV_CMD_TYPE_32  = (32);

[DllImport("visa32.dll")] public static extern System.UInt32  viVxiServantResponse(System.Int32 vi, System.Int16 mode, System.UInt32 resp);
/* mode values include VI_VXI_RESP16, VI_VXI_RESP32, and the next 2 values */
public const System.Int32 VI_VXI_RESP_NONE  = (0);
public const System.Int32 VI_VXI_RESP_PROT_ERROR  = (-1);

#endregion

	}
}

