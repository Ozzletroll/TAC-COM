using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAC_COM.Audio.Utils
{
    internal class LinearDBConverter
    {
        public static float LinearToDecibel(float value)
        {
            return 20 * (float)Math.Log10(value);
        }

        public static float DecibelToLinear(float value)
        {
            return (float)Math.Pow(10, value / 20.0);
        }
    }
}
