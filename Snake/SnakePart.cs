using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake;

public class SnakePart
{
    private readonly Texture2D _atlas;
    private SnakePart? _linkedPart;
    private Point _location;
    private Direction _rotation;
    public Point Location => _location;
    public Direction Rotation => _rotation;

    public virtual Point TextureOffset => new Point(64, 0);

    public SnakePart(Texture2D atlas)
    {
        _atlas = atlas;
    }

    public void Update()
    {
        _linkedPart?.Update();
        _linkedPart?.SetLocation(_location.X, _location.Y);
        _linkedPart?.SetRotation(_rotation);

        switch (_rotation)
        {
            case Direction.Up: // 0f
                _location.Y -= 1;
                break;
            case Direction.Right: // 1.57079637f
                _location.X += 1;
                break;
            case Direction.Down: // 3.14159274f
                _location.Y += 1;
                break;
            case Direction.Left: // 4.71238899f
                _location.X -= 1;
                break;
        }
    }

    public bool IsCollidingWithBehind()
    {
        return IsCollidingWithBehind(_location);
    }
    private bool IsCollidingWithBehind(Point point)
    {
        if (_linkedPart != null)
        {
            var currentPart = _linkedPart;
            if (currentPart._location.X == point.X && currentPart._location.Y == point.Y)
            {
                return true;
            }
            while (currentPart != null)
            {
                if (currentPart._location.X == point.X && currentPart._location.Y == point.Y)
                {
                    return true;
                }

                currentPart = currentPart._linkedPart;
            }
        }
        return false;
    }    
    
    public bool IsCollidingWithPoint(Point point)
    {
        if (_location.X == point.X && _location.Y == point.Y)
        {
            return true;
        }
        return IsCollidingWithBehind(point);
    }
    

    public void SetLocation(int x, int y)
    {
        _location.X = x;
        _location.Y = y;
    }

    public void SetRotation(Direction rotation)
    {
        switch (_rotation)
        {
            case Direction.Up:
                if (rotation is Direction.Left or Direction.Right)
                {
                    _rotation = rotation;
                }
                break;
            case Direction.Right:
                if (rotation is Direction.Up or Direction.Down)
                {
                    _rotation = rotation;
                }
                break;
            case Direction.Down:
                if (rotation is Direction.Left or Direction.Right)
                {
                    _rotation = rotation;
                }                
                break;
            case Direction.Left:
                if (rotation is Direction.Up or Direction.Down)
                {
                    _rotation = rotation;
                }
                break;            
        }
    }

    public void AddPart(SnakePart newPart)
    {
        // Add everypart infront of the last part in the list
        var partToAddTo = this;
        while (partToAddTo?._linkedPart != null)
        {
            partToAddTo = partToAddTo._linkedPart;
        }
        newPart.SetRotation(_rotation);
        switch (_rotation)
        {
            case Direction.Up:
                newPart.SetLocation(partToAddTo._location.X, partToAddTo._location.Y + 1);
                break;
            case Direction.Left:
                newPart.SetLocation(partToAddTo._location.X+1, partToAddTo._location.Y);
                break;
            case Direction.Down:
                newPart.SetLocation(partToAddTo._location.X, partToAddTo._location.Y - 1);
                break;
            case Direction.Right:
                newPart.SetLocation(partToAddTo._location.X-1, partToAddTo._location.Y);
                break;
        }
        partToAddTo._linkedPart = newPart;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _linkedPart?.Draw(spriteBatch);
        var rotation = DirectionToRotation();
        spriteBatch.Draw(_atlas, new Vector2((_location.X * 32)+16, (_location.Y * 32)+16),
            new Rectangle(TextureOffset, new Point(32)), Color.White, rotation, new Vector2(16, 16), Vector2.One,
            SpriteEffects.None, 1);
    }

    private float DirectionToRotation()
    {
        switch (_rotation)
        {
            case Direction.Up:
                return 0f;
            case Direction.Right:
                return 1.57079637f;
            case Direction.Down:
                return 3.14159274f;
            case Direction.Left:
                return 4.71238899f;
            default:
                return 0f;
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}