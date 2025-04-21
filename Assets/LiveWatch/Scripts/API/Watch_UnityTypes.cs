#pragma warning disable CS0162
using System;
using System.Linq;
using System.Collections.Generic;
using Ingvar.LiveWatch;

namespace Ingvar.LiveWatch
{
	// It's completely generated class, avoid modifying!
	public static  class WatchUnity
	{
		private static string _tempStr;
		private static HashSet<string> _tempStrSet = new();
		private static Dictionary<object, HashSet<string>> _tempSetDict = new();

		#region UnityEngine.Vector2

		public static WatchReference<UnityEngine.Vector2> GetOrAdd(string path, Func<UnityEngine.Vector2> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector2>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector2>();
#endif
		}

		public static WatchReference<UnityEngine.Vector2> GetOrAdd<T>(WatchReference<T> parent, string path, Func<UnityEngine.Vector2> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector2, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector2>();
#endif
		}

		public static WatchReference<UnityEngine.Vector2> Setup(WatchReference<UnityEngine.Vector2> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector2> Push(WatchReference<UnityEngine.Vector2> watchReference, UnityEngine.Vector2 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector2>(watchReference, nameof(UnityEngine.Vector2.x)), value.x, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector2>(watchReference, nameof(UnityEngine.Vector2.y)), value.y, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector2> Push(string path, UnityEngine.Vector2 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector2>(path), value, maxRecursionDepth);
		}

		#endregion

		#region UnityEngine.Vector3

		public static WatchReference<UnityEngine.Vector3> GetOrAdd(string path, Func<UnityEngine.Vector3> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector3>();
#endif
		}

		public static WatchReference<UnityEngine.Vector3> GetOrAdd<T>(WatchReference<T> parent, string path, Func<UnityEngine.Vector3> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector3>();
#endif
		}

		public static WatchReference<UnityEngine.Vector3> Setup(WatchReference<UnityEngine.Vector3> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector3> Push(WatchReference<UnityEngine.Vector3> watchReference, UnityEngine.Vector3 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector3>(watchReference, nameof(UnityEngine.Vector3.x)), value.x, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector3>(watchReference, nameof(UnityEngine.Vector3.y)), value.y, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector3>(watchReference, nameof(UnityEngine.Vector3.z)), value.z, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector3> Push(string path, UnityEngine.Vector3 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3>(path), value, maxRecursionDepth);
		}

		#endregion

		#region UnityEngine.Vector4

		public static WatchReference<UnityEngine.Vector4> GetOrAdd(string path, Func<UnityEngine.Vector4> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector4>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector4>();
#endif
		}

		public static WatchReference<UnityEngine.Vector4> GetOrAdd<T>(WatchReference<T> parent, string path, Func<UnityEngine.Vector4> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector4, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Vector4>();
#endif
		}

		public static WatchReference<UnityEngine.Vector4> Setup(WatchReference<UnityEngine.Vector4> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector4> Push(WatchReference<UnityEngine.Vector4> watchReference, UnityEngine.Vector4 value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector4>(watchReference, nameof(UnityEngine.Vector4.w)), value.w, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector4>(watchReference, nameof(UnityEngine.Vector4.x)), value.x, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector4>(watchReference, nameof(UnityEngine.Vector4.y)), value.y, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, UnityEngine.Vector4>(watchReference, nameof(UnityEngine.Vector4.z)), value.z, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Vector4> Push(string path, UnityEngine.Vector4 value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector4>(path), value, maxRecursionDepth);
		}

		#endregion

		#region UnityEngine.Transform

		public static WatchReference<UnityEngine.Transform> GetOrAdd(string path, Func<UnityEngine.Transform> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Transform>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Transform>();
#endif
		}

		public static WatchReference<UnityEngine.Transform> GetOrAdd<T>(WatchReference<T> parent, string path, Func<UnityEngine.Transform> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Transform, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<UnityEngine.Transform>();
#endif
		}

		public static WatchReference<UnityEngine.Transform> Setup(WatchReference<UnityEngine.Transform> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Transform> Push(WatchReference<UnityEngine.Transform> watchReference, UnityEngine.Transform value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, nameof(UnityEngine.Transform.localPosition)), value.localPosition, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, "localRotation"), value.localEulerAngles, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, nameof(UnityEngine.Transform.localScale)), value.localScale, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, nameof(UnityEngine.Transform.position)), value.position, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, "rotation"), value.eulerAngles, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, UnityEngine.Transform>(watchReference, "scale"), value.lossyScale, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<UnityEngine.Transform> Push(string path, UnityEngine.Transform value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Transform>(path), value, maxRecursionDepth);
		}

		#endregion
	}
}
