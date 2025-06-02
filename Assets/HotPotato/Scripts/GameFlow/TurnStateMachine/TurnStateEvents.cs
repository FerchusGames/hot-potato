using HotPotato.Bomb;
using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine
{
    public struct TurnStartEnterStateEvent : IEvent { }

    public struct TurnStartExitStateEvent : IEvent { }
    
    public struct BombTickingEnterStateEvent : IEvent { }
    public struct BombTickingExitStateEvent : IEvent { }
    public struct BombTickingUpdateStateEvent : IEvent { }
    
    public struct ModuleInteractedEnterStateEvent : IEvent
    {
        public GameObject ModuleObject;
        public BombModuleSettings Settings;
    }
    public struct ModuleInteractedExitStateEvent : IEvent { }
    
    public struct ModuleExplodedExitStateEvent : IEvent { }
    
    public struct ModuleDefusedExitStateEvent : IEvent { }
    
    public struct MovingBombEnterStateEvent : IEvent { }
    
    public struct AbilityPlayingEnterStateEvent : IEvent { }
}