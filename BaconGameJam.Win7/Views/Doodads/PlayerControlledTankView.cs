using System;
using System.Collections.Generic;
using System.Linq;
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
        //private readonly List<Vector2> points;
        private Texture2D pixel;
        private readonly IKeyboardInputManager keyInput;

        public PlayerControlledTankView(PlayerControlledTank tank, IInputManager input, IKeyboardInputManager keyInput)
            : base(tank)
        {
            this.tank = tank;
            this.input = input;

            this.input.MouseDown += this.OnMouseDown;
            //this.input.DragStarted += this.OnDragStarted;
            //this.input.Dragged += this.OnDragged;
            //this.input.DragEnded += this.OnDragEnded;
            //this.points = new List<Vector2>();

            this.keyInput = keyInput;
            this.keyInput.KeyDown += this.OnKeyDown;
        }

        protected override void OnLoad(ContentManager content)
        {
            this.pixel = content.Load<Texture2D>("Images/InGame/Pixel");
        }

        /*
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.points.Count < 4)
            {
                return;
            }

            IEnumerable<Vector2> thePoints = this.points;
            Vector2 control1 = thePoints.First() + (thePoints.First() - (this.tank.Position * Constants.PixelsPerMeter)) * 15;
            Vector2 control2 = thePoints.Last() +  (thePoints.Last() - thePoints.Reverse().Skip(1).First()) * 15;

            for (float time = 0; time <= 1; time += 0.02f)
            {
                Vector2 point = this.GetPoint(
                    time,
                    thePoints.First(),
                    control1,
                    control2,
                    thePoints.Last());

                spriteBatch.Draw(
                    this.pixel,
                    point,
                    null,
                    Color.Yellow,
                    0,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    0);
            }

            //for (int i = 0; i < this.points.Count - 1; i++)
            //{
            //    Vector2 start = this.points[i];
            //    Vector2 stop = this.points[i + 1];
            //    Vector2 delta = stop - start;

            //    Rectangle rect = new Rectangle((int)start.X, (int)start.Y, (int)delta.Length(), 1);
            //    float rotation = (float)Math.Atan2(delta.Y, delta.X);
            //    spriteBatch.Draw(
            //        this.pixel,
            //        rect,
            //        null,
            //        Color.Yellow,
            //        rotation,
            //        Vector2.Zero,
            //        SpriteEffects.None,
            //        0);
            //}
        }
        */

       private Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float cx = 3 * (p1.X - p0.X);

            float cy = 3 * (p1.Y - p0.Y);


            float bx = 3 * (p2.X - p1.X) - cx;

            float by = 3 * (p2.Y - p1.Y) - cy;

            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;

            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

            return new Vector2(resX, resY);
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