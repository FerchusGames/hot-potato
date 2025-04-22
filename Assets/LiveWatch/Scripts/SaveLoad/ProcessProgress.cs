using UnityEngine;

namespace Ingvar.LiveWatch
{
    public class ProcessProgress
    {
        public long CurrentCount { get; set; }
        public long MaxCount { get; set; }
        public string StatusText { get; set; }
        public float Progress => MaxCount == 0 ? 0 : Mathf.Clamp01((float)CurrentCount / MaxCount);

        public void Clear()
        {
            CurrentCount = 0;
            MaxCount = 0;
            StatusText = string.Empty;
        }
    }
}