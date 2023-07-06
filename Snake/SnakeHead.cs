using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake;

public class SnakeHead: SnakePart
{
    public override Point TextureOffset => new(32, 0);

    public SnakeHead(Point location, Direction rotation, Texture2D atlas) : base(atlas)
    {
        SetLocation(location.X, location.Y);
        SetRotation(rotation);
    }
}