﻿//----------------------------------------------------------------------------- 
// <copyright file="DeviceData.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System.Collections.Generic;

 namespace LJXNative.Data
{
	#region Enum
	/// <summary>Device communication state</summary>
	public enum DeviceStatus
	{
		NoConnection = 0,
		Ethernet,
		EthernetFast,
	};
	#endregion

	/// <summary>
	/// Device data class
	/// </summary>
	public class DeviceData
	{
		#region Field
		/// <summary>Connection state</summary>
		private DeviceStatus _status = DeviceStatus.NoConnection;

		#endregion

		#region Property
		/// <summary>
		/// Status property
		/// </summary>
		public DeviceStatus Status
		{ 
			get { return _status; }
			set
			{
				ProfileData.Clear();
				ProfileDataHighSpeed.Clear();
				SimpleArrayData.Clear();
				SimpleArrayDataHighSpeed.Clear();
				EthernetConfig = new LJX8IF_ETHERNET_CONFIG();
				_status = value; 
			} 
		}

		/// <summary>Ethernet settings</summary>
		public LJX8IF_ETHERNET_CONFIG EthernetConfig { get; set; }
		/// <summary>Profile data</summary>
		public List<ProfileData> ProfileData { get; }
		/// <summary>Profile data for high speed communication</summary>
		public List<ProfileData> ProfileDataHighSpeed { get; }
		/// <summary>Simple array data</summary>
		public ProfileSimpleArrayStore SimpleArrayData { get; }
		/// <summary>Simple array data for high speed communication</summary>
		public ProfileSimpleArrayStore SimpleArrayDataHighSpeed { get; }
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public DeviceData()
		{
			EthernetConfig = new LJX8IF_ETHERNET_CONFIG();
			ProfileData = new List<ProfileData>();
			ProfileDataHighSpeed = new List<ProfileData>();
			SimpleArrayData = new ProfileSimpleArrayStore(); ;
			SimpleArrayDataHighSpeed = new ProfileSimpleArrayStore(); ;
		}
		#endregion

		#region Method
		/// <summary>
		/// Connection status acquisition
		/// </summary>
		/// <returns>Connection status for display</returns>
		public string GetStatusString()
		{
			string status = _status.ToString();
			switch (_status)
			{			
			case DeviceStatus.Ethernet:
			case DeviceStatus.EthernetFast:
				status += string.Format("---{0}.{1}.{2}.{3}", EthernetConfig.abyIpAddress[0], EthernetConfig.abyIpAddress[1],
					EthernetConfig.abyIpAddress[2], EthernetConfig.abyIpAddress[3]);
				break;
			}
			return status;
		}
		#endregion
	}
}
