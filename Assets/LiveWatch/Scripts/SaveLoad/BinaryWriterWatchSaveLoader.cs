using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public class BinaryWriterWatchSaveLoader : WatchSaveLoaderBase
    {
        public override string Extension => "watch";
        public static string SerializationVersion => "1.0.1";

        private List<WatchVariable> _tempVariables = new();
        private CancellationTokenSource _cancellation;

        public override void Save(string path, WatchStorage watches, Action<bool, string> onFinish)
        {
            if (IsSaving)
                CancelSave();

            Progress.Clear();
            _cancellation = new CancellationTokenSource();
            
            Task.Run(() =>
            {
                var result = Saving(path, watches, out var message);
                onFinish?.Invoke(result, message);
            });
        }

        public override void CancelSave()
        {
            if (!IsSaving)
                return;
            
            _cancellation?.Cancel();
        }

        private bool Saving(string path, WatchStorage watches, out string message)
        {
            IsSaving = true;

            var succeed = false;
            message = string.Empty;

            while (WatchServices.VariableUpdater.IsUpdatingNow)
                Thread.SpinWait(1);

            var maxValuesCount = WatchServices.VariableUpdater.ValuesCount;
            
            watches.GetAllChildRecursive(
                _tempVariables, 
                WatchFilters.None, 
                WatchFilters.None);
                
            foreach (var variables in _tempVariables)
            {
                if (_cancellation.Token.IsCancellationRequested)
                    return false;
                    
                Progress.MaxCount += Mathf.Min(variables.Values.OriginalKeys.Count, maxValuesCount);
            }

            try
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                using var writer = new BinaryWriter(fs, Encoding.UTF8);
                writer.Write(SerializationVersion);
                writer.Write(Progress.MaxCount);
                writer.Write(Watch.Watches, maxValuesCount, _cancellation.Token, Progress);
                succeed = !_cancellation.IsCancellationRequested;
            }
            catch (Exception e)
            {
                message = $"Failed to save watches to a binary file. Path: {path}. Error: {e.Message}. Trace: {e.StackTrace}";
                Debug.LogError(message);
            }

            if (succeed)
            {
                message = $"Succeed to save watches to a binary file. Path: {path}";
                Debug.Log(message);
            }
            
            IsSaving = false;
            return succeed;
        }

        public override void Load(string path, Action<bool, WatchStorage, string> onFinish)
        {
            if (IsLoading)
                CancelLoad();
            
            Progress.Clear();
            _cancellation = new CancellationTokenSource();
           
            Task.Run(() =>
            {
                var result = Loading(path, out var loadedWatches, out var message);
                onFinish?.Invoke(result, loadedWatches, message);
            });
        }

        public override void CancelLoad()
        {
            if (!IsLoading)
                return;
            
            _cancellation?.Cancel();
        }

        private bool Loading(string path, out WatchStorage watches, out string message)
        {
            IsLoading = true;

            watches = default;
            message = string.Empty;
            var succeed = false;
            
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                using var reader = new BinaryReader(fs, Encoding.UTF8);
                var version = reader.ReadString();

                if (version != SerializationVersion)
                {
                    message = $"Failed to load watches from a binary file. Version mismatch! Current version: {SerializationVersion}. File version: {version}. Path: {path}";
                    Debug.LogError(message);
                }
                else
                {
                    Progress.MaxCount = reader.ReadInt64();
                    watches = reader.ReadWatchStorage(_cancellation.Token, Progress);
                    succeed = !_cancellation.IsCancellationRequested;
                }
            }
            catch (Exception e)
            {
                message = $"Failed to load watches from a binary file. Path: {path}. Error: {e.Message}. Trace: {e.StackTrace}";
                Debug.LogError(message);
            }
            
            IsLoading = false;

            if (succeed)
            {
                message = $"Succeed to read watches from a binary file. Path: {path}";
                Debug.Log(message);
            }
            
            return succeed;
        }
    }
}