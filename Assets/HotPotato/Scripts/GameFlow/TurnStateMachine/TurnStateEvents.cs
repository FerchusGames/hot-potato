using HotPotato.Bomb;

namespace HotPotato.GameFlow.TurnStateMachine
{
    public struct BombTickingEnterStateEvent : IEvent { }
    public struct BombTickingExitStateEvent : IEvent { }
    public struct BombTickingUpdateStateEvent : IEvent { }
    
    public struct ModuleInteractedEnterStateEvent : IEvent
    {
        public BombModuleSettings Settings;
    }
    public struct ModuleInteractedExitStateEvent : IEvent { }
}