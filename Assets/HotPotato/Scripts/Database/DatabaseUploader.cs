using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Modulo {
    public int numero;
    public int color;
    public int tipo;
    public int letra;
    public int esTrampa;
}

[System.Serializable]
public class Pista {
    public int tipoValor;
    public int valor;
    public int noTrampas;
}

[System.Serializable]
public class RoundData {
    public int resultado;
    public string fecha;
    public List<Modulo> modulos;
    public List<Pista> pistas;
}

public class DatabaseUploader : MonoBehaviour {
    public string uploadUrl = "https://yourdomain.com/insert_round.php";

    public void UploadRound(RoundData data) {
        StartCoroutine(UploadDataCoroutine(data));
    }

    IEnumerator UploadDataCoroutine(RoundData data) {
        string json = JsonUtility.ToJson(data);
        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log("Upload successful: " + request.downloadHandler.text);
        } else {
            Debug.LogError("Upload failed: " + request.error);
        }
    }
}

