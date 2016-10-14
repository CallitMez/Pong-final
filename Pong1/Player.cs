using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

// +————————————————————————————————————————————————————————————————————————————————————————————————————————+
// |                                                                                                        |
// |                                     You found the file for                                             |
// |                                        the Player class                                                |
// |                                                                                                        |
// |                                            ~TheMez                                                     |
// |                                                                                                        |
// +————————————————————————————————————————————————————————————————————————————————————————————————————————+

namespace Pong1
{
    public class Player
    {
        public Vector2 pos, vec, respos;
        public Texture2D sprite, lifeSprite;
        public string name;
        public Dictionary<string, Keys> ctrl = new Dictionary<string, Keys>();
        public bool AI;
        public int lives;

        public Player(Vector2 pos1, Texture2D sprite1, Texture2D lifeSprite1, string name1, Keys up, Keys down, bool AIon)
        {
            pos = pos1;
			respos = pos1;
            sprite = sprite1;
            name = name1;
            lifeSprite = lifeSprite1;
            ctrl.Add("Up", up);
            ctrl.Add("Down", down);
            AI = AIon;
            lives = 3;
        }
		
		public void Reset()
		{
			pos = respos;
		}
        void AI_Move(Ball ball)
        {
            float height;
            if (name == "red")
            {
                if (ball.vec.X < 0)
                {
                    float dis = (ball.pos.X - 21) / ball.vec.X;
                    height = ball.pos.Y - (dis * ball.vec.Y);
                    while (height < 32 || height > 464)
                    {
                        if (height < 32) height = 64 - height;
                        if (height > 464) height = 928 - height;
                    }
                    if (height > pos.Y && height > pos.Y + sprite.Height) pos.Y += 5;
                    else if (height < pos.Y && height < pos.Y + sprite.Height) pos.Y -= 5;
                }
            }

            if (name == "blue")
            {
                if (ball.pos.Y > pos.Y + 0.5f*sprite.Height - 2 && ball.pos.Y > pos.Y + 0.5f * sprite.Height + 3) pos.Y += 5;
                else if (ball.pos.Y < pos.Y + 0.5f * sprite.Height - 2 && ball.pos.Y < pos.Y + 0.5f * sprite.Height + 3) pos.Y -= 5;
            }
        }

        public void drawLives(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            if (name == "red")
                for (int i = 1; i <= lives; i++)
                    spriteBatch.Draw(lifeSprite, new Vector2((i - 1) * lifeSprite.Width, 0), Color.White);
            else if (name == "blue")
                for (int i = 1; i <= lives; i++)
                    spriteBatch.Draw(lifeSprite, new Vector2(GraphicsDevice.Viewport.Width - (i * lifeSprite.Width), 0), Color.White);
        }

        public void UpdateMovement(HashSet<Ball> balls)
        {
            // Get the current keyboard state
            KeyboardState currentKBState = Keyboard.GetState();

            // I should put this method in the Player.cs file
            // But that messes with the current code :/
            /* 
            +————————————————————————————————————————————————————+
            | not sure if the AI will be in the final version    |
            | but it is coded in                                 |
            |                                                    |
            | (probabaly not compatible with the way I           |
            | rewrote the ball(s) to be in a list)               |
            |                                                    |
            | EDIT: It's compatible, not compatible with         |
            | more than 1 ball though                            |
            +————————————————————————————————————————————————————+ 
            */
            if (AI)
            {
                //AI_Move(balls[0]);
            }
            else
            {
                // +————————————————————————————————————————————————————+
                // |  Single line if statements because used to Python  |
                // |                     ~TheMez                        |
                // +————————————————————————————————————————————————————+
                if (currentKBState.IsKeyDown(ctrl["Down"])) pos.Y += 5;
                if (currentKBState.IsKeyDown(ctrl["Up"])) pos.Y -= 5;
            }
            if (pos.Y > 384) pos.Y = 384;
            if (pos.Y < 32) pos.Y = 32;
        }
        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, pos, Color.White);
            drawLives(GraphicsDevice, spriteBatch);
        }
    }
}
