// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace Avalonia.Rendering
{
    /// <summary>
    /// Definition of the <see cref="Renderer"/> class.
    /// </summary>
    public class Renderer : IDisposable, IRenderer
    {
        private readonly IRenderLoop _renderLoop;
        private readonly IRenderRoot _root;
        private IRenderTarget _renderTarget;
        private bool _dirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="renderLoop"></param>
        public Renderer(IRenderRoot root, IRenderLoop renderLoop)
        {
            Contract.Requires<ArgumentNullException>(root != null);

            _root = root;
            _renderLoop = renderLoop;
            _renderLoop.Tick += OnRenderLoopTick;
        }

        /// <summary>
        /// Adds a dirty visual.
        /// </summary>
        /// <param name="pVisual"></param>
        public void AddDirty(IVisual pVisual)
        {
            _dirty = true;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            _renderLoop.Tick -= OnRenderLoopTick;
        }

        /// <summary>
        /// Renders in the given region.
        /// </summary>
        /// <param name="pRegion"></param>
        public void Render(Rect pRegion)
        {
            if (_renderTarget == null)
            {
                _renderTarget = _root.CreateRenderTarget();
            }

            try
            {
                _renderTarget.Render(_root);
            }
            catch (RenderTargetCorruptedException ex)
            {
                Logging.Logger.Information("Renderer", this, "Render target was corrupted. Exception: {0}", ex);
                _renderTarget.Dispose();
                _renderTarget = null;
            }
            finally
            {
                _dirty = false;
            }
        }

        /// <summary>
        /// Delegate called on each render loop tick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRenderLoopTick(object sender, EventArgs e)
        {
            if (_dirty)
            {
                _root.Invalidate(new Rect(_root.ClientSize));
            }
        }
    }
}
