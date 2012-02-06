/* Copyright (C) 2012 by SkyForce (twice@eml.ru)
 * This work is licensed under the 
 * Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License. 
 * To view a copy of this license, visit 
 * http://creativecommons.org/licenses/by-nc-nd/3.0/ or send a letter to Creative Commons, 
 * 444 Castro Street, Suite 900, Mountain View, California, 94041, USA.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Tan4ik
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch _spriteBatch;
        Texture2D _tank1, _cannon1, _tank2, _cannon2, _cannonball1, _cannonball2, _back, _textureNormal;

        Effect _deferred;

        Tank _tankmodel1, _tankmodel2;
        Lights _lights;

        int _flag = 0;

        SpriteFont _spr;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {PreferredBackBufferWidth = 800, PreferredBackBufferHeight = 600};
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spr = Content.Load<SpriteFont>("spritefont");
            _tank1 = Content.Load<Texture2D>("tank1");
            _tank2 = Content.Load<Texture2D>("tank2");
            _cannon1 = Content.Load<Texture2D>("cannon1");
            _cannon2 = Content.Load<Texture2D>("cannon2");
            _cannonball1 = Content.Load<Texture2D>("cannonball1");
            _cannonball2 = Content.Load<Texture2D>("cannonball2");
            _back = Content.Load<Texture2D>("test1");
            _textureNormal = Content.Load<Texture2D>("test1_map");

            _deferred = Content.Load<Effect>("deferred");

            _tankmodel1 = new Tank(_tank1, _cannon1, _cannonball1, new Vector2(50, 550), 1,_spr);
            _tankmodel2 = new Tank(_tank2, _cannon2, _cannonball2, new Vector2(750, 50), 2,_spr);
            _lights = new Lights(_back, _textureNormal, _deferred);


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
            if (kb.IsKeyDown(Keys.Q) && _flag == 0)
                this.Exit();
            if (kb.IsKeyDown(Keys.Escape))
            {
                _flag = 0;
                _tankmodel1 = new Tank(_tank1, _cannon1, _cannonball1, new Vector2(50, 500), 1, _spr);
                _tankmodel2 = new Tank(_tank2, _cannon2, _cannonball2, new Vector2(750, 100), 2,_spr);
                _lights = new Lights(_back, _textureNormal, _deferred);
            }
            if (kb.IsKeyDown(Keys.Space))
                _flag = 1;


            // TODO: Add your update logic here
            if (_flag==1)
            {
                _tankmodel1.Update(kb, _tankmodel2.pos, 1, gameTime);
                _tankmodel2.Update(kb, _tankmodel1.pos, 2, gameTime);
                _lights.Update(_tankmodel1.pos, _tankmodel2.pos);
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
            if (_flag == 1)
            {
                _lights.Draw(_spriteBatch);
            }

            _spriteBatch.Begin();

            if (_flag == 0)
            {
                _spriteBatch.DrawString(_spr, "Press Space to start game", new Vector2(50, 50), Color.YellowGreen);

            }
            else if(_flag == 1)
            {
                _tankmodel1.Draw(_spriteBatch);
                _tankmodel2.Draw(_spriteBatch);
            }
            
            _spriteBatch.End();

            

            base.Draw(gameTime);
        }

    }
}
