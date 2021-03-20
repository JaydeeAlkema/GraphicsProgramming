using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstGame_OpenGL
{
	public static class ImprovedKeyboard
	{
		static KeyboardState currentKeyState;
		static KeyboardState previousKeyState;

		public static KeyboardState GetState()
		{
			previousKeyState = currentKeyState;
			currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
			return currentKeyState;
		}
		public static bool IsKeyPressed( Keys key, bool oneShot )
		{
			if( !oneShot ) return currentKeyState.IsKeyDown( key );
			return currentKeyState.IsKeyDown( key ) && !previousKeyState.IsKeyDown( key );
		}
	}
}
