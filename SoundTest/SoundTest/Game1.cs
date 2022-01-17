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

namespace SoundTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private DynamicSoundEffectInstance _soundEngine;
        private AudioComponentContainer mixer;
        private bool _paintWhite;
        private bool _doPaintWhite;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            _soundEngine = new DynamicSoundEffectInstance(_sampleRate, AudioChannels.Stereo);
            _soundEngine.BufferNeeded += new EventHandler<EventArgs>(_soundEngine_BufferNeeded);
            base.Initialize();
        }

        void _soundEngine_BufferNeeded(object sender, EventArgs e)
        {
#if TRACE_ON
            if (_soundEngine.PendingBufferCount < 2)
                System.Diagnostics.Trace.TraceWarning("Pending: {0}", _soundEngine.PendingBufferCount);
#endif
            SubmitBuffer();
            SubmitBuffer();
        }

        const int _channels = 2;

        private void SubmitBuffer()
        {
            _sequencer.UpdatePosition(_frame, _sampleSize / (_channels + sizeof(short)));
            mixer.WriteToOutput(_soundbuffers, _offset, _sampleSize, _frame);
            _soundEngine.SubmitBuffer(_soundbuffers, _offset, _sampleSize);
            _offset += _sampleSize;
            if (_offset >= _soundbuffers.Length)
                _offset = 0;
            _frame++;
        }

        const int bufferCount = 4;
        public static int _sampleSize;

        byte[] _soundbuffers; 
        private int _offset;
        private int _frame;
        private MusicSequencer _sequencer;
        private const int _sampleRate = 41000;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mixer = Mixer.Create();

            // const int bufferMilliseconds = 11;
            _sampleSize = 2048; //  _soundEngine.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(bufferMilliseconds)); 
            mixer.SetBlockSize(2, 2);

            var sampler = LoadSampler();
            mixer.AddComponent("sampler", sampler);
            // mixer.AddComponent("test signal", new SineGenerator(_sampleRate, 2, 16));

            _soundbuffers = new byte[bufferCount * _sampleSize];

            _sequencer = new MusicSequencer(_sampleRate, 181);

            foreach (var item in mixer.GetComponents())
            {
                if (item.Value is INotePlayer)
                {
                    var track = new SequencerTrack(item.Value as INotePlayer);
                    _sequencer.AddTrack(item.Key, track);
                    AddQuarterNotesToTrack(track);
                }
            }

        }

        private void AddQuarterNotesToTrack(SequencerTrack track)
        {
            for (int meas = 0; meas < 2; meas++)
            {
                for (int beat = 0; beat < 4; beat++)
                {
                    var loc = new EventLocation();
                    loc.Beat = beat;
                    loc.Measure = meas;
                    _sequencer.AddEvent(track, loc);
                }
            }
        }

        private IAudioComponent LoadSampler()
        {
            using (var file = Microsoft.Xna.Framework.TitleContainer.OpenStream("Bass.01.wav"))
            {
                var waveFile = SampleFile.OpenWAVFile(file);
                var sampler = SamplerInstrument.Create(waveFile);
                return sampler;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.Content.Unload();
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

            // TODO: Add your update logic here
            if (_paintWhite)
            {
                _paintWhite = false;
                _doPaintWhite = true;
            }

            if (_soundEngine.State != SoundState.Playing)
            {
                _soundEngine.Play();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_doPaintWhite ? Color.White : Color.CornflowerBlue);
#if DEBUG
            if (_doPaintWhite)
            // System.Diagnostics.Debugger.Break();
            {
            }
#endif 
            _doPaintWhite = false;
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public SoundEffect _sound { get; set; }

        protected override void EndRun()
        {
            base.EndRun();
            Utilities.FlushLog();
        }
    }
}
