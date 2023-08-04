using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace RGB;

public static class SpriteBatchExtensions
{
    private static Texture2D _pixel;
    
    private static Texture2D GetPixel(SpriteBatch spriteBatch)
    {
        if (_pixel == null)
        {
            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _pixel.SetData(new[] { Color.White });
        }

        return _pixel;
    }

    public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 position, int radius, Color color)
    {
        var pixel = GetPixel(spriteBatch);
        for (int y = -radius; y <= radius; y++)
        for (int x = -radius; x <= radius; x++)
        {
            if (x * x + y * y <= radius * radius)
            {
                spriteBatch.Draw(pixel, position + new Vector2(x, y), color);
            }
        }
    }

    public static void DrawCross(this SpriteBatch spriteBatch, Vector2 position, int radius, Color color)
    {
        var pixel = GetPixel(spriteBatch);
        for (int y = -radius; y <= radius; y++)
        for (int x = -radius; x <= radius; x++)
        {
            if (x * x + y * y <= radius * radius)
            {
                if (x == 0 || y == 0)
                {
                    spriteBatch.Draw(pixel, position + new Vector2(x, y), color);
                }
            }
        }
    }

    public static void DrawPolygon(this SpriteBatch spriteBatch, Polygon polygon)
    {
        var vertices = polygon.Vertices;
        for (var i = 0; i < vertices.Length; i++)
        {
            var j = (i + 1) % vertices.Length;
            var start = vertices[i];
            var end = vertices[j];
            spriteBatch.DrawLine(start, end, Color.Red, 2);
        }
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness, float layerDepth = 0)
    {
        var pixel = GetPixel(spriteBatch);

        var edge = end - start;
        var rotation = MathF.Atan2(edge.Y, edge.X);
        var lineScale = new Vector2(edge.Length(), thickness);

        spriteBatch.Draw(
            _pixel, 
            start, 
            null, 
            color, 
            rotation, 
            Vector2.Zero, 
            lineScale, 
            SpriteEffects.None, 
            layerDepth);
    }
}