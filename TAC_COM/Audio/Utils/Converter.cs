
namespace TAC_COM.Audio.Utils
{
    /// <summary>
    /// Class to provide basic linear-to-decibel
    /// and decibel-to-linear value converstions.
    /// </summary>
    public class LinearDBConverter
    {
        /// <summary>
        /// Convert a given linear gain value to a
        /// decibel value.
        /// </summary>
        /// <param name="value"> The linear gain value to convert.</param>
        /// <returns> The converted value in decibels.</returns>
        public static float LinearToDecibel(float value)
        {
            return 20 * (float)Math.Log10(value);
        }

        /// <summary>
        /// Convert a given decibel gain value to a
        /// linear value.
        /// </summary>
        /// <param name="value"> The decibel value to convert.</param>
        /// <returns> The converted value as a linear expression.</returns>
        public static float DecibelToLinear(float value)
        {
            return (float)Math.Pow(10, value / 20.0);
        }
    }
}
