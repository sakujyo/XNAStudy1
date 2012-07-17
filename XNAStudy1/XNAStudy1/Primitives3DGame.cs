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
    /// ���̃T���v���́A�����́A���A�~���Ȃǂ� 3D �W�I���g�� �v���~�e�B�u��
    /// �`�悷����@�������܂��B
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

        // �v���~�e�B�u ���f���̃��X�g�ƁA���ݑI������Ă���v���~�e�B�u 
        // ���f�����i�[���܂��B
        List<GeometricPrimitive> primitives = new List<GeometricPrimitive>();

        int currentPrimitiveIndex = 0;

        // ���C���t���[���̃��X�^���C�Y��Ԃ��i�[���܂��B
        RasterizerState wireFrameState;

        // ���F�̃J���[�̃��X�g�ƁA���ݑI������Ă��钅�F�̃J���[���i�[���܂��B
        List<Color> colors = new List<Color>
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.White,
            Color.Black,
        };

        int currentColorIndex = 0;

        // ���C���t���[�� ���[�h�Ń����_�����O���Ă��邩�ǂ����B
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
        /// �O���t�B�b�N �R���e���c��ǂݍ��݂܂��B
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
        /// ���W�b�N���A�Q�[���Ŏ��s�ł���悤�ɂ��܂��B
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();

            base.Update(gameTime);
        }


        /// <summary>
        /// ����́A�Q�[�����̂��`�悷��Ƃ��ɌĂяo����܂��B
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

            // �J�����̍s����쐬���A�I�u�W�F�N�g����]�����܂��B
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            float yaw = time * 0.4f;
            float pitch = time * 0.7f;
            float roll = time * 1.1f;

            Vector3 cameraPosition = new Vector3(0, 0, 2.5f);

            float aspect = GraphicsDevice.Viewport.AspectRatio;

            Matrix world = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

            // ���݂̃v���~�e�B�u��`�悵�܂��B
            GeometricPrimitive currentPrimitive = primitives[currentPrimitiveIndex];
            Color color = colors[currentColorIndex];

            currentPrimitive.Draw(world, view, projection, color);

            // �h��Ԃ����[�h�̃����_�����O�X�e�[�g�����Z�b�g���܂��B
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            // �I�[�o�[���C �e�L�X�g��`�悵�܂��B
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
        /// �ݒ���I���܂��͕ύX������͒l�������܂��B
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

            // �I�����`�F�b�N���܂��B
            if (IsPressed(Keys.Escape, Buttons.Back))
            {
                Exit();
            }

            // �v���~�e�B�u��ύX���邩�ǂ����B
            Viewport viewport = GraphicsDevice.Viewport;
            int halfWidth = viewport.Width / 2;
            int halfHeight = viewport.Height / 2;
            Rectangle topOfScreen = new Rectangle(0, 0, viewport.Width, halfHeight);
            if (IsPressed(Keys.A, Buttons.A) || LeftMouseIsPressed(topOfScreen))
            {
                currentPrimitiveIndex = (currentPrimitiveIndex + 1) % primitives.Count;
            }


            // �J���[��ύX���邩�ǂ����B
            Rectangle botLeftOfScreen = new Rectangle(0, halfHeight, halfWidth, halfHeight);
            if (IsPressed(Keys.B, Buttons.B) || LeftMouseIsPressed(botLeftOfScreen))
            {
                currentColorIndex = (currentColorIndex + 1) % colors.Count;
            }


            // ���C���t���[����؂�ւ��邩�ǂ����B
            Rectangle botRightOfScreen = new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight);
            if (IsPressed(Keys.Y, Buttons.Y) || LeftMouseIsPressed(botRightOfScreen))
            {
                isWireframe = !isWireframe;
            }
        }


        /// <summary>
        /// �w�肳�ꂽ�L�[�܂��̓{�^���������ꂽ���ǂ������m�F���܂��B
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
    /// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�B
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
