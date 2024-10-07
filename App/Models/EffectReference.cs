using CSCore;
using System.Reflection;

namespace TAC_COM.Models
{
    public class EffectReference(Type type)
    {
        public Type EffectType = type;
        public Dictionary<string, object>? Parameters;

        public ISampleSource CreateInstance(ISampleSource? sourceParameter)
        {
            if (EffectType != null)
            {
                ConstructorInfo? constructor = EffectType.GetConstructors()
                    .FirstOrDefault(ctor => ctor.GetParameters().Length == 1 &&
                                            ctor.GetParameters()[0].ParameterType == typeof(ISampleSource));

                if (constructor != null)
                {
                    var instance = constructor.Invoke([sourceParameter]) as ISampleSource;

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
                }
            }
            throw new InvalidOperationException("Effect failed to instantiate.");
        }
    }
}