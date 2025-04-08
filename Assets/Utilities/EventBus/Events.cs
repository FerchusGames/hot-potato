using System.Collections.Generic;
using HotPotato.AbilitySystem;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.Player;
using UnityEngine;

public interface IEvent { }

public struct TurnOwnerChangedEvent : IEvent
{
    public bool IsMyTurn;
    public IAbility Ability;
}
public struct PlayerJoinedEvent : IEvent
{
    public IPlayerController PlayerController;
}
public struct GeneratedClueDataEvent : IEvent
{
    public ClueData ClueData;
}
public struct RoundStartedEvent : IEvent { }
public struct RoundEndedEvent : IEvent { }
public struct MatchResetEvent : IEvent { }
public struct MatchEndedEvent : IEvent { }
public struct ModulesSpawnedEvent : IEvent
{
    public List<BombModuleSettings> SettingsList;
}

public struct ModuleClickedEvent : IEvent
{
    public BombModule Module;
}

public struct ModuleInteractedEvent : IEvent
{
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

public struct AbilityStartedEvent : IEvent
{
    public IAbility Ability;
}
public struct AbilityFinishedEvent : IEvent { }