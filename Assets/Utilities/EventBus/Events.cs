using System.Collections.Generic;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.Player;

public interface IEvent { }

public struct TurnOwnerChangedEvent : IEvent
{
    public bool IsMyTurn;
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
public struct ModuleExplodedEvent : IEvent { }
public struct ModuleDefusedEvent : IEvent { }
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
public struct TimerExpiredEvent : IEvent { }
