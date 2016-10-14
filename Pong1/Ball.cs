using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

// +————————————————————————————————————————————————————————————————————————————————————————————————————————+
// |                                                                                                        |
// |                                     You found the file for                                             |
// |                                         the Ball class                                                 |
// |                                                                                                        |
// |                                            ~TheMez                                                     |
// |                                                                                                        |
// +————————————————————————————————————————————————————————————————————————————————————————————————————————+
namespace Pong1
{
    public class Ball
    {
        public Vector2 pos, vec, pos1;
        public Texture2D sprite;
        public float vel;
        Vector2 tl = new Vector2(0, 0); // Top left
        Vector2 bl = new Vector2(0, 1); // Bottom left
        Vector2 br = new Vector2(1, 1); // Bottom right
        Vector2 tr = new Vector2(1, 0); // Top right
        // I forgot what the above thingies do, but I'm sure they're useful

        public Ball(Texture2D sprite1, Vector2 poss, Random rnnd)
        {
            pos1 = poss;
            sprite = sprite1;
            Respawn(rnnd);
        }

        Vector2 angle_to_vector(double ang, float mod)
        {
            return new Vector2((float)Math.Cos(ang) * mod, (float)Math.Sin(ang) * mod);
        }
        double vector_to_angle(Vector2 vec, float mod)
        {
            return Math.Atan2(vec.Y / mod, vec.X / mod);
        }
        double deg_to_rad(double ang)
        {
            return ang * 2 * Math.PI / 360;
        }
        double rad_to_deg(double ang)
        {
            return ang * 360 / (2 * Math.PI);
        }

        public void Respawn(Random rnd)
        {
            pos = pos1;
            double Angle = 90;
            vel = 5;
            while ((Angle < 300 && Angle > 240) || (Angle > 60 && Angle < 120))
            {
                Angle = rnd.NextDouble() * 360;
            }

            vec = angle_to_vector(deg_to_rad(Angle), vel);
        }

        bool CollidesWith(Player p)
        {
            if (p.name == "blue")
            {
                if (corner(tr).X + vec.X > p.pos.X && corner(br).Y + vec.Y > p.pos.Y && corner(tr).Y < (p.pos.Y + p.sprite.Height)) return true;
                else return false;
            }
            if (p.name == "red")
            {
                if (corner(tl).X + vec.X < p.pos.X + 16 && corner(bl).Y + vec.Y > p.pos.Y && corner(tl).Y < (p.pos.Y + p.sprite.Height)) return true;
                else return false;
            }
            return false;
        }
        Vector2 corner(Vector2 corn)
        {
            if (corn == tl) return new Vector2(pos.X, pos.Y);
            if (corn == bl) return new Vector2(pos.X, pos.Y + sprite.Height);
            if (corn == br) return new Vector2(pos.X + sprite.Width, pos.Y + sprite.Height);
            if (corn == tr) return new Vector2(pos.X + sprite.Width, pos.Y);
            return new Vector2(-1, -1);
        }

        void bounceY()
        {
            // If the ball would be at top or bottom border
            if (pos.Y + vec.Y > 465 || pos.Y + vec.Y < 32)
            {
                // Turn around (Bounce)
                vec.Y = -vec.Y;
            }
        }
        void bounceX(List<Player> players)
        {
            foreach (Player pl in players) {
                if (CollidesWith(pl))
                {
                    float v;
                    float plMidY = pl.pos.Y + 0.5f * pl.sprite.Height;
                    float mul = -(plMidY - pos.Y - (0.5f * sprite.Height)) / (0.5f * pl.sprite.Height);
                    if (mul > 1) mul = 1;
                    else if (mul < -1) mul = -1;

                    if (pl.name == "blue") v = (float)deg_to_rad((360f + 60f * mul) % 360);
                    else if (pl.name == "red") v = (float)deg_to_rad(180f - 60f * mul);
                    else v = (float)deg_to_rad(0.0); // This shouldn't happen at any time

                    vec = angle_to_vector(v, vel);
                    vec.X = -vec.X;
                    calcVel();
                }
            }
        }

        void calcVel()
        {
            double v = vector_to_angle(vec, vel);
            vel = (float)Math.Sqrt(vel * vel + 4);
            vec = angle_to_vector(v, vel);
        }

        public void Update(List<Player> players)
        {
            bounceY();
            bounceX(players);
            pos.X = pos.X + vec.X;
            pos.Y = pos.Y + vec.Y;
        }
        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, pos, Color.White);
        }
    }
}
