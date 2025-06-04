using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils.DataAPI
{
    public struct GraphQlQuery
    {
        public string query;
        public object variables;
    }
    
    public class GraphqlUtils
    {
        private const string URL = "http://localhost:4000/graphql";
        private const string requestName = "Content-Type";
        private const string requestType = "application/json";
        private static JObject GetData(JObject data) => data == null ? null : JObject.Parse(data["data"].ToString());

        public static async UniTask<T> GetModel<T>(GraphQlQuery query, string queryName, CancellationToken token)
        {
            T modelData = default;
            var webRequest = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbPOST);
            var json = JsonConvert.SerializeObject(query);
            var payload = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(payload);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader(requestName, requestType);

            await webRequest.SendWebRequest().WithCancellation(token);

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"GraphQL Error: {webRequest.error}");
                return modelData;
            }

            var data = JObject.Parse(webRequest.downloadHandler.text);
            var dataObject = GetData(data);

            if (dataObject?[queryName] != null)
            {
                modelData = dataObject[queryName].ToObject<T>();
            }
            else
            {
                Debug.LogWarning($"Query result '{queryName}' was not found in the response.");
            }

            return modelData;
        }
    }
}