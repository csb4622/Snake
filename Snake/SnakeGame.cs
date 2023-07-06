using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake;

public class SnakeGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _atlas;
    private SpriteFont _font;
    private SoundEffect _bite;

    private SnakeHead _snake;
    private Food? _food;
    private int _movementTimer;
    private int _currentMovementTime;
    private int _currentinputTime;
    private int _score;
    private bool _started = false;
    private bool _dead = false;

    public SnakeGame()
    {
        this.Window.Title = "Snake";
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;

        _currentMovementTime = 0;
        _movementTimer = 100;
        _currentinputTime = 0;
        _score = 0;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 30 * 32;
        _graphics.PreferredBackBufferHeight = 30 * 32;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // TODO: use this.Content to load your game content here
        _atlas = Content.Load<Texture2D>("Sheet");
        _font = Content.Load<SpriteFont>("Arial");
        _bite = Content.Load<SoundEffect>("Bite");
    }


    private void UpdateGame(GameTime gameTime)
    {
        _currentinputTime += gameTime.ElapsedGameTime.Milliseconds;

        if (_currentinputTime > _movementTimer)
        {
            if (Keyboard.HasBeenPressed(Keys.Up))
            {
                _currentinputTime = 0;
                _snake.SetRotation(Direction.Up);
            }
            else if (Keyboard.HasBeenPressed(Keys.Right))
            {
                _currentinputTime = 0;
                _snake.SetRotation(Direction.Right);
            }
            else if (Keyboard.HasBeenPressed(Keys.Down))
            {
                _currentinputTime = 0;
                _snake.SetRotation(Direction.Down);
            }
            else if (Keyboard.HasBeenPressed(Keys.Left))
            {
                _currentinputTime = 0;
                _snake.SetRotation(Direction.Left);
            }
        }

        // TODO: Add your update logic here
        _currentMovementTime += gameTime.ElapsedGameTime.Milliseconds;
        if (_currentMovementTime > _movementTimer)
        {
            _currentMovementTime = 0;
            _snake.Update();
        }

        CheckForFoodCollision();
        if (_snake.IsCollidingWithBehind())
        {
            _dead = true;
        }
        CheckForWallCollision();

        if (_food == null)
        {
            GenerateFood();
        }
    }
    
    protected override void Update(GameTime gameTime)
    {
        Keyboard.GetState();
        if (Keyboard.IsPressed(Keys.Escape))
        {
            Exit();
        }

        if (_started && !_dead)
        {
            UpdateGame(gameTime);
        }
        else
        {
            if (Keyboard.IsPressed(Keys.Enter))
            {
                _dead = false;
                _started = true;
                _score = 0;
                _snake = new SnakeHead(new Point(15, 15), 0, _atlas);
                _snake.AddPart(new SnakePart(_atlas));
                _food = null;
            }   
        }
        
        base.Update(gameTime);
    }

    private void GenerateFood()
    {
        while (_food == null)
        {
            var point = new Point(Random.Shared.Next(1, 29), Random.Shared.Next(1, 28));
            if (!_snake.IsCollidingWithPoint(point))
            {
                _food = new Food(_atlas, point);
            }
        }
    }
    

    private void CheckForWallCollision()
    {
        if (_snake.Location.X is 0 or 30 || _snake.Location.Y is 0 or 28)
        {
            _dead = true;
        }
    }

    private void CheckForFoodCollision()
    {
        if (_food != null)
        {
            if (_snake.Location.X == _food.Location.X && _snake.Location.Y == _food.Location.Y)
            {
                _snake.AddPart(new SnakePart(_atlas));
                _food = null;
                ++_score;
                _bite.Play(.5f, 0f, 1f);
            }
        }
    }


    private void DrawGame(GameTime gameTime)
    {
        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        for (var y = 0; y < 29; ++y)
        {
            for (var x = 0; x < 30; ++x)
            {
                if (x == 0 || y == 0 || x == 29 || y == 28)
                {
                    _spriteBatch.Draw(_atlas, new Vector2((x*32), (y*32)), new Rectangle(0,0,32,32), Color.White);                    
                }
            }
        }
        _food?.Draw(_spriteBatch);
        _snake.Draw(_spriteBatch);
        
        _spriteBatch.DrawString(_font, _score.ToString(), new Vector2(15*32, 29*32), Color.Black);
        //
        _spriteBatch.End();
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (_dead)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Final score was {_score}, Press Enter to try again", new Vector2(10*32, 15*32), Color.Black);
            _spriteBatch.End();
        }
        else if(!_started)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Press Enter to start", new Vector2(12*32, 15*32), Color.Black);
            _spriteBatch.End();
        }
        else
        {
            DrawGame(gameTime);    
        }
        
        base.Draw(gameTime);
    }
}