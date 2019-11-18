﻿//----------------------------------------------------------------------------- 
// <copyright file="ThreadSafeBuffer.cs" company="KEYENCE">
//	 Copyright (c) 2019 KEYENCE CORPORATION. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

 using System.Collections.Generic;

 namespace LJXNative
{
	/// <summary>
	/// Thread-safe class for array storage
	/// </summary>
	public static class ThreadSafeBuffer
	{
		#region Constant
		private const int BatchFinalizeFlagBitCount = 16; 
		#endregion

		#region Field
		/// <summary>Data buffer</summary>
		private static List<int[]>[] _buffer = new List<int[]>[NativeMethods.DeviceCount];
		/// <summary>Buffer for the amount of data</summary>
		private static uint[] _count = new uint[NativeMethods.DeviceCount];
		/// <summary>Object for exclusive control</summary>
		private static object[] _syncObject = new object[NativeMethods.DeviceCount];
		/// <summary>Callback function notification parameter</summary>
		private static uint[] _notify = new uint[NativeMethods.DeviceCount];
		/// <summary>Batch number</summary>
		private static int[] _batchNo = new int[NativeMethods.DeviceCount];
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		static ThreadSafeBuffer()
		{
			for (int i = 0; i < NativeMethods.DeviceCount; i++)
			{
				_buffer[i] = new List<int[]>();
				_syncObject[i] = new object();
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// Get buffer data count
		/// </summary>
		/// <returns>buffer data count</returns>
		public static int GetBufferDataCount(int index)
		{
			return _buffer[index].Count;
		}

		/// <summary>
		/// Element addition
		/// </summary>
		/// <param name="index">User information set when high-speed communication was initialized</param>
		/// <param name="value">Additional element</param>
		/// <param name="notify">Parameter for notification</param>
		public static void Add(int index, List<int[]> value, uint notify)
		{
			lock (_syncObject[index])
			{
				_buffer[index].AddRange(value);
				_count[index] += (uint)value.Count;
				_notify[index] |= notify;
				// Add the batch number if the batch has been finalized.
				if ((notify & (0x1 << BatchFinalizeFlagBitCount)) != 0) _batchNo[index]++;
			}
		}

		/// <summary>
		/// Clear elements.
		/// </summary>
		/// <param name="index">Device ID</param>
		public static void Clear(int index)
		{
			lock (_syncObject[index])
			{
				_buffer[index].Clear();
			}
		}

		/// <summary>
		/// Clear the buffer.
		/// </summary>
		/// <param name="index">Device ID</param>
		public static void ClearBuffer(int index)
		{
			Clear(index);
			ClearCount(index);
			_batchNo[index] = 0;
			ClearNotify(index);
		}

		/// <summary>
		/// Get element.
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="notify">Parameter for notification</param>
		/// <param name="batchNo">Batch number</param>
		/// <returns>Element</returns>
		public static List<int[]> Get(int index, out uint notify, out int batchNo)
		{
			List<int[]> value = new List<int[]>();
			lock (_syncObject[index])
			{
				value.AddRange(_buffer[index]);
				_buffer[index].Clear();
				notify = _notify[index];
				_notify[index] = 0;
				batchNo = _batchNo[index];
			}
			return value;
		}

		/// <summary>
		/// Add the count
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="count">Count</param>
		/// <param name="notify">Parameter for notification</param>
		internal static void AddCount(int index, uint count, uint notify)
		{
			lock (_syncObject[index])
			{
				_count[index] += count;
				_notify[index] |= notify;
				// Add the batch number if the batch has been finalized.
				if ((notify & (0x1 << BatchFinalizeFlagBitCount)) != 0) _batchNo[index]++;
			}
		}

		/// <summary>
		/// Get the count
		/// </summary>
		/// <param name="index">Device ID</param>
		/// <param name="notify">Parameter for notification</param>
		/// <param name="batchNo">Batch number</param>
		/// <returns></returns>
		internal static uint GetCount(int index, out uint notify, out int batchNo)
		{
			lock (_syncObject[index])
			{
				notify = _notify[index];
				_notify[index] = 0;
				batchNo = _batchNo[index];
				return _count[index];
			}
		}

		/// <summary>
		/// Clear the number of elements.
		/// </summary>
		/// <param name="index">Device ID</param>
		private static void ClearCount(int index)
		{
			lock (_syncObject[index])
			{
				_count[index] = 0;
			}
		}

		/// <summary>
		/// Clear notifications.
		/// </summary>
		/// <param name="index">Device ID</param>
		private static void ClearNotify(int index)
		{
			lock (_syncObject[index])
			{
				_notify[index] = 0;
			}
		}

		#endregion
	}
}
