/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Input;

namespace Tan4ik
{
    class Shell
    {
        Texture2D shell;
        Vector2 shellCenter;
        ArrayList pos;
        int w = 800, h = 600;
        double x, y;
        DateTime prev;
        bool shoot;

        public Shell(Texture2D shell)
        {
            this.shell = shell;
            shellCenter = new Vector2(x: shell.Width / 2, y: shell.Height / 2);
            pos = new ArrayList();
            prev = DateTime.Now;
        }

        public int Update(KeyboardState kb, int typeKeys, Vector2 posTank, float rotation, float turretRotation, Vector2 enemypos, int score)
        {
            if (typeKeys == 1) shoot = kb.IsKeyDown(Keys.LeftShift);
            if (typeKeys == 2) shoot = kb.IsKeyDown(Keys.Enter);
            if (shoot && (DateTime.Now - prev).Milliseconds >= 300)
            {
                pos.Add(new Vector3((float)(posTank.X - 55 * Math.Cos(turretRotation)), (float)(posTank.Y - 55 * Math.Sin(turretRotation)), turretRotation));
                prev = DateTime.Now;
            }
            x = Math.Abs(64 * Math.Cos(rotation) + 64 * Math.Sin(rotation)) / 2;
            y = Math.Abs(70 * Math.Cos(rotation) + 70 * Math.Sin(rotation)) / 2;
            
            for (int i = 0; i < pos.Count; i++)
            {
                pos[i] = Upball(((Vector3)pos[i]).X, ((Vector3)pos[i]).Y, ((Vector3)pos[i]).Z);

                if (((Vector3)pos[i]).X >= enemypos.X - x  && ((Vector3)pos[i]).X <= enemypos.X + x && ((Vector3)pos[i]).Y >= (enemypos.Y - y) && ((Vector3)pos[i]).Y <= enemypos.Y + y)
                {
                    score += 5;
                    pos.Remove(pos[i]);
                } else
                
                if (((Vector3)pos[i]).X == -5)
                    pos.Remove(pos[i]);
               
            }
            return score;
        }

        Vector3 Upball(float x, float y, float z)
        {
            x -= (float)(8 * Math.Cos(z));
            y -= (float)(8 * Math.Sin(z));
            if (x > w || x < 0 || y > h || y < 0)
                x = -5;
            return new Vector3(x, y, z);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector3 ball in pos)
            {
                spriteBatch.Draw(shell, new Vector2(ball.X, ball.Y), null, Color.White, 0f, shellCenter, 1f, SpriteEffects.None, 1f);
            }
        }
    }

}

