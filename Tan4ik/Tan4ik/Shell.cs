/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        DateTime prev;

        public Shell(Texture2D shell)
        {
            this.shell = shell;
            shellCenter = new Vector2(shell.Width / 2, shell.Height / 2);
            pos = new ArrayList();
            prev = DateTime.Now;
        }

        public void Update(KeyboardState kb, Vector2 posTurret, float turretRotation)
        {
            if (kb.IsKeyDown(Keys.Space) && (DateTime.Now - prev).Milliseconds >= 300)
            {
                pos.Add(new Vector3((float)(posTurret.X - 55 * Math.Cos(turretRotation)), (float)(posTurret.Y - 55 * Math.Sin(turretRotation)), turretRotation));
                prev = DateTime.Now;
            }

            for (int i = 0; i < pos.Count; i++)
            {
                pos[i] = upball(((Vector3)pos[i]).X, ((Vector3)pos[i]).Y, ((Vector3)pos[i]).Z);
                if (((Vector3)pos[i]).X == -5)
                    pos.Remove(pos[i]);
            }
        }

        Vector3 upball(float x, float y, float z)
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

