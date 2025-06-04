using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Utils.AddressableLoader
{
    public static class AddressableLoader
    {
        public static async UniTask<T> InstantiateAsync<T>(string styleName)
        {
            var task = Addressables.InstantiateAsync(styleName).Task.AsUniTask();
            var asset = await task;
            return asset.GetComponent<T>();
        }
    }
}