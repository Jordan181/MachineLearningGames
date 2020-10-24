using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MachineLearningGames.Framework.Extensions
{
    public static class Texture2DExtensions
    {
        public static Color[] GetPixelColorData(this Texture2D texture)
        {
            var colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);
            return colors;
        }
    }
}
