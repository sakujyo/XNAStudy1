#region File Description
//-----------------------------------------------------------------------------
// CubePrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation.All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Primitives3D
{
    /// <summary>
    /// �����̂�`�悷�邽�߂̃W�I���g�� �v���~�e�B�u �N���X�B
    /// </summary>
    public class CubePrimitive : GeometricPrimitive
    {
        /// <summary>
        /// ����̐ݒ���g�p���āA�V���������̃v���~�e�B�u���\�z���܂��B
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1)
        {
        }


        /// <summary>
        /// �w�肳�ꂽ�T�C�Y�ŐV���������̃v���~�e�B�u���\�z���܂��B
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice, float size)
        {
            // �����̂� 6 �̖ʂ������A���ꂼ�ꂪ�ʂ̕����������Ă��܂��B
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            // �e�ʂ����ɍ쐬���܂��B
            foreach (Vector3 normal in normals)
            {
                // �ʂ̖@���ɑ΂��Đ����ȃx�N�g���ƌ݂��ɑ΂��Đ����ȃx�N�g����
                //  2 �̃x�N�g�����擾���܂��B
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                // �ʂ����� 6 �̃C���f�b�N�X (2 �̃g���C�A���O��)�B
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // �ʂ����� 4 �̒��_�B
                AddVertex((normal - side1 - side2) * size / 2, normal, new Vector2(0, 0));
								AddVertex((normal - side1 + side2) * size / 2, normal, new Vector2(1, 0));
								AddVertex((normal + side1 + side2) * size / 2, normal, new Vector2(1, 1));
								AddVertex((normal + side1 - side2) * size / 2, normal, new Vector2(0, 1));
            }

            InitializePrimitive(graphicsDevice);
        }

				//internal void setTexture(Texture2D myTexture)
				//{

				//  throw new System.NotImplementedException();
				//}
		}
}
