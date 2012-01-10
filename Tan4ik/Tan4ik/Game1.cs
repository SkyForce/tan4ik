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
        bool ok, f = false;
        DateTime prevdt=DateTime.Now;
        SpriteFont spr;

        NetworkSessionProperties searchProperties = null;
        AvailableNetworkSessionCollection availableSessions;
        AvailableNetworkSession availableSession;
        NetworkSession session;
        int sessionIndex = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));
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

        static protected void SignIn()
        {
            if (!Guide.IsVisible)
                Guide.ShowSignIn(1, true);
        }

        void HostGame()
        {
            if (SignedInGamer.SignedInGamers.Count == 0)
                SignIn();

            else if (SignedInGamer.SignedInGamers.Count == 1)
            {
                NetworkSessionProperties sessionProperties = new NetworkSessionProperties();

                int maximumGamers = 2;  // The maximum supported is 31
                int privateGamerSlots = 0;
                int maximumLocalPlayers = 1;


                // Create the session
                session = NetworkSession.Create(
                    NetworkSessionType.PlayerMatch,
                    maximumLocalPlayers, maximumGamers, privateGamerSlots,
                    sessionProperties);

                //isServer = true;
                session.AllowHostMigration = true;
                session.AllowJoinInProgress = true;
                flag = 3;
            }
        }

        void FindGame()
        { 
            if (SignedInGamer.SignedInGamers.Count == 0)
                    SignIn();
                else if (SignedInGamer.SignedInGamers.Count == 1)
                {
                    int maximumLocalPlayers = 1;
                    availableSessions = NetworkSession.Find(
                        NetworkSessionType.SystemLink, maximumLocalPlayers,
                        searchProperties);

                    if (availableSessions.Count != 0)
                     availableSession = availableSessions[sessionIndex];

                    if (availableSession != null)
                    {
                        string HostGamerTag = availableSession.HostGamertag;
                        int GamersInSession = availableSession.CurrentGamerCount;
                        int OpenPrivateGamerSlots =
                            availableSession.OpenPrivateGamerSlots;
                        int OpenPublicGamerSlots =
                            availableSession.OpenPublicGamerSlots;
                        string sessionInformation =
                            "Session available from gamertag " + HostGamerTag +
                            "\n" + GamersInSession +
                            " players already in this session. \n" +
                            +OpenPrivateGamerSlots +
                            " open private player slots available. \n" +
                            +OpenPublicGamerSlots + " public player slots available.";
                        GraphicsDevice.Clear(Color.CornflowerBlue);
                        spriteBatch.DrawString(spr, sessionInformation,
                            new Vector2(100, 50), Color.Gray);
                    }
                    else flag = 0;
                }
        }

        void JoinGame()
        {
            if (availableSessions.Count - 1 >= sessionIndex)
            {
                session = NetworkSession.Join(availableSessions[sessionIndex]);

                session.GamerJoined += new EventHandler<GamerJoinedEventArgs>(session_GamerJoined);
                session.GamerLeft +=
                    new EventHandler<GamerLeftEventArgs>(session_GamerLeft);
                session.GameStarted +=
                    new EventHandler<GameStartedEventArgs>(session_GameStarted);
                session.GameEnded +=
                    new EventHandler<GameEndedEventArgs>(session_GameEnded);
                session.SessionEnded +=
                    new EventHandler<NetworkSessionEndedEventArgs>(
                        session_SessionEnded);
            }
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
            if (kb.IsKeyDown(Keys.Escape))
                this.Exit();
            if (!f && kb.IsKeyDown(Keys.Space))
                flag = 1;
            if (!f && kb.IsKeyDown(Keys.X))
                flag = 2;
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
            else if (flag == 2)
            {
                FindGame();
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
                spriteBatch.DrawString(spr, "Press X to try find available sessions", new Vector2(50, 70), Color.Gray);
                spriteBatch.DrawString(spr, "Press G to create session", new Vector2(50, 90), Color.Gray);
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
