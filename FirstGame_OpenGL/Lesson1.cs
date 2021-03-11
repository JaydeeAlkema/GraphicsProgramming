using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FirstGame_OpenGL
{
	class Lesson1 : Lesson
	{
		BasicEffect effect;

		private List<Cube> cubes = new List<Cube>();

		Random rand = new Random();

		public override void Initialize()
		{
			base.Initialize();

			Cube cube1 = new Cube
			{
				Position = new Vector3(1, 1, 1),
				ColorVert1 = Color.Red,
				ColorVert2 = Color.Green,
				ColorVert3 = Color.Blue,
				ColorVert4 = Color.Yellow,
				ColorMode = CubeColorMode.Preset
			};

			cubes.Add(cube1);
		}

		public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
		{
			base.LoadContent(Content, graphics, spriteBatch);

			effect = new BasicEffect(graphics.GraphicsDevice);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			ChangeCubeColorModeOnPlayerInput();
		}

		public override void Draw(GameTime gameTime, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, graphics, spriteBatch);

			GraphicsDevice device = graphics.GraphicsDevice;
			device.Clear(Color.Black);

			effect.World =
			Matrix.Identity * Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds) *
			Matrix.Identity * Matrix.CreateRotationX((float)gameTime.TotalGameTime.TotalSeconds) *
			Matrix.Identity * Matrix.CreateRotationZ((float)gameTime.TotalGameTime.TotalSeconds);
			effect.View = Matrix.CreateLookAt(-Vector3.Forward * 10, Vector3.Zero, Vector3.Up);
			effect.Projection = Matrix.CreatePerspectiveFieldOfView((MathF.PI / 180f) * 65f, device.Viewport.AspectRatio, 0.1f, 100f);

			effect.VertexColorEnabled = true;
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				foreach(Cube cube in cubes)
				{
					cube.Draw(device, gameTime);
				}
			}
		}

		private void ChangeCubeColorModeOnPlayerInput()
		{
			if(Keyboard.GetState().IsKeyDown(Keys.D1)) foreach(Cube cube in cubes) cube.ColorMode = CubeColorMode.Preset;
			if(Keyboard.GetState().IsKeyDown(Keys.D2)) foreach(Cube cube in cubes) cube.ColorMode = CubeColorMode.Disco;
			if(Keyboard.GetState().IsKeyDown(Keys.D3)) foreach(Cube cube in cubes) cube.ColorMode = CubeColorMode.Headache;
			if(Keyboard.GetState().IsKeyDown(Keys.D4)) foreach(Cube cube in cubes) cube.ColorMode = CubeColorMode.ColorIncrease;
		}

	}
}
