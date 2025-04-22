using System;
using System.Collections.Concurrent;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;

namespace Dissonance.Integrations.FMOD_Playback
{
    /// <summary>
    /// Reference counting lock system for FMOD Bus/ChannelGroups.
    /// </summary>
    internal class FMODChannelGroupLocks
        : MonoBehaviour
    {
        private static readonly Log Log = Logs.Create(LogCategory.Playback, nameof(FMODChannelGroupLocks));

        #region singleton
        private static FMODChannelGroupLocks _instance;

        public static FMODChannelGroupLocks Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("FMODChannelGroupLocks Singleton") {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    go.AddComponent<FMODChannelGroupLocks>();
                    _instance = go.GetComponent<FMODChannelGroupLocks>();
                }

                return _instance;
            }
        }

        [UsedImplicitly] private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;
        }

        [UsedImplicitly] private void OnDestroy()
        {
            if (this == _instance)
                _instance = null;

            ReleaseAll();
        }
        #endregion

        private readonly ConcurrentDictionary<GUID, int> _lockCounter = new ConcurrentDictionary<GUID, int>();

        private static bool Check(RESULT result, string message)
        {
            if (result != RESULT.OK)
            {
                Log.Warn(message + $" FMOD Result: {result}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lock the given bus and increment the lock count for this bus
        /// </summary>
        /// <param name="busID"></param>
        /// <returns>A handle that can be used to unlock the bus, or null if a lock could not be taken for some reason</returns>
        public Handle? LockBus(string busID)
        {
            if (string.IsNullOrEmpty(busID))
                return default;

            GUID busGUID;
            try
            {
                busGUID = GUID.Parse(busID);
            }
            catch (FormatException)
            {
                return default;
            }

            // Get the bus handle and it's unique ID from FMOD
            if (!Check(RuntimeManager.StudioSystem.getBusByID(busGUID, out var bus), $"Failed to get output bus `{busID}` from FMOD."))
                return default;
            if (!Check(bus.getID(out var guid), "Failed to get bus ID"))
                return default;

            // Lock the channel to ensure it exists. If the channel is already locked that's ok
            var lockResult = bus.lockChannelGroup();
            if (lockResult != RESULT.OK && lockResult != RESULT.ERR_ALREADY_LOCKED)
                if (!Check(bus.lockChannelGroup(), $"Failed to lock bus `{busID}` channel group."))
                    return default;

            // Increment the lock counter for this bus
            _lockCounter.AddOrUpdate(guid, 1, IncrementLockCount);

            // Flush commands to ensure that FMOD has processed that lock
            if (!Check(RuntimeManager.StudioSystem.flushCommands(), "Failed to flush FMOD commands."))
            {
                // Something has gone wrong if flushing doesn't work. Make a best effort attempt to unlock the bus.
                var count = _lockCounter.AddOrUpdate(guid, 0, DecrementLockCount);
                if (count == 0)
                    bus.unlockChannelGroup();

                return default;
            }

            // Now the bus channel group is certain to exist
            if (!Check(bus.getChannelGroup(out var group), $"Failed to get bus `{busID}` channel group."))
            {
                // Something has gone wrong if we can't get a channel group for the bus. Make a best effort attempt to unlock the bus.
                var count = _lockCounter.AddOrUpdate(guid, 0, DecrementLockCount);
                if (count == 0)
                    bus.unlockChannelGroup();

                return default;
            }

            return new Handle(guid, busID, bus, group);
        }

        /// <summary>
        /// Unlock a previously locked bus. Decrements the count and unlocks in FMOD if count is zero
        /// </summary>
        /// <param name="handle"></param>
        public void UnlockBus(Handle handle)
        {
            if (!handle.IsValid)
                return;

            var count = _lockCounter.AddOrUpdate(handle.GUID, 0, DecrementLockCount);
            if (count == 0)
            {
                var bus = handle.Bus;
                bus.unlockChannelGroup();
            }
        }

        private void ReleaseAll()
        {
            var locks = _lockCounter.ToList();
            _lockCounter.Clear();

            foreach (var item in locks)
                if (Check(RuntimeManager.StudioSystem.getBusByID(item.Key, out var bus), $"Failed to get Bus by ID `{bus}`"))
                    bus.unlockChannelGroup();
        }

        private static int DecrementLockCount(GUID id, int count)
        {
            return count - 1;
        }

        private static int IncrementLockCount(GUID _, int count)
        {
            return count + 1;
        }

        public struct Handle
        {
            public readonly bool IsValid;
            public readonly GUID GUID;
            public readonly string Name;
            public readonly Bus Bus;
            public readonly ChannelGroup ChannelGroup;
            
            internal Handle(GUID guid, string name, Bus bus, ChannelGroup channelGroup)
            {
                IsValid = true;
                GUID = guid;
                Name = name;
                Bus = bus;
                ChannelGroup = channelGroup;
            }
        }
    }
}
