Dissonance FMOD Integration
===========================

This package integrates Dissonance Voice Chat (https://assetstore.unity.com/packages/tools/audio/dissonance-voice-chat-70078?aid=1100lJDF) with
the FMOD audio system (https://assetstore.unity.com/packages/tools/audio/fmod-for-unity-161631?aid=1100lJDF). To use this package you must have
purchased and installed Dissonance and FMOD - **you will encounter build errors until you have done this**!

Setup (Playback)
================

Drag the `FMODPlaybackPrefab` into the `Playback Prefab` field of your DissonanceComms object. Dissonance will now use FMOD to play back all
Dissonance audio.

If you are constructing a custom playback prefab (https://placeholder-software.co.uk/dissonance/docs/Tutorials/Playback-Prefab.html) use the
`FMODVoicePlayback` component instead of the normal `VoicePlayback` component. Do not attach a `SamplePlaybackComponent` or an `AudioSource`.

If you want voice to play through a specific bus in the FMOD audio system you should set the `Output Audio Bus` field of the `FMODVoicePlayback`
component to the full path of the bus. e.g. "bus:/Example". **Be aware** that if you mute this bus (or solo another bus) this **will** cause a
desync in the voice system!