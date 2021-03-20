using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FirstGame_OpenGL
{
	class Lesson3 : Lesson
	{
		private Effect effect;
		Vector3 LightPosition = Vector3.Right * 2 + Vector3.Up * 2 + Vector3.Backward * 2;

		private Model sphere, cube;
		private Texture2D day, night, clouds, moon, mars;
		private TextureCube sky;

		private float yaw, pitch;
		private int prevX, prevY;

		private int previousScrollValue;
		private float scrollOffset = 0;

		private SpriteFont arial;

		public override void Initialize()
		{
			previousScrollValue = Mouse.GetState().ScrollWheelValue;
		}

		public override void Update( GameTime gameTime )
		{
			MouseState mstate = Mouse.GetState();

			if( mstate.LeftButton == ButtonState.Pressed )
			{
				yaw -= ( mstate.X - prevX ) * 0.005f;
				pitch -= ( mstate.Y - prevY ) * 0.005f;

				pitch = MathF.Min( MathF.Max( pitch, -MathF.PI * 0.45f ), MathF.PI * 0.45f );
			}

			SetScrollOffset();

			prevX = mstate.X;
			prevY = mstate.Y;
		}

		public override void LoadContent( ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			// Load Font
			arial = Content.Load<SpriteFont>( "Fonts/Default" );

			effect = Content.Load<Effect>( "Shaders/Lesson3" );

			day = Content.Load<Texture2D>( "Textures/day" );
			night = Content.Load<Texture2D>( "Textures/night" );
			clouds = Content.Load<Texture2D>( "Textures/clouds" );

			moon = Content.Load<Texture2D>( "Textures/2k_moon" );

			mars = Content.Load<Texture2D>( "Textures/5672_mars_4k_color" );

			sky = Content.Load<TextureCube>( "Textures/sky_cube" );

			sphere = Content.Load<Model>( "Models/uv_sphere" );
			cube = Content.Load<Model>( "Models/cube" );

			LoadModelEffects( sphere );
			LoadModelEffects( cube );
		}

		public override void Draw( GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			GraphicsDevice device = graphics.GraphicsDevice;

			float time = ( float )gameTime.TotalGameTime.TotalSeconds;
			LightPosition = Vector3.Left * 200;

			Vector3 cameraPos = ( -Vector3.Forward * ( 10 + scrollOffset ) );
			cameraPos = Vector3.Transform( cameraPos, Quaternion.CreateFromYawPitchRoll( yaw, pitch, 0 ) );

			Matrix World = Matrix.CreateWorld( Vector3.Zero, Vector3.Forward, Vector3.Up );
			Matrix View = Matrix.CreateLookAt( cameraPos, Vector3.Zero, Vector3.Up );

			SetEffectParameters( device, time, cameraPos, World, View );

			device.Clear( Color.Black );

			// Sky
			effect.CurrentTechnique = effect.Techniques["Sky"];
			device.DepthStencilState = DepthStencilState.None;
			device.RasterizerState = RasterizerState.CullNone;
			RenderModel( cube, Matrix.CreateTranslation( cameraPos ) );

			// Earth
			device.DepthStencilState = DepthStencilState.Default;
			device.RasterizerState = RasterizerState.CullCounterClockwise;
			effect.CurrentTechnique = effect.Techniques["Earth"];
			RenderModel( sphere, Matrix.CreateScale( 0.01f ) *
			Matrix.CreateRotationZ( time ) *
			Matrix.CreateRotationY( MathF.PI / 180 * 23 ) * World );

			// Moon
			effect.CurrentTechnique = effect.Techniques["Moon"];
			RenderModel( sphere, Matrix.CreateTranslation( Vector3.Down * 20 ) *
			Matrix.CreateScale( 0.0020f ) *
			Matrix.CreateRotationZ( time - time * 0.03333333f ) * World );

			// Mars
			effect.CurrentTechnique = effect.Techniques["Mars"];
			RenderModel( sphere, Matrix.CreateTranslation( Vector3.Down * -20 ) *
			Matrix.CreateScale( 0.0033f ) *
			Matrix.CreateRotationZ( time - time * 0.03333333f ) * World );


			// Draw Text to Screen
			spriteBatch.Begin();
			spriteBatch.DrawString( arial, "Use the scrollwheel to zoom in/out", new Vector2( 0, 0 ), Color.White );
			spriteBatch.End();
		}

		private void SetEffectParameters( GraphicsDevice device, float time, Vector3 cameraPos, Matrix World, Matrix View )
		{
			effect.Parameters["World"].SetValue( World );
			effect.Parameters["View"].SetValue( View );
			effect.Parameters["Projection"].SetValue( Matrix.CreatePerspectiveFieldOfView( ( MathF.PI / 180f ) * 25f, device.Viewport.AspectRatio, 0.001f, 1000f ) );

			effect.Parameters["DayTex"].SetValue( day );
			effect.Parameters["NightTex"].SetValue( night );
			effect.Parameters["CloudsTex"].SetValue( clouds );
			effect.Parameters["MoonTex"].SetValue( moon );
			effect.Parameters["MarsTex"].SetValue( mars );
			effect.Parameters["SkyTex"].SetValue( sky );

			effect.Parameters["LightPosition"].SetValue( LightPosition );
			effect.Parameters["CameraPosition"].SetValue( cameraPos );

			effect.Parameters["Time"].SetValue( time );

			effect.CurrentTechnique.Passes[0].Apply();
		}

		public void RenderModel( Model m, Matrix parentMatrix )
		{
			Matrix[] transforms = new Matrix[m.Bones.Count];
			m.CopyAbsoluteBoneTransformsTo( transforms );

			effect.CurrentTechnique.Passes[0].Apply();

			foreach( ModelMesh mesh in m.Meshes )
			{
				effect.Parameters["World"].SetValue( parentMatrix * transforms[mesh.ParentBone.Index] );

				mesh.Draw();
			}
		}

		public void LoadModelEffects( Model m )
		{
			foreach( ModelMesh mesh in m.Meshes )
			{
				foreach( ModelMeshPart meshPart in mesh.MeshParts )
				{
					meshPart.Effect = effect;
				}
			}
		}

		private void SetScrollOffset()
		{
			if( Mouse.GetState().ScrollWheelValue < previousScrollValue )
			{
				scrollOffset += 2;
			}
			else if( Mouse.GetState().ScrollWheelValue > previousScrollValue )
			{
				scrollOffset -= 2;
			}
			previousScrollValue = Mouse.GetState().ScrollWheelValue;
		}
	}
}