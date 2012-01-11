/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * Это произведение распространяется по лицензии 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported. 
 * Чтобы ознакомиться с экземпляром этой лицензии, 
 * посетите http://creativecommons.org/licenses/by-nc-nd/3.0/
 * или отправьте письмо на адрес Creative Commons: 
 * 171 Second Street, Suite 300, San Francisco, California, 94105, USA.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using Microsoft.Xna.Framework.Net;

namespace Tan4ik
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tank, cannon, cannonball;
        Vector2 post, posc, origint, originc, originb;
        ArrayList posb;

        float rotation=0,rotationc=0, scale=1.0f, depth=1.0f, quadm;
        int w, h, flag = 0;
        bool ok;
        DateTime prevdt=DateTime.Now;
        SpriteFont spr;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            w = graphics.PreferredBackBufferWidth = 800;
            h = graphics.PreferredBackBufferHeight = 600;
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

            base.Initialize();
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
            spr = Content.Load<SpriteFont>("spritefont");
            tank = Content.Load<Texture2D>("tank");
            cannon = Content.Load<Texture2D>("cannon");
            cannonball = Content.Load<Texture2D>("cannonball");

            post = new Vector2(500, 400);
            posc = new Vector2(500, 400);
            posb = new ArrayList();

            origint = new Vector2(tank.Width/2,tank.Height/2);
            originc = new Vector2(tank.Width/2+20, cannon.Height/2);
            originb = new Vector2(cannonball.Width / 2, cannonball.Height / 2);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        Vector3 upball(float x, float y, float z)
        {
            x -= (float)(8 * Math.Cos(z));
            y -= (float)(8 * Math.Sin(z));
            if (x > w || x < 0 || y > h || y < 0)
                x = -5;
            return new Vector3(x, y, z);
        }

        

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Q))
                this.Exit();
            if (kb.IsKeyDown(Keys.Escape))
                flag = 0;
            var mousepos = Mouse.GetState();
            if (kb.IsKeyDown(Keys.Space))
                flag = 1;


            // TODO: Add your update logic here
            if (flag==1)
            {
                for (int i = 0; i < posb.Count; i++)
                {
                    posb[i] = upball(((Vector3)posb[i]).X, ((Vector3)posb[i]).Y, ((Vector3)posb[i]).Z);
                    if (((Vector3)posb[i]).X == -5)
                        posb.Remove(posb[i]);
                }

                var dt = DateTime.Now - prevdt;

                if (kb.IsKeyDown(Keys.Space) && dt.Milliseconds >= 300)
                {
                    posb.Add(new Vector3((float)(posc.X - 55 * Math.Cos(rotationc)), (float)(posc.Y - 55 * Math.Sin(rotationc)), rotationc));
                    prevdt = DateTime.Now;
                }

                quadm = (rotation % MathHelper.TwoPi);
                if (quadm < 0)
                    quadm += MathHelper.TwoPi;

                ok = ((post.Y > 0) || (quadm <= MathHelper.TwoPi && quadm >= MathHelper.Pi)) &&
                    ((post.Y < h) || (quadm >= 0 && quadm <= MathHelper.Pi)) &&
                    ((post.X > 0) || (quadm >= MathHelper.PiOver2 && quadm <= 3 * MathHelper.PiOver2)) &&
                    ((post.X < w) || (quadm <= MathHelper.PiOver2 || quadm >= 3 * MathHelper.PiOver2));

                if (kb.IsKeyDown(Keys.Up) && ok)
                {

                    post.X -= (float)(3 * Math.Cos(rotation));
                    posc.X -= (float)(3 * Math.Cos(rotation));

                    post.Y -= (float)(3 * Math.Sin(rotation));
                    posc.Y -= (float)(3 * Math.Sin(rotation));

                }

                ok = ((post.Y > 0) || (quadm >= 0 && quadm <= MathHelper.Pi)) &&
                    ((post.Y < h) || (quadm >= MathHelper.Pi && quadm <= MathHelper.TwoPi)) &&
                    ((post.X > 0) || (quadm <= MathHelper.PiOver2 || quadm >= 3 * MathHelper.PiOver2)) &&
                    ((post.X < w) || (quadm >= MathHelper.PiOver2 && quadm <= 3 * MathHelper.PiOver2));

                if (kb.IsKeyDown(Keys.Down) && ok)
                {

                    post.X += (float)(3 * Math.Cos(rotation));
                    posc.X += (float)(3 * Math.Cos(rotation));


                    post.Y += (float)(3 * Math.Sin(rotation));
                    posc.Y += (float)(3 * Math.Sin(rotation));

                }

                if (kb.IsKeyDown(Keys.Right))
                {
                    rotation += 0.02f;
                    rotationc += 0.02f;
                }

                if (kb.IsKeyDown(Keys.Left))
                {
                    rotation -= 0.02f;
                    rotationc -= 0.02f;
                }

                if (kb.IsKeyDown(Keys.A))
                {
                    rotationc -= 0.04f;
                }
                if (kb.IsKeyDown(Keys.D))
                {
                    rotationc += 0.04f;
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

            if (flag == 0)
            {
                spriteBatch.DrawString(spr, "Press Space to start game", new Vector2(50, 50), Color.Gray);

            }
            else if(flag == 1)
            {
                
                spriteBatch.Draw(tank, post, null, Color.White, rotation, origint, scale, SpriteEffects.None, depth);
                spriteBatch.Draw(cannon, posc, null, Color.White, rotationc, originc, scale, SpriteEffects.None, depth);

                foreach (Vector3 ball in posb)
                {
                    spriteBatch.Draw(cannonball, new Vector2(ball.X, ball.Y), null, Color.White, 0f, originb, 1f, SpriteEffects.None, 1f);
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void session_GamerJoined(object sender, GamerJoinedEventArgs e)
        {

        }
        void session_GamerLeft(object sender, GamerLeftEventArgs e)
        {
        }
        void session_GameStarted(object sender, GameStartedEventArgs e)
        {
        }
        void session_GameEnded(object sender, GameEndedEventArgs e)
        {
        }
        void session_SessionEnded(object sender, NetworkSessionEndedEventArgs e)
        {
        }
    }
}
