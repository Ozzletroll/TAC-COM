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
        /// applied to the pre-compression primary signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPreCompressionEffects();

        /// <summary>
        /// Method to return all <see cref="EffectReference"/>s to be
        /// applied to the post-compression primary signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPostCompressionEffects();

        /// <summary>
        /// Method to return all <see cref="EffectReference"/>s to be
        /// applied to the pre-compression parallel signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPreCompressionParallelEffects();

        /// <summary>
        /// Method to return all <see cref="EffectReference"/>s to be
        /// applied to the post-compression parallel signal.
        /// </summary>
        /// <returns> A list of all the <see cref="EffectReference"/>s.</returns>
        public abstract List<EffectReference> GetPostCompressionParallelEffects();
    }
}
