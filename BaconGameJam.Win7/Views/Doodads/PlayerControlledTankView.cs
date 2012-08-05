using BaconGameJam.Common;
using BaconGameJam.Common.Models.Doodads;
using BaconGameJam.Win7.Views.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BaconGameJam.Win7.Views.Doodads
{
    public class PlayerControlledTankView : TankView
    {
        private readonly PlayerControlledTank tank;
        private readonly IInputManager input;
        private Texture2D pixel;
        private readonly IKeyboardInputManager keyInput;

        public PlayerControlledTankView(
            PlayerControlledTank tank, 
            IInputManager input, 
            IKeyboardInputManager keyInput)
            : base(tank)
        {
            this.tank = tank;
            this.input = input;

            this.input.MouseDown += this.OnMouseDown;
            this.keyInput = keyInput;
            this.keyInput.KeyDown += this.OnKeyDown;
        }

        public override void Dispose()
        {
            this.input.MouseDown -= this.OnMouseDown;
            this.keyInput.KeyDown -= this.OnKeyDown;
        }

        protected override void OnLoad(ContentManager content)
        {
            this.pixel = content.Load<Texture2D>("Images/InGame/Pixel");
        }

        private void OnMouseDown(object sender, InputEventArgs e)
        {
            Vector2 physicalPosition = new Vector2(e.X, e.Y) / Constants.PixelsPerMeter;
            if (this.tank.FireMissileCommand.CanExecute(physicalPosition))
            {
                this.tank.FireMissileCommand.Execute(physicalPosition);
            }
        }

        /*
        private void OnDragStarted(object sender, InputEventArgs e)
        {
            this.points.Add(new Vector2(e.X, e.Y));
        }

        private void OnDragged(object sender, InputEventArgs e)
        {
            this.points.Add(new Vector2(e.X, e.Y));
        }

        private void OnDragEnded(object sender, InputEventArgs e)
        {
            this.points.Clear();
            Vector2 physicalPosition = new Vector2(e.X, e.Y) / Constants.PixelsPerMeter;
            //this.tank.MoveCommand.Execute(physicalPosition);
        }*/

        private void OnKeyDown(object send, KeyboardEventArgs e)
        {
            this.tank.MovingUp = e.IsUp;
            this.tank.MovingDown = e.IsDown;
            this.tank.MovingLeft = e.IsLeft;
            this.tank.MovingRight = e.IsRight;
        }
    }
}