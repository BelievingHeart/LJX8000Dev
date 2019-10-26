﻿//----------------------------------------------------------------------------- 
// <copyright file="DataExporter.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System;
 using System.Collections.Generic;
 using System.Diagnostics;
 using System.IO;
 using System.Text;
 using LJXNative.Data;

 namespace LJXNative
{
	/// <summary>
	/// Data export class
	/// </summary>
	public static class DataExporter
	{
		#region Constant
		private const string HeightDataAdditionalFilePath = "_height.csv";
		private const string LuminanceDataAdditionalFilePath = "_luminance.csv";
		private const int ProfileDataStartIndex = 0;
		#endregion

		#region Method
		/// <summary>
		/// Output profile data to a file.
		/// </summary>
		/// <param name="profileDataList">Profile data to output</param>
		/// <param name="savePath">Full path to the file to save</param>
		/// <remarks>Output data in TSV format.</remarks>
		public static bool SaveProfile(List<ProfileData> profileDataList, string savePath)
		{
			try
			{
				if (string.IsNullOrEmpty(savePath)) return false;
				if (profileDataList.Count == 0) return false;
				string fileNameForHeight = GetDeterminantFilePath(savePath, HeightDataAdditionalFilePath);
				ExportMultipleData(profileDataList, fileNameForHeight, ProfileDataStartIndex);

				if (!ProfileData.GetIsLuminanceOutput(profileDataList[0].ProfInfo)) return true;
				string fileNameForLuminance = GetDeterminantFilePath(savePath, LuminanceDataAdditionalFilePath);
				int luminanceOutputDataStartIndex = profileDataList[0].ProfInfo.nProfileDataCount;
				ExportMultipleData(profileDataList, fileNameForLuminance, luminanceOutputDataStartIndex);
				return true;
			}
			catch (Exception)
			{
				Debugger.Break();
				return false;
			}
		}

		/// <summary>
		/// Export processing 
		/// </summary>
		/// <param name="profileDataList">Profile data list</param>
		/// <param name="savePath">Save file path</param>
		/// <param name="startIndex">Data start index for saving in one profile</param>
		private static void ExportMultipleData(List<ProfileData> profileDataList, string savePath, int startIndex)
		{
			// Save the profile
			using (StreamWriter streamWriter = new StreamWriter(savePath, false, Encoding.GetEncoding("utf-16")))
			{
				// Output the data of each profile
				foreach (ProfileData profile in profileDataList)
				{
					StringBuilder stringBuilder = new StringBuilder();
					short profileDataCount = profile.ProfInfo.nProfileDataCount;

					for (int i = 0; i < profileDataCount; i++)
					{
						stringBuilder.AppendFormat("{0}\t", profile.ProfData[startIndex + i]);
					}
					streamWriter.WriteLine(stringBuilder);
				}
			}
		}

		/// <summary>
		/// Get file path
		/// </summary>
		/// <param name="fileName">original file name</param>
		/// <param name="fileAdditionalName">file name addition to original name</param>
		/// <returns>file path</returns>
		private static string GetDeterminantFilePath(string fileName, string fileAdditionalName)
		{
			string fileDirectoryPath = Path.GetDirectoryName(fileName);
			string fileNameExceptFileExtension = Path.GetFileNameWithoutExtension(fileName);
			string filePathExceptFileExtension = Path.Combine(fileDirectoryPath, fileNameExceptFileExtension);
			return filePathExceptFileExtension + fileAdditionalName;
		}

		#endregion
	}
}
