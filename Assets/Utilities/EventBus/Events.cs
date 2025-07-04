﻿using System.Collections.Generic;
using HotPotato.AbilitySystem;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.Player;
using HotPotato.UI;
using UnityEngine;

public interface IEvent { }

public struct TurnOwnerChangedEvent : IEvent
{
    public bool IsMyTurn;
}
public struct PlayerJoinedEvent : IEvent
{
    public IPlayerController PlayerController;
}
public struct OwnedPlayerSpawnedEvent : IEvent
{
    public GameObject PlayerObject;
}
public struct GeneratedClueDataEvent : IEvent
{
    public ClueData ClueData;
}
public struct ClientClueFieldInstantiatedEvent : IEvent
{
    public BombClueType ClueType;
    public KeyValuePair<int, int> Clue;
}
public struct RoundStartedEvent : IEvent { }
public struct RoundEndedEvent : IEvent { }
public struct MatchResetEvent : IEvent { }
public struct MatchEndedEvent : IEvent { }
public struct ModulesSpawnedEvent : IEvent
{
    public List<BombModuleSettings> SettingsList;
}
public struct ModulesSettingsListCreatedEvent : IEvent
{
    public List<BombModuleSettings> SettingsList;
}
public struct ModuleClickedEvent : IEvent
{
    public BombModule Module;
}

public struct ModuleInteractedEvent : IEvent
{
    public BombModule Module;
    public BombModuleSettings Settings;
}
public struct LoseRoundEvent : IEvent { }

public struct WinRoundEvent : IEvent
{
    public int WinCount;
}
public struct WinMatchEvent : IEvent
{
    public int WinCount;
}
public struct LoseMatchEvent : IEvent { }
public struct StartNextRoundEvent : IEvent { }
public struct StartNextMatchEvent : IEvent { }
public struct TimerExpiredEvent : IEvent { }
public struct ChangeSceneRequestEvent : IEvent
{
    public string SceneToLoadName;
}

public struct TransportingClientsToSceneEvent : IEvent
{
    public int PlayerCount;
}

public struct MoveBombToPlayerEvent : IEvent
{
    public Vector3 PlayerPosition;
}
public struct BombHasReachedPlayerEvent : IEvent { }

public struct BombExplodedClientEvent : IEvent { }

public struct AbilityPlayingEvent : IEvent
{
    public IAbility Ability;
}
public struct AbilityFinishedEvent : IEvent
{
    public AbilityType AbilityType;
}

public struct AbilitySelectRequestedEvent : IEvent
{
    public AbilityType AbilityType;
}

public struct AbilityDeselectRequestedEvent : IEvent { }

public struct AbilitySelectedEvent : IEvent
{
    public AbilityType AbilityType;
}