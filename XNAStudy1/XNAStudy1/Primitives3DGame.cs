#region File Description
//-----------------------------------------------------------------------------
// Primitives3DGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace Primitives3D
{
    /// <summary>
    /// このサンプルは、立方体、球、円柱などの 3D ジオメトリ プリミティブを
    /// 描画する方法を示します。
    /// </summary>
    public class Primitives3DGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
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

        int currentColorIndex = 0;

        // ワイヤフレーム モードでレンダリングしているかどうか。
        bool isWireframe;


        #endregion

        #region Initialization


        public Primitives3DGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
#endif
        }


        /// <summary>
        /// グラフィック コンテンツを読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("hudfont");

            primitives.Add(new CubePrimitive(GraphicsDevice));
						//primitives.Add(new SpherePrimitive(GraphicsDevice));
						//primitives.Add(new CylinderPrimitive(GraphicsDevice));
						//primitives.Add(new TorusPrimitive(GraphicsDevice));
						//primitives.Add(new TeapotPrimitive(GraphicsDevice));

            wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// ロジックを、ゲームで実行できるようにします。
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            base.Update(gameTime);
        }


        /// <summary>
        /// これは、ゲーム自体が描画するときに呼び出されます。
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
            spriteBatch.DrawString(spriteFont, text, new Vector2(48, 48), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        #endregion

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


    #region Entry Point

    /// <summary>
    /// アプリケーションのメイン エントリ ポイント。
    /// </summary>
		//static class Program
		//{
		//    static void Main()
		//    {
		//        using (Primitives3DGame game = new Primitives3DGame())
		//        {
		//            game.Run();
		//        }
		//    }
		//}

    #endregion
}
