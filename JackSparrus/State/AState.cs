using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackSparrus.State
{
    public abstract class AState : IState
    { 
        public string NextStateId
        {
            get;
            protected set;
        }

        public virtual void RunState(MainWindow window)
        {
            this.NextStateId = string.Empty;
        }

        protected void ClickOnNextArea(Direction direction)
        {
            Size screenSize = WindowManager.GetScreenSize();

            Random rand = new Random();
            Point pointToClick = new Point(rand.Next(0, 5), rand.Next(0, 5));
            switch (direction)
            {
                case Direction.UP:
                    pointToClick.X += screenSize.Width / 2 + 50;
                    pointToClick.Y = 0;
                    break;
                case Direction.RIGHT:
                    pointToClick.X += screenSize.Width - 135;
                    pointToClick.Y += screenSize.Height / 2;
                    break;
                case Direction.DOWN:
                    pointToClick.X += screenSize.Width / 2 - 50;
                    pointToClick.Y += screenSize.Height - 130;
                    break;
                case Direction.LEFT:
                    pointToClick.X += 135;
                    pointToClick.Y += screenSize.Height / 2;
                    break;
            }

            WindowManager.ClickOn(pointToClick.X, pointToClick.Y);

            Thread.Sleep(50 + rand.Next(0, 20));
            WindowManager.MoveMouseTo(0, 0);
        }
    }
}
