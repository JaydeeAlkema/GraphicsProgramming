using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstGame_DirectX
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		VertexPositionColor[] vertices = {
			//+X
			new VertexPositionColor( new Vector3( -1, 1, 1 ), Color.Red ),
			new VertexPositionColor( new Vector3( 1, -1, 1 ), Color.Green ),
			new VertexPositionColor( new Vector3( -1, -1, 1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( 1, 1, 1 ), Color.Yellow ),
			
			//-X
			new VertexPositionColor( new Vector3( -1, 1, -1 ), Color.Red ),
			new VertexPositionColor( new Vector3( 1, -1, -1 ), Color.Green ),
			new VertexPositionColor( new Vector3( -1, -1, -1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( 1, 1, -1 ), Color.Yellow ),

			//+Z
			new VertexPositionColor( new Vector3( 1, -1, 1 ), Color.Green ),
			new VertexPositionColor( new Vector3( 1, 1, -1 ), Color.Yellow ),
			new VertexPositionColor( new Vector3( 1, -1, -1 ), Color.Green ),
			new VertexPositionColor( new Vector3( 1, 1, 1 ), Color.Yellow ),

			//-Z
			new VertexPositionColor( new Vector3( -1, -1, 1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( -1, 1, -1 ), Color.Red ),
			new VertexPositionColor( new Vector3( -1, -1, -1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( -1, 1, 1 ), Color.Red ),

			//+Y
			new VertexPositionColor( new Vector3( -1, -1, 1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( 1, -1, -1 ), Color.Green ),
			new VertexPositionColor( new Vector3( -1, -1, -1 ), Color.Blue ),
			new VertexPositionColor( new Vector3( 1, -1, 1 ), Color.Green ),

			//-Y
			new VertexPositionColor( new Vector3( -1, 1, -1 ), Color.Red ),
			new VertexPositionColor( new Vector3( 1, 1, 1 ), Color.Yellow ),
			new VertexPositionColor( new Vector3( -1, 1, 1 ), Color.Red ),
			new VertexPositionColor( new Vector3( 1, 1, -1 ), Color.Yellow ),
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

		BasicEffect effect;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);

			_graphics.PreferredBackBufferWidth = 500;
			_graphics.PreferredBackBufferHeight = 500;

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

			effect = new BasicEffect(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			effect.World =
			Matrix.Identity * Matrix.CreateRotationY(-(float)gameTime.TotalGameTime.TotalSeconds) *
			Matrix.Identity * Matrix.CreateRotationX((float)gameTime.TotalGameTime.TotalSeconds) *
			Matrix.Identity * Matrix.CreateRotationZ(-(float)gameTime.TotalGameTime.TotalSeconds);
			effect.View = Matrix.CreateLookAt(-Vector3.Forward * 5, Vector3.Zero, Vector3.Up);
			effect.Projection = Matrix.CreatePerspectiveFieldOfView((MathHelper.Pi / 180f) * 65f, GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

			effect.VertexColorEnabled = true;
			effect.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3);

			base.Draw(gameTime);
		}
	}
}
