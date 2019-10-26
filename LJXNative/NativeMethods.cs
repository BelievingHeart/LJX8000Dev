﻿//----------------------------------------------------------------------------- 
// <copyright file="NativeMethods.cs.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System;
 using System.Runtime.InteropServices;

 namespace LJXNative
{
	#region Enum

	/// <summary>
	/// Return value definition
	/// </summary>
	public enum Rc
	{
		/// <summary>Normal termination</summary>
		Ok = 0x0000,
		/// <summary>Failed to open the device</summary>
		ErrOpenDevice = 0x1000,
		/// <summary>Device not open</summary>
		ErrNoDevice,
		/// <summary>Command send error</summary>
		ErrSend,
		/// <summary>Response reception error</summary>
		ErrReceive,
		/// <summary>Timeout</summary>
		ErrTimeout,
		/// <summary>No free space</summary>
		ErrNomemory,
		/// <summary>Parameter error</summary>
		ErrParameter,
		/// <summary>Received header format error</summary>
		ErrRecvFmt,

		/// <summary>Not open error (for high-speed communication)</summary>
		ErrHispeedNoDevice = 0x1009,
		/// <summary>Already open error (for high-speed communication)</summary>
		ErrHispeedOpenYet,
		/// <summary>Already performing high-speed communication error (for high-speed communication)</summary>
		ErrHispeedRecvYet,
		/// <summary>Insufficient buffer size</summary>
		ErrBufferShort,
	}

	/// Definition that indicates the "setting type" in LJX8IF_TARGET_SETTING structure.
	public enum SettingType : byte
	{
		/// <summary>Environment setting</summary>
		Environment = 0x01,
		/// <summary>Common measurement setting</summary>
		Common = 0x02,
		/// <summary>Measurement Program setting</summary>
		Program00 = 0x10,
		Program01,
		Program02,
		Program03,
		Program04,
		Program05,
		Program06,
		Program07,
		Program08,
		Program09,
		Program10,
		Program11,
		Program12,
		Program13,
		Program14,
		Program15,
	};


	/// Get batch profile position specification method designation
	public enum LJX8IF_BATCH_POSITION : byte
	{
		/// <summary>From current</summary>
		LJX8IF_BATCH_POSITION_CURRENT = 0x00,
		/// <summary>Specify position</summary>
		LJX8IF_BATCH_POSITION_SPEC = 0x02,
		/// <summary>From current after commitment</summary>
		LJX8IF_BATCH_POSITION_COMMITED = 0x03,
		/// <summary>Current only</summary>
		LJX8IF_BATCH_POSITION_CURRENT_ONLY = 0x04,
	};

	/// Setting value storage level designation
	public enum LJX8IF_SETTING_DEPTH : byte
	{
		/// <summary>Settings write area</summary>
		LJX8IF_SETTING_DEPTH_WRITE = 0x00,
		/// <summary>Active measurement area</summary>
		LJX8IF_SETTING_DEPTH_RUNNING = 0x01,
		/// <summary>Save area</summary>
		LJX8IF_SETTING_DEPTH_SAVE = 0x02,
	};


	/// Get profile target buffer designation
	public enum LJX8IF_PROFILE_BANK : byte
	{
		/// <summary>Active surface</summary>
		LJX8IF_PROFILE_BANK_ACTIVE = 0x00,
		/// <summary>Inactive surface</summary>	
		LJX8IF_PROFILE_BANK_INACTIVE = 0x01,
	};

	/// Get profile position specification method designation
	public enum LJX8IF_PROFILE_POSITION : byte
	{
		/// <summary>From current</summary>
		LJX8IF_PROFILE_POSITION_CURRENT = 0x00,
		/// <summary>From oldest</summary>
		LJX8IF_PROFILE_POSITION_OLDEST = 0x01,
		/// <summary>Specify position</summary>
		LJX8IF_PROFILE_POSITION_SPEC = 0x02,
	};

	#endregion

	#region Structure
	/// <summary>
	/// Version Information
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_VERSION_INFO
	{
		public int nMajorNumber;
		public int nMinorNumber;
		public int nRevisionNumber;
		public int nBuildNumber;
	};

	/// <summary>
	/// Ethernet settings structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_ETHERNET_CONFIG
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] abyIpAddress;
		public ushort wPortNo;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;

	};

	/// <summary>
	/// Setting item designation structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_TARGET_SETTING
	{
		public byte byType;
		public byte byCategory;
		public byte byItem;
		public byte reserve;
		public byte byTarget1;
		public byte byTarget2;
		public byte byTarget3;
		public byte byTarget4;
	};

	/// <summary>
	/// Profile information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_PROFILE_INFO
	{
		public byte byProfileCount;
		public byte reserve1;
		public byte byLuminanceOutput;
		public byte reserve2;
		public short nProfileDataCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve3;
		public int lXStart;
		public int lXPitch;
	};

	/// <summary>
	/// Profile header information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_PROFILE_HEADER
	{
		public uint reserve;
		public uint dwTriggerCount;
		public int lEncoderCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] reserve2;
	};

	/// <summary>
	/// Profile footer information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_PROFILE_FOOTER
	{
		public uint reserve;
	};

	/// <summary>
	/// Get profile request structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_GET_PROFILE_REQUEST
	{
		public byte byTargetBank;
		public byte byPositionMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetProfileNo;
		public byte byGetProfileCount;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// Get profile request structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_GET_BATCH_PROFILE_REQUEST
	{
		public byte byTargetBank;
		public byte byPositionMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetBatchNo;
		public uint dwGetProfileNo;
		public byte byGetProfileCount;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// Get profile response structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_GET_PROFILE_RESPONSE
	{
		public uint dwCurrentProfileNo;
		public uint dwOldestProfileNo;
		public uint dwGetTopProfileNo;
		public byte byGetProfileCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
	};

	/// <summary>
	/// Get profile response structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_GET_BATCH_PROFILE_RESPONSE
	{
		public uint dwCurrentBatchNo;
		public uint dwCurrentBatchProfileCount;
		public uint dwOldestBatchNo;
		public uint dwOldestBatchProfileCount;
		public uint dwGetBatchNo;
		public uint dwGetBatchProfileCount;
		public uint dwGetBatchTopProfileNo;
		public byte byGetProfileCount;
		public byte byCurrentBatchCommited;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
	};

	/// <summary>
	/// High-speed communication start preparation request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJX8IF_HIGH_SPEED_PRE_START_REQUEST
	{
		public byte bySendPosition;		// Send start position
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;		// Reservation 
	};

	#endregion

	#region Method
	/// <summary>
	/// Callback function for high-speed communication
	/// </summary>
	/// <param name="pBuffer">Received profile data pointer</param>
	/// <param name="dwSize">Size in units of bytes of one profile</param>
	/// <param name="dwCount">Number of profiles</param>
	/// <param name="dwNotify">Finalization condition</param>
	/// <param name="dwUser">Thread ID</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void HighSpeedDataCallBack(IntPtr pBuffer, uint dwSize, uint dwCount, uint dwNotify, uint dwUser);

	/// <summary>
	/// Callback function for high-speed communication simple array
	/// </summary>
	/// <param name="pProfileHeaderArray">Received header data array pointer</param>
	/// <param name="pHeightProfileArray">Received profile data array pointer</param>
	/// <param name="pLuminanceProfileArray">Received luminance profile data array pointer</param>
	/// <param name="dwLuminanceEnable">The value indicating whether luminance data output is enable or not</param>
	/// <param name="dwProfileDataCount">The data count of one profile</param>
	/// <param name="dwCount">Number of profiles</param>
	/// <param name="dwNotify">Finalization condition</param>
	/// <param name="dwUser">Thread ID</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void HighSpeedDataCallBackForSimpleArray(IntPtr pProfileHeaderArray, IntPtr pHeightProfileArray, IntPtr pLuminanceProfileArray, uint dwLuminanceEnable, uint dwProfileDataCount, uint dwCount, uint dwNotify, uint dwUser);

	/// <summary>
	/// Function definitions
	/// </summary>
	public class NativeMethods
	{
		/// <summary>
		/// Number of connectable devices
		/// </summary>
		public static int DeviceCount
		{
			get { return 6; }
		}
		
		/// <summary>
		/// Fixed value for the bytes of environment settings data 
		/// </summary>
		public static UInt32 EnvironmentSettingSize
		{
			get { return 60; }
		}

		/// <summary>
		/// Fixed value for the bytes of common measurement settings data 
		/// </summary>
		public static UInt32 CommonSettingSize
		{
			get { return 20; }
		}

		/// <summary>
		/// Fixed value for the bytes of program settings data 
		/// </summary>
		public static UInt32 ProgramSettingSize
		{
			get { return 10980; }
		}

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_Initialize();

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_Finalize();

		[DllImport("LJX8_IF.dll")]
		internal static extern LJX8IF_VERSION_INFO LJX8IF_GetVersion();

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_EthernetOpen(int lDeviceId, ref LJX8IF_ETHERNET_CONFIG pEthernetConfig);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_CommunicationClose(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_RebootController(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ReturnToFactorySetting(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ControlLaser(int lDeviceId, byte byState);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetError(int lDeviceId, byte byReceivedMax, ref byte pbyErrCount, IntPtr pwErrCode);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ClearError(int lDeviceId, short wErrCode);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_TrgErrorReset(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetTriggerAndPulseCount(int lDeviceId, ref uint pdwTriggerCount, ref int plEncoderCount);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetHeadTemperature(int lDeviceId, ref short pnSensorTemperature, ref short pnProcessorTemperature, ref short pnCaseTemperature);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetSerialNumber(int lDeviceId, IntPtr pControllerSerialNo, IntPtr pHeadSerialNo);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetAttentionStatus(int lDeviceId, ref ushort pwAttentionStatus);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_Trigger(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_StartMeasure(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_StopMeasure(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ClearMemory(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_SetSetting(int lDeviceId, byte byDepth, LJX8IF_TARGET_SETTING targetSetting, IntPtr pData, uint dwDataSize, ref uint pdwError);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetSetting(int lDeviceId, byte byDepth, LJX8IF_TARGET_SETTING targetSetting, IntPtr pData, uint dwDataSize);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_InitializeSetting(int lDeviceId, byte byDepth, byte byTarget);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ReflectSetting(int lDeviceId, byte byDepth, ref uint pdwError);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_RewriteTemporarySetting(int lDeviceId, byte byDepth);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_CheckMemoryAccess(int lDeviceId, ref byte pbyBusy);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_ChangeActiveProgram(int lDeviceId, byte byProgramNo);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetActiveProgram(int lDeviceId, ref byte pbyProgramNo);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetProfile(int lDeviceId, ref LJX8IF_GET_PROFILE_REQUEST pReq,
		ref LJX8IF_GET_PROFILE_RESPONSE pRsp, ref LJX8IF_PROFILE_INFO pProfileInfo, IntPtr pdwProfileData, uint dwDataSize);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetBatchProfile(int lDeviceId, ref LJX8IF_GET_BATCH_PROFILE_REQUEST pReq,
		ref LJX8IF_GET_BATCH_PROFILE_RESPONSE pRsp, ref LJX8IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwBatchData, uint dwDataSize);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_GetBatchSimpleArray(int lDeviceId, ref LJX8IF_GET_BATCH_PROFILE_REQUEST pReq,
		ref LJX8IF_GET_BATCH_PROFILE_RESPONSE pRsp, ref LJX8IF_PROFILE_INFO pProfileInfo,
		IntPtr pProfileHeaderArray, IntPtr pHeightProfileArray, IntPtr pLuminanceProfileArray);

		[DllImport("LJX8_IF.dll")]
		internal static extern int LJX8IF_InitializeHighSpeedDataCommunication(
		int lDeviceId, ref LJX8IF_ETHERNET_CONFIG pEthernetConfig, ushort wHighSpeedPortNo,
		HighSpeedDataCallBack pCallBack, uint dwProfileCount, uint dwThreadId);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(
		int lDeviceId, ref LJX8IF_ETHERNET_CONFIG pEthernetConfig, ushort wHighSpeedPortNo,
		HighSpeedDataCallBackForSimpleArray pCallBackSimpleArray, uint dwProfileCount, uint dwThreadId);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_PreStartHighSpeedDataCommunication(
		int lDeviceId, ref LJX8IF_HIGH_SPEED_PRE_START_REQUEST pReq,
		ref LJX8IF_PROFILE_INFO pProfileInfo);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_StartHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_StopHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJX8_IF.dll")]
		public static extern int LJX8IF_FinalizeHighSpeedDataCommunication(int lDeviceId);
	}
	#endregion

}
