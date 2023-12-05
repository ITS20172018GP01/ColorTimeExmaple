using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace ColorTimeExmaple
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private float frameSpan;
        private double totalMl;
        private double frameCountDown;
        private TimeSpan updateDuration;
        Color c = Color.White;
        string text = "Sample Color";
        TimeSpan t = TimeSpan.FromSeconds(5);
        int colorRange = 255;
        float updateRate = 0.4126f; //0.4126f;
        float colorTick = 0;
        private float count;
        public enum COUNT_STATE { NONE, STARTED, COUNTING, FINISHED}
        COUNT_STATE countState = COUNT_STATE.NONE;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("message");
            // 1 second = 1000 milliseconds
            // 255 color range
            // total milliseconds in the count down
            totalMl = t.TotalMilliseconds;
            // calculate the color range reduction over the count down time
            frameCountDown = t.TotalMilliseconds/colorRange;
            // calculate a time span that we can use to compare the time to update the color
            // we adjust the interval as there are rounding errors as timespan uses ints
            // but TotalMilliseconds is a double
            // decreasing by total seconds (int)totalMl/1000 
            updateDuration = TimeSpan.FromMilliseconds(frameCountDown - (int)totalMl / 1000);
            //updateDuration = new TimeSpan(0, 0, 0, 0, (int)frameCountDown - (int)totalMl/1000 );
            // Start the clock rolling
            countState = COUNT_STATE.STARTED;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch (countState)
            {
                case COUNT_STATE.STARTED:
                    // go straight into tcounting down
                    // We could do some setup work here is necessary
                    countState = COUNT_STATE.COUNTING;
                    break;
                case COUNT_STATE.COUNTING:
                    // decrese the update interval
                    updateDuration -= gameTime.ElapsedGameTime;
                    if(updateDuration.TotalMilliseconds <= 0)
                    {
                        if(c.A > 0)
                            c.A -= 1;
                        updateDuration = TimeSpan.FromMilliseconds(frameCountDown - (int)totalMl / 1000);

                        //updateDuration = new TimeSpan(0, 0, 0, 0, (int)frameCountDown - (int)totalMl / 1000);
                    }
                    if (t.TotalMilliseconds > 0)
                    {
                        t -= gameTime.ElapsedGameTime;
                    }
                    else countState = COUNT_STATE.FINISHED;
                    break;

                case COUNT_STATE.FINISHED:
                    Exit();
                    break;
                default:
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
            _spriteBatch.DrawString(font,text,Vector2.Zero,c);
            _spriteBatch.DrawString(font, c.A.ToString(), Vector2.Zero + 
                new Vector2(0,font.MeasureString(text).Y + 10), c);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}