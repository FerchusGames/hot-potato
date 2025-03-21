using System.Collections.Generic;
using HotPotato.Bomb;

public interface IEvent { }

public struct TurnOwnerChangedEvent : IEvent
{
    public bool isMyTurn;
}
public struct RoundStartedEvent : IEvent { }
public struct RoundEndedEvent : IEvent { }
public struct MatchResetEvent : IEvent { }
public struct MatchEndedEvent : IEvent { }
public struct ModulesSpawnedEvent : IEvent
{
    public List<BombModuleSettings> settingsList;
}

public struct ModuleClickedEvent : IEvent { }

public struct LoseRoundEvent : IEvent { }

public struct WinRoundEvent : IEvent
{
    public int winCount;
}
public struct WinMatchEvent : IEvent
{
    public int winCount;
}
public struct LoseMatchEvent : IEvent { }
public struct TimerExpiredEvent : IEvent { }