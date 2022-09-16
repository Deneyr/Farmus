using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JackSparrus
{
    class WindowManager
    {
        private static IntPtr DOFUSPTR; 

        public const int WM_KEYDOWN = 0x100;
        public const int WM_CHAR = 0x102;
        public const int WM_KEYUP = 0x101;
        public const int WM_COMMAND = 0x111;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int x1;
            public int x2;
            public int y1;
            public int y2;
        }

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(
        int hWnd,
        int Msg,
        int wParam,
        IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern int FindWindowEx(
        int hwndParent,
        int hwndChildAfter,
        string strClassName,
        string strWindowName);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public static void InitWindowManager()
        {
            DOFUSPTR = GetDofusPtr();
        }

        private static IntPtr GetDofusPtr()
        {
            IntPtr dofusPtr = IntPtr.Zero;
            Process process = null;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains("Dofus"))
                {
                    dofusPtr = pList.MainWindowHandle;
                    process = pList;
                }
            }

            return dofusPtr;
        }

        public static Bitmap CreateScreenBitmap()
        {
            SetForegroundWindow(DOFUSPTR);

            // Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                Screen.PrimaryScreen.Bounds.Height,
                                PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

            return bmpScreenshot;
        }

        public static Bitmap CreateScreenBitmap(RECT zone)
        {
            zone.x1 = Math.Max(0, zone.x1);
            zone.x2 = Math.Max(0, zone.x2);

            zone.y1 = Math.Min(zone.y1, Screen.PrimaryScreen.Bounds.Size.Width - 1);
            zone.y2 = Math.Min(zone.y2, Screen.PrimaryScreen.Bounds.Size.Height - 1);

            int width = zone.y1 - zone.x1 + 1;
            int height = zone.y2 - zone.x2 + 1;


            SetForegroundWindow(DOFUSPTR);

            // Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                Screen.PrimaryScreen.Bounds.Height,
                                PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(zone.x1,
                                        zone.x2,
                                        0,
                                        0,
                                        new Size(width, height),
                                            CopyPixelOperation.SourceCopy);

            return bmpScreenshot;
        }

        public static Size GetScreenSize()
        {
            return Screen.PrimaryScreen.Bounds.Size;
        }

        public static void ToggleTransparency()
        {
            IntPtr NULL = new IntPtr(0);

            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYDOWN, (int)Keys.F5, NULL);
            Thread.Sleep(100);
            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYUP, (int)Keys.F5, NULL);
            Thread.Sleep(1000);
        }

        public static void PressReleaseButton(Keys key)
        {
            IntPtr NULL = new IntPtr(0);

            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYDOWN, (int)key, NULL);
            Thread.Sleep(100);
            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYUP, (int)key, NULL);
        }

        public static void PressTransparentEntitiesButton()
        {
            IntPtr NULL = new IntPtr(0);

            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYDOWN, (int)Keys.ShiftKey, NULL);
            Thread.Sleep(100);
            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYUP, (int)Keys.ShiftKey, NULL);
            Thread.Sleep(100);
            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYDOWN, (int)Keys.D2, NULL);
            Thread.Sleep(100);
            WindowManager.SendMessage((int)DOFUSPTR, WindowManager.WM_KEYUP, (int)Keys.D2, NULL);
            Thread.Sleep(100);
        }

        public static void ClickOn(int posX, int posY)
        {
            SetCursorPos(posX, posY);
            mouse_event(MOUSEEVENTF_LEFTDOWN, posX, posY, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, posX, posY, 0, 0);
            return;
        }

        public static void MoveMouseTo(int posX, int posY)
        {
            
            float time = 0;
            float maxTime = 500;
            Random rand = new Random();

            POINT mousePos;
            do
            {
                GetCursorPos(out mousePos);

                int offsetPosX;
                int offsetPosY;
                if (time < maxTime)
                {
                    offsetPosX = (int)((posX - mousePos.X) * time / maxTime);
                    offsetPosY = (int)((posY - mousePos.Y) * time / maxTime);
                }
                else
                {
                    offsetPosX = (int)(posX - mousePos.X);
                    offsetPosY = (int)(posY - mousePos.Y);
                }

                SetCursorPos(mousePos.X + offsetPosX, mousePos.Y + offsetPosY);

                int sleepTime = rand.Next(20, 40);
                time += sleepTime;
                Thread.Sleep(sleepTime);
            } while (Math.Abs(mousePos.X - posX) > 2 || Math.Abs(mousePos.X - posX) > 2);
            
        }

        private static IntPtr CreateLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        public static bool IsFightStarted(IntPtr hWnd)
        {
            SetForegroundWindow(hWnd);

            // Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                Screen.PrimaryScreen.Bounds.Height,
                                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var gfxScreenshot = Graphics.FromImage(bmpScreenshot))
            {

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                // Save the screenshot to the specified path that the user has chosen.
                //bmpScreenshot.Save("fightEtalon.png", ImageFormat.Png);

                //return new List<RECT>();

                int nbGreen = 0;
                for (int i = 1352; i < 1486; i++)
                {
                    for (int j = 982; j < 1025; j++)
                    {
                        Color color = bmpScreenshot.GetPixel(i, j);

                        if (color.G > 50 && color.B < 10)
                        {
                            nbGreen++;
                        }
                    }
                }

                bmpScreenshot.Dispose();

                return nbGreen > 2000;
            }
        }

        public static POINT FindOpponent(IntPtr hWnd)
        {
            POINT result = new POINT();

            SetForegroundWindow(hWnd);

            // Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                Screen.PrimaryScreen.Bounds.Height,
                                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var gfxScreenshot = Graphics.FromImage(bmpScreenshot))
            {

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);
                bool[,] matrix = new bool[bmpScreenshot.Height, bmpScreenshot.Width];

                //bmpScreenshot = new Bitmap("fightEtalon.png");

                int nbPoints = 0;

                result.X = 0;
                result.Y = 0;

                //Bitmap bit = new Bitmap(bmpScreenshot);

                for (int i = 0; i < bmpScreenshot.Width; i++)
                {
                    for (int j = 0; j < bmpScreenshot.Height; j++)
                    {
                        Color color = bmpScreenshot.GetPixel(i, j);

                        matrix[j, i] = false;
                        if (color.B > 200 && color.R == 0 && color.G == 0)
                        {
                            matrix[j, i] = true;

                            result.X += i;
                            result.Y += j;

                            nbPoints++;
                        }
                    }
                }

                if (nbPoints > 0)
                {
                    result.X /= nbPoints;
                    result.Y /= nbPoints;

                    result.X -= 5;
                    result.Y -= 5;
                }

                //using (Graphics graphics = Graphics.FromImage(bit))
                //{
                //    using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
                //    {
                //        graphics.FillRectangle(myBrush, new Rectangle(result.X - 10 , result.Y - 10, 20, 20));
                //        //graphics.FillRectangle(myBrush, new Rectangle(978, 261, 6, 200));
                //    }
                //}

                //bit.Save("ScreenshotFight.png", ImageFormat.Png);

                bmpScreenshot.Dispose();

                return result;
            }
        }

        private static readonly List<Color> phorreurColors = new List<Color>
        {
                Color.FromArgb(203, 205, 154), //Horn light
                Color.FromArgb(145, 148, 103), //Horn dark
                Color.FromArgb(49, 89, 77), //Skin light
                Color.FromArgb(37, 56, 48), //Skin medium
                Color.FromArgb(34, 66, 59), //Skin dark
                Color.FromArgb(25, 48, 50), //Skin darkest
                Color.FromArgb(230, 179, 133), //Light noze
                Color.FromArgb(166, 107, 53), //Dark noze & belly
                Color.FromArgb(255, 255, 204), //Eye
        };
        public static List<POINT> FindInterestPoints(Bitmap opaqueScreen, Bitmap transpaScreen)
        {
            const int x1 = 320, x2 = 1600, y1 = 20, y2 = 920;

            List<POINT> result = new List<POINT>();

            bool[,] matrix = new bool[opaqueScreen.Width, opaqueScreen.Height];
            Bitmap diffBitmap = new Bitmap(opaqueScreen.Width, opaqueScreen.Height);

            // Find all differencies
            Bitmap bit4 = new Bitmap(opaqueScreen.Width, opaqueScreen.Height);
            for (int x = x1; x < x2; x++)
            {
                for (int y = y1; y < y2; y++)
                {
                    Color color1 = transpaScreen.GetPixel(x, y);
                    Color color2 = opaqueScreen.GetPixel(x, y);

                    Color diff = Color.FromArgb(Math.Abs(color1.R - color2.R), Math.Abs(color1.G - color2.G), Math.Abs(color1.B - color2.B));

                    if (diff.R > 10 || diff.G > 10 || diff.B > 10)
                    {
                        foreach (Color item in phorreurColors)
                        {
                            int distR = Math.Abs(opaqueScreen.GetPixel(x, y).R - item.R);
                            int distG = Math.Abs(opaqueScreen.GetPixel(x, y).G - item.G);
                            int distB = Math.Abs(opaqueScreen.GetPixel(x, y).B - item.B);
                            if (distR + distG + distB < 200)
                            {
                                matrix[x, y] = true;
                                bit4.SetPixel(x, y, Color.Red);
                            }
                        }
                    }
                    
                    diffBitmap.SetPixel(x, y, diff);
                }
            }

            // Try to find the phorreur
            const int xStep = 88, yStep = 22, checkSize = 10;
            bool skipFirst = true;

            for (int y = y1 + yStep; y < y2; y += yStep)
            {
                skipFirst = !skipFirst;

                for (int x = x1 + (xStep / (skipFirst ? 1 : 2)); x < x2; x += xStep)
                {
                    bit4.SetPixel(x, y, Color.Blue);

                    int count = 0;
                    for (int i = -checkSize; i < checkSize; i++)
                    {
                        for (int j = -checkSize; j < checkSize; j++)
                        {
                            if (matrix[x + i, y + j])
                            {
                                bit4.SetPixel(x + i, y + j, Color.Purple);
                                count++;
                            }
                            else
                                bit4.SetPixel(x + i, y + j, Color.Green);
                        }
                    }

                    if (count > 30)
                        result.Add(new POINT() { X = x, Y = y });
                }
            }
            return result;
        }

        public static void CleanImage(Bitmap image1)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    Color color = image1.GetPixel(i, j);

                    int test = color.R + color.G + color.B;

                    if(min > test)
                    {
                        min = test;
                    }

                    if(max < test)
                    {
                        max = test;
                    }
                }
            }

            int mean = (min + max) / 2;

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    Color color = image1.GetPixel(i, j);

                    int test = color.R + color.G + color.B;

                    if (test < mean)
                    {
                        image1.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        image1.SetPixel(i, j, Color.Black);
                    }
                }
            }
        }

        public static Bitmap CreateDiffBitbmap(Bitmap image1, Bitmap image2)
        {
            Bitmap returnBitmap = new Bitmap(image1.Width, image1.Height);

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    Color color1 = image2.GetPixel(i, j);
                    Color color2 = image1.GetPixel(i, j);

                    Color diff = Color.FromArgb(Math.Abs(color1.R - color2.R), Math.Abs(color1.G - color2.G), Math.Abs(color1.B - color2.B));
                    returnBitmap.SetPixel(i, j, diff);
                }
            }

            return returnBitmap;
        }

        private static RECT FindRect(bool[,] matrix, bool[,] alreadyComputedMatrix, int startIndex, int endIndex, int row)
        {
            RECT result = new RECT();
            while (startIndex - 1 >= 0 && matrix[row, startIndex - 1])
            {
                startIndex--;
            }

            while (endIndex + 1 < matrix.GetLength(1) && matrix[row, endIndex + 1])
            {
                endIndex++;
            }

            int min = startIndex;
            int max = endIndex;
            int minRow = row;
            int maxRow = row;

            int minUnderRow = -1;
            int maxUnderRow = -1;
            for (int z = startIndex; z <= endIndex; z++)
            {
                if (alreadyComputedMatrix[row, z] == false)
                {
                    alreadyComputedMatrix[row, z] = true;
                    if (row + 1 < matrix.GetLength(0))
                    {
                        if (matrix[row + 1, z])
                        {
                            if (minUnderRow < 0)
                            {
                                minUnderRow = z;
                            }
                            maxUnderRow = z;
                        }
                    }
                }
            }

            if (minUnderRow >= 0)
            {
                RECT childRect = FindRect(matrix, alreadyComputedMatrix, minUnderRow, maxUnderRow, row + 1);

                if (childRect.x1 < min)
                {
                    min = childRect.x1;
                }

                if (childRect.y1 > max)
                {
                    max = childRect.y1;
                }

                maxRow = childRect.y2;
            }

            result.x1 = min;
            result.y1 = max;
            result.x2 = row;
            result.y2 = maxRow;

            return result;
        }
    }
}
