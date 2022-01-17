using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using System.Xml;
using AudioLibrary;
using TheGrid.UI;
using System.Threading;
using System.Collections.Generic;
#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif


namespace TheGrid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        const string _storageFileName = "Helium10.dat";

        GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; private set; } 

        GamePage[] _pages;

        // must be in sync with Initialize method!
        public enum Pages
        {
#if OPTIONS_PAGE
            Options,
#endif
            Synth,
            Bass,
            Drums,
        };

        const string _synthTrackName = "Synth";
        const string _drumTrackName = "Drums";
        const string _bassTrackName = "Bass";

        GamePage ActivePage
        {
            get
            {
                return _pages[_currentPage];
            }
        }

        int _currentPage = (int) Pages.Drums;

        // input states
        public struct State
        {            
            public GamePadState GamePadState;
            public KeyboardState KeyboardState;
            public MouseState MouseState;
        }

        State _currentState;
        State _previousState;
        NavigationBar _navigation;
        private bool _isLoading = false;

        private int _loadingCount;
        private AudioEngine _audioEngine;
        private SplashScreen _splashScreen;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
#if WINDOWS_PHONE
            InitPhoneServices();
#endif
        }
#if WINDOWS_PHONE
        private void InitPhoneServices()
        {
            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(Current_Activated);
            PhoneApplicationService.Current.Closing += new EventHandler<ClosingEventArgs>(Current_Closing);
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(Current_Deactivated);
            PhoneApplicationService.Current.Launching += new EventHandler<LaunchingEventArgs>(Current_Launching);
        }

        void Current_Launching(object sender, LaunchingEventArgs e)
        {
            LoadGameState(false);
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            SaveGameState(true);
        }

        void Current_Closing(object sender, ClosingEventArgs e)
        {
            SaveGameState(false);
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            LoadGameState(true);
        }
#elif WINDOWS
        private void LoadLastDocument()
        {
            var dirName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filename = System.IO.Path.Combine(dirName, _storageFileName);
            if (!System.IO.File.Exists(filename))
                return;
            try
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    LoadGameXML(stream);
                }
            }
            catch(Exception){}
        }

        private void SaveLastDocument()
        {
            var dirName = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var filename = System.IO.Path.Combine(dirName, _storageFileName);
            using (var stream = System.IO.File.Open(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                SaveGameXML(stream);
            }
        }
#endif 


#region serialization
        private void LoadGameState(bool isTombstoning)
        {
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorageFile.FileExists(_storageFileName))
                    return;
                using (var stream = isolatedStorageFile.OpenFile(_storageFileName, FileMode.Open, FileAccess.Read))
                {
                    LoadGameXML(stream);
                }
            }
        }

        private void LoadGameXML(Stream stream)
        {
            try
            {
                using (var reader = XmlReader.Create(stream))
                {
                    ReadGameState(reader);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);

            }
        }

        private void ReadGameState(XmlReader reader)
        {
            reader.ReadToDescendant("AppState");

            if (_audioEngine.ReadXML(reader))
                reader.ReadEndElement();// AppState
        }

        private void SaveGameState(bool isTombstoning)
        {
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = isolatedStorageFile.OpenFile(_storageFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    SaveGameXML(stream);

                }
            }
        }

        private void SaveGameXML(Stream stream)
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;

            using (var writer = XmlWriter.Create(stream, settings))
            {
                SaveGameState(writer);
            }
#if DEBUG
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
                System.Diagnostics.Debug.WriteLine(reader.ReadToEnd());
#endif
        }

        private void SaveGameState(XmlWriter writer)
        {
            System.Diagnostics.Debug.WriteLine("Start saving state.");

            writer.WriteStartDocument();
            writer.WriteStartElement("AppState");

            _audioEngine.SaveXML(writer);

            writer.WriteEndElement();// AppState

            System.Diagnostics.Debug.WriteLine("End saving state.");
        }
#endregion //serialization


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _isLoading = true;
            _audioEngine = AudioEngine.Create(44100, Microsoft.Xna.Framework.Audio.AudioChannels.Stereo);

            this.IsMouseVisible = true;

            //this._graphics.IsFullScreen = true;
            //this._graphics.ApplyChanges();
            
            base.Initialize();
        }

        private void CreateUI()
        {
            _pages = new GamePage[] {
#if OPTIONS_PAGE
                new OptionsPage(),
#endif
                GridPage.Create(
                    _synthTrackName,
                    CreateTrackFacade(_synthTrackName, Scales.IntrigueScale, MusicKey.Db, 2), 
                    ButtonSelectBehavior.Paint, 
                    (int row) => Color.White,
                    2 /* ticks per button */), 
                GridPage.Create(
                    _bassTrackName,
                    CreateTrackFacade(_bassTrackName, Scales.IntrigueScale, MusicKey.Db, 2), 
                    ButtonSelectBehavior.OnlyOne, 
                    (int row) => Color.White, 
                    2 /* ticks per button */),
                GridPage.Create(
                    _drumTrackName,
                    CreateTrackFacade(_drumTrackName, Scales.Chromatic, MusicKey.C, 4), 
                    ButtonSelectBehavior.Paint, 
                    (int row) => (row % 2 ) == 0 ? Color.White : Color.LightSlateGray, 
                    4 )
            };
            _navigation = new NavigationBar();
        }

        void _track_PreloadStarted(object sender, EventArgs e)
        {
            _loadingCount++;
        }

        void Track_PreloadFinished(object sender, EventArgs e)
        {
            _loadingCount--;
            if (_loadingCount <= 0)
            {
                _isLoading = false;
                _audioEngine.Start();
                _splashScreen.StartFade();

            }
        }


        private TrackGridFacade CreateTrackFacade(string trackName, IEnumerable<NoteValue> scaleNotes, MusicKey key, int columnsPerQuarter)
        {
            var sequencer = _audioEngine.Sequencer;
            var track = sequencer.Tracks[trackName];

            track.PreloadStarted += new EventHandler(_track_PreloadStarted);
            track.PreloadFinished += new EventHandler(Track_PreloadFinished);

            return new TrackGridFacade(track, sequencer, new NoteEventMap(scaleNotes, GridPage.Rows, (int)key), columnsPerQuarter);
        }

        void LoadAudioComponents()
        {
            System.Diagnostics.Debug.Assert(_audioEngine != null);

            _audioEngine.AddInstrumentTrack(_drumTrackName, SamplerInstrument.Create(SoundBank.GetPatch("RealDrums"), 16), 1, 1.0d);
            _audioEngine.AddInstrumentTrack(_synthTrackName, SamplerInstrument.Create(SoundBank.GetPatch("Glock"), 16), 2, 1.0d);
            _audioEngine.AddInstrumentTrack(_bassTrackName, SamplerInstrument.Create(SoundBank.GetPatch("Bass2"), 1), 2, 0.25d);
            // _audioEngine.AddInstrumentTrack(_bassTrackName, SynthesizerInstrument.Create(Waveform.Sine), 2);
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Utilities.LoadGlobalContent(Content);
            
            _splashScreen = SplashScreen.Create(this, Utilities.LoadingScreen, 1000);
            _splashScreen.Enabled = true;
            _splashScreen.Initialize();
            ThreadPool.QueueUserWorkItem(o => { LoadContentAsync(); });
        }

        private void LoadContentAsync()
        {
            var startTicks = System.DateTime.Now;
            Debug.WriteLine(String.Format("LoadContentAsync started."));
            _track_PreloadStarted(this, new EventArgs());
            SoundBank.InitStaticContent(Content);
            MusicalButton.InitStaticContent(Content);
            NavigationButton.InitStaticContent(Content);

            LoadAudioComponents();

            CreateUI();

            foreach (var page in _pages)
            {
                page.Initialize(GraphicsDevice.Viewport.TitleSafeArea, GraphicsDevice.Viewport.Bounds, Content);
            }
            _navigation.Initialize(GraphicsDevice.Viewport.TitleSafeArea, GraphicsDevice.Viewport.Bounds, Content);
            _navigation.NavigationRequest += new EventHandler<NavigationEventArgs>(OnNavigationRequest);
            _navigation.ClearRequest += new EventHandler(OnClearRequest);

            SetActivePage(Pages.Drums);
#if WINDOWS
            LoadLastDocument();
#endif
            Debug.WriteLine(String.Format("LoadContentAsync finished ({0}ms)", (System.DateTime.Now - startTicks).TotalMilliseconds));
            Track_PreloadFinished(this, new EventArgs());
        }

        void OnClearRequest(object sender, EventArgs ignored)
        {
            var pages = from p in _pages
                        where p is GridPage
                        select (GridPage)p;
            foreach (var page in pages)
                page.Clear();                      

        }

        void OnNavigationRequest(object sender, NavigationEventArgs args)
        {
            SetActivePage(args.Page);
        }

        private void SetActivePage(Pages page)
        {
            _currentPage = (int)page;
            _navigation.SetActivePage(page);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            SoundBank.UnloadContent(Content);
            NavigationButton.UnloadContent(Content);
            MusicalButton.UnloadContent(Content);
        }

        protected override void EndRun()
        {
#if WINDOWS
            SaveLastDocument();
#endif 
            base.EndRun();
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (IsExitRequested())
            {
                this._audioEngine.Stop();
                this.Exit();
            }
            UpdateInputState();

            if (!_isLoading)
            {

                // TODO: Normalize mouse state so that we get "Paint" effects in addition to "clicked" locations
                // if (currentMouseState.LeftButton == ButtonState.Pressed)

                ActivePage.HandleInput(_currentState, _previousState);
                _navigation.HandleInput(_currentState, _previousState);

                foreach (var page in this._pages)
                {
                    page.Update(gameTime);
                }
                _navigation.Update(gameTime);
                _splashScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void UpdateInputState()
        {
            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            _previousState = _currentState;

            // Read the current state of the keyboard and gamepad and store it
            _currentState.KeyboardState = Keyboard.GetState();
            _currentState.GamePadState = GamePad.GetState(PlayerIndex.One);
            _currentState.MouseState = Mouse.GetState();
        }

        private static bool IsExitRequested()
        {
            return ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                (Keyboard.GetState(PlayerIndex.One).GetPressedKeys().Contains(Keys.Escape)));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // Start drawing
            SpriteBatch.Begin();

            if (_isLoading)
            {
            }
            else
            {
                ActivePage.Draw(SpriteBatch);
                _navigation.Draw(SpriteBatch);
            }
            _splashScreen.Draw(gameTime);
            
            // Stop drawing
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
