using System;
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
    public string uploadUrl = "https://rociochavezml.com/insert_round.php";

    private EventBinding<ClientClueFieldInstantiatedEvent> _clientClueFieldInstantiatedEventBinding;
    private EventBinding<ModulesSettingsListCreatedEvent> _modulesSettingsListCreatedEventBinding;
    
    private List<Modulo> _modulos = new List<Modulo>();
    private List<Pista> _pistas = new List<Pista>();
    
    private void Start()
    {
        _clientClueFieldInstantiatedEventBinding = new EventBinding<ClientClueFieldInstantiatedEvent>(OnClientClueFieldInstantiated);
        EventBus<ClientClueFieldInstantiatedEvent>.Register(_clientClueFieldInstantiatedEventBinding);
        
        _modulesSettingsListCreatedEventBinding = new EventBinding<ModulesSettingsListCreatedEvent>(OnModulesSettingsListCreated);
        EventBus<ModulesSettingsListCreatedEvent>.Register(_modulesSettingsListCreatedEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<ClientClueFieldInstantiatedEvent>.Deregister(_clientClueFieldInstantiatedEventBinding);
        EventBus<ModulesSettingsListCreatedEvent>.Deregister(_modulesSettingsListCreatedEventBinding);
    }

    private void OnClientClueFieldInstantiated(ClientClueFieldInstantiatedEvent clientClueFieldInstantiatedEvent)
    {
        Pista currentPista = new Pista();
        currentPista.tipoValor = (int)clientClueFieldInstantiatedEvent.ClueType + 1;
        currentPista.valor = clientClueFieldInstantiatedEvent.Clue.Key + 1; 
        currentPista.noTrampas = clientClueFieldInstantiatedEvent.Clue.Value;
        
        _pistas.Add(currentPista);
    }

    private void OnModulesSettingsListCreated(ModulesSettingsListCreatedEvent modulesSettingsListCreatedEvent)
    {
        Modulo currentModulo = new Modulo();
        
        foreach (var setting in modulesSettingsListCreatedEvent.SettingsList)
        {
            currentModulo.color = setting.ColorIndex + 1; 
            currentModulo.numero = setting.NumberIndex + 1;
            currentModulo.tipo = setting.ModuleTypeIndex + 1;
            currentModulo.letra = setting.LetterIndex + 1;
            currentModulo.esTrampa = setting.IsTrap ? 1 : 0;
            
            _modulos.Add(currentModulo);
        }
    }
    
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

