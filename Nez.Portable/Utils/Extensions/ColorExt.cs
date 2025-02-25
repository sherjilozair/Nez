﻿using System;
using Microsoft.Xna.Framework;


namespace Nez
{
	public static class ColorExt
	{
		private const string HEX = "0123456789ABCDEF";


		static public byte HexToByte(char c)
		{
			return (byte) HEX.IndexOf(char.ToUpper(c));
		}


		static public Color Invert(this Color color)
		{
			return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
		}


		static public Color HexToColor(string hex)
		{
			float r = (HexToByte(hex[0]) * 16 + HexToByte(hex[1])) / 255.0f;
			float g = (HexToByte(hex[2]) * 16 + HexToByte(hex[3])) / 255.0f;
			float b = (HexToByte(hex[4]) * 16 + HexToByte(hex[5])) / 255.0f;

			return new Color(r, g, b);
		}


		public static Color HexToColor(int hex)
		{
			var r = (byte) (hex >> 16);
			var g = (byte) (hex >> 8);
			var b = (byte) (hex >> 0);

			return new Color(r, g, b);
		}


		static public Color Grayscale(this Color color)
		{
			return new Color((int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11),
				(int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11),
				(int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11),
				color.A);
		}


		public static Color Add(this Color color, Color second)
		{
			return new Color(color.R + second.R, color.G + second.G, color.B + second.B, color.A + second.A);
		}


		/// <summary>
		/// first - second
		/// </summary>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		public static Color Subtract(this Color color, Color second)
		{
			return new Color(color.R - second.R, color.G - second.G, color.B - second.B, color.A - second.A);
		}


		public static Color Multiply(this Color self, Color second)
		{
			return new Color
			{
				R = (byte) (self.R * second.R / 255),
				G = (byte) (self.G * second.G / 255),
				B = (byte) (self.B * second.B / 255),
				A = (byte) (self.A * second.A / 255)
			};
		}


		/// <summary>
		/// linearly interpolates Color from - to
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="t">T.</param>
		public static Color Lerp(Color from, Color to, float t)
		{
			var t255 = (int) (t * 255);
			return new Color(from.R + (to.R - from.R) * t255 / 255, from.G + (to.G - from.G) * t255 / 255,
				from.B + (to.B - from.B) * t255 / 255, from.A + (to.A - from.A) * t255 / 255);
		}


		/// <summary>
		/// linearly interpolates Color from - to
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="t">T.</param>
		public static void Lerp(ref Color from, ref Color to, out Color result, float t)
		{
			result = new Color();
			var t255 = (int) (t * 255);
			result.R = (byte) (from.R + (to.R - from.R) * t255 / 255);
			result.G = (byte) (from.G + (to.G - from.G) * t255 / 255);
			result.B = (byte) (from.B + (to.B - from.B) * t255 / 255);
			result.A = (byte) (from.A + (to.A - from.A) * t255 / 255);
		}
	}
}