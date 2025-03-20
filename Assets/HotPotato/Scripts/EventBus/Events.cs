using System.Collections.Generic;
using HotPotato.Bomb;

public interface IEvent { }

public struct TurnChangedEvent : IEvent { }
public struct RoundStartedEvent : IEvent { }
public struct RoundEndedEvent : IEvent { }
public struct MatchEndedEvent : IEvent { }
public struct ModulesSpawnedEvent : IEvent
{
    public List<BombModuleSettings> settingsList;
}