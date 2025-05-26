using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

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
    private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
    private EventBinding<WinRoundEvent> _winRoundEventBinding;
    
    private List<Modulo> _modulos = new List<Modulo>();
    private List<Pista> _pistas = new List<Pista>();
    private int _resultado = 0;
    
    private void Start()
    {
        _clientClueFieldInstantiatedEventBinding = new EventBinding<ClientClueFieldInstantiatedEvent>(OnClientClueFieldInstantiated);
        EventBus<ClientClueFieldInstantiatedEvent>.Register(_clientClueFieldInstantiatedEventBinding);
        
        _modulesSettingsListCreatedEventBinding = new EventBinding<ModulesSettingsListCreatedEvent>(OnModulesSettingsListCreated);
        EventBus<ModulesSettingsListCreatedEvent>.Register(_modulesSettingsListCreatedEventBinding);
        
        _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(OnLoseRound);
        EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);
        
        _winRoundEventBinding = new EventBinding<WinRoundEvent>(OnWinRound);
        EventBus<WinRoundEvent>.Register(_winRoundEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<ClientClueFieldInstantiatedEvent>.Deregister(_clientClueFieldInstantiatedEventBinding);
        EventBus<ModulesSettingsListCreatedEvent>.Deregister(_modulesSettingsListCreatedEventBinding);
        EventBus<LoseRoundEvent>.Deregister(_loseRoundEventBinding);
        EventBus<WinRoundEvent>.Deregister(_winRoundEventBinding);
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
        foreach (var setting in modulesSettingsListCreatedEvent.SettingsList)
        {
            Modulo currentModulo = new Modulo();
            
            currentModulo.color = setting.ColorIndex + 1; 
            currentModulo.numero = setting.NumberIndex + 1;
            currentModulo.tipo = setting.ModuleTypeIndex + 1;
            currentModulo.letra = setting.LetterIndex + 1;
            currentModulo.esTrampa = setting.IsTrap ? 1 : 0;
            
            _modulos.Add(currentModulo);
        }
    }

    private void OnLoseRound(LoseRoundEvent loseRoundEvent)
    {
        _resultado = 1;
        CreateAndUploadRoundData();
    }

    private void OnWinRound(WinRoundEvent winRoundEvent)
    {
        _resultado = 2;
        CreateAndUploadRoundData();
    }
    
    private void CreateAndUploadRoundData() {
        RoundData roundData = new RoundData();
        roundData.resultado = _resultado;
        roundData.fecha = DateTime.Now.ToString("yyyy-MM-dd");
        roundData.modulos = _modulos;
        roundData.pistas = _pistas;
        
        UploadRound(roundData);
    }
    
    private void UploadRound(RoundData data)
    {
        StartCoroutine(UploadDataCoroutine(data));
        ClearData();
    }

    private void ClearData()
    {
        _modulos.Clear();
        _pistas.Clear();
        _resultado = 0;
    }

    IEnumerator UploadDataCoroutine(RoundData data) {
        string json = JsonConvert.SerializeObject(data);
        Debug.Log("Sending JSON: " + json);
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

