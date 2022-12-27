using UnityEngine;
using UnityEngine.UI;

namespace Yarde.Utils.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithR(this Color c, float r) => new Color(r, c.g, c.b, c.a);
        public static Color WithG(this Color c, float g) => new Color(c.r, g, c.b, c.a);
        public static Color WithB(this Color c, float b) => new Color(c.r, c.g, b, c.a);
        public static Color WithA(this Color c, float a) => new Color(c.r, c.g, c.b, a);
        public static Color ToColor(this string hexa)
        {
            if (!string.IsNullOrEmpty(hexa) && hexa[0] != '#')
            {
                hexa = '#' + hexa;
            }

            if (ColorUtility.TryParseHtmlString(hexa, out Color color))
                return color;
            return Color.magenta;
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}