using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
namespace FirstGame_OpenGL
{
	public enum CubeColorMode
	{
		Preset = 0,
		Disco = 1,
		Headache = 2,
		ColorIncrease = 3
	}

	class Cube
	{
		private CubeColorMode colorMode;
		private Color colorVert1;
		private Color colorVert2;
		private Color colorVert3;
		private Color colorVert4;

		public VertexPositionColor[] Vertices { get; private set; }
		public int[] Indices { get; private set; }
		public int Primitives { get { return Indices.Length / 3; } private set { } }

		public Vector3 Position { get; set; }
		public Color ColorVert1 { get => colorVert1; set => colorVert1 = value; }
		public Color ColorVert2 { get => colorVert2; set => colorVert2 = value; }
		public Color ColorVert3 { get => colorVert3; set => colorVert3 = value; }
		public Color ColorVert4 { get => colorVert4; set => colorVert4 = value; }
		public CubeColorMode ColorMode { get => colorMode; set => colorMode = value; }

		public void Draw(GraphicsDevice device, GameTime gameTime)
		{
			switch(colorMode)
			{
				case CubeColorMode.Preset:
					break;
				case CubeColorMode.Disco:
					DiscoMode();
					break;
				case CubeColorMode.Headache:
					HeadacheMode();
					break;
				case CubeColorMode.ColorIncrease:
					ColorIncrease(gameTime);
					break;
				default:
					break;
			}

			VertexPositionColor[] vertices = {
			//+X
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, Position.Z), colorVert1),
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, Position.Z), colorVert2),
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, Position.Z), colorVert4),
			
			//-X
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, -Position.Z), colorVert1),
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, -Position.Z), colorVert2),
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, -Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, -Position.Z), colorVert4),

			//+Z
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, Position.Z), colorVert2),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, -Position.Z), colorVert4),
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, -Position.Z), colorVert2),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, Position.Z), colorVert4),

			//-Z
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, -Position.Z), colorVert1),
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, -Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, Position.Z), colorVert1),

			//+Y
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, -Position.Z), colorVert2),
			new VertexPositionColor(new Vector3(-Position.X , -Position.Y, -Position.Z), colorVert3),
			new VertexPositionColor(new Vector3(Position.X , -Position.Y, Position.Z), colorVert2),

			//-Y
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, -Position.Z), colorVert1),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, Position.Z), colorVert4),
			new VertexPositionColor(new Vector3(-Position.X , Position.Y, Position.Z), colorVert1),
			new VertexPositionColor(new Vector3(Position.X , Position.Y, -Position.Z), colorVert4),
			};

			int[] indices = {
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

			//RIGHT
			//triangle 1
			8, 9, 10,
			//triangle 2
			8, 11, 9,
			//LEFT
			//triangle 1
			12, 14, 13,
			//triangle 2
			12, 13, 15,

			//DOWN
			//triangle 1
			16, 17, 18,
			//triangle 2
			16, 19, 17,
			//UP
			//triangle 1
			20, 21, 22,
			//triangle 2
			20, 23, 21
			};

			Vertices = vertices;
			Indices = indices;

			device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length, Indices, 0, Primitives);
		}

		public void DiscoMode()
		{
			Random rand = new Random(System.DateTime.Now.Millisecond);
			colorVert1 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert2 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert3 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert4 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
		}

		public void HeadacheMode()
		{
			Random rand = new Random(System.DateTime.Now.Millisecond);
			colorVert1 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			colorVert2 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			colorVert3 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
			colorVert4 = new Color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
		}

		//TODO:
		// Actually fix this thing. As of right now it just fades the whole cube to white...
		public void ColorIncrease(GameTime gameTime)
		{
			Random rand = new Random(System.DateTime.Now.Millisecond);
			colorVert1 = Color.Lerp(ColorVert1, new Color(rand.Next(200, 255), rand.Next(200, 255), rand.Next(200, 255)), (float)gameTime.ElapsedGameTime.TotalSeconds);
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert2 = Color.Lerp(ColorVert2, new Color(rand.Next(200, 255), rand.Next(200, 255), rand.Next(200, 255)), (float)gameTime.ElapsedGameTime.TotalSeconds);
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert3 = Color.Lerp(ColorVert3, new Color(rand.Next(200, 255), rand.Next(200, 255), rand.Next(200, 255)), (float)gameTime.ElapsedGameTime.TotalSeconds);
			rand = new Random(System.DateTime.Now.Millisecond);
			colorVert4 = Color.Lerp(ColorVert4, new Color(rand.Next(200, 255), rand.Next(200, 255), rand.Next(200, 255)), (float)gameTime.ElapsedGameTime.TotalSeconds);

			Console.WriteLine(colorVert1);
		}
	}
}
