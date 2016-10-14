using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Pong1;

// +————————————————————————————————————————————————————————————+———————————————————————————————————————————————————+
// | There might be unused pieces of code, for example the AI   |  Why the commentbox you ask?                      |
// | If so, the cause will be a big code rewrite I'm doing,     |           How am I supposed to find my comments   |
// | that makes certain pieces of code unusable. This is NOT    |           if I don't have a commentbox...         |
// | junk code because it's fully functional and could be       |                                                   |
// | implemented with a few small edits, but lack of time might +———————————————————————————————————————————————————+
// | make it impossible to implement them. If so, I apologise.  |                                                   |
// |                                                            |       TheMez is responsible for all these         |
// |                        ~ TheMez                            |       commentboxes...                             |
// +————————————————————————————————————————————————————————————+———————————————————————————————————————————————————+

class Pong : Game
{
    // +————————————————————————————————————————————————————————————————————————————————————————————————————————+
    // |                                                                                                        |
    // |                                   This list of variables makes                                         |
    // |                                             me cry                                                     |
    // |                                                                                                        |
    // |                                            ~TheMez                                                     |
    // |                                    (I'll try to fix it soon)                                           |
    // +————————————————————————————————————————————————————————————————————————————————————————————————————————+
    private SpriteFont font;
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Color background;
    Texture2D title, redWin, blueWin, BWbal, BWspeler, BWtopBar, BWlives;
    Random rnd = new Random();
    KeyboardState currentKBState = Keyboard.GetState();
    int state = 1, option = 1;
    bool secondBall = false;
    int ballAmount = 50;
    HashSet<Ball> balls = new HashSet<Ball>();
    //List<Ball> balls = new List<Ball>();
    List<Player> players = new List<Player>();

    static void Main()
    {
        Pong game = new Pong();
        game.Run();
    }

    public Pong()
    {
        Content.RootDirectory = "Content";
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferHeight = 480;
        graphics.PreferredBackBufferWidth = 800;
        // Fullscreen setting
        graphics.IsFullScreen = false;
        graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        title = Content.Load<Texture2D>("WIP Title");
        redWin = Content.Load<Texture2D>("WIP RedWins");
        blueWin = Content.Load<Texture2D>("WIP BlueWins");
        font = Content.Load<SpriteFont>("CoolFontStuff");

        BWbal = Content.Load<Texture2D>("BWbal");
        BWspeler = Content.Load<Texture2D>("BWSpeler");
        BWtopBar = Content.Load<Texture2D>("BWtopBar");
        BWlives = Content.Load<Texture2D>("BWlives");
        int WindowWidth = GraphicsDevice.Viewport.Width;
        int WindowHeight = GraphicsDevice.Viewport.Height;
        Vector2 WindowSize = new Vector2(WindowWidth, WindowHeight);
        for (int i = 0; i < ballAmount; i++)
        {
            balls.Add(new Ball(Content.Load<Texture2D>("BWbal"), new Vector2(323, 232), rnd));
        }
        /*Ball b1 = new Ball(Content.Load<Texture2D>("BWbal"), new Vector2(392, 232));
        if (secondBall)
        {
            Ball b2 = new Ball(Content.Load<Texture2D>("BWbal"), new Vector2(323, 232));
            balls.Add(b2);
        }
        balls.Add(b1);*/

        Player p1 = new Player(new Vector2(5f, 192f), BWspeler, BWlives,"red", Keys.W, Keys.S, false);
        Player p2 = new Player(new Vector2(780f, 192f), BWspeler, BWlives, "blue", Keys.Up, Keys.Down, false);
        
        players.Add(p1);
        players.Add(p2);
        ResetValues();
    }

    void ResetValues(string side = "init")
    {
        foreach (Ball b in balls)
        {
            b.Respawn(rnd);
        }
        if (side == "init")
        {
            foreach (Player player in players)
            {
                player.lives = 3;
                player.pos.Y = (GraphicsDevice.Viewport.Height - 32) / 2;
            }
        }

    }

    protected override void Update(GameTime gameTime)
    {
        // Get the current keyboard state
        KeyboardState prevKBstate = currentKBState;
        currentKBState = Keyboard.GetState();
        background = Color.Black;
        // Exit game
        if (currentKBState.IsKeyDown(Keys.Escape)) Exit();

        if ((state == 1 || state == 2 || state == 4 || state == 5) && currentKBState.IsKeyDown(Keys.Space)) state = 3; // Just some state changing
        if (currentKBState.IsKeyDown(Keys.O)) state = 2; // Same
        if (currentKBState.IsKeyDown(Keys.U) && !prevKBstate.IsKeyDown(Keys.U)) secondBall = !secondBall;

        // +—————————————————————————————————————————————————————————————————————————————————————+
        // |                                                                                     |
        // |                                     The magic of                                    |
        // |                                  the options screen                                 |
        // |                                    happens in here                                  |
        // |                                                                                     |
        // +—————————————————————————————————————————————————————————————————————————————————————+
        if (state == 2)
        {
            if (currentKBState.IsKeyDown(Keys.Enter) && !prevKBstate.IsKeyDown(Keys.Enter))
            {
                if (option == 0) foreach (Player player in players) if (player.name == "red") player.AI = !player.AI;
                if (option == 1) foreach (Player player in players) if (player.name == "blue") player.AI = !player.AI;
            }
            if (currentKBState.IsKeyDown(Keys.Down) && !prevKBstate.IsKeyDown(Keys.Down)) option = (option + 1) % 2;
            if (currentKBState.IsKeyDown(Keys.Up) && !prevKBstate.IsKeyDown(Keys.Up))
            {
                option = (option - 1) % 2;
                if (option < 0) option = 2 + option;
            }
        }
        // +—————————————————————————————————————————————————————————————————————————————————————+
        // |                                                                                     |
        // |                                      Main game                                      |
        // |                                        stuff                                        |
        // |                                                                                     |
        // +—————————————————————————————————————————————————————————————————————————————————————+
        if (state == 3)
        {
            if (currentKBState.IsKeyDown(Keys.R) && !prevKBstate.IsKeyDown(Keys.R)) ResetValues();
            foreach (Player player in players) player.UpdateMovement(balls);
            foreach (Ball ball in balls) ball.Update(players);
            foreach (Ball ball in balls)
            {
                if (ball.pos.X < 0)
                {
                    foreach (Player player in players) if (player.name == "red") {
						player.lives -= 1;
						//foreach (Player p in players) p.Reset();
					}
                    ball.Respawn(rnd);
                }
                if (ball.pos.X > GraphicsDevice.Viewport.Width)
                {
                    foreach (Player player in players) if (player.name == "blue") {
						player.lives -= 1;
						//foreach (Player p in players) p.Reset();
					}
                    ball.Respawn(rnd);
                }
            }

            // Reset game
            foreach (Player player in players)
            {
                if (player.name == "blue" && player.lives == 0) state = 4;
                if (player.name == "red" && player.lives == 0) state = 5;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(background);
        spriteBatch.Begin();
        if (state == 1) spriteBatch.Draw(title, Vector2.Zero, Color.White);

        if (state == 2)
        {
            Vector2 e = font.MeasureString("Red"); //       e, f and g. Isn't it obvious
            Vector2 f = font.MeasureString("Blue"); //      we had more but just removed
            Vector2 g = font.MeasureString("Toggle AI"); // them because they're useless
            Color blink = new Color(255 * (gameTime.TotalGameTime.Seconds % 2), 255 * (gameTime.TotalGameTime.Seconds % 2), 255 * (gameTime.TotalGameTime.Seconds % 2));
            spriteBatch.DrawString(font, "Toggle AI", new Vector2(400 - (0.2f * g.X), 150), Color.White, 0, Vector2.Zero, 0.4f, new SpriteEffects(), 1f);
            if (option == 0) {
                spriteBatch.DrawString(font, "Red", new Vector2(350 - (0.15f * e.X), 190), blink, 0, Vector2.Zero, 0.3f, new SpriteEffects(), 1f);
                spriteBatch.DrawString(font, "Blue", new Vector2(450 - (0.15f * f.X), 190), Color.White, 0, Vector2.Zero, 0.3f, new SpriteEffects(), 1f);
            } if (option == 1) {
                spriteBatch.DrawString(font, "Red", new Vector2(350 - (0.15f * e.X), 190), Color.White, 0, Vector2.Zero, 0.3f, new SpriteEffects(), 1f);
                spriteBatch.DrawString(font, "Blue", new Vector2(450 - (0.15f * f.X), 190), blink, 0, Vector2.Zero, 0.3f, new SpriteEffects(), 1f);
            }
        }

        if (state == 3)
        {
            spriteBatch.Draw(BWtopBar, Vector2.Zero, Color.White); // Think about the order of draws... If this one goes last then lives will be hidden
            foreach (Ball ball in balls) ball.Draw(GraphicsDevice, spriteBatch);
            foreach (Player player in players) player.Draw(GraphicsDevice, spriteBatch);
        }

        if (state == 4)
        {
            foreach (Player player in players) player.lives = 3;
            spriteBatch.Draw(redWin, Vector2.Zero, Color.White);
        }

        if (state == 5)
        {
            foreach (Player player in players) player.lives = 3;
            spriteBatch.Draw(blueWin, Vector2.Zero, Color.White);
        }

        // Arguably the best game state in the entire game of Pong. It's the thing you don't want to see
        if (state == 6)
        {
            spriteBatch.DrawString(font, "SOMETHING WENT WRONG", Vector2.Zero, new Color(0, 0, 0), 0, Vector2.Zero, 0.4f, new SpriteEffects(), 1f);
        }
        spriteBatch.End();
    }
}