using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JackSparrus.State
{
    public class GoHintState : AState
    {
        public override void RunState(MainWindow window)
        {
            base.RunState(window);

            TreasureRow row = window.TreasureHub.GetCurrentTreasureRow();

            Point currentPosition = window.TreasureHub.GetCurrentPosition();

            int nbCaseToMove = window.WebManager.GetHintDistance(currentPosition, row.Direction, row.RowText, out string mostAccurateHint);

            row.WebRowText = mostAccurateHint;
            row.WebHintPosition = TreasureHub.GetNewPosition(row.Direction, currentPosition, nbCaseToMove);
            window.UpdateHubArray();

            Random rand = new Random();

            for(int i = 0; i < nbCaseToMove; i++)
            {
                this.ClickOnNextArea(row.Direction);
                Thread.Sleep(5000);
            }


            window.TreasureHub.ValidateRow(row.Direction, nbCaseToMove);

            Thread.Sleep(2000);

            this.NextStateId = "getHint";
        }
    }
}
