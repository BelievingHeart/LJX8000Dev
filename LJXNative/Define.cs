﻿//----------------------------------------------------------------------------- 
// <copyright file="Define.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
namespace LJXNative
{
	/// <summary>
	/// Constant class
	/// </summary>
	public static class Define
	{
		#region Constant

		public enum LjxHeadSamplingPeriod
		{
			LjxHeadSamplingPeriod10Hz = 0,
			LjxHeadSamplingPeriod20Hz,
			LjxHeadSamplingPeriod50Hz,
			LjxHeadSamplingPeriod100Hz,
			LjxHeadSamplingPeriod200Hz,
			LjxHeadSamplingPeriod500Hz,
			LjxHeadSamplingPeriod1KHz,
			LjxHeadSamplingPeriod2KHz,
			LjxHeadSamplingPeriod4KHz,
			LjxHeadSamplingPeriod8KHz,
			LjxHeadSamplingPeriod16KHz,
		}

		public enum LuminanceOutput
		{
			LuminanceOutputOn,
			LuminanceOutputOff
		}

		/// <summary>
		/// Maximum amount of data for 1 profile
		/// </summary>
		public const int MaxProfileCount = LjxHeadMeasureRangeFull;

		/// <summary>
		/// Device ID (fixed to 0)
		/// </summary>
		public const int DeviceId = 0;

		/// <summary>
		/// Maximum profile count that store to buffer.
		/// </summary>
#if WIN64
		public const int BufferFullCount = 120000;
#else
		public const int BufferFullCount = 30000;
#endif
// @Point
//  32-bit architecture cannot allocate huge memory and the buffer limit is more strict.

		/// <summary>
		/// Measurement range X direction of LJ-X Head
		/// </summary>
		public const int LjxHeadMeasureRangeFull = 3200;
		public const int LjxHeadMeasureRangeThreeFourth = 2400;
		public const int LjxHeadMeasureRangeHalf = 1600;
		public const int LjxHeadMeasureRangeQuarter = 800;

		/// <summary>
		/// Light reception characteristic
		/// </summary>
		public const int ReceivedBinningOff = 1;
		public const int ReceivedBinningOn = 2;

		public const int ThinningXOff = 1;
		public const int ThinningX2 = 2;
		public const int ThinningX4 = 4;


		/// <summary>
		/// Measurement range X direction of LJ-V Head
		/// </summary>
		public const int MeasureRangeFull = 800;
		public const int MeasureRangeMiddle = 600;
		public const int MeasureRangeSmall = 400;
		#endregion
	}	
}
