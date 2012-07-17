#region File Description
//-----------------------------------------------------------------------------
// GeometricPrimitive.cs
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
#endregion

namespace Primitives3D
{
    /// <summary>
    /// �P���ȃW�I���g�� �v���~�e�B�u ���f���̊��N���X�B����́A���_�o�b�t�@�[�A
    /// �C���f�b�N�X �o�b�t�@�[�A����у��f����`�悷�邽�߂̃��\�b�h��
    /// �񋟂��܂��B����̃^�C�v�̃v���~�e�B�u�̃N���X (CubePrimitive�A
    /// SpherePrimitive �Ȃ�) �́A���̋��ʂ̊��N���X����h�����A
    /// AddVertex ���\�b�h����� AddIndex ���\�b�h���g�p���āA���̃W�I���g����
    /// �w�肵�܂��B
    /// </summary>
    public abstract class GeometricPrimitive : IDisposable
    {
        #region Fields


        // �v���~�e�B�u ���f���̍\�z�̃v���Z�X���ɁA���_����уC���f�b�N�X��
        // �f�[�^�́ACPU ��ł����̊Ǘ��Ώۂ̃��X�g���Ɋi�[����܂��B
				//List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
				List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
				List<ushort> indices = new List<ushort>();


        // ���ׂẴW�I���g�����w�肳���ƁAInitializePrimitive ���\�b�h���A
        // ���_����уC���f�b�N�X�̃f�[�^�������̃o�b�t�@�[��
        // �R�s�[���A�o�b�t�@�[���A�����̃f�[�^���A�����I�ȃ����_�����O��
        // ���߂ɏ�����������ꂽ GPU �Ɋi�[���܂��B
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect basicEffect;
				private Texture2D texture;
				private VertexDeclaration vertexDeclaration = null;


        #endregion

        #region Initialization


        /// <summary>
        /// �V�������_���v���~�e�B�u ���f���ɒǉ����܂��B����́A
        /// �������v���Z�X���ɁAInitializePrimitive �̑O��
        /// �Ăяo���K�v������܂��B
        /// </summary>
        protected void AddVertex(Vector3 position, Vector3 normal, Vector2 vertexCoord)
        {
						//vertices.Add(new VertexPositionNormal(position, normal));
					
					var v = new VertexPositionNormalTexture(position, normal, vertexCoord);
				
						vertices.Add(v);
						//vertices.Add(new VertexPositionTexture(position, Vector2.Zero));
        }


        /// <summary>
        /// �V�����C���f�b�N�X���v���~�e�B�u ���f���ɒǉ����܂��B����́A
        /// �������v���Z�X���ɁAInitializePrimitive �̑O��
        /// �Ăяo���K�v������܂��B
        /// </summary>
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            indices.Add((ushort)index);
        }


        /// <summary>
        /// ���݂̒��_�̃C���f�b�N�X���N�G�����܂��B����́A0 ����
        /// �J�n���AAddVertex ���Ăяo����邽�тɃC���N�������g���܂��B
        /// </summary>
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }


        /// <summary>
        /// AddVertex ����� AddIndex �̌Ăяo���ɂ���āA���ׂẴW�I���g����
        /// �w�肳���ƁA���̃��\�b�h�́A���_����уC���f�b�N�X�̃f�[�^���A
        /// �����I�ȃ����_�����O�̂��߂ɏ�����
        /// ������ꂽ GPU �t�H�[�}�b�g �o�b�t�@�[�ɃR�s�[���܂��B
        protected void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
            // ���_�f�[�^�̃t�H�[�}�b�g���������_�錾���쐬���܂��B

            // ���_�o�b�t�@�[���쐬���A����ɒ��_�f�[�^���R�s�[���܂��B
            vertexBuffer = new VertexBuffer(graphicsDevice,
																						//typeof(VertexPositionNormal),
																						typeof(VertexPositionNormalTexture),
                                            vertices.Count, BufferUsage.None);

            vertexBuffer.SetData(vertices.ToArray());

            // �C���f�b�N�X �o�b�t�@�[���쐬���A����ɃC���f�b�N�X �f�[�^��
            // �R�s�[���܂��B
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                          indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());

            // �v���~�e�B�u�̃����_�����O�Ɏg�p����� BasicEffect ���쐬���܂��B
            basicEffect = new BasicEffect(graphicsDevice);

            basicEffect.EnableDefaultLighting();

						SamplerState ss = new SamplerState();
						ss.AddressU = TextureAddressMode.Clamp;
						ss.AddressV = TextureAddressMode.Clamp;
						graphicsDevice.SamplerStates[0] = ss;
        }


        /// <summary>
        /// �t�@�C�i���C�U�[�B
        /// </summary>
        ~GeometricPrimitive()
        {
            Dispose(false);
        }


        /// <summary>
        /// ���̃I�u�W�F�N�g�ɂ���Ďg�p���ꂽ���\�[�X��������܂��B
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// ���̃I�u�W�F�N�g�ɂ���Ďg�p���ꂽ���\�[�X��������܂��B
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();

                if (indexBuffer != null)
                    indexBuffer.Dispose();

                if (basicEffect != null)
                    basicEffect.Dispose();
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// �w�肳�ꂽ�G�t�F�N�g���g�p���āA�v���~�e�B�u ���f����`�悵�܂��B
        /// ���[���h/�r���[/�ˉe�s��ƐF���w�肷�邾���̑���
        ///  Draw �̃I�[�o�[���[�h�Ƃ͈قȂ�A���̃��\�b�h��
        /// �����_�����O�X�e�[�g��ݒ肵�Ȃ����߁A�Ăяo���O�ɁA
        /// ���ׂẴX�e�[�g���K�؂Ȓl�ɐݒ肳��Ă��邱�Ƃ�
        /// �m�F����K�v������܂��B
        /// </summary>
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            // ���_�錾�A���_�o�b�t�@�[�A����уC���f�b�N�X �o�b�t�@�[��
            // �ݒ肵�܂��B
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            graphicsDevice.Indices = indexBuffer;            


					//SurfaceFormat.Vector2
            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();

                int primitiveCount = indices.Count / 3;

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                                                     vertices.Count, 0, primitiveCount);

            }
        }


        /// <summary>
        /// ����̃��C�e�B���O�ŁABasicEffect �V�F�[�_�[���g�p����
        /// �v���~�e�B�u ���f����`�悵�܂��B
        /// �J�X�^�� �G�t�F�N�g���w�肷����� 1 �� Draw ��
        /// �I�[�o�[���[�h�Ƃ͈قȂ�A���̃��\�b�h�́A�d�v��
        /// �����_�����O�X�e�[�g�� 3D ���f���̃����_�����O��
        /// �K�؂Ȓl�ɐݒ肷�邽�߁A�Ăяo���O��
        /// �����̃X�e�[�g��ݒ肷��K�v�͂���܂���B
        /// </summary>
        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            // BasicEffect �̃p�����[�^�[��ݒ肵�܂��B
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.DiffuseColor = color.ToVector3();
            basicEffect.Alpha = color.A / 255.0f;
						basicEffect.TextureEnabled = true;
					

					//var o = new VertexPositionTexture(/*Vector3*/position, /*Vector2*/textureCoordinate);
						//var o = new VertexPositionTexture(Vector3.Zero, Vector2.Zero);
						var en = basicEffect.TextureEnabled;
							//devSAKUJYO
						basicEffect.Texture = texture;

            GraphicsDevice device = basicEffect.GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
					//this.vertexDeclaration = new VertexDeclaration(device, VertexPositionTexture.VertexDeclaration.e

					
					if (color.A < 255)
            {
                // �A���t�@ �u�����f�B���O���g�p���������_�����O��
                // �����_�����O�X�e�[�g��ݒ肵�܂��B
                device.BlendState = BlendState.AlphaBlend;
            }
            else
            {
                // �s�����ȃ����_�����O�̃����_�����O�X�e�[�g��ݒ肵�܂��B
                device.BlendState = BlendState.Opaque;
            }

            // BasicEffect ���g�p���āA���f����`�悵�܂��B
            Draw(basicEffect);
        }

        #endregion
				public void setTexture(Texture2D t)
				{
					texture = t;
				}
    }
}
