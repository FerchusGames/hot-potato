using UnityEngine;

namespace HotPotato.Audio
{
    public enum MusicTrack
    {
        Track1 = 0,
        Track2 = 1,
    }
    
    public class MusicTrackChanger : MonoBehaviour
    {
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        private bool _isFirstRound = true;
        
        private MusicTrack _currentMusicTrack = MusicTrack.Track1;
        
        private void Start()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(ChangeMusicTrack);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }

        private void OnDestroy()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
        }

        private void ChangeMusicTrack()
        {
            if (_isFirstRound)
            {
                _isFirstRound = false;
                return;
            }

            SetMusicTrack(_currentMusicTrack == MusicTrack.Track1 ? MusicTrack.Track2 : MusicTrack.Track1);
        }

        private void SetMusicTrack(MusicTrack track)
        {
            _currentMusicTrack = track;
            AudioManager.Instance.SetMusicTrack(_currentMusicTrack);
        }
    }
}