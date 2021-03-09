using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace FirstGame_DirectX
{
	class Lesson3 : Lesson
	{
		private Effect effect;
		Vector3 LightPosition = Vector3.Right * 2 + Vector3.Up * 2 + Vector3.Backward * 2;

		Model sphere;
		Texture2D day, night, clouds;

		public override void LoadContent( ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			effect = Content.Load<Effect>( "Lesson3" );

			day = Content.Load<Texture2D>( "day" );
			night = Content.Load<Texture2D>( "night" );
			clouds = Content.Load<Texture2D>( "clouds" );

			sphere = Content.Load<Model>( "uv_sphere" );
			LoadModelEffects( sphere );
		}

		public override void Draw( GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch )
		{
			GraphicsDevice device = graphics.GraphicsDevice;

			float time = ( float )gameTime.TotalGameTime.TotalSeconds;
			LightPosition = new Vector3( MathF.Cos( time ), 0, MathF.Sin( time ) ) * 200;

			Vector3 cameraPos = -Vector3.Forward * 10 + Vector3.Up * 5 + Vector3.Right * 5;

			Matrix World = Matrix.CreateWorld( Vector3.Zero, Vector3.Forward, Vector3.Up );
			Matrix View = Matrix.CreateLookAt( cameraPos, Vector3.Zero, Vector3.Up );

			effect.Parameters["World"].SetValue( World );
			effect.Parameters["View"].SetValue( View );
			effect.Parameters["Projection"].SetValue( Matrix.CreatePerspectiveFieldOfView( ( MathF.PI / 180f ) * 25f, device.Viewport.AspectRatio, 0.001f, 1000f ) );

			effect.Parameters["DayTex"].SetValue( day );
			effect.Parameters["NightTex"].SetValue( night );
			effect.Parameters["CloudsTex"].SetValue( clouds );

			effect.Parameters["LightPosition"].SetValue( LightPosition );
			effect.Parameters["CameraPosition"].SetValue( cameraPos );

			effect.CurrentTechnique.Passes[0].Apply();

			device.Clear( Color.Black );
			RenderModel( sphere, World * Matrix.CreateScale( 0.01f ) );
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
	}
}