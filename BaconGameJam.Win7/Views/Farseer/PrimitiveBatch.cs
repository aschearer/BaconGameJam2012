﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Farseer
{
    public class PrimitiveBatch : IDisposable
    {
        private const int DefaultBufferSize = 500;

        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        private BasicEffect _basicEffect;

        // the device that we will issue draw calls to.
        private GraphicsDevice _device;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        private bool _hasBegun;

        private bool _isDisposed;
        private VertexPositionColor[] _lineVertices;
        private int _lineVertsCount;
        private VertexPositionColor[] _triangleVertices;
        private int _triangleVertsCount;


        /// <summary>
        /// the constructor creates a new PrimitiveBatch and sets up all of the internals
        /// that PrimitiveBatch will need.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public PrimitiveBatch(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, DefaultBufferSize)
        {
        }

        public PrimitiveBatch(GraphicsDevice graphicsDevice, int bufferSize)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            this._device = graphicsDevice;

            this._triangleVertices = new VertexPositionColor[bufferSize - bufferSize % 3];
            this._lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];

            // set up a new basic effect, and enable vertex colors.
            this._basicEffect = new BasicEffect(graphicsDevice);
            this._basicEffect.VertexColorEnabled = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public void SetProjection(ref Matrix projection)
        {
            this._basicEffect.Projection = projection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._isDisposed)
            {
                if (this._basicEffect != null)
                    this._basicEffect.Dispose();

                this._isDisposed = true;
            }
        }


        /// <summary>
        /// Begin is called to tell the PrimitiveBatch what kind of primitives will be
        /// drawn, and to prepare the graphics card to render those primitives.
        /// </summary>
        /// <param name="projection">The projection.</param>
        /// <param name="view">The view.</param>
        public void Begin(ref Matrix projection, ref Matrix view)
        {
            if (this._hasBegun)
            {
                throw new InvalidOperationException("End must be called before Begin can be called again.");
            }

            //tell our basic effect to begin.
            this._basicEffect.Projection = projection;
            this._basicEffect.View = view;
            this._basicEffect.CurrentTechnique.Passes[0].Apply();

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
            this._hasBegun = true;
        }

        public bool IsReady()
        {
            return this._hasBegun;
        }

        public void AddVertex(Vector2 vertex, Color color, PrimitiveType primitiveType)
        {
            if (!this._hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before AddVertex can be called.");
            }
            if (primitiveType == PrimitiveType.LineStrip ||
                primitiveType == PrimitiveType.TriangleStrip)
            {
                throw new NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.");
            }

            if (primitiveType == PrimitiveType.TriangleList)
            {
                if (this._triangleVertsCount >= this._triangleVertices.Length)
                {
                    this.FlushTriangles();
                }
                this._triangleVertices[this._triangleVertsCount].Position = new Vector3(vertex, -0.1f);
                this._triangleVertices[this._triangleVertsCount].Color = color;
                this._triangleVertsCount++;
            }
            if (primitiveType == PrimitiveType.LineList)
            {
                if (this._lineVertsCount >= this._lineVertices.Length)
                {
                    this.FlushLines();
                }
                this._lineVertices[this._lineVertsCount].Position = new Vector3(vertex, 0f);
                this._lineVertices[this._lineVertsCount].Color = color;
                this._lineVertsCount++;
            }
        }


        /// <summary>
        /// End is called once all the primitives have been drawn using AddVertex.
        /// it will call Flush to actually submit the draw call to the graphics card, and
        /// then tell the basic effect to end.
        /// </summary>
        public void End()
        {
            if (!this._hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before End can be called.");
            }

            // Draw whatever the user wanted us to draw
            this.FlushTriangles();
            this.FlushLines();

            this._hasBegun = false;
        }

        private void FlushTriangles()
        {
            if (!this._hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }
            if (this._triangleVertsCount >= 3)
            {
                int primitiveCount = this._triangleVertsCount / 3;
                // submit the draw call to the graphics card
                this._device.SamplerStates[0] = SamplerState.AnisotropicClamp;
                this._device.DrawUserPrimitives(PrimitiveType.TriangleList, this._triangleVertices, 0, primitiveCount);
                this._triangleVertsCount -= primitiveCount * 3;
            }
        }

        private void FlushLines()
        {
            if (!this._hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }
            if (this._lineVertsCount >= 2)
            {
                int primitiveCount = this._lineVertsCount / 2;
                // submit the draw call to the graphics card
                this._device.SamplerStates[0] = SamplerState.AnisotropicClamp;
                this._device.DrawUserPrimitives(PrimitiveType.LineList, this._lineVertices, 0, primitiveCount);
                this._lineVertsCount -= primitiveCount * 2;
            }
        }
    }
}