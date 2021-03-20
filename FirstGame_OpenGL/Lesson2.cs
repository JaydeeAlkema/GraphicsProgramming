using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstGame_OpenGL
{
	class Lesson2 : Lesson
	{
		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		public struct VertexPositionColorNormal : IVertexType
		{
			public Vector3 Position;
			public Color Color;
			public Vector3 Normal;
			public Vector2 Texture;

			static readonly VertexDeclaration _vertexDeclaration = new VertexDeclaration
			(
				new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ),
				new VertexElement( 12, VertexElementFormat.Color, VertexElementUsage.Color, 0 ),
				new VertexElement( 16, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0 ),
				new VertexElement( 28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0 )
			);

			VertexDeclaration IVertexType.VertexDeclaration => _vertexDeclaration;

			public VertexPositionColorNormal( Vector3 position, Color color, Vector3 normal, Vector2 texture )
			{
				Position = position;
				Color = color;
				Normal = normal;
				Texture = texture;
			}
		}

		private VertexPositionColorNormal[] vertices = {
			//FRONT
			new VertexPositionColorNormal( new Vector3(-1f, 1f, 1f), Color.Red, Vector3.Backward, new Vector2(0,1) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, 1f), Color.Red, Vector3.Backward, new Vector2(1,0) ),
			new VertexPositionColorNormal( new Vector3(-1f, -1f, 1f), Color.Red, Vector3.Backward, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(1f, 1f, 1f), Color.Red, Vector3.Backward, new Vector2(1,1) ),

			//BACK
			new VertexPositionColorNormal( new Vector3(-1f, 1f, -1f), Color.Green, Vector3.Forward, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, -1f), Color.Green, Vector3.Forward, new Vector2(1,1) ),
			new VertexPositionColorNormal( new Vector3(-1f, -1f, -1f), Color.Green, Vector3.Forward, new Vector2(0,1) ),
			new VertexPositionColorNormal( new Vector3(1f, 1f, -1f), Color.Green, Vector3.Forward, new Vector2(1,0) ),

			//LEFT
			new VertexPositionColorNormal( new Vector3(-1f, 1f, -1f), Color.Blue, Vector3.Left, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(-1f, -1f, 1f), Color.Blue, Vector3.Left, new Vector2(1,1) ),
			new VertexPositionColorNormal( new Vector3(-1f, -1f, -1f), Color.Blue, Vector3.Left, new Vector2(1,0) ),
			new VertexPositionColorNormal( new Vector3(-1f, 1f, 1f), Color.Blue, Vector3.Left, new Vector2(0,1) ),

			//RIGHT
			new VertexPositionColorNormal( new Vector3(1f, 1f, -1f), Color.Cyan, Vector3.Right, new Vector2(1,0) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, 1f), Color.Cyan, Vector3.Right, new Vector2(0,1) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, -1f), Color.Cyan, Vector3.Right, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(1f, 1f, 1f), Color.Cyan, Vector3.Right, new Vector2(1,1) ),

			//TOP
			new VertexPositionColorNormal( new Vector3(-1f, 1f, 1f), Color.Magenta, Vector3.Up, new Vector2(1,1) ),
			new VertexPositionColorNormal( new Vector3(1f, 1f, -1f), Color.Magenta, Vector3.Up, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(-1f, 1f, -1f), Color.Magenta, Vector3.Up, new Vector2(1,0) ),
			new VertexPositionColorNormal( new Vector3(1f, 1f, 1f), Color.Magenta, Vector3.Up, new Vector2(0,1) ),

			//BOTTOM
			new VertexPositionColorNormal( new Vector3(-1f, -1f, 1f), Color.Yellow, Vector3.Down, new Vector2(0,1) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, -1f), Color.Yellow, Vector3.Down, new Vector2(1,0) ),
			new VertexPositionColorNormal( new Vector3(-1f, -1f, -1f), Color.Yellow, Vector3.Down, new Vector2(0,0) ),
			new VertexPositionColorNormal( new Vector3(1f, -1f, 1f), Color.Yellow, Vector3.Down, new Vector2(1,1) ),
		};

		private int[] indices = {
			//FRONT
			//triangle 1
			0, 1, 2,
			//triangle 2
			0, 3, 1,
			
			//BACK
			//triangle 1
			4, 6, 5,
			//triangle 2
			4, 5, 7,
			
			//LEFT
			//triangle 1
			8, 9, 10,
			//triangle 2
			8, 11, 9,

			//RIGHT
			//triangle 1
			12, 14, 13,
			//triangle 2
			12, 13, 15,

			//TOP
			//triangle 1
			16, 18, 17,
			//triangle 2
			16, 17, 19,

			//BOTTOM
			//triangle 1
			20, 21, 22,
			//triangle 2
			20, 23, 21
		};

		private Effect myEffect;

		private List<Texture2D> textures = new List<Texture2D>();
		private List<Texture2D> texturesNormals = new List<Texture2D>();

		private SpriteFont arial;

		private int textureIndex = 0;
		private float normalsStrength = 1;

		Vector3 LightPosition = Vector3.Right * 2 + Vector3.Up * 2 + Vector3.Backward * 2;

		public override void LoadContent( ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			myEffect = Content.Load<Effect>( "Shaders/Lesson2" );

			LoadTextures( Content );

			// Load Font
			arial = Content.Load<SpriteFont>( "Fonts/Default" );
		}

		public override void Update( GameTime gameTime )
		{
			ChangeTextureIndexOnInput();
			ChangeNormalsStrengthOnInput();
		}

		public override void Draw( GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			GraphicsDevice device = graphics.GraphicsDevice;

			float time = ( float )gameTime.TotalGameTime.TotalSeconds;
			LightPosition = new Vector3( MathF.Cos( time ), 1f, MathF.Sin( time ) ) * 2;

			Vector3 cameraPos = -Vector3.Forward * 10 + Vector3.Up * 5 + Vector3.Right * 5;

			Matrix World = Matrix.CreateWorld( Vector3.Zero, Vector3.Forward, Vector3.Up );
			Matrix View = Matrix.CreateLookAt( cameraPos, Vector3.Zero, Vector3.Up );

			myEffect.Parameters["World"].SetValue( World );
			myEffect.Parameters["View"].SetValue( View );
			myEffect.Parameters["Projection"].SetValue( Matrix.CreatePerspectiveFieldOfView( ( MathF.PI / 180f ) * 25f, device.Viewport.AspectRatio, 0.001f, 1000f ) );
			myEffect.Parameters["MainTex"].SetValue( textures[textureIndex] );
			myEffect.Parameters["NormalTex"].SetValue( texturesNormals[textureIndex] );
			myEffect.Parameters["LightPosition"].SetValue( LightPosition );
			myEffect.Parameters["normalStrenth"].SetValue( normalsStrength );

			myEffect.CurrentTechnique.Passes[0].Apply();

			device.Clear( Color.Black );
			device.DrawUserIndexedPrimitives( PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3 );

			// Draw Text to Screen
			spriteBatch.Begin();
			spriteBatch.DrawString( arial, "Press 0-5 to change texture. - Current Texture Index: " + textureIndex, new Vector2( 0, 0 ), Color.White );
			spriteBatch.DrawString( arial, "Press +/- to change Normals Intensity. - Current Normals Strength: " + normalsStrength, new Vector2( 0, 16 ), Color.White );
			spriteBatch.End();
		}

		private void ChangeTextureIndexOnInput()
		{
			if( Keyboard.GetState().IsKeyDown( Keys.D1 ) ) textureIndex = 1;
			if( Keyboard.GetState().IsKeyDown( Keys.D2 ) ) textureIndex = 2;
			if( Keyboard.GetState().IsKeyDown( Keys.D3 ) ) textureIndex = 3;
			if( Keyboard.GetState().IsKeyDown( Keys.D4 ) ) textureIndex = 4;
			if( Keyboard.GetState().IsKeyDown( Keys.D5 ) ) textureIndex = 5;
			if( Keyboard.GetState().IsKeyDown( Keys.D0 ) ) textureIndex = 0;
		}

		private void LoadTextures( ContentManager Content )
		{
			// Texture 0
			textures.Add( Content.Load<Texture2D>( "Textures/Rock034_2K_Color" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Rock034_2K_Normal" ) );

			// Texture 1
			textures.Add( Content.Load<Texture2D>( "Textures/Bricks051_1K_Color" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Bricks051_1K_Normal" ) );

			// Texture 2
			textures.Add( Content.Load<Texture2D>( "Textures/Ground037_2K_Color" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Ground037_2K_Normal" ) );

			// Texture 3
			textures.Add( Content.Load<Texture2D>( "Textures/Ground039_2K_Color" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Ground039_2K_Normal" ) );

			// Texture 4
			textures.Add( Content.Load<Texture2D>( "Textures/Snow006_2K_Color" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Snow006_2K_Normal" ) );

			// Texture 5
			textures.Add( Content.Load<Texture2D>( "Textures/Water_001_COLOR" ) );
			texturesNormals.Add( Content.Load<Texture2D>( "Textures/Water_001_NORM" ) );
		}
		private void ChangeNormalsStrengthOnInput()
		{
			ImprovedKeyboard.GetState();
			if( ImprovedKeyboard.IsKeyPressed( Keys.OemPlus, true ) && normalsStrength < 10 )
			{
				normalsStrength++;
			}
			else if( ImprovedKeyboard.IsKeyPressed( Keys.OemMinus, true ) && normalsStrength > 0 )
			{
				normalsStrength--;
			}
		}
	}
}
