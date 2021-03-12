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
		private Lesson lesson1;
		private Lesson lesson2;
		private Lesson lesson3;
		private Lesson lesson4;

		private List<Lesson> lessons = new List<Lesson>();

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

			lesson1 = new Lesson1();
			lesson2 = new Lesson2();
			lesson3 = new Lesson3();
			lesson4 = new Lesson4();

			lessons.Add( lesson1 );
			lessons.Add( lesson2 );
			lessons.Add( lesson3 );
			lessons.Add( lesson4 );

			currentLesson = lesson1;
		}

		protected override void Initialize()
		{
			foreach( Lesson lesson in lessons )
			{
				lesson.Initialize();
			}
			currentLesson.Initialize();

			_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 8 * 7;
			_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 8 * 7; ;
			_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch( GraphicsDevice );

			foreach( Lesson lesson in lessons )
			{
				lesson.LoadContent( Content, _graphics, _spriteBatch );
			}
			currentLesson.LoadContent( Content, _graphics, _spriteBatch );
		}

		protected override void Update( GameTime gameTime )
		{
			if( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
				Exit();

			SwitchLessonScenes();

			currentLesson.Update( gameTime );
			base.Update( gameTime );
		}

		protected override void Draw( GameTime gameTime )
		{
			GraphicsDevice.Clear( Color.CornflowerBlue );

			currentLesson.Draw( gameTime, _graphics, _spriteBatch );
			base.Draw( gameTime );
		}

		private void SwitchLessonScenes()
		{
			if( Keyboard.GetState().IsKeyDown( Keys.D1 ) )
			{
				currentLesson = lesson1;
			}
			else if( Keyboard.GetState().IsKeyDown( Keys.D2 ) )
			{
				currentLesson = lesson2;
			}
			else if( Keyboard.GetState().IsKeyDown( Keys.D3 ) )
			{
				currentLesson = lesson3;
			}
			else if( Keyboard.GetState().IsKeyDown( Keys.D4 ) )
			{
				currentLesson = lesson4;
			}
		}
	}
}
