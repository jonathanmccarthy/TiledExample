using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace TiledExample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        private SpriteFont _tileTextFont;
        private string _tileText;
        private Vector2 _tileTextPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _tileTextFont = Content.Load<SpriteFont>("Fonts/Bold18Font");
            _tiledMap = Content.Load<TiledMap>("Map/samplemap");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _tileText = "";
            _tileTextPosition = new Vector2(10, 10);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _tiledMapRenderer.Update(gameTime);

            // show tile at the mouse position
            MouseState mouse = Mouse.GetState();
            UpdateTileText(mouse.X, mouse.Y);

            base.Update(gameTime);
        }

        private void UpdateTileText(int x, int y)
        {
            if (ContainsXY(x, y))
            {
                int tileX, tileY;
                GetTileXYAtPoint(x, y, out tileX, out tileY);
                var tile = _tiledMap.TileLayers[0].GetTile((ushort)tileX, (ushort)tileY);
                _tileText = $"{tileX}, {tileY} TileType: [{tile.GlobalIdentifier - 1}]";
            }
        }

        public bool ContainsXY(int x, int y)
        {
            int tileX = x / 32;
            int tileY = y / 32;

            return (tileX >= 0) && (tileX < _tiledMap.Width) && (tileY >= 0) && (tileY < _tiledMap.Height);
        }

        public void GetTileXYAtPoint(int x, int y, out int tileX, out int tileY)
        {
            tileX = x / 32;
            tileY = y / 32;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _tiledMapRenderer.Draw();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_tileTextFont, _tileText, _tileTextPosition, Color.Yellow, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
