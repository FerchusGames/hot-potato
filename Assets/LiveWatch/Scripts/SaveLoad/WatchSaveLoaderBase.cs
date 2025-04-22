using System;

namespace Ingvar.LiveWatch
{
    public abstract class WatchSaveLoaderBase
    {
        public bool IsSaving { get; protected set; }
        public bool IsLoading { get; protected set; }
        public ProcessProgress Progress { get; } = new();
        public abstract string Extension { get; }
        public abstract void Save(string path, WatchStorage watches, Action<bool, string> onFinish);
        public abstract void CancelSave();
        public abstract void Load(string path, Action<bool, WatchStorage, string> onFinish);
        public abstract void CancelLoad();
    }
}