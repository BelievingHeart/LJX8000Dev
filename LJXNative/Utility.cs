﻿//----------------------------------------------------------------------------- 
// <copyright file="Utility.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System.Runtime.InteropServices;
 using System.Text;

 namespace LJXNative
{
	/// <summary>
	/// Utility class
	/// </summary>
	static class Utility
	{
		#region Constant
		/// <summary>
		/// value for head temperature display
		/// </summary>
		private const int DivideValueForHeadTemperatureDisplay = 100;
		/// <summary>
		///  head temperature invalid value
		/// </summary>
		private const int HeadTemperatureInvalidValue = 0xFFFF;
		#endregion

		#region Enum
		/// <summary>
		/// Structure classification
		/// </summary>
		public enum TypeOfStructure
		{
			ProfileHeader,
			ProfileFooter,
		}
		#endregion

		#region Method

		#region Get the byte size

		/// <summary>
		/// Get the byte size of the structure.
		/// </summary>
		/// <param name="type">Structure whose byte size you want to get.</param>
		/// <returns>Byte size</returns>
		public static int GetByteSize(TypeOfStructure type)
		{
			switch (type)
			{
				case TypeOfStructure.ProfileHeader:
					LJX8IF_PROFILE_HEADER profileHeader = new LJX8IF_PROFILE_HEADER();
					return Marshal.SizeOf(profileHeader);

				case TypeOfStructure.ProfileFooter:
					LJX8IF_PROFILE_FOOTER profileFooter = new LJX8IF_PROFILE_FOOTER();
					return Marshal.SizeOf(profileFooter);
			}

			return 0;
		}
		#endregion

		#region Acquisition of log 

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="profileInfo">profileInfo</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertProfileInfoToLogString(LJX8IF_PROFILE_INFO profileInfo)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  Profile Data Num			: {0}", profileInfo.byProfileCount));
			string luminanceOutput = profileInfo.byLuminanceOutput == 0
				? @"OFF"
				: @"ON";
			sb.AppendLine(string.Format(@"  Luminance output			: {0}", luminanceOutput));
			sb.AppendLine(string.Format(@"  Profile Data Points			: {0}", profileInfo.nProfileDataCount));
			sb.AppendLine(string.Format(@"  X coordinate of the first point	: {0}", profileInfo.lXStart));
			sb.Append(string.Format(@"  X-direction interval		: {0}", profileInfo.lXPitch));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="response">"Get batch profile" command response</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertBatchProfileResponseToLogString(LJX8IF_GET_BATCH_PROFILE_RESPONSE response)
		{
			StringBuilder sb = new StringBuilder();

			// Profile information of the profile obtained
			sb.AppendLine(string.Format(@"  CurrentBatchNo			: {0}", response.dwCurrentBatchNo));
			sb.AppendLine(string.Format(@"  CurrentBatchProfileCount		: {0}", response.dwCurrentBatchProfileCount));
			sb.AppendLine(string.Format(@"  OldestBatchNo			: {0}", response.dwOldestBatchNo));
			sb.AppendLine(string.Format(@"  OldestBatchProfileCount		: {0}", response.dwOldestBatchProfileCount));
			sb.AppendLine(string.Format(@"  GetBatchNo			: {0}", response.dwGetBatchNo));
			sb.AppendLine(string.Format(@"  GetBatchProfileCount		: {0}", response.dwGetBatchProfileCount));
			sb.AppendLine(string.Format(@"  GetBatchTopProfileNo		: {0}", response.dwGetBatchTopProfileNo));
			sb.AppendLine(string.Format(@"  GetProfileCount			: {0}", response.byGetProfileCount));
			sb.Append(string.Format(@"  CurrentBatchCommited		: {0}", response.byCurrentBatchCommited));

			return sb;
		}

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="response">"Get profile" command response</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertProfileResponseToLogString(LJX8IF_GET_PROFILE_RESPONSE response)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(@"  CurrentProfileNo	: {0}", response.dwCurrentProfileNo));
			sb.AppendLine(string.Format(@"  OldestProfileNo	: {0}", response.dwOldestProfileNo));
			sb.AppendLine(string.Format(@"  GetTopProfileNo	: {0}", response.dwGetTopProfileNo));
			sb.Append(string.Format(@"  GetProfileCount	: {0}", response.byGetProfileCount));

			return sb;
		}

		#endregion

		/// <summary>
		/// Get the string for log output.
		/// </summary>
		/// <param name="sensorTemperature">sensor Temperature</param>
		/// <param name="processorTemperature">processor Temperature</param>
		/// <param name="caseTemperature">case Temperature</param>
		/// <returns>String for log output</returns>
		public static StringBuilder ConvertHeadTemperatureLogString(short sensorTemperature, short processorTemperature, short caseTemperature)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format(@"  SensorTemperature	: {0}", GetTemperatureString(sensorTemperature)));
			sb.AppendLine(string.Format(@"  ProcessorTemperature	: {0}", GetTemperatureString(processorTemperature)));
			sb.Append(string.Format(@"  CaseTemperature		: {0}", GetTemperatureString(caseTemperature)));

			return sb;
		}

		private static string GetTemperatureString(short temperature)
		{
			if ((temperature & HeadTemperatureInvalidValue) == HeadTemperatureInvalidValue)
			{
				return "----";
			}
			return string.Format(@"{0} C", (decimal)temperature/ DivideValueForHeadTemperatureDisplay);
		}
		#endregion

		#region Get and Set Ethernet setting

		#endregion
	}
}
