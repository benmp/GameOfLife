using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;


/* TODOS
 * Get Adjacent Cells Based On Position
 * Isolated Death Cycle
 * Overpopulated Death Cycle
 * Growth Cycle
*/

namespace GameOfLife
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Cell> cells;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cells = new List<Cell>();
            Texture2D image = Content.Load<Texture2D>("Images/redsquare");
            CreateBoard(cells, image);

            base.Initialize();
        }

        public void CreateBoard(List<Cell> cells, Texture2D texture2D)
        {
            for (int h = 0; h < graphics.GraphicsDevice.Viewport.Width; h+=10)
            {
                for (int w = 0; w < graphics.GraphicsDevice.Viewport.Height; w+=10)
                {
                    Cell cell = new Cell();
                    cell.Initialize(graphics.GraphicsDevice, texture2D, h, w, StaticRandom.Rand() % 5 == 0 ? true : false);
                    cells.Add(cell);
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Cell cell in cells.Where(c => c.IsAlive))
            {
                spriteBatch.Draw(cell.Texture2D, cell.Position, Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }


    public class Cell
    {
        public Texture2D Texture2D;
        public Vector2 Position;
        public bool IsAlive;

        public void Initialize(GraphicsDevice graphicsDevice, Texture2D texture2D, float positionX, float positionY, bool isAlive)
        {
            IsAlive = isAlive;
            Texture2D = texture2D; //new Texture2D(graphicsDevice, (int)size, (int)size);
            SetColor(texture2D.Height);

            Position = new Vector2(positionX, positionY);
        }

        public void SetColor(int size)
        {
            Color[] colorData = new Color[size * size];
            for (int i = 0; i < size*size; i++)
                colorData[i] = Color.Red;

            Texture2D.SetData(colorData);
        }
    }
}
