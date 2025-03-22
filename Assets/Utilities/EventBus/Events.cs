using System.Collections.Generic;
using HotPotato.Bomb;
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