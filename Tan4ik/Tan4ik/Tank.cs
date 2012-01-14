using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using Microsoft.Xna.Framework.Content;

namespace Tan4ik
{
    class Tank
    {
        int w = 800, h = 600;
        Vector2 pos, tankCenter, turretCenter;
        public Vector2 posTurret;
        float rotation;
        public float turretRotation;
        const double a = 0.2;
        double currentSpeed;
        bool ok;
        float quadm;
        const float ws = 0.05f;
        Texture2D tank, cannon;

        public Tank(Texture2D tank, Texture2D cannon, Vector2 pos)
        {
            this.tank = tank;
            this.cannon = cannon;


            this.pos = pos;
            tankCenter = new Vector2(tank.Width / 2, tank.Height / 2);
            rotation = 0;
            
            currentSpeed = 0;

            
            turretRotation = 0;
            posTurret = this.pos;
            turretCenter = new Vector2(tank.Width / 2 + 20, cannon.Height / 2);

        }

        public void Update(KeyboardState kb, GameTime gameTime)
        {
            if (!kb.IsKeyDown(Keys.Up) && !kb.IsKeyDown(Keys.Down) && currentSpeed > 0 && isOKUp())
                Brake(gameTime, true);
            else if (!kb.IsKeyDown(Keys.Up) && !kb.IsKeyDown(Keys.Down) && currentSpeed > 0 && !isOKUp())
                currentSpeed = 0;

            if (!kb.IsKeyDown(Keys.Up) && !kb.IsKeyDown(Keys.Down) && currentSpeed < 0 && isOKDown())
                Acceleration(gameTime, true);
            else if (!kb.IsKeyDown(Keys.Up) && !kb.IsKeyDown(Keys.Down) && currentSpeed < 0 && !isOKDown())
               currentSpeed = 0;

            if (kb.IsKeyDown(Keys.Up) && isOKUp())
            {

                Acceleration(gameTime, false);

            }
            else if (!isOKUp())
                currentSpeed = 0;


            if (kb.IsKeyDown(Keys.Down) && isOKDown())
            {
                Brake(gameTime, false);
            }
            else if (!isOKDown())
                currentSpeed = 0;

            if (kb.IsKeyDown(Keys.Right))
            {
                TurnRight();
            }

            if (kb.IsKeyDown(Keys.Left))
            {
                TurnLeft();
            }

            if (kb.IsKeyDown(Keys.A))
            {
                TurnTurretLeft();
            }
            if (kb.IsKeyDown(Keys.D))
            {
                TurnTurretRight();
            }
        }

        bool isOKUp()
        {
            quadm = (rotation % MathHelper.TwoPi);
            if (quadm < 0)
                quadm += MathHelper.TwoPi;

            ok = ((pos.Y > 0) || (quadm <= MathHelper.TwoPi && quadm >= MathHelper.Pi)) &&
                    ((pos.Y < h) || (quadm >= 0 && quadm <= MathHelper.Pi)) &&
                    ((pos.X > 0) || (quadm >= MathHelper.PiOver2 && quadm <= 3 * MathHelper.PiOver2)) &&
                    ((pos.X < w) || (quadm <= MathHelper.PiOver2 || quadm >= 3 * MathHelper.PiOver2));

            return ok;
        }

        bool isOKDown()
        {
            quadm = (rotation % MathHelper.TwoPi);
            if (quadm < 0)
                quadm += MathHelper.TwoPi;
            ok = ((pos.Y > 0) || (quadm >= 0 && quadm <= MathHelper.Pi)) &&
                    ((pos.Y < h) || (quadm >= MathHelper.Pi && quadm <= MathHelper.TwoPi)) &&
                    ((pos.X > 0) || (quadm <= MathHelper.PiOver2 || quadm >= 3 * MathHelper.PiOver2)) &&
                    ((pos.X < w) || (quadm >= MathHelper.PiOver2 && quadm <= 3 * MathHelper.PiOver2));
            return ok;
        }

        void Acceleration(GameTime time, bool isTrenieOnly)
        {
            if (isOKUp())
            {
                if (!isTrenieOnly) currentSpeed += (2*a) / (time.ElapsedGameTime.Milliseconds);
                else currentSpeed += a / (time.ElapsedGameTime.Milliseconds);
                posTurret.X = pos.X -= (float)(currentSpeed * Math.Cos(rotation));
                posTurret.Y = pos.Y -= (float)(currentSpeed * Math.Sin(rotation));
            }
            else currentSpeed = 0;
        }

        void Brake(GameTime time, bool isTrenieOnly)
        {
            if (isOKDown())
            {
                if (!isTrenieOnly) currentSpeed -= (2*a) / (time.ElapsedGameTime.Milliseconds);
                else currentSpeed -= a / (time.ElapsedGameTime.Milliseconds);
                posTurret.X = pos.X -= (float)(currentSpeed * Math.Cos(rotation));
                posTurret.Y = pos.Y -= (float)(currentSpeed * Math.Sin(rotation));
            }
            else currentSpeed = 0;
        }

        void TurnLeft()
        {
            rotation -= 0.02f;
            turretRotation -= 0.02f;
        }

        void TurnRight()
        {
            rotation += 0.02f;
            turretRotation += 0.02f;
        }

        void TurnTurretLeft()
        {
            turretRotation -= 0.04f;
        }

        void TurnTurretRight()
        {
            turretRotation += 0.04f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tank, pos, null, Color.White, rotation, tankCenter, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(cannon, posTurret, null, Color.White, turretRotation, turretCenter, 1f, SpriteEffects.None, 1f);
        }

    }
}
