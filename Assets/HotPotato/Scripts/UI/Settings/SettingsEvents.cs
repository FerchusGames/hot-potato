namespace HotPotato.UI.Settings
{
    public struct CameraSelectedEvent : IEvent
    {
        public string CameraName;
    }
    
    public struct MicrophoneSelectedEvent : IEvent
    {
        public string MicrophoneName;
    }
}
