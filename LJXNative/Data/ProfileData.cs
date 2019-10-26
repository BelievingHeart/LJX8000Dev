﻿//----------------------------------------------------------------------------- 
// <copyright file="ProfileData.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System;
 using System.Runtime.InteropServices;
 using System.Text;

 namespace LJXNative.Data
{
	/// <summary>
	/// Profile data class
	/// </summary>
	public class ProfileData
	{
		#region constant
		private const int LUMINANCE_OUTPUT_ON_VALUE = 1;
		public  const int MULTIPLE_VALUE_FOR_LUMINANCE_OUTPUT = 2; 
		#endregion

		#region Field
		/// <summary>
		/// Profile data
		/// </summary>
		private int[] _profData;

		/// <summary>
		/// Profile information
		/// </summary>
		private LJX8IF_PROFILE_INFO _profileInfo;

		#endregion

		#region Property
		/// <summary>
		/// Profile Data
		/// </summary>
		public int[] ProfData
		{
			get { return _profData; }
		}

		/// <summary>
		/// Profile Imformation
		/// </summary>
		public LJX8IF_PROFILE_INFO ProfInfo
		{
			get { return _profileInfo; }
		}
		 #endregion

		#region Method
		/// <summary>
		/// Constructor
		/// </summary>
		public ProfileData(int[] receiveBuffer, LJX8IF_PROFILE_INFO profileInfo)
		{
			SetData(receiveBuffer, profileInfo);
		}

		/// <summary>
		/// Constructor Overload
		/// </summary>
		/// <param name="receiveBuffer">Receive buffer</param>
		/// <param name="startIndex">Start position</param>
		/// <param name="profileInfo">Profile information</param>
		public ProfileData(int[] receiveBuffer, int startIndex, LJX8IF_PROFILE_INFO profileInfo)
		{
			int bufIntSize = CalculateDataSize(profileInfo);
			int[] bufIntArray = new int[bufIntSize];
			_profileInfo = profileInfo;

			Array.Copy(receiveBuffer, startIndex, bufIntArray, 0, bufIntSize);
			SetData(bufIntArray, profileInfo);
		}

		/// <summary>
		/// Set the members to the arguments.
		/// </summary>
		/// <param name="receiveBuffer">Receive buffer</param>
		/// <param name="profileInfo">Profile information</param>
		private void SetData(int[] receiveBuffer, LJX8IF_PROFILE_INFO profileInfo)
		{
			_profileInfo = profileInfo;

			// Extract the header.
			int headerSize = Utility.GetByteSize(Utility.TypeOfStructure.ProfileHeader) /  Marshal.SizeOf(typeof(int));
			int[] headerData = new int[headerSize];
			Array.Copy(receiveBuffer, 0, headerData, 0, headerSize);

			// Extract the footer.
			int footerSize = Utility.GetByteSize(Utility.TypeOfStructure.ProfileFooter) /  Marshal.SizeOf(typeof(int));
			int[] footerData = new int[footerSize];
			Array.Copy(receiveBuffer, receiveBuffer.Length - footerSize, footerData, 0, footerSize);

			// Extract the profile data.
			int profSize = receiveBuffer.Length - headerSize - footerSize;
			_profData = new int[profSize];
			Array.Copy(receiveBuffer, headerSize, _profData, 0, profSize);
		}

		/// <summary>
		/// Data size calculation
		/// </summary>
		/// <param name="profileInfo">Profile information</param>
		/// <returns>Profile data size</returns>
		public static int CalculateDataSize(LJX8IF_PROFILE_INFO profileInfo)
		{
			LJX8IF_PROFILE_HEADER header = new LJX8IF_PROFILE_HEADER();
			LJX8IF_PROFILE_FOOTER footer = new LJX8IF_PROFILE_FOOTER();

			int multipleValue = GetIsLuminanceOutput(profileInfo) ? MULTIPLE_VALUE_FOR_LUMINANCE_OUTPUT : 1;
			return profileInfo.nProfileDataCount * multipleValue + (Marshal.SizeOf(header) + Marshal.SizeOf(footer)) /  Marshal.SizeOf(typeof(int));
		}

		public static bool GetIsLuminanceOutput(LJX8IF_PROFILE_INFO profileInfo)
		{
			return profileInfo.byLuminanceOutput == LUMINANCE_OUTPUT_ON_VALUE;
		}

		/// <summary>
		/// Create the X-position string from the profile information.
		/// </summary>
		/// <param name="profileInfo">Profile information</param>
		/// <returns>X-position string</returns>
		public static string GetXPositionString(LJX8IF_PROFILE_INFO profileInfo)
		{
			StringBuilder sb = new StringBuilder();
			// Data position calculation
			double posX = profileInfo.lXStart;
			double deltaX = profileInfo.lXPitch;

			int singleProfileCount = profileInfo.nProfileDataCount;
			int dataCount = profileInfo.byProfileCount;

			for (int i = 0; i < dataCount; i++)
			{
				for (int j = 0; j < singleProfileCount; j++)
				{
					sb.AppendFormat("{0}\t", (posX + deltaX * j));
				}
			}
			return sb.ToString();
		}

		#endregion
	}
}
