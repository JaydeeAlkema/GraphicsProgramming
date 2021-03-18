using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FirstGame_OpenGL
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Lesson currentLesson;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager( this )
			{
				GraphicsProfile = GraphicsProfile.HiDef,
			};

			IsMouseVisible = false;

			// Cap framerate at 144hz
			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromSeconds( 1d / 144d ); //60);

			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			currentLesson = new Lesson5();
		}

		protected override void Initialize()
		{
			currentLesson.Initialize();

			_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 8 * 7;
			_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 8 * 7; ;
			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch( GraphicsDevice );

			currentLesson.LoadContent( Content, _graphics, _spriteBatch );
		}

		protected override void Update( GameTime gameTime )
		{
			if( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
				Exit();

			currentLesson.Update( gameTime );
			base.Update( gameTime );
		}

		protected override void Draw( GameTime gameTime )
		{
			GraphicsDevice.Clear( Color.CornflowerBlue );

			currentLesson.Draw( gameTime, _graphics, _spriteBatch );
			base.Draw( gameTime );
		}
	}
}
