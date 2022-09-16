using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackSparrus.State
{
    public class GetHintState: AState
    {
        public override void RunState(MainWindow window)
        {
            base.RunState(window);

            Bitmap screenBitmap = WindowManager.CreateScreenBitmap();
            window.TreasureHub.UpdateHubFrom(screenBitmap);

            screenBitmap.Dispose();

            window.UpdateHubArray();

            TreasureRow currentRow = window.TreasureHub.GetCurrentTreasureRow();
            if (currentRow != null)
            {
                if (currentRow.RowText.ToLower().Contains("phorreur"))
                {
                    this.NextStateId = "goPhorreur";
                }
                else
                {
                    this.NextStateId = "goHint";
                }

            }
            else
            {

                window.TreasureHub.ValidateStep();

                //this.NextStateId = "goHint";
            }
        }
    }
}
