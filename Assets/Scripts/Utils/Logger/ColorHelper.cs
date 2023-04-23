using System.Globalization;
using UnityEngine;

namespace Yarde.Utils.Logger
{
    public static class ColorHelper
    {
        /// <summary>
        /// Thread-safe color conversion.
        /// </summary>
        /// <param name="hexColor">HTML HEX color.</param>
        /// <returns>Color32 or null if unknown.</returns>
        public static Color32? ConvertHtmlHexToColor32(string hexColor)
        {
            // Remove starting '#' if any
            hexColor = hexColor.Replace("#", string.Empty);

            // We only support HEX6 or HEX3
            if (hexColor.Length != 3 && hexColor.Length != 6)
            {
                Debug.LogError($"The provided HTML HEX color \"{hexColor}\" is not supported, only HEX6 and HEX3 are");
                return null;
            }

            // Expand HEX3 to HEX6
            if (hexColor.Length == 3)
            {
                hexColor = $"{hexColor[0]}{hexColor[0]}{hexColor[1]}{hexColor[1]}{hexColor[2]}{hexColor[2]}";
            }

            var r = byte.Parse($"{hexColor[0]}{hexColor[1]}", NumberStyles.HexNumber);
            var g = byte.Parse($"{hexColor[2]}{hexColor[3]}", NumberStyles.HexNumber);
            var b = byte.Parse($"{hexColor[4]}{hexColor[5]}", NumberStyles.HexNumber);

            return new Color32(r, g, b, 1);
        }
    }
}