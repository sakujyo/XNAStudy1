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
    /// 単純なジオメトリ プリミティブ モデルの基底クラス。これは、頂点バッファー、
    /// インデックス バッファー、およびモデルを描画するためのメソッドを
    /// 提供します。特定のタイプのプリミティブのクラス (CubePrimitive、
    /// SpherePrimitive など) は、この共通の基底クラスから派生し、
    /// AddVertex メソッドおよび AddIndex メソッドを使用して、そのジオメトリを
    /// 指定します。
    /// </summary>
    public abstract class GeometricPrimitive : IDisposable
    {
        #region Fields


        // プリミティブ モデルの構築のプロセス中に、頂点およびインデックスの
        // データは、CPU 上でこれらの管理対象のリスト内に格納されます。
				//List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
				List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
				List<ushort> indices = new List<ushort>();


        // すべてのジオメトリが指定されると、InitializePrimitive メソッドが、
        // 頂点およびインデックスのデータをこれらのバッファーに
        // コピーし、バッファーが、これらのデータを、効率的なレンダリングの
        // ために準備が整えられた GPU に格納します。
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect basicEffect;
				private Texture2D texture;
				private VertexDeclaration vertexDeclaration = null;


        #endregion

        #region Initialization


        /// <summary>
        /// 新しい頂点をプリミティブ モデルに追加します。これは、
        /// 初期化プロセス中に、InitializePrimitive の前に
        /// 呼び出す必要があります。
        /// </summary>
        protected void AddVertex(Vector3 position, Vector3 normal, Vector2 vertexCoord)
        {
						//vertices.Add(new VertexPositionNormal(position, normal));
					
					var v = new VertexPositionNormalTexture(position, normal, vertexCoord);
				
						vertices.Add(v);
						//vertices.Add(new VertexPositionTexture(position, Vector2.Zero));
        }


        /// <summary>
        /// 新しいインデックスをプリミティブ モデルに追加します。これは、
        /// 初期化プロセス中に、InitializePrimitive の前に
        /// 呼び出す必要があります。
        /// </summary>
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");

            indices.Add((ushort)index);
        }


        /// <summary>
        /// 現在の頂点のインデックスをクエリします。これは、0 から
        /// 開始し、AddVertex が呼び出されるたびにインクリメントします。
        /// </summary>
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }


        /// <summary>
        /// AddVertex および AddIndex の呼び出しによって、すべてのジオメトリが
        /// 指定されると、このメソッドは、頂点およびインデックスのデータを、
        /// 効率的なレンダリングのために準備が
        /// 整えられた GPU フォーマット バッファーにコピーします。
        protected void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
            // 頂点データのフォーマットを示す頂点宣言を作成します。

            // 頂点バッファーを作成し、それに頂点データをコピーします。
            vertexBuffer = new VertexBuffer(graphicsDevice,
																						//typeof(VertexPositionNormal),
																						typeof(VertexPositionNormalTexture),
                                            vertices.Count, BufferUsage.None);

            vertexBuffer.SetData(vertices.ToArray());

            // インデックス バッファーを作成し、それにインデックス データを
            // コピーします。
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
                                          indices.Count, BufferUsage.None);

            indexBuffer.SetData(indices.ToArray());

            // プリミティブのレンダリングに使用される BasicEffect を作成します。
            basicEffect = new BasicEffect(graphicsDevice);

            basicEffect.EnableDefaultLighting();

						SamplerState ss = new SamplerState();
						ss.AddressU = TextureAddressMode.Clamp;
						ss.AddressV = TextureAddressMode.Clamp;
						graphicsDevice.SamplerStates[0] = ss;
        }


        /// <summary>
        /// ファイナライザー。
        /// </summary>
        ~GeometricPrimitive()
        {
            Dispose(false);
        }


        /// <summary>
        /// このオブジェクトによって使用されたリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// このオブジェクトによって使用されたリソースを解放します。
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
        /// 指定されたエフェクトを使用して、プリミティブ モデルを描画します。
        /// ワールド/ビュー/射影行列と色を指定するだけの他の
        ///  Draw のオーバーロードとは異なり、このメソッドは
        /// レンダリングステートを設定しないため、呼び出す前に、
        /// すべてのステートが適切な値に設定されていることを
        /// 確認する必要があります。
        /// </summary>
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            // 頂点宣言、頂点バッファー、およびインデックス バッファーを
            // 設定します。
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
        /// 既定のライティングで、BasicEffect シェーダーを使用して
        /// プリミティブ モデルを描画します。
        /// カスタム エフェクトを指定するもう 1 つの Draw の
        /// オーバーロードとは異なり、このメソッドは、重要な
        /// レンダリングステートを 3D モデルのレンダリングに
        /// 適切な値に設定するため、呼び出す前に
        /// これらのステートを設定する必要はありません。
        /// </summary>
        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            // BasicEffect のパラメーターを設定します。
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
                // アルファ ブレンディングを使用したレンダリングの
                // レンダリングステートを設定します。
                device.BlendState = BlendState.AlphaBlend;
            }
            else
            {
                // 不透明なレンダリングのレンダリングステートを設定します。
                device.BlendState = BlendState.Opaque;
            }

            // BasicEffect を使用して、モデルを描画します。
            Draw(basicEffect);
        }

        #endregion
				public void setTexture(Texture2D t)
				{
					texture = t;
				}
    }
}
