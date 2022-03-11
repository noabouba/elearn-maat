using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winlit
{
    public static class Pallete
    {
        public readonly static Color DarkerGray = Color.FromArgb(71, 71, 71);
        public readonly static Color DarkGray = Color.FromArgb(105, 105, 105);
        public readonly static Color LightGray = Color.FromArgb(210, 210, 210);
        public readonly static Color LightBlue = Color.FromArgb(81, 158, 244);
        public readonly static Color Blue = Color.FromArgb(52, 148, 255);
        public readonly static Color DarkerBlue = Color.FromArgb(0, 120, 255);
        public readonly static Color SlateDarkGray = Color.FromArgb(125, 125, 125);
        public readonly static Color AlternateBlue = Color.FromArgb(93, 212, 253);
        public readonly static Color CalmBlue = Color.FromArgb(118, 207, 252);
    }
    public static class Fonts
    {
        private static FontFamily family = FontFamily.Families.ToList().Find(x => x.Name.Contains("Lucida Sans"));

        public static readonly Font Big = new Font(family, 18, FontStyle.Bold);
        public static readonly Font Regular = new Font(family, 16);
        public static readonly Font Small = new Font(family, 10);
    }

}
