using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake;

public class Food
{
    public readonly Point Location;
    public readonly Texture2D Atlas;
    private Rectangle _textureOffset => new Rectangle(96, 0,32, 32);

    public Food(Texture2D atlas, Point location)
    {
        Atlas = atlas;
        Location = new Point(location.X, location.Y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Atlas, new Vector2(Location.X*32, Location.Y*32), _textureOffset, Color.White);
    }
}