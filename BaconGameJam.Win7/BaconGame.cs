using BaconGameJam.Common;
using BaconGameJam.Common.Models.Sounds;
using BaconGameJam.Win7.ViewModels;
using BaconGameJam.Win7.ViewModels.States;
using BaconGameJam.Win7.Views;
using BaconGameJam.Win7.Views.Input;
using BaconGameJam.Win7.Views.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BaconGameJam.Win7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BaconGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ConductorView conductorView;
        private MouseInputManager inputManager;
        private KeyboardInputManager keyInputManager;
        private SoundManagerView soundManagerView;

        public BaconGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = Constants.ScreenWidth;
            this.graphics.PreferredBackBufferHeight = Constants.ScreenHeight;
            //this.graphics.IsFullScreen = true;
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Bootstrapper bootstrapper = new Bootstrapper(this.Content, this.spriteBatch);

            this.conductorView = bootstrapper.GetInstance<ConductorView>();
            this.inputManager = bootstrapper.GetInstance<MouseInputManager>();
            this.keyInputManager = bootstrapper.GetInstance<KeyboardInputManager>();
            this.soundManagerView = bootstrapper.GetInstance<SoundManagerView>();

            IConductorViewModel conductorViewModel = bootstrapper.GetInstance<IConductorViewModel>();
            conductorViewModel.Push(typeof(TitleViewModel));

            this.soundManagerView.LoadContent(this.Content);
            ISoundManager soundManager = bootstrapper.GetInstance<ISoundManager>();
            soundManager.PlayMusic();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState state = Mouse.GetState();
            this.inputManager.Update(state.LeftButton, new Point(state.X, state.Y));

            KeyboardState keyboardState = Keyboard.GetState();
            this.keyInputManager.Update(keyboardState);

            this.conductorView.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Blue);

            this.conductorView.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
