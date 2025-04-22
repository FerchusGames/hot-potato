using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    [DefaultExecutionOrder(-99999)]
    public class TD_WatchManager : MonoBehaviour
    {
        private void Awake()
        {
            Watch.DestroyAll();

            Watch.GetOrAdd("Frame", () => Time.frameCount)
                .SetAlwaysCollapsable()
                .SetSortOrder(TD_WatchSortOrder.Frame);

            Watch.GetOrAdd("FPS", () => (int)(1f / Time.unscaledDeltaTime))
                .SetAlwaysCollapsable()
                .SetSortOrder(TD_WatchSortOrder.FPS)
                .AddConditionalValueFormat((fps) => fps < 30, WatchValueFormat.Red)
                .AddConditionalValueFormat((fps) => fps is >= 30 and < 50, WatchValueFormat.Yellow)
                .AddConditionalValueFormat((fps) => fps >= 50, WatchValueFormat.Green);

            Watch.GetOrAdd("TimeScale", () => Time.timeScale)
                .SetAlwaysCollapsable()
                .SetSortOrder(TD_WatchSortOrder.TimeScale);
        }
        
        private void LateUpdate()
        {
            Watch.UpdateAll();
        }
    }
}
