using System;

namespace BaconGameJam.Win7.Views.Input
{
    public interface IInputManager
    {
        /// <summary>
        /// Fired when the mouse is fired pressed.
        /// </summary>
        event EventHandler<InputEventArgs> MouseDown;

        /// <summary>
        /// Fired when the mouse is moved but not pressed.
        /// </summary>
        event EventHandler<InputEventArgs> MouseMoved;

        /// <summary>
        /// Fired when the mouse is released without moving.
        /// </summary>
        event EventHandler<InputEventArgs> Click;

        /// <summary>
        /// Fired when the mouse moves after being pressed.
        /// </summary>
        event EventHandler<InputEventArgs> DragStarted;

        /// <summary>
        /// Fired when the mouse moves a second time after being pressed.
        /// </summary>
        event EventHandler<InputEventArgs> Dragged;

        /// <summary>
        /// Fired when the mouse is released after moving.
        /// </summary>
        event EventHandler<InputEventArgs> DragEnded;
    }
}