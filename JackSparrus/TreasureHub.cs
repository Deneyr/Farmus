using IronOcr;
using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JackSparrus
{
    public class TreasureHub
    {
        private Bitmap hubStartBitmap;

        private Bitmap[] arrowBitmaps;

        private Bitmap[] flagBitmaps;

        private Point startFirstRow;

        private OcrApi tEngine;

        private Point treasureHuntStart;

        public List<TreasureRow> Rows
        {
            get;
            private set;
        }

        public TreasureHub()
        {
            hubStartBitmap = new Bitmap(@"Assets\hubStart.png");

            this.arrowBitmaps = new Bitmap[]
            {
                new Bitmap(@"Assets\arrowUp.png"),
                new Bitmap(@"Assets\arrowRight.png"),
                new Bitmap(@"Assets\arrowDown.png"),
                new Bitmap(@"Assets\arrowLeft.png")
            };

            this.flagBitmaps = new Bitmap[]
            {
                new Bitmap(@"Assets\flagFill.png"),
                new Bitmap(@"Assets\flagEmpty.png")
            };

            this.Rows = new List<TreasureRow>();
            
            this.tEngine = OcrApi.Create();
            this.tEngine.Init(Patagames.Ocr.Enums.Languages.French);
        }

        public void UpdateHubFrom(Bitmap screen)
        {
            List<TreasureRow> previousRows = new List<TreasureRow>(this.Rows);

            this.Rows.Clear();

            Bitmap startBitmap = screen.Clone(new Rectangle(0, 0, 60, 140), screen.PixelFormat);
            Point hubStartPoint = SearchPattern(startBitmap, this.hubStartBitmap, out float bestResult);
            startBitmap.Dispose();

            this.startFirstRow = new Point(hubStartPoint.X + 23, hubStartPoint.Y + 69);

            Point startRow = this.startFirstRow;

            Bitmap iconBitmap = screen.Clone(new Rectangle(startRow.X, startRow.Y, 22, 31), screen.PixelFormat);
            Bitmap flagBitmap = screen.Clone(new Rectangle(startRow.X + 260, startRow.Y, 19, 31), screen.PixelFormat);
            Bitmap rowBitmap = screen.Clone(new Rectangle(startRow.X + 22, startRow.Y, 285 - 19, 31), screen.PixelFormat);

            int i = 0;
            bool isActiveRow = true;
            while(this.IsEndRow(rowBitmap) == false)
            {
                Direction rowDirection = Direction.NONE;
                if (isActiveRow)
                {
                    if (i == 0)
                    {
                        rowDirection = Direction.START;
                    }
                    else
                    {
                        rowDirection = this.SearchDirection(iconBitmap);
                    }
                }

                string rowText = "?";
                if (isActiveRow)
                {
                    int newWidth = this.GetWidthTextRow(rowBitmap, i);
                    Bitmap newTextBitmap = rowBitmap.Clone(new Rectangle(0, 0, newWidth, rowBitmap.Height), screen.PixelFormat);
                    //newTextBitmap.Save("test.png", ImageFormat.Png);

                    rowText = GetIronOCRText(newTextBitmap);

                    newTextBitmap.Dispose();
                }

                if(i == 0)
                {
                    this.SetTreasureHuntStart(rowText);
                    rowText = "Départ [" + this.treasureHuntStart.X + ";" + this.treasureHuntStart.Y + "]";
                }

                if (i != 0 && isActiveRow)
                {
                    isActiveRow = this.IsFlagFill(flagBitmap);
                }

                TreasureRow newRow = new TreasureRow(rowDirection, rowText, isActiveRow);
                this.Rows.Add(newRow);

                if (isActiveRow && i < previousRows.Count)
                {
                    newRow.WebRowText = previousRows[i].WebRowText;
                    newRow.WebHintPosition = previousRows[i].WebHintPosition;
                }

                startRow.Y += 31;

                iconBitmap.Dispose();
                flagBitmap.Dispose();
                rowBitmap.Dispose();
                iconBitmap = screen.Clone(new Rectangle(startRow.X, startRow.Y, 22, 31), screen.PixelFormat);
                flagBitmap = screen.Clone(new Rectangle(startRow.X + 288, startRow.Y, 19, 31), screen.PixelFormat);
                rowBitmap = screen.Clone(new Rectangle(startRow.X + 22, startRow.Y, 285 - 19, 31), screen.PixelFormat);

                i++;
            }

            iconBitmap.Dispose();
            flagBitmap.Dispose();
            rowBitmap.Dispose();
        }

        private int GetWidthTextRow(Bitmap rowBitmap, int offsetJ)
        {
            int i = rowBitmap.Width - 1;

            while(i >= 0 && this.IsThereTextInColumn(rowBitmap, i, rowBitmap.Height / 2 - offsetJ) == false)
            {
                i--;
            }

            return Math.Min(rowBitmap.Width, i + 10);
        }

        private void SetTreasureHuntStart(string startRow)
        {
            Regex rx = new Regex(@"([\+-]?\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(startRow);

            if(matches.Count >= 2)
            {
                this.treasureHuntStart = new Point(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
            }
        }

        private bool IsThereTextInColumn(Bitmap rowBitmap, int i, int j)
        {
            //int minJ = rowBitmap.Height;
            //int maxJ = 0;
            //for (int j = 0; j < rowBitmap.Height; j++)
            //{
            //    Color colorPixel = rowBitmap.GetPixel(row, j);

            //    if (colorPixel.R + colorPixel.G + colorPixel.B < 200)
            //    {
            //        if(j > maxJ)
            //        {
            //            maxJ = j;
            //        }
            //        if (j < minJ)
            //        {
            //            minJ = j;
            //        }
            //    }
            //}

            System.Drawing.Color colorPixel = rowBitmap.GetPixel(i, j);

            if (colorPixel.R + colorPixel.G + colorPixel.B < 200)
            {
                return true;
            }

            return false; // maxJ > minJ && rowBitmap.Height - maxJ > 1;
        }

        private bool IsFlagFill(Bitmap image)
        {
            int i = 0;
            int bestDirectionIndex = -1;
            float bestResult = float.MaxValue;
            foreach (Bitmap pattern in this.flagBitmaps)
            {
                SearchPattern(image, pattern, out float result);

                if (result < bestResult)
                {
                    bestDirectionIndex = i;

                    bestResult = result;
                }

                i++;
            }

            return bestDirectionIndex == 0;
        }

        private Direction SearchDirection(Bitmap image)
        {
            int i = 0;
            int bestDirectionIndex = -1;
            float bestResult = float.MaxValue;
            foreach(Bitmap pattern in this.arrowBitmaps)
            {
                SearchPattern(image, pattern, out float result);

                if(result < bestResult)
                {
                    bestDirectionIndex = i;

                    bestResult = result;
                }

                i++;
            }

            return (Direction) bestDirectionIndex;
        }

        private bool IsEndRow(Bitmap row)
        {
            int mean = 0;
            for (int i = 0; i < row.Width; i++)
            {
                System.Drawing.Color color = row.GetPixel(i, 16);
                mean += color.R + color.G + color.B;
            }

            mean /= row.Width;

            return mean < 200;
        }

        public void ValidateRow(Direction direction, int displacement)
        {
            TreasureRow currentRow = this.GetCurrentTreasureRow();
            int indexCurrentRow = this.Rows.IndexOf(currentRow);

            currentRow.WebHintPosition = this.GetNewPosition(direction, displacement);

            Point pointToClick = new Point(this.startFirstRow.X + 275, this.startFirstRow.Y + indexCurrentRow * 31 + 5);

            WindowManager.ClickOn(pointToClick.X, pointToClick.Y);

            Size screenSize = WindowManager.GetScreenSize();
            Random rand = new Random();
            Thread.Sleep(50 + rand.Next(0, 20));
            //WindowManager.MoveMouseTo(screenSize.Width / 2 + rand.Next(-50, 50), screenSize.Height / 2 + rand.Next(-50, 50));

            //Bitmap newScreenShot = WindowManager.CreateScreenBitmap();
            //this.UpdateHubFrom(newScreenShot);
        }

        public void ValidateStep()
        {
            Point pointToClick = new Point(this.startFirstRow.X + 288 - 5, this.startFirstRow.Y + this.Rows.Count * 31 + 25);

            WindowManager.MoveMouseTo(pointToClick.X, pointToClick.Y);

            WindowManager.ClickOn(pointToClick.X, pointToClick.Y);

            this.Rows.Clear();
        }

        public Point GetNewPosition(Direction direction, int displacement)
        {
            Point previousPosition = this.GetCurrentPosition();

            Point newPosition = TreasureHub.GetNewPosition(direction, previousPosition, displacement);
            return newPosition;
        }

        public Point GetCurrentPosition()
        {
            TreasureRow currentRow = this.GetCurrentTreasureRow();
            int indexCurrentRow = this.Rows.IndexOf(currentRow);

            Point currentPosition = this.treasureHuntStart;
            if (indexCurrentRow > 1)
            {
                currentPosition = this.Rows[indexCurrentRow - 1].WebHintPosition;
            }

            return currentPosition;
        }

        public TreasureRow GetCurrentTreasureRow()
        {
            return this.Rows.FirstOrDefault(pElem => pElem.FlagActivated == false);
        }

        public Point SearchPattern(Bitmap screen, Bitmap pattern, out float bestResult)
        {
            bestResult = float.MaxValue;

            Point startHubPoint = new Point();

            for (int i = 0; i < screen.Width - pattern.Width + 1; i++)
            {
                for (int j = 0; j < screen.Height - pattern.Height + 1; j++)
                {

                    float comparaisonResult = 0;
                    for (int x = 0; x < pattern.Width; x++)
                    {
                        for (int y = 0; y < pattern.Height; y++)
                        {
                            System.Drawing.Color colorScreen = screen.GetPixel(i + x, j + y);
                            System.Drawing.Color colorPattern = pattern.GetPixel(x, y);

                            int diffR = colorScreen.R - colorPattern.R;
                            int diffG = colorScreen.G - colorPattern.G;
                            int diffB = colorScreen.B - colorPattern.B;

                            comparaisonResult += diffR * diffR + diffG * diffG + diffB * diffB;
                        }
                    }

                    if(comparaisonResult < bestResult)
                    {
                        startHubPoint.X = i;
                        startHubPoint.Y = j;

                        bestResult = comparaisonResult;
                    }
                }
            }

            return startHubPoint;
        }

        public string GetTextIn(Bitmap image)
        {
            try
            {
                WindowManager.CleanImage(image);

                image.Save("testet.png", ImageFormat.Png);

                return this.tEngine.GetTextFromImage(image).Trim();
            }
            catch (System.NullReferenceException)
            {
                return "";
            }
        }

        public string GetIronOCRText(Bitmap image)
        {
            var Ocr = new IronTesseract();
            Ocr.Language = OcrLanguage.French;

            var result = string.Empty;

            using (var input = new OcrInput())
            {
                input.AddImage(image);
                // Add image filters if needed
                // Input.Deskew();
                // Input.DeNoise();
                result = Ocr.Read(input).Text;
            }

            return result;
        }

        public static Point GetNewPosition(Direction direction, Point position, int nbCase)
        {
            switch (direction)
            {
                case Direction.UP:
                    return new Point(position.X, position.Y - nbCase);
                case Direction.RIGHT:
                    return new Point(position.X + nbCase, position.Y);
                case Direction.DOWN:
                    return new Point(position.X, position.Y + nbCase);
                case Direction.LEFT:
                    return new Point(position.X - nbCase, position.Y);
            }
            return position;
        }

        public static string GetDirectionString(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return "top";
                case Direction.RIGHT:
                    return "right";
                case Direction.DOWN:
                    return "bottom";
                case Direction.LEFT:
                    return "left";
            }
            return string.Empty;
        }

        public static Direction GetReverseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.RIGHT:
                    return Direction.LEFT;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
            }
            return direction;
        }

    }

    public class TreasureRow
    {
        private static string[] arrowSources = new string[]
        {
            @"Assets\arrowUp.png",
            @"Assets\arrowRight.png",
            @"Assets\arrowDown.png",
            @"Assets\arrowLeft.png"
        };

        private static string[] flagSources = new string[]
        {
            @"Assets\flagFill.png",
            @"Assets\flagEmpty.png"
        };

        public Direction Direction
        {
            get;
            set;
        }

        public string RowText
        {
            get;
            set;
        }

        public string WebRowText
        {
            get;
            set;
        }

        public string WebHintPositionText
        {
            get
            {
                if(this.WebHintPosition == Point.Empty)
                {
                    return string.Empty;
                }
                return "[" + this.WebHintPosition.X + ";" + this.WebHintPosition.Y + "]";
            }
        }

        public Point WebHintPosition
        {
            get;
            set;
        }

        public bool FlagActivated
        {
            get;
            set;
        }

        public BitmapImage DirectionImage
        {
            get
            {
                if (this.Direction >= 0)
                {
                    return new BitmapImage(new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + arrowSources[(int)this.Direction]));
                }
                return new BitmapImage();
            }
        }

        public BitmapImage FlagActivatedImage
        {
            get
            {
                if (this.Direction >= 0)
                {
                    return new BitmapImage(new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + (this.FlagActivated ? flagSources[0] : flagSources[1])));
                }
                return new BitmapImage();
            }
        }

        public TreasureRow(Direction direction, string rowText, bool flagActivated)
        {
            this.Direction = direction;
            this.RowText = rowText;
            this.WebRowText = string.Empty;
            this.WebHintPosition = Point.Empty;
            this.FlagActivated = flagActivated;
        }
    }

    public enum Direction
    {
        NONE = -2,
        START = -1,
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }
}
