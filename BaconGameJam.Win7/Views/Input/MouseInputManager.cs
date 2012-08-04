using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BaconGameJam.Win7.Views.Input
{
    public class MouseInputManager : IInputManager
    {
        public event EventHandler<InputEventArgs> MouseDown;
        public event EventHandler<InputEventArgs> Click;
        public event EventHandler<InputEventArgs> DragStarted;
        public event EventHandler<InputEventArgs> Dragged;
        public event EventHandler<InputEventArgs> DragEnded;

        private bool isDragging;
        private bool buttonDown;
        private Point oldPosition;

        public void Update(ButtonState buttonState, Point position)
        {
            switch (buttonState)
            {
                case ButtonState.Released:
                    this.HandleMouseUp(position);
                    break;
                case ButtonState.Pressed:
                    this.HandleMouseDown(position);
                    break;
            }

            this.oldPosition = position;
        }

        private void HandleMouseUp(Point position)
        {
            if (this.isDragging)
            {
                this.isDragging = false;
                this.buttonDown = false;
                if (this.DragEnded != null)
                {
                    this.DragEnded(this, new InputEventArgs(position.X, position.Y));
                }
            }
            else if (this.buttonDown)
            {
                this.buttonDown = false;
                if (this.Click != null)
                {
                    this.Click(this, new InputEventArgs(position.X, position.Y));
                }
            }
        }

        private void HandleMouseDown(Point position)
        {
            if (this.buttonDown && this.HasMouseMoved(position))
            {
                this.HandleMouseMoveWhenMouseIsDown(position);
            }
            else if (!this.buttonDown)
            {
                this.buttonDown = true;
                if (this.MouseDown != null)
                {
                    this.MouseDown(this, new InputEventArgs(position.X, position.Y));
                }
            }
        }

        private void HandleMouseMoveWhenMouseIsDown(Point position)
        {
            if (!this.isDragging)
            {
                this.isDragging = true;
                if (this.DragStarted != null)
                {
                    this.DragStarted(this, new InputEventArgs(position.X, position.Y));
                }
            }
            else
            {
                if (this.Dragged != null)
                {
                    this.Dragged(this, new InputEventArgs(position.X, position.Y));
                }
            }
        }

        private bool HasMouseMoved(Point position)
        {
            return !position.Equals(this.oldPosition);
        }
    }
}