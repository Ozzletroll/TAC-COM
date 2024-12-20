using TAC_COM.Models;

namespace TAC_COM.Audio.EffectsChains
{
    /// <summary>
    /// The base class defining the effects chains of a <see cref="Profile"/>.
    /// </summary>
    public abstract class BaseChain
    {
        /// <summary>
        /// Method to return all <see cref="EffectReference"/>s to be
        /// applied to the pre-distortion signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPreDistortionEffects();

        /// <summary>
        /// Method to return all <see cref="EffectReference"/>s to be
        /// applied to the post-distortion signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPostDistortionEffects();
    }
}
