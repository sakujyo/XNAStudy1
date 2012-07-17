using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Primitives3D;

namespace XNAStudy1
{
	/// <summary>
	/// 基底 Game クラスから派生した、ゲームのメイン クラスです。
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture tex1;
		private Texture2D myTexture;
		private Vector2 spritePosition = Vector2.Zero;
		private Vector2 spriteSpeed = new Vector2(40.0f, 60.0f);

		#region Fields

		//GraphicsDeviceManager graphics;

		//SpriteBatch spriteBatch;
		SpriteFont spriteFont;

		KeyboardState currentKeyboardState;
		KeyboardState lastKeyboardState;
		GamePadState currentGamePadState;
		GamePadState lastGamePadState;
		MouseState currentMouseState;
		MouseState lastMouseState;

		// プリミティブ モデルのリストと、現在選択されているプリミティブ 
		// モデルを格納します。
		List<GeometricPrimitive> primitives = new List<GeometricPrimitive>();

		int currentPrimitiveIndex = 0;

		// ワイヤフレームのラスタライズ状態を格納します。
		RasterizerState wireFrameState;

		// 着色のカラーのリストと、現在選択されている着色のカラーを格納します。
		List<Color> colors = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Black,
        };

		int currentColorIndex = 3;

		// ワイヤフレーム モードでレンダリングしているかどうか。
		bool isWireframe;


		#endregion

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			//tex1 = Content.Load<Texture>(string assetName);
			// assetNameで示されるのがコンテンツ？
			//tex1 = Content.Load<Texture>("aaa");
#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
#endif
		}

		/// <summary>
		/// ゲームの開始前に実行する必要がある初期化を実行できるようにします。
		/// ここで、要求されたサービスを問い合わせて、非グラフィック関連のコンテンツを読み込むことができます。
		/// base.Initialize を呼び出すと、任意のコンポーネントが列挙され、
		/// 初期化もされます。
		/// </summary>
		protected override void Initialize()
		{
			// TODO: ここに初期化ロジックを追加します。

			base.Initialize();
		}

		/// <summary>
		/// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
		/// 読み込みます。
		/// </summary>
		protected override void LoadContent()
		{
			// 新規の SpriteBatch を作成します。これはテクスチャーの描画に使用できます。
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。
			//tex1 = Content.Load<Texture>(string assetName);
			// assetNameで示されるのがコンテンツ？

			//myTexture = Content.Load<Texture2D>("XNAContentTestBMPPhoto1");
			myTexture = Content.Load<Texture2D>("Ane1");
			
			//tex1 = Content.Load<Texture>("XNAContentTestBMPPhoto1");

			var p1 = new CubePrimitive(GraphicsDevice);
			p1.setTexture(myTexture);

			primitives.Add(p1);
			//primitives.Add(new CubePrimitive(GraphicsDevice));

			wireFrameState = new RasterizerState()
			{
				FillMode = FillMode.WireFrame,
				CullMode = CullMode.None,
			};
		}

		/// <summary>
		/// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
		/// アンロードします。
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
		}

		/// <summary>
		/// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
		/// ゲーム ロジックを、実行します。
		/// </summary>
		/// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
		protected override void Update(GameTime gameTime)
		{
			// ゲームの終了条件をチェックします。
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: ここにゲームのアップデート ロジックを追加します。
			//Update メソッド (フレームごとに呼び出される) では、オブジェクトの移動、
			//プレイヤー入力の取得、オブジェクト間の衝突の結果の判断などのゲーム ロジックを更新します
			base.Update(gameTime);
			UpdateSprite(gameTime);

			HandleInput();
		}

		#region Update Sub Function
		private void UpdateSprite(GameTime gameTime)
		{
			spritePosition += spriteSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
			int MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
			int MinX = 0;
			int MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
			int MinY = 0;
			if (spritePosition.X > MaxX)
			{
				spriteSpeed.X *= -1;
				spritePosition.X = MaxX;
			}
			if (spritePosition.X < MinX)
			{
				spriteSpeed.X *= -1;
				spritePosition.X = MinX;
			}
			if (spritePosition.Y > MaxY)
			{
				spriteSpeed.Y *= -1;
				spritePosition.Y = MaxY;
			}
			if (spritePosition.Y < MinY)
			{
				spriteSpeed.Y *= -1;
				spritePosition.Y = MinY;
			}
						
			//throw new NotImplementedException();
		}
		#endregion

		/// <summary>
		/// ゲームが自身を描画するためのメソッドです。
		/// </summary>
		/// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: ここに描画コードを追加します。
			//Draw メソッド (フレームごとに呼び出される) では、画面上の背景および
			//その他すべてのゲーム オブジェクトのレンダリングを行います。

			//devSAKUJYO
			//spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
			//spriteBatch.Draw(myTexture, spritePosition, Color.White);
			//spriteBatch.End();
			////devSAKUJYO

			if (isWireframe)
			{
				GraphicsDevice.RasterizerState = wireFrameState;
			}
			else
			{
				GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
			}

			// カメラの行列を作成し、オブジェクトを回転させます。
			float time = (float)gameTime.TotalGameTime.TotalSeconds;

			float yaw = time * 0.4f;
			float pitch = time * 0.7f;
			float roll = time * 1.1f;

			Vector3 cameraPosition = new Vector3(0, 0, 2.5f);

			float aspect = GraphicsDevice.Viewport.AspectRatio;

			Matrix world = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
			Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

			// 現在のプリミティブを描画します。
			GeometricPrimitive currentPrimitive = primitives[currentPrimitiveIndex];
			Color color = colors[currentColorIndex];
			
			currentPrimitive.Draw(world, view, projection, color);

			// 塗りつぶしモードのレンダリングステートをリセットします。
			GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

			// オーバーレイ テキストを描画します。
			string text = "A or tap top of screen = Change primitive\n" +
										"B or tap bottom left of screen = Change color\n" +
										"Y or tap bottom right of screen = Toggle wireframe";

			spriteBatch.Begin();
			//TODO: テキストの描画？フォントをコンテンツとして読み込む必要がある？
			//spriteBatch.DrawString(spriteFont, text, new Vector2(48, 48), Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
			//base.Draw(gameTime);
		}

		#region Handle Input


		/// <summary>
		/// 設定を終了または変更する入力値を扱います。
		/// </summary>
		void HandleInput()
		{
			lastKeyboardState = currentKeyboardState;
			lastGamePadState = currentGamePadState;
			lastMouseState = currentMouseState;

#if WINDOWS_PHONE
            currentKeyboardState = new KeyboardState();
#else
			currentKeyboardState = Keyboard.GetState();
#endif
			currentGamePadState = GamePad.GetState(PlayerIndex.One);
			currentMouseState = Mouse.GetState();

			// 終了をチェックします。
			if (IsPressed(Keys.Escape, Buttons.Back))
			{
				Exit();
			}

			// プリミティブを変更するかどうか。
			Viewport viewport = GraphicsDevice.Viewport;
			int halfWidth = viewport.Width / 2;
			int halfHeight = viewport.Height / 2;
			Rectangle topOfScreen = new Rectangle(0, 0, viewport.Width, halfHeight);
			if (IsPressed(Keys.A, Buttons.A) || LeftMouseIsPressed(topOfScreen))
			{
				currentPrimitiveIndex = (currentPrimitiveIndex + 1) % primitives.Count;
			}


			// カラーを変更するかどうか。
			Rectangle botLeftOfScreen = new Rectangle(0, halfHeight, halfWidth, halfHeight);
			if (IsPressed(Keys.B, Buttons.B) || LeftMouseIsPressed(botLeftOfScreen))
			{
				currentColorIndex = (currentColorIndex + 1) % colors.Count;
			}


			// ワイヤフレームを切り替えるかどうか。
			Rectangle botRightOfScreen = new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight);
			if (IsPressed(Keys.Y, Buttons.Y) || LeftMouseIsPressed(botRightOfScreen))
			{
				isWireframe = !isWireframe;
			}
		}


		/// <summary>
		/// 指定されたキーまたはボタンが押されたかどうかを確認します。
		/// </summary>
		bool IsPressed(Keys key, Buttons button)
		{
			return (currentKeyboardState.IsKeyDown(key) &&
							lastKeyboardState.IsKeyUp(key)) ||
						 (currentGamePadState.IsButtonDown(button) &&
							lastGamePadState.IsButtonUp(button));
		}

		bool LeftMouseIsPressed(Rectangle rect)
		{
			return (currentMouseState.LeftButton == ButtonState.Pressed &&
							lastMouseState.LeftButton != ButtonState.Pressed &&
							rect.Contains(currentMouseState.X, currentMouseState.Y));
		}

		#endregion
	}
}
