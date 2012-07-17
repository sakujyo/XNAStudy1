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
    /// 立方体を描画するためのジオメトリ プリミティブ クラス。
    /// </summary>
    public class CubePrimitive : GeometricPrimitive
    {
        /// <summary>
        /// 既定の設定を使用して、新しい立方体プリミティブを構築します。
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1)
        {
        }


        /// <summary>
        /// 指定されたサイズで新しい立方体プリミティブを構築します。
        /// </summary>
        public CubePrimitive(GraphicsDevice graphicsDevice, float size)
        {
            // 立方体は 6 つの面を持ち、それぞれが別の方向を向いています。
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            // 各面を順に作成します。
            foreach (Vector3 normal in normals)
            {
                // 面の法線に対して垂直なベクトルと互いに対して垂直なベクトルの
                //  2 つのベクトルを取得します。
                Vector3 side1 = new Vector3(normal.Y, normal.Z, normal.X);
                Vector3 side2 = Vector3.Cross(normal, side1);

                // 面あたり 6 つのインデックス (2 つのトライアングル)。
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // 面あたり 4 つの頂点。
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
