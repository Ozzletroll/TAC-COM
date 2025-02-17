using System.Reflection;
using CSCore;

namespace TAC_COM.Models
{
    /// <summary>
    /// Represents a reference to an instantiable <see cref="ISampleSource"/> EffectReferenceWrapper,
    /// to be instantiated by the <see cref="AudioProcessor"/> as part of the signal chain.
    /// </summary>
    /// <remarks>
    /// All DSP effect wrappers found in "/Audio/DSP/EffectReferenceWrappers"
    /// may be used as an EffectReference type parameter.
    /// </remarks>
    /// <param name="type"> The type of the <see cref="ISampleSource"/> DSP effect wrapper.</param>
    public class EffectReference(Type type)
    {
        public Type EffectType = type;
        public Dictionary<string, object>? Parameters;

        /// <summary>
        /// Creates an instance of the class of the type <see cref="EffectType"/>,
        /// using the parameters of <see cref="Parameters"/>.
        /// </summary>
        /// <param name="sourceParameter"> The source signal to pass into the instantiated effect.</param>
        /// <returns> The <see cref="ISampleSource"/> with the instantiated effect applied.</returns>
        /// <exception cref="InvalidOperationException"> Thrown if effect fails to instantiate. </exception>
        public ISampleSource CreateInstance(ISampleSource sourceParameter)
        {
            ConstructorInfo? constructor = EffectType.GetConstructors()
                .FirstOrDefault(ctor => ctor.GetParameters().Length == 1 &&
                                        ctor.GetParameters()[0].ParameterType == typeof(ISampleSource));

            ISampleSource? instance = constructor?.Invoke([sourceParameter]) as ISampleSource;

            if (instance != null && Parameters != null)
            {
                foreach (var param in Parameters)
                {
                    var property = EffectType.GetProperty(param.Key);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(instance, param.Value);
                    }
                }
            }
            if (instance != null) return instance;
            else throw new InvalidOperationException($"Effect {EffectType.FullName} failed to instantiate.");
        }
    }
}