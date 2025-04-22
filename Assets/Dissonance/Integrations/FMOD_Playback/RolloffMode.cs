using FMOD;

namespace Dissonance.Integrations.FMOD_Playback
{
    /// <summary>
    /// These rolloff modes correspond to the FMOD rolloff modes.
    /// </summary>
    /// <remarks>https://fmod.com/resources/documentation-api?version=2.00&amp;page=core-api-common.html#fmod_3d_inverserolloff</remarks>
    public enum RolloffMode
        : uint
    {
        /// <summary>
        /// Rolloff model where:
        ///  - mindistance = full volume
        ///  - maxdistance = where sound stops attenuating
        /// and rolloff is fixed according to the global rolloff factor.
        /// </summary>
        Inverse = MODE._3D_INVERSEROLLOFF,
        
        /// <summary>
        /// Linear rolloff model where mindistance = full volume, maxdistance = silence.
        /// </summary>
        Linear = MODE._3D_LINEARROLLOFF,

        /// <summary>
        /// Linear-square rolloff model where mindistance = full volume, maxdistance = silence.
        /// </summary>
        LinearSquared = MODE._3D_LINEARSQUAREROLLOFF,

        /// <summary>
        /// This sound will follow the inverse rolloff model at distances close to mindistance
        /// and a linear-square rolloff close to maxdistance. 
        /// </summary>
        InverseTapered = MODE._3D_INVERSETAPEREDROLLOFF,
    }
}
