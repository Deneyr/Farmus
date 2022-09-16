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

namespace Farmus
{
    class Program
    {
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
            public int left;
            public int top;
            public int right;
            public int bottom;
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

        public static int indexRoom = 100;

        public static int side = 0;

        static void Main(string[] args)
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = null;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains("Dofus"))
                {
                    hWnd = pList.MainWindowHandle;
                    process = pList;
                }
            }

            // --- END REAL ---

            TreasureHub tresorHub = new TreasureHub();

            Bitmap screen = new Bitmap("testTresor.png");


            SetForegroundWindow(hWnd);

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

            tresorHub.UpdateHubFrom(bmpScreenshot);

            //// Save the screenshot to the specified path that the user has chosen.
            //bmpScreenshot.Save("test.png", ImageFormat.Png);

            return;

            //var Ocr = new AdvancedOcr()
            //{
            //    CleanBackgroundNoise = true,
            //    EnhanceContrast = true,
            //    EnhanceResolution = true,
            //    Language = IronOcr.Languages.French.OcrLanguagePack,
            //    Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
            //    ColorSpace = AdvancedOcr.OcrColorSpace.Color,
            //    DetectWhiteTextOnDarkBackgrounds = true,
            //    InputImageType = AdvancedOcr.InputTypes.Document,
            //    RotateAndStraighten = true,
            //    ReadBarCodes = true,
            //    ColorDepth = 4
            //};
            //var Result = Ocr.Read(@"TestOCR.png");
            //File.WriteAllText("test.txt", Result.Text);



            //return;

            // --- START REAL --- 

            IntPtr NULL = new IntPtr(0);

            //while (IsFightStarted(hWnd) == false);

            //while (IsFightStarted(hWnd))
            //{
            //    //IntPtr NULL = new IntPtr(0);

            //    Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.F1, NULL);
            //    Thread.Sleep(100);
            //    Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.F1, NULL);

            //    Thread.Sleep(5000);

            //    Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.R, NULL);
            //    Thread.Sleep(100);
            //    Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.R, NULL);

            //    POINT oppPosition = FindOpponent(hWnd);

            //    Thread.Sleep(100);
            //    ClickOn((int)hWnd, oppPosition.X, oppPosition.Y);

            //    Thread.Sleep(1000);

            //    Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.R, NULL);
            //    Thread.Sleep(100);
            //    Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.R, NULL);

            //    Thread.Sleep(100);
            //    ClickOn((int)hWnd, oppPosition.X, oppPosition.Y);
            //    Thread.Sleep(300);
            //    ClickOn((int)hWnd, 0, 0);

            //    Thread.Sleep(2000);
            //}

            //while (true)
            //{
            //    POINT mousePos;
            //    GetCursorPos(out mousePos);

            //    Console.WriteLine(mousePos.X + " : " + mousePos.Y);
            //}

            POINT[] minePoints = new POINT[]
            {
                new POINT(){X = 396, Y = 480 },
                new POINT(){X = 442, Y = 496 },
                new POINT(){X = 477, Y = 479 },
                new POINT(){X = 532, Y = 453 },
                new POINT(){X = 639, Y = 482 },
                new POINT(){X = 914, Y = 801 },
                new POINT(){X = 962, Y = 827 },
                new POINT(){X = 1008, Y = 788 },
                new POINT(){X = 864, Y = 286 },
                new POINT(){X = 908, Y = 262 },
                new POINT(){X = 968, Y = 273 },
                new POINT(){X = 1061, Y = 315 },
                new POINT(){X = 1383, Y = 483 },
                new POINT(){X = 1425, Y = 503 },
                new POINT(){X = 1521, Y = 546 },
                new POINT(){X = 1560, Y = 533 },
            };

            POINT[] minePoints2 = new POINT[]
            {
                new POINT(){X = 1240, Y = 496 }
            };

            POINT[] minePoints3 = new POINT[]
            {
                new POINT(){X = 1194, Y = 235 }
            };

            POINT[] minePoints4 = new POINT[]
            {
                new POINT(){X = 686, Y = 534 },
                new POINT(){X = 718, Y = 504 },
                new POINT(){X = 1003, Y = 493 },
                new POINT(){X = 1106, Y = 482 }
            };

            POINT waypoint1 = new POINT() { X = 1172, Y = 478};
            POINT waypoint2 = new POINT() { X = 564, Y = 830};

            // Bauxite mine
            POINT[] minePointsBaux = new POINT[]
            {
                new POINT(){X = 720, Y = 320 },
                new POINT(){X = 963, Y = 214 },
                new POINT(){X = 1519, Y = 584 }
            };
            POINT[] minePointsBaux2 = new POINT[]
            {
                new POINT(){X = 532, Y = 496 },
                new POINT(){X = 578, Y = 478 },
                new POINT(){X = 638, Y = 479 },
                new POINT(){X = 679, Y = 500 },

                new POINT(){X = 874, Y = 361 },
                new POINT(){X = 925, Y = 335 },
                new POINT(){X = 1010, Y = 284 },
                new POINT(){X = 1425, Y = 442 }
            };
            POINT[] minePointsBaux3 = new POINT[]
            {
                new POINT(){X = 1382, Y = 524 },
                new POINT(){X = 1423, Y = 535 }
            };
            POINT[] minePointsBaux4 = new POINT[]
            {
                new POINT(){X = 1011, Y = 387 }
            };

            POINT waypointBaux = new POINT() { X = 1448, Y = 582 };

            POINT waypointBaux2 = new POINT() { X = 1355, Y = 441 };
            POINT waypointBaux2b = new POINT() { X = 468, Y = 653 };

            POINT waypointBaux3 = new POINT() { X = 1310, Y = 419 };
            POINT waypointBaux3b = new POINT() { X = 890, Y = 770 };

            POINT waypointBaux4 = new POINT() { X = 796, Y = 630 };

            // Obsi mine
            POINT[] minePointsObsi = new POINT[]
            {
                new POINT(){X = 1342, Y = 552 }
            };

            // Bitmap
            Bitmap bauxiteEtalonBitmap = new Bitmap("BauxiteEtalon.png");
            Bitmap bauxiteEtalon2Bitmap = new Bitmap("BauxiteEtalon2.png");
            Bitmap bauxiteEtalon3Bitmap = new Bitmap("BauxiteEtalon3.png");
            Bitmap bauxiteEtalon4Bitmap = new Bitmap("BauxiteEtalon4.png");

            Bitmap obsiEtalonBitmap = new Bitmap("ObsiEtalon.png");

            FindOpponent(hWnd);

            Random rand = new Random();

            while (true)
            {

                int counter = rand.Next(10, 15);

                if ((int)rand.NextDouble() * 1000 == 777)
                {
                    counter = rand.Next(150, 200);
                }

                if ((int) (rand.NextDouble() * 10) == 5)
                {
                    BreakIdle(hWnd, NULL);
                }

                for (int i = 0; i < counter; i++)
                {
                    Bitmap etalon = null;
                    POINT[] minePointsFilon = new POINT[1];

                    //etalon = "SilverEtalon.png";
                    //minePointsFilon = minePoints4;

                    switch (indexRoom)
                    {
                        case 0:
                            etalon = bauxiteEtalonBitmap;
                            minePointsFilon = minePointsBaux;
                            break;
                        case 1:
                            etalon = bauxiteEtalon2Bitmap;
                            minePointsFilon = minePointsBaux2;
                            break;
                        case 2:
                            etalon = bauxiteEtalon3Bitmap;
                            minePointsFilon = minePointsBaux3;
                            break;
                        case 3:
                            etalon = bauxiteEtalon4Bitmap;
                            minePointsFilon = minePointsBaux4;
                            break;

                        case 100:
                            etalon = obsiEtalonBitmap;
                            minePointsFilon = minePointsObsi;
                            break;
                    }

                    List<RECT> listResult = CaptureScreen2(hWnd, minePointsFilon, etalon);

                    if (listResult.Count > 0)
                    {
                        RECT result = listResult[(int)(rand.NextDouble() * listResult.Count)];

                        int x = result.left + (int)((rand.NextDouble() * ((result.right - result.left) / 3f)) + (result.right - result.left) / 3f);
                        int y = result.top + (int)((rand.NextDouble() * ((result.bottom - result.top) / 3f)) + (result.bottom - result.top) / 3f);

                        //MoveMouseTo(x, y);

                        //Program.SendMessage((int)hWnd, Program.WM_LBUTTONDOWN, 0x00000001, CreateLParam(x, y));
                        //Thread.Sleep(100);
                        //Program.SendMessage((int)hWnd, Program.WM_LBUTTONUP, 0x00000001, CreateLParam(x + (int)(rand.NextDouble() * 4) - 2, y + (int)(rand.NextDouble() * 4) - 2));

                        ClickOn((int)hWnd, x, y);

                        Thread.Sleep(5000);

                        while (IsFightStarted(hWnd))
                        {
                            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.F1, NULL);
                            Thread.Sleep(100);
                            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.F1, NULL);

                            Thread.Sleep(5000);

                            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.Q, NULL);
                            Thread.Sleep(100);
                            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.Q, NULL);

                            ClickOn((int)hWnd, 1821, 814);

                            Thread.Sleep(500);

                            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.R, NULL);
                            Thread.Sleep(100);
                            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.R, NULL);

                            POINT oppPosition = FindOpponent(hWnd);

                            Thread.Sleep(100);
                            ClickOn((int)hWnd, oppPosition.X, oppPosition.Y);

                            Thread.Sleep(1000);

                            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.R, NULL);
                            Thread.Sleep(100);
                            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.R, NULL);

                            Thread.Sleep(100);
                            ClickOn((int)hWnd, oppPosition.X, oppPosition.Y);
                            Thread.Sleep(300);
                            ClickOn((int)hWnd, 0, 0);

                            Thread.Sleep(2000);
                        }

                        int posX = 1000 - (int)(rand.NextDouble() * 4);
                        int posY = 500 - (int)(rand.NextDouble() * 4);

                        //ClickOn((int)hWnd, posX, posY);
                    }
                    Thread.Sleep(500);
                }

                if (indexRoom != 100)
                {
                    Point waypoint = new Point();

                    switch (indexRoom)
                    {
                        case 0:
                            waypoint = waypointBaux;
                            indexRoom = 1;

                            side = 0;
                            break;
                        case 1:
                            if (side == 0)
                            {
                                waypoint = waypointBaux2;
                                indexRoom = 2;
                            }
                            else
                            {
                                waypoint = waypointBaux2b;
                                indexRoom = 0;
                            }
                            break;
                        case 2:
                            if (side == 0)
                            {
                                waypoint = waypointBaux3;
                                indexRoom = 3;
                            }
                            else
                            {
                                waypoint = waypointBaux3b;
                                indexRoom = 1;
                            }
                            break;
                        case 3:
                            waypoint = waypointBaux4;
                            indexRoom = 2;

                            side = 1;
                            break;
                    }
                    //if (indexRoom == 0)
                    //{
                    //    waypoint = waypoint1;
                    //    indexRoom = 1;
                    //}
                    //else
                    //{
                    //    waypoint = waypoint2;
                    //    indexRoom = 0;
                    //}

                    waypoint.X = waypoint.X - (int)(rand.NextDouble() * 4);
                    waypoint.Y = waypoint.Y - (int)(rand.NextDouble() * 4);

                    ClickOn((int)hWnd, waypoint.X, waypoint.Y);

                    Thread.Sleep(5000);
                }
            }

        }

        private static void BreakIdle(IntPtr hWnd, IntPtr NULL)
        {
            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.D3, NULL);
            Thread.Sleep(100);
            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.D3, NULL);

            Thread.Sleep(3000);

            Program.SendMessage((int)hWnd, Program.WM_KEYDOWN, (int)Keys.D3, NULL);
            Thread.Sleep(100);
            Program.SendMessage((int)hWnd, Program.WM_KEYUP, (int)Keys.D3, NULL);

            Thread.Sleep(3000);
        }

        private static void ClickOn(int ptr, int posX, int posY)
        {
            Random rand = new Random();

            //MoveMouseTo(posX, posY);
            Program.SendMessage((int)ptr, Program.WM_LBUTTONDOWN, 0x00000001, CreateLParam(posX, posY));
            Thread.Sleep(100);
            Program.SendMessage((int)ptr, Program.WM_LBUTTONUP, 0x00000001, CreateLParam(posX + (int)(rand.NextDouble() * 4) - 2, posY + (int)(rand.NextDouble() * 4) - 2));
        }

        private static void MoveMouseTo(int posX, int posY)
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
                    offsetPosX = (int) ((posX - mousePos.X) * time / maxTime);
                    offsetPosY = (int) ((posY - mousePos.Y) * time / maxTime);
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
            } while(Math.Abs(mousePos.X - posX) > 5 || Math.Abs(mousePos.X - posX) > 5);
        }

        private static IntPtr CreateLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        // private static int counter = 0;

        private static List<RECT> CaptureScreen2(IntPtr hWnd, POINT[] minePoints, Bitmap etalon)
        {
            List<RECT> result = new List<RECT>();

            SetForegroundWindow(hWnd);

            // Create a new bitmap.
            //var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
            //                    Screen.PrimaryScreen.Bounds.Height,
            //                    PixelFormat.Format32bppArgb);

            //var bmpScreenshot = new Bitmap(20,
            //                    20,
            //                    PixelFormat.Format32bppArgb);


            // Create a graphics object from the bitmap.
            //using (var gfxScreenshot = Graphics.FromImage(bmpScreenshot))
            //{

            //    // Take the screenshot from the upper left corner to the right bottom corner.
            //    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
            //                                Screen.PrimaryScreen.Bounds.Y,
            //                                0,
            //                                0,
            //                                Screen.PrimaryScreen.Bounds.Size,
            //                                CopyPixelOperation.SourceCopy);

            //// Save the screenshot to the specified path that the user has chosen.
            //if (counter % 10 == 0)
            //{
            //    bmpScreenshot.Save("test.png", ImageFormat.Png);
            //    counter++;
            //}

            //return new List<RECT>();

            // bmpScreenshot.Save("TestOCR.png", ImageFormat.Png);

            //Bitmap bit1 = new Bitmap(etalon);


            foreach (POINT point in minePoints)
            {
                RECT rect = new RECT();
                rect.left = point.X - 10;
                rect.right = point.X + 10;
                rect.top = point.Y - 10;
                rect.bottom = point.Y + 10;

                rect.left = Math.Max(0, rect.left);
                rect.right = Math.Min(Screen.PrimaryScreen.Bounds.Size.Width, rect.right);
                rect.top = Math.Max(0, rect.top);
                rect.bottom = Math.Min(Screen.PrimaryScreen.Bounds.Size.Height, rect.bottom);

                int width = rect.right - rect.left + 1;
                int height = rect.bottom - rect.top + 1;

                using (var bmpScreenshot = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    using (var gfxScreenshot = Graphics.FromImage(bmpScreenshot))
                    {

                        // Take the screenshot from the upper left corner to the right bottom corner.
                        gfxScreenshot.CopyFromScreen(rect.left,
                                                    rect.top,
                                                    0,
                                                    0,
                                                    new Size(width, height),
                                                    CopyPixelOperation.SourceCopy);

                        int nbDiffPixel = 0;
                        int nbTotalPixel = 0;
                        for (int i = rect.left; i <= rect.right; i++)
                        {
                            for (int j = rect.top; j <= rect.bottom; j++)
                            {
                                Color color1 = etalon.GetPixel(i, j);
                                Color color2 = bmpScreenshot.GetPixel(i - rect.left, j - rect.top);

                                Color diff = Color.FromArgb(Math.Abs(color1.R - color2.R), Math.Abs(color1.G - color2.G), Math.Abs(color1.B - color2.B));

                                if (diff.R > 10 || diff.G > 10 || diff.B > 10)
                                {
                                    nbDiffPixel++;
                                }
                                nbTotalPixel++;
                            }
                        }

                        if (((float)nbDiffPixel) / nbTotalPixel > 0.02f)
                        {
                            result.Add(rect);
                        }
                    }
                }
            }

            return result;
            //}
        }

        private static bool IsFightStarted(IntPtr hWnd)
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

        private static POINT FindOpponent(IntPtr hWnd)
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

        private static List<RECT> CaptureScreen(IntPtr hWnd)
        {
            List<RECT> result = new List<RECT>();

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
                //bmpScreenshot.Save("Screenshot2.png", ImageFormat.Png);

                Bitmap bit1 = new Bitmap("etalon.png");
                //bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);

                //bmpScreenshot = new Bitmap("Screenshot.png");

                //bmpScreenshot = new Bitmap("Screenshot2.png");

                Bitmap bit3 = new Bitmap(bit1.Width, bit1.Height);
                //Bitmap bit7 = new Bitmap(bit3);

                for (int i = 0; i < bmpScreenshot.Width; i++)
                {
                    for (int j = 0; j < bmpScreenshot.Height; j++)
                    {
                        Color color1 = bit1.GetPixel(i, j);
                        Color color2 = bmpScreenshot.GetPixel(i, j);

                        Color diff = Color.FromArgb(Math.Abs(color1.R - color2.R), Math.Abs(color1.G - color2.G), Math.Abs(color1.B - color2.B));
                        bit3.SetPixel(i, j, diff);

                        //if(diff.R > 20 || diff.G > 20 || diff.B > 20)
                        //{
                        //    bit7.SetPixel(i, j, Color.Red);
                        //}
                        //else
                        //{
                        //    bit7.SetPixel(i, j, Color.Black);
                        //}
                    }
                }
                //bit7.Save("Screenshot2.png", ImageFormat.Png);
                bool[,] matrix = new bool[bmpScreenshot.Height, bmpScreenshot.Width];


                // Bitmap bit6 = new Bitmap(bit1.Width, bit1.Height);
                for (int j = 0; j < bmpScreenshot.Height; j++)
                {
                    int indexPresent = -1;
                    for (int i = 0; i < bmpScreenshot.Width; i++)
                    {
                        Color color = bit3.GetPixel(i, j);

                        matrix[j, i] = false;
                        //bit6.SetPixel(i, j, Color.Black);
                        if (color.R > 20 || color.G > 20 || color.B > 20)
                        {

                            if (indexPresent >= 0 && i - indexPresent < 20)
                            {
                                for (int z = 0; z < i - indexPresent; z++)
                                {
                                    matrix[j, indexPresent + z] = true;

                                    //bit6.SetPixel(indexPresent + z, j, Color.Red);
                                }
                            }

                            indexPresent = i;
                        }
                    }
                }
                //bit6.Save("Screenshot3.png", ImageFormat.Png);

                // Bitmap bit4 = new Bitmap(bit1.Width, bit1.Height);
                for (int i = 0; i < bmpScreenshot.Width; i++)
                {
                    int indexPresent = -1;
                    for (int j = 0; j < bmpScreenshot.Height; j++)
                    {
                        bool color = matrix[j, i];

                        matrix[j, i] = false;

                        //bit4.SetPixel(i, j, Color.Black);
                        if (color)
                        {

                            if (indexPresent >= 0 && j - indexPresent < 30)
                            {
                                for (int z = 0; z < j - indexPresent; z++)
                                {
                                    matrix[indexPresent + z, i] = true;

                                    //bit4.SetPixel(i, indexPresent + z, Color.Red);
                                }
                            }

                            indexPresent = j;
                        }
                    }
                }
                //bit4.Save("Screenshot4.png", ImageFormat.Png);

                bool[,] alreadyComputedMatrix = new bool[bmpScreenshot.Height, bmpScreenshot.Width];
                for (int j = 0; j < bmpScreenshot.Height; j++)
                {
                    for (int i = 0; i < bmpScreenshot.Width; i++)
                    {
                        alreadyComputedMatrix[j, i] = false;
                    }
                }

                //Bitmap bit5 = new Bitmap(bmpScreenshot);
                for (int j = 0; j < bmpScreenshot.Height; j++)
                {
                    for (int i = 0; i < bmpScreenshot.Width; i++)
                    {
                        bool color = matrix[j, i];

                        if (alreadyComputedMatrix[j, i] == false && color)
                        {
                            int z = 0;
                            while (z + i < bmpScreenshot.Width && matrix[j, i + z])
                            {
                                z++;
                            }

                            RECT rectangle = FindRect(matrix, alreadyComputedMatrix, i, i + z - 1, j);

                            if (rectangle.right - rectangle.left + 1 > 20 && rectangle.bottom - rectangle.top + 1 > 20)
                            // && rectangle.right - rectangle.left + 1 < 100 && rectangle.bottom - rectangle.top + 1 < 500)
                            {
                                if (rectangle.right - rectangle.left + 1 < 300 || rectangle.bottom - rectangle.top + 1 < 300)
                                {

                                    if (rectangle.right > 280 && rectangle.left + 1 < 1640
                                        && rectangle.bottom < 930)
                                    {
                                        Rectangle rect = new Rectangle(rectangle.left, rectangle.top, rectangle.right - rectangle.left + 1, rectangle.bottom - rectangle.top + 1);

                                        Rectangle waypoint = new Rectangle(1373, 660, 1443 - 1373, 719 - 660);
                                        if (waypoint.IntersectsWith(rect) == false)
                                        {
                                            //using (Graphics graphics = Graphics.FromImage(bit5))
                                            //{
                                            //    using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
                                            //    {
                                            //        graphics.FillRectangle(myBrush, new Rectangle(rectangle.left, rectangle.top, rectangle.right - rectangle.left + 1, rectangle.bottom - rectangle.top + 1));
                                            //        //graphics.FillRectangle(myBrush, new Rectangle(978, 261, 6, 200));
                                            //    }
                                            //}

                                            result.Add(rectangle);
                                        }
                                    }
                                }
                                else
                                {
                                    Random rand = new Random();

                                    int posX = 1373 - (int)rand.NextDouble() * 4;
                                    int posY = 660 - (int)rand.NextDouble() * 4;

                                    MoveMouseTo(posX, posY);

                                    Program.SendMessage((int)hWnd, Program.WM_LBUTTONDOWN, 0x00000001, CreateLParam(posX, posY));
                                    Thread.Sleep(100);
                                    Program.SendMessage((int)hWnd, Program.WM_LBUTTONUP, 0x00000001, CreateLParam(posX + (int)(rand.NextDouble() * 4) - 2, posY + (int)(rand.NextDouble() * 4) - 2));
                                }
                            }
                        }
                    }
                }

                bmpScreenshot.Dispose();
                bit1.Dispose();
                bit3.Dispose();

                //bit5.Save("Screenshot5.png", ImageFormat.Png);

                return result;
            }
        }

        private static RECT FindRect(bool[,] matrix, bool[,] alreadyComputedMatrix, int startIndex, int endIndex, int row)
        {
            RECT result = new RECT();
            while(startIndex - 1 >= 0 && matrix[row, startIndex - 1])
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

            if(minUnderRow >= 0)
            {
                RECT childRect = FindRect(matrix, alreadyComputedMatrix, minUnderRow, maxUnderRow, row + 1);

                if(childRect.left < min)
                {
                    min = childRect.left;
                }

                if (childRect.right > max)
                {
                    max = childRect.right;
                }

                maxRow = childRect.bottom;
            }

            result.left = min;
            result.right = max;
            result.top = row;
            result.bottom = maxRow;

            return result;
        }
    }
}
