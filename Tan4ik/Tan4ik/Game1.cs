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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using Microsoft.Xna.Framework.Net;
//using tan4ik;

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
        Tank mytank;
        Shell shellPack;

        int w, h, flag = 0;

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

            mytank = new Tank(tank, cannon, new Vector2(500, 400));
            shellPack = new Shell(cannonball);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Q))
                this.Exit();
            if (kb.IsKeyDown(Keys.Escape))
                flag = 0;
            if (kb.IsKeyDown(Keys.Space))
                flag = 1;


            // TODO: Add your update logic here
            if (flag==1)
            {
                mytank.Update(kb, gameTime);
                shellPack.Update(kb, mytank.posTurret, mytank.turretRotation);
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
                mytank.Draw(spriteBatch);
                shellPack.Draw(spriteBatch);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
