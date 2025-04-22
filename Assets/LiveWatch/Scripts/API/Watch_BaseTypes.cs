#pragma warning disable CS0162
using System;
using System.Linq;
using System.Collections.Generic;
using Ingvar.LiveWatch;

namespace Ingvar.LiveWatch
{
	// It's completely generated class, avoid modifying!
	public static partial class Watch
	{
		private static string _tempStr;
		private static HashSet<string> _tempStrSet = new();
		private static Dictionary<object, HashSet<string>> _tempSetDict = new();

		#region System.Double

		public static WatchReference<System.Double> GetOrAdd(string path, Func<System.Double> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Double>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Double>();
#endif
		}

		public static WatchReference<System.Double> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Double> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Double, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Double>();
#endif
		}

		public static WatchReference<System.Double> Setup(WatchReference<System.Double> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Double);
#endif
			return watchReference;
		}

		public static WatchReference<System.Double> Push(WatchReference<System.Double> watchReference, System.Double value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushDouble(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Double> Push(string path, System.Double value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Double>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Single

		public static WatchReference<System.Single> GetOrAdd(string path, Func<System.Single> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Single>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Single>();
#endif
		}

		public static WatchReference<System.Single> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Single> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Single, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Single>();
#endif
		}

		public static WatchReference<System.Single> Setup(WatchReference<System.Single> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Float);
#endif
			return watchReference;
		}

		public static WatchReference<System.Single> Push(WatchReference<System.Single> watchReference, System.Single value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushFloat(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Single> Push(string path, System.Single value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Int32

		public static WatchReference<System.Int32> GetOrAdd(string path, Func<System.Int32> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int32>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int32>();
#endif
		}

		public static WatchReference<System.Int32> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Int32> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int32>();
#endif
		}

		public static WatchReference<System.Int32> Setup(WatchReference<System.Int32> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Int);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int32> Push(WatchReference<System.Int32> watchReference, System.Int32 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushInt(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int32> Push(string path, System.Int32 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.String

		public static WatchReference<System.String> GetOrAdd(string path, Func<System.String> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.String>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.String>();
#endif
		}

		public static WatchReference<System.String> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.String> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.String, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.String>();
#endif
		}

		public static WatchReference<System.String> Setup(WatchReference<System.String> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.String> Push(WatchReference<System.String> watchReference, System.String value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushString(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.String> Push(string path, System.String value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.String>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Boolean

		public static WatchReference<System.Boolean> GetOrAdd(string path, Func<System.Boolean> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Boolean>();
#endif
		}

		public static WatchReference<System.Boolean> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Boolean> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Boolean>();
#endif
		}

		public static WatchReference<System.Boolean> Setup(WatchReference<System.Boolean> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Bool);
#endif
			return watchReference;
		}

		public static WatchReference<System.Boolean> Push(WatchReference<System.Boolean> watchReference, System.Boolean value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushBool(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Boolean> Push(string path, System.Boolean value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Decimal

		public static WatchReference<System.Decimal> GetOrAdd(string path, Func<System.Decimal> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Decimal>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Decimal>();
#endif
		}

		public static WatchReference<System.Decimal> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Decimal> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Decimal, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Decimal>();
#endif
		}

		public static WatchReference<System.Decimal> Setup(WatchReference<System.Decimal> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Double);
#endif
			return watchReference;
		}

		public static WatchReference<System.Decimal> Push(WatchReference<System.Decimal> watchReference, System.Decimal value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushDecimal(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Decimal> Push(string path, System.Decimal value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Decimal>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Int64

		public static WatchReference<System.Int64> GetOrAdd(string path, Func<System.Int64> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int64>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int64>();
#endif
		}

		public static WatchReference<System.Int64> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Int64> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int64, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int64>();
#endif
		}

		public static WatchReference<System.Int64> Setup(WatchReference<System.Int64> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Double);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int64> Push(WatchReference<System.Int64> watchReference, System.Int64 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushLong(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int64> Push(string path, System.Int64 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int64>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Int16

		public static WatchReference<System.Int16> GetOrAdd(string path, Func<System.Int16> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int16>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int16>();
#endif
		}

		public static WatchReference<System.Int16> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Int16> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Int16, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Int16>();
#endif
		}

		public static WatchReference<System.Int16> Setup(WatchReference<System.Int16> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Int);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int16> Push(WatchReference<System.Int16> watchReference, System.Int16 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushShort(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Int16> Push(string path, System.Int16 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int16>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Byte

		public static WatchReference<System.Byte> GetOrAdd(string path, Func<System.Byte> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Byte>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Byte>();
#endif
		}

		public static WatchReference<System.Byte> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Byte> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Byte, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Byte>();
#endif
		}

		public static WatchReference<System.Byte> Setup(WatchReference<System.Byte> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Int);
#endif
			return watchReference;
		}

		public static WatchReference<System.Byte> Push(WatchReference<System.Byte> watchReference, System.Byte value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushByte(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Byte> Push(string path, System.Byte value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Byte>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.UInt64

		public static WatchReference<System.UInt64> GetOrAdd(string path, Func<System.UInt64> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.UInt64>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.UInt64>();
#endif
		}

		public static WatchReference<System.UInt64> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.UInt64> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.UInt64, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.UInt64>();
#endif
		}

		public static WatchReference<System.UInt64> Setup(WatchReference<System.UInt64> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Double);
#endif
			return watchReference;
		}

		public static WatchReference<System.UInt64> Push(WatchReference<System.UInt64> watchReference, System.UInt64 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushULong(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.UInt64> Push(string path, System.UInt64 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.UInt64>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.UInt16

		public static WatchReference<System.UInt16> GetOrAdd(string path, Func<System.UInt16> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.UInt16>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.UInt16>();
#endif
		}

		public static WatchReference<System.UInt16> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.UInt16> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.UInt16, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.UInt16>();
#endif
		}

		public static WatchReference<System.UInt16> Setup(WatchReference<System.UInt16> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Int);
#endif
			return watchReference;
		}

		public static WatchReference<System.UInt16> Push(WatchReference<System.UInt16> watchReference, System.UInt16 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushUShort(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.UInt16> Push(string path, System.UInt16 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.UInt16>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.SByte

		public static WatchReference<System.SByte> GetOrAdd(string path, Func<System.SByte> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.SByte>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.SByte>();
#endif
		}

		public static WatchReference<System.SByte> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.SByte> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.SByte, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.SByte>();
#endif
		}

		public static WatchReference<System.SByte> Setup(WatchReference<System.SByte> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.Int);
#endif
			return watchReference;
		}

		public static WatchReference<System.SByte> Push(WatchReference<System.SByte> watchReference, System.SByte value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushSByte(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.SByte> Push(string path, System.SByte value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.SByte>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Char

		public static WatchReference<System.Char> GetOrAdd(string path, Func<System.Char> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Char>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Char>();
#endif
		}

		public static WatchReference<System.Char> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Char> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Char, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Char>();
#endif
		}

		public static WatchReference<System.Char> Setup(WatchReference<System.Char> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Char> Push(WatchReference<System.Char> watchReference, System.Char value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushChar(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Char> Push(string path, System.Char value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Char>(path), value, maxRecursionDepth);
		}

		#endregion
	}
}
