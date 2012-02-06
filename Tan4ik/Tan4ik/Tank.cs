/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tan4ik
{
    class Tank
    {
        public int w = 800, h = 600, score = 0, typeKeys;
        public Vector2 pos, tankCenter, turretCenter;
        public Vector2 posTurret;
        float rotation;
        public float turretRotation;
        const double a = 0.1;
        double currentSpeed;
        bool ok;
        float quadm;
        const float ws = 0.05f;
        Texture2D tank, cannon;
        bool forward, back, turnRight, turnLeft, turnTurretRight, turnTurretLeft;
        SpriteFont spr;
        Shell shellPack;

        public Tank(Texture2D tank, Texture2D cannon, Texture2D shell, Vector2 pos, int typeKeys, SpriteFont spr)
        {
            this.tank = tank;
            this.cannon = cannon;
            shellPack = new Shell(shell);

            this.pos = pos;
            tankCenter = new Vector2(tank.Width / 2, tank.Height / 2);
            
            currentSpeed = 0;

            posTurret = this.pos;

            turretCenter = new Vector2(tank.Width / 2 + 20, cannon.Height / 2);

            if (typeKeys == 1)
            {
                rotation = MathHelper.Pi;
                turretRotation = MathHelper.Pi;
            }
            else if(typeKeys == 2)
            {
                rotation = 0;
                turretRotation = 0;
            }

            this.spr = spr;

        }
        
        public void Update(KeyboardState kb, Vector2 enemypos, int typeKeys, GameTime gameTime)
        {
            score = shellPack.Update(kb, typeKeys, pos, rotation, turretRotation, enemypos, score);
            this.typeKeys = typeKeys;
            if (typeKeys == 1)
            {
                forward = kb.IsKeyDown(Keys.W);
                back = kb.IsKeyDown(Keys.S);
                turnRight = kb.IsKeyDown(Keys.D);
                turnLeft = kb.IsKeyDown(Keys.A);
                turnTurretRight = kb.IsKeyDown(Keys.E);
                turnTurretLeft = kb.IsKeyDown(Keys.Q);
            }
            else if (typeKeys == 2)
            {
                forward = kb.IsKeyDown(Keys.NumPad5);
                back = kb.IsKeyDown(Keys.NumPad2);
                turnRight = kb.IsKeyDown(Keys.NumPad3);
                turnLeft = kb.IsKeyDown(Keys.NumPad1);
                turnTurretRight = kb.IsKeyDown(Keys.NumPad6);
                turnTurretLeft = kb.IsKeyDown(Keys.NumPad4);
            }
            if (!forward && !back && currentSpeed > 0 && IsOkUp())
                Brake(gameTime, true);
            else if (!forward && !back && currentSpeed > 0 && !IsOkUp())
                currentSpeed = 0;

            if (!forward && !back && currentSpeed < 0 && IsOkDown())
                Acceleration(gameTime, true);
            else if (!forward && !back && currentSpeed < 0 && !IsOkDown())
               currentSpeed = 0;

            if (forward && IsOkUp())
            {

                Acceleration(gameTime, false);

            }
            else if (!IsOkUp())
                currentSpeed = 0;


            if (back && IsOkDown())
            {
                Brake(gameTime, false);
            }
            else if (!IsOkDown())
                currentSpeed = 0;

            if (turnRight)
            {
                TurnRight();
            }

            if (turnLeft)
            {
                TurnLeft();
            }

            if (turnTurretLeft)
            {
                TurnTurretLeft();
            }
            if (turnTurretRight)
            {
                TurnTurretRight();
            }

        }

        bool IsOkUp()
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

        bool IsOkDown()
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

        void Acceleration(GameTime time, bool onlyFriction)
        {
            if (IsOkUp())
            {
                if (!onlyFriction) currentSpeed += (3*a) / (time.ElapsedGameTime.Milliseconds);
                else currentSpeed += 2*a / (time.ElapsedGameTime.Milliseconds);
                posTurret.X = pos.X -= (float)(currentSpeed * Math.Cos(rotation));
                posTurret.Y = pos.Y -= (float)(currentSpeed * Math.Sin(rotation));
            }
            else currentSpeed = 0;
        }

        void Brake(GameTime time, bool onlyFriction)
        {
            if (IsOkDown())
            {
                if (!onlyFriction) currentSpeed -= (3*a) / (time.ElapsedGameTime.Milliseconds);
                else currentSpeed -= 2*a / (time.ElapsedGameTime.Milliseconds);
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
            shellPack.Draw(spriteBatch);
            if (typeKeys == 1) spriteBatch.DrawString(spr, "Score: " + score, new Vector2(10, 10), Color.Green);
            if (typeKeys == 2) spriteBatch.DrawString(spr, "Score: " + score, new Vector2(700, 10), Color.Red);
        }

    }
}
