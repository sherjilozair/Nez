﻿using System;
using Nez;
using Nez.Systems;
using Nez.Textures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Nez
{
	/// <summary>
	/// overlays a mosaic on top of the final render. Useful only for pixel perfect pixel art.
	/// </summary>
	public class PixelMosaicRenderDelegate : IFinalRenderDelegate
	{
		public Scene scene { get; set; }

		Effect effect;
		Texture2D _mosaicTexture;
		RenderTexture _mosaicRenderTex;
		int _lastMosaicScale = -1;


		public void onAddedToScene()
		{
			effect = scene.contentManager.LoadEffect( "nez/effects/MultiTextureOverlay.mgfxo" );
		}


		void createMosaicTexture( int size )
		{
			if( _mosaicTexture != null )
				_mosaicTexture.Dispose();
			
			_mosaicTexture = new Texture2D( Core.graphicsDevice, size, size );
			var colors = new uint[size * size];

			for( var i = 0; i < colors.Length; i++ )
				colors[i] = 0x808080;
			
			colors[0] = 0xffffffff;
			colors[size * size - 1] = 0xff000000;

			for( var x = 1; x < size - 1; x++ )
			{
				colors[x * size] = 0xffE0E0E0;
				colors[x * size + 1] = 0xffffffff;
				colors[x * size + size - 1] = 0xff000000;
			}

			for( var y = 1; y < size - 1; y++ )
			{
				colors[y] = 0xffffffff;
				colors[( size - 1 ) * size + y] = 0xff000000;
			}

			_mosaicTexture.SetData<uint>( colors );
			effect.Parameters["secondTexture"].SetValue( _mosaicTexture );
		}


		public void onSceneBackBufferSizeChanged( int newWidth, int newHeight )
		{
			// dont recreate the mosaic unless we really need to
			if( _lastMosaicScale != scene.pixelPerfectScale )
			{
				createMosaicTexture( scene.pixelPerfectScale );
				_lastMosaicScale = scene.pixelPerfectScale;
			}

			if( _mosaicRenderTex != null && _mosaicRenderTex.renderTarget2D.IsDisposed )
				_mosaicRenderTex.resize( newWidth * scene.pixelPerfectScale, newHeight * scene.pixelPerfectScale );
			else
				_mosaicRenderTex = new RenderTexture( newWidth * scene.pixelPerfectScale, newHeight * scene.pixelPerfectScale );

			// based on the look of games by: http://deepnight.net/games/strike-of-rage/
			// use the mosaic to render to a full sized RenderTexture repeating the mosaic
			Core.graphicsDevice.SetRenderTarget( _mosaicRenderTex );
			Graphics.instance.spriteBatch.Begin( 0, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, effect );
			Graphics.instance.spriteBatch.Draw( _mosaicTexture, Vector2.Zero, new Rectangle( 0, 0, _mosaicRenderTex.renderTarget2D.Width, _mosaicRenderTex.renderTarget2D.Height ), Color.White );
			Graphics.instance.spriteBatch.End();

			// let our Effect know about our rendered, full screen mosaic
			effect.Parameters["secondTexture"].SetValue( _mosaicRenderTex );
		}


		public void handleFinalRender( Color letterboxColor, RenderTexture source, Rectangle finalRenderDestinationRect, SamplerState samplerState )
		{
			// we can just draw directly to the screen here with our effect
			Core.graphicsDevice.SetRenderTarget( null );
			Core.graphicsDevice.Clear( letterboxColor );
			Graphics.instance.spriteBatch.Begin( 0, BlendState.Opaque, samplerState, DepthStencilState.None, RasterizerState.CullNone, effect );
			Graphics.instance.spriteBatch.Draw( source, finalRenderDestinationRect, Color.White );
			Graphics.instance.spriteBatch.End();
		}


		public void unload()
		{
			_mosaicTexture.Dispose();
			_mosaicRenderTex.unload();
		}

	}
}
