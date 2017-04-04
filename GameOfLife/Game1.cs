using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;


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
        TimeSpan updateTimer;

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

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60);

            base.Initialize();
        }

        public void CreateBoard(List<Cell> cells, Texture2D texture2D)
        {
            for (int h = 0; h < graphics.GraphicsDevice.Viewport.Width; h+=10)
            {
                for (int w = 0; w < graphics.GraphicsDevice.Viewport.Height; w+=10)
                {
                    Cell cell = new Cell();
                    cell.Initialize(graphics.GraphicsDevice, texture2D, h, w, StaticRandom.Rand() % 2 == 0 ? true : false);
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

            updateTimer += gameTime.ElapsedGameTime;

            if (updateTimer.TotalMilliseconds > 1000f / 1)
            {
                updateTimer = TimeSpan.Zero;
                foreach (Cell cell in cells)
                {
                    cell.SimulateCellLifeCycle(cells);
                }

                foreach (Cell cell in cells)
                {
                    cell.ApplyNextState();
                }
            }

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
        public List<Cell> AdjacentCells;
        public bool NextState;

        public void Initialize(GraphicsDevice graphicsDevice, Texture2D texture2D, float positionX, float positionY, bool isAlive)
        {
            IsAlive = isAlive;
            NextState = isAlive;
            Texture2D = texture2D; //new Texture2D(graphicsDevice, (int)size, (int)size);
            SetColor(texture2D.Height);

            Position = new Vector2(positionX, positionY);
            AdjacentCells = new List<Cell>();
        }

        public void SetColor(int size)
        {
            Color[] colorData = new Color[size * size];
            for (int i = 0; i < size*size; i++)
                colorData[i] = Color.Black;

            Texture2D.SetData(colorData);
        }

        private void GetAdjacentCells(List<Cell> cells)
        {
            if (AdjacentCells.Count != 0)
            {
                return;
            }

            Cell adjacentCell = cells.FirstOrDefault(c => c.Position.X == -10 + Position.X && c.Position.Y == -10 + Position.Y); //Top Left
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == Position.X && c.Position.Y == -10 + Position.Y ); //Top Middle
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == 10 + Position.X && c.Position.Y == -10 + Position.Y ); //Top Right
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == 10 + Position.X && c.Position.Y == Position.Y ); //Right
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == 10 + Position.X && c.Position.Y == 10 + Position.Y ); //Bottom Right
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == Position.X && c.Position.Y == 10 + Position.Y ); //Bottom
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == -10 + Position.X && c.Position.Y == 10 + Position.Y ); //Bottom Left
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
            adjacentCell = cells.FirstOrDefault(c => c.Position.X == -10 + Position.X && c.Position.Y == Position.Y ); //Left
            if (adjacentCell != null)
            {
                AdjacentCells.Add(adjacentCell);
            }
        }

        public void SimulateCellLifeCycle(List<Cell> cells)
        {
            GetAdjacentCells(cells);
            int count = GetAliveAdjacentCells();
            CheckUnderpopulatedCell(count);
            CheckOverpopulatedCell(count);
            CheckReproducedCell(count);
        }

        private int GetAliveAdjacentCells()
        {
            return AdjacentCells.Count(c => c.IsAlive);
        }

        private void CheckUnderpopulatedCell(int count)
        {
            if (IsAlive && count < 2)
            {
                NextState = false;
            }
        }

        private void CheckOverpopulatedCell(int count)
        {
            if (IsAlive && count > 3)
            {
                NextState = false;
            }
        }

        private void CheckReproducedCell(int count)
        {
            if (!IsAlive && count == 3)
            {
                NextState = true;
            }
        }

        public void ApplyNextState()
        {
            if (NextState != IsAlive)
            {
                IsAlive = NextState;
            }
        }
    }
}
