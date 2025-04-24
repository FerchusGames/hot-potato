using FMODUnity;
using UnityEngine;

namespace HotPotato.Audio
{
    [CreateAssetMenu(fileName = "EventReferenceSO", menuName = "HotPotato/Audio/EventReferenceSO")]
    public class EventReferenceSO : ScriptableObject
    {
        [field: SerializeField] public EventReference EventReference { get; private set; }
    }
}