#pragma warning disable CS0162
using System;
using System.Linq;
using System.Collections.Generic;
using Ingvar.LiveWatch;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
	// It's completely generated class, avoid modifying!
	public static  class TD_Watches
	{
		private static string _tempStr;
		private static HashSet<string> _tempStrSet = new();
		private static Dictionary<object, HashSet<string>> _tempSetDict = new();

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobManager

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobManager>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobManager, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobManager value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>, Ingvar.LiveWatch.TowerDefenceDemo.MobManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobManager.Mobs)), value.Mobs, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobManager> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobManager value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobManager>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> GetOrAdd(string path, Func<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> Setup(WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> Push(WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> watchReference, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(watchReference, nameof(System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>.Count)), value.Count, maxRecursionDepth - 1);

			var counter = 0;
			if (!_tempSetDict.TryGetValue(value, out var strSet))
				_tempSetDict.Add(value, strSet = new HashSet<string>());
			else
				strSet.Clear();

			foreach (var pair in value)
			{
				var str = pair.Key;
				strSet.Add(str);

				var valueWatch = Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain,System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(watchReference, str), pair.Value, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);

				if (++counter >= 100)
					break;
			}

			foreach (var childName in watchReference.GetChildNames())
			{
				if (!strSet.Contains(childName) && WatchServices.ReferenceCreator.IsDictionaryValue(WatchServices.ReferenceCreator.GetOrAdd<Any, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(watchReference, childName)))
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain,System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(watchReference, childName), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>> Push(string path, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobMain

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobMain>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobMain>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (!WatchServices.ReferenceCreator.IsSetUp(watchReference))
			{
				WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.CurrentHealth)).SetMinMaxModeAsCustom(0,20);
				WatchServices.ReferenceCreator.GetOrAdd<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.Id)).SetSortOrder(-1);
				WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.WaypointMover)).SetAlwaysCollapsable();
			}
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobMain value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.CurrentHealth)), value.CurrentHealth, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.String, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.Id)), value.Id, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.IsAlive)), value.IsAlive, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.SpawnTime)), value.SpawnTime, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobType, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.Type)), value.Type, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover, Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobMain.WaypointMover)), value.WaypointMover, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobMain> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobMain value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobType

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobType>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobType, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobType value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushString(watchReference, WatchServices.NameBuilder.GetStringFromEnum<Ingvar.LiveWatch.TowerDefenceDemo.MobType>((int)value));
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobType> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobType value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobType>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (!WatchServices.ReferenceCreator.IsSetUp(watchReference))
			{
				WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover.TravelledDist)).SetDecimalPlaces(4);
			}
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover.RotationSpeed)), value.RotationSpeed, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover.Speed)), value.Speed, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover.TravelledDist)), value.TravelledDist, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWaypointMover>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushString(watchReference, WatchServices.NameBuilder.GetStringFromEnum<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType>((int)value));
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelStateType>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.HealthManager

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.HealthManager value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.HealthManager.CurrentHealth)), value.CurrentHealth, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.HealthManager.MaxHealth)), value.MaxHealth, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.HealthManager value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.HealthManager>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.WaveManager

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.WaveManager.IsActiveWave)), value.IsActiveWave, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.WaveManager.MaxWave)), value.MaxWave, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(watchReference, "SpawnIndex"), value.CurrentSpawn, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(watchReference, "WaveIndex"), value.CurrentWave, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.WaveManager value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.WaveManager>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager.CurrentEnergy)), value.CurrentEnergy, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EnergyManager>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager.Slots)), value.Slots, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildManager>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> GetOrAdd(string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> Setup(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> Push(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> watchReference, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>(watchReference, nameof(System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>.Count)), value.Count, maxRecursionDepth - 1);

			var index = 0;

			foreach (var item in value)
			{
				var elementWatch = Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), item, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsCollectionValue(elementWatch);

				if (++index >= 100)
					break;
			}

			if (watchReference.ChildCount - 1 > index)
			{
				for (; index < watchReference.ChildCount - 1; index++)
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>> Push(string path, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<UnityEngine.Vector3, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot.BuildLocation)), value.BuildLocation, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot.Id)), value.Id, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Boolean, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot.IsOccupied)), value.IsOccupied, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot.Tower)), value.Tower, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBuildSlot>(path), value, maxRecursionDepth);
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

		#region Ingvar.LiveWatch.TowerDefenceDemo.TowerBase

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBase.Id)), value.Id, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.TowerBase.Type)), value.Type, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.TowerType

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.TowerType value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			WatchServices.ReferenceCreator.PushString(watchReference, WatchServices.NameBuilder.GetStringFromEnum<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>((int)value));
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.TowerType> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.TowerType value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> GetOrAdd(string path, Func<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> Setup(WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> Push(WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> watchReference, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(watchReference, nameof(System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>.Count)), value.Count, maxRecursionDepth - 1);

			var counter = 0;
			if (!_tempSetDict.TryGetValue(value, out var strSet))
				_tempSetDict.Add(value, strSet = new HashSet<string>());
			else
				strSet.Clear();

			foreach (var pair in value)
			{
				var str = pair.Key;
				strSet.Add(str);

				var valueWatch = Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase,System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(watchReference, str), pair.Value, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);

				if (++counter >= 100)
					break;
			}

			foreach (var childName in watchReference.GetChildNames())
			{
				if (!strSet.Contains(childName) && WatchServices.ReferenceCreator.IsDictionaryValue(WatchServices.ReferenceCreator.GetOrAdd<Any, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(watchReference, childName)))
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.TowerBase,System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(watchReference, childName), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>> Push(string path, System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<System.String, Ingvar.LiveWatch.TowerDefenceDemo.TowerBase>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig.MaxHealth)), value.MaxHealth, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig.StartGold)), value.StartGold, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>, Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig.Waves)), value.Waves, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.LevelConfig>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> GetOrAdd(string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> Setup(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> Push(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> watchReference, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>(watchReference, nameof(System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>.Count)), value.Count, maxRecursionDepth - 1);

			var index = 0;

			foreach (var item in value)
			{
				var elementWatch = Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWave,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), item, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsCollectionValue(elementWatch);

				if (++index >= 100)
					break;
			}

			if (watchReference.ChildCount - 1 > index)
			{
				for (; index < watchReference.ChildCount - 1; index++)
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWave,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>> Push(string path, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobWave

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWave, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobWave value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>, Ingvar.LiveWatch.TowerDefenceDemo.MobWave>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobWave.Spawns)), value.Spawns, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobWave> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobWave value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobWave>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> GetOrAdd(string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> Setup(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> Push(WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> watchReference, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>(watchReference, nameof(System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>.Count)), value.Count, maxRecursionDepth - 1);

			var index = 0;

			foreach (var item in value)
			{
				var elementWatch = Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), item, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsCollectionValue(elementWatch);

				if (++index >= 100)
					break;
			}

			if (watchReference.ChildCount - 1 > index)
			{
				for (; index < watchReference.ChildCount - 1; index++)
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn,System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>(watchReference, WatchServices.NameBuilder.GetCollectionItemName(index)), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>> Push(string path, System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.List<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn.Delay)), value.Delay, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn.MobCount)), value.MobCount, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobMain, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn.MobPrefab)), value.MobPrefab, maxRecursionDepth - 1);
			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Single, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn.SpawnDelayBetween)), value.SpawnDelayBetween, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.MobSpawn>(path), value, maxRecursionDepth);
		}

		#endregion

		#region Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> GetOrAdd(string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> GetOrAdd<T>(WatchReference<T> parent, string path, Func<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>();
#endif
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> Setup(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> Push(WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> watchReference, Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>, Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig.MobKillRewards)), value.MobKillRewards, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>, Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig.TowerBuildCosts)), value.TowerBuildCosts, maxRecursionDepth - 1);
			Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>, Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>(watchReference, nameof(Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig.TowerSellPrices)), value.TowerSellPrices, maxRecursionDepth - 1);

			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig> Push(string path, Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<Ingvar.LiveWatch.TowerDefenceDemo.EconomyConfig>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> GetOrAdd(string path, Func<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> Setup(WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> Push(WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> watchReference, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(watchReference, nameof(System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>.Count)), value.Count, maxRecursionDepth - 1);

			var counter = 0;
			if (!_tempSetDict.TryGetValue(value, out var strSet))
				_tempSetDict.Add(value, strSet = new HashSet<string>());
			else
				strSet.Clear();

			foreach (var pair in value)
			{
				var str = WatchServices.NameBuilder.GetStringFromEnum<Ingvar.LiveWatch.TowerDefenceDemo.MobType>((int)pair.Key);
				strSet.Add(str);

				var valueWatch = Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32,System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(watchReference, str), pair.Value, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);

				if (++counter >= 100)
					break;
			}

			foreach (var childName in watchReference.GetChildNames())
			{
				if (!strSet.Contains(childName) && WatchServices.ReferenceCreator.IsDictionaryValue(WatchServices.ReferenceCreator.GetOrAdd<Any, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(watchReference, childName)))
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<System.Int32,System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(watchReference, childName), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>> Push(string path, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.MobType, System.Int32>>(path), value, maxRecursionDepth);
		}

		#endregion

		#region System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> GetOrAdd(string path, Func<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> GetOrAdd<T>(WatchReference<T> parent, string path, Func<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> valueGetter)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			var watchReference = Setup(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>, T>(parent, path));

			WatchServices.ReferenceCreator.SetUpdateCall(watchReference, () =>
			{
				var value = valueGetter();
				Push(watchReference, value);
			});

			return watchReference;
#else
			return WatchServices.ReferenceCreator.Empty<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>();
#endif
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> Setup(WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> watchReference)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			return WatchServices.ReferenceCreator.TrySetupAs(watchReference, WatchValueType.String);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> Push(WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> watchReference, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32> value, int maxRecursionDepth = 10)
		{
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
			if (maxRecursionDepth <= 0 || !Watch.IsLive || WatchServices.ReferenceCreator.IsInvalidType(watchReference))
				return watchReference;

			Setup(watchReference);
			if (value == null)
				return WatchServices.ReferenceCreator.PushNull(watchReference, maxRecursionDepth - 1);

			Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(watchReference, nameof(System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>.Count)), value.Count, maxRecursionDepth - 1);

			var counter = 0;
			if (!_tempSetDict.TryGetValue(value, out var strSet))
				_tempSetDict.Add(value, strSet = new HashSet<string>());
			else
				strSet.Clear();

			foreach (var pair in value)
			{
				var str = WatchServices.NameBuilder.GetStringFromEnum<Ingvar.LiveWatch.TowerDefenceDemo.TowerType>((int)pair.Key);
				strSet.Add(str);

				var valueWatch = Watch.Push(WatchServices.ReferenceCreator.GetOrAdd<System.Int32,System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(watchReference, str), pair.Value, maxRecursionDepth - 1);
				WatchServices.ReferenceCreator.MarkAsDictionaryValue(valueWatch);

				if (++counter >= 100)
					break;
			}

			foreach (var childName in watchReference.GetChildNames())
			{
				if (!strSet.Contains(childName) && WatchServices.ReferenceCreator.IsDictionaryValue(WatchServices.ReferenceCreator.GetOrAdd<Any, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(watchReference, childName)))
					WatchServices.ReferenceCreator.PushEmpty(WatchServices.ReferenceCreator.GetOrAdd<System.Int32,System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(watchReference, childName), true, maxRecursionDepth - 1);
			}
			WatchServices.ReferenceCreator.PushNonBasic(watchReference, value);
#endif
			return watchReference;
		}

		public static WatchReference<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>> Push(string path, System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32> value, int maxRecursionDepth = 10)
		{
			return Push(WatchServices.ReferenceCreator.GetOrAdd<System.Collections.Generic.Dictionary<Ingvar.LiveWatch.TowerDefenceDemo.TowerType, System.Int32>>(path), value, maxRecursionDepth);
		}

		#endregion
	}
}
