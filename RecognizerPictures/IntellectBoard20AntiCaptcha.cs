using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using ClassLibraryNeuralNetworks;

namespace RecognizerPictures
{
    public class IntellectBoard20AntiCaptcha : IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }
        private readonly NeuralNW _net;

        public IntellectBoard20AntiCaptcha()
        {
            _net = new NeuralNW(Directory.GetCurrentDirectory() + "\\..\\..\\..\\RecognizerPictures\\nwFiles\\IntellectBoard20.nw");
        }

        public String recognizeImage(Bitmap imageSource)
        {
            Bitmap blackAndWhiteimage = MakeBlackAndWhitePicture(imageSource);
            Bitmap withoutnoiseimage = DeleteNoisePixels(blackAndWhiteimage);
            Bitmap img = DeleteLinesInRow(withoutnoiseimage);
            Bitmap[] symbols = CutImageIntoPieces(img);

            string newPath = Path.Combine(Environment.CurrentDirectory, "temp20\\");
            Directory.CreateDirectory(newPath);
            StringBuilder result = new StringBuilder();
            const string nameoffile = "weight";

            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = ResizeBitmap(DeleteLinesInColumn(DeleteLinesInRow(symbols[i], 1), 1), 8, 10);
                symbols[i].Save(newPath + nameoffile + string.Format("{0:00}", i) + ".bmp");
                SaveBin(newPath, nameoffile + string.Format("{0:00}", i), symbols[i]);
                result.Append(Recognize(newPath + nameoffile + string.Format("{0:00}", i) + ".in.txt", _net));
            }

            Directory.Delete(newPath, true);

            return result.ToString();

        }

        #region Делает изображение чёрно-белым

        /// <summary>
        /// Делает изображение чёрно-белым, ориентируясь на "небелые" пиксели
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <returns>Чёрно-белое изображение</returns>
        public static Bitmap MakeBlackAndWhitePicture(Bitmap img)
        {
            Bitmap newImg = new Bitmap(img.Width,img.Height);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color cl = img.GetPixel(i, j);

                    newImg.SetPixel(i, j, cl.Name != "ffffffff" ? Color.Black : Color.White);
                }
            }

            return newImg;
        }

        #endregion

        #region Удаляет пискельный шум

        /// <summary>
        /// Удаляет пискельный шум, ориентируется на количество пикселей вокруг
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="pixelAround">Количество пикселей вокруг</param>
        /// <returns>Изображение без пискельного шума</returns>
        public static Bitmap DeleteNoisePixels(Bitmap image, int pixelAround = 2)
        {

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var countBlackPixel = 0;
                    if (image.GetPixel(i, j).Name == "ff000000")
                        countBlackPixel++;
                    if (i + 1 < image.Width && image.GetPixel(i + 1, j).Name == "ff000000")
                        countBlackPixel++;
                    if (i - 1 > 0 && image.GetPixel(i - 1, j).Name == "ff000000")
                        countBlackPixel++;
                    if (j + 1 < image.Height && image.GetPixel(i, j + 1).Name == "ff000000")
                        countBlackPixel++;
                    if (j - 1 > 0 && image.GetPixel(i, j - 1).Name == "ff000000")
                        countBlackPixel++;
                    if (i + 1 < image.Width && j + 1 < image.Height && image.GetPixel(i + 1, j + 1).Name == "ff000000")
                        countBlackPixel++;
                    if (i + 1 < image.Width && j - 1 > 0 && image.GetPixel(i + 1, j - 1).Name == "ff000000")
                        countBlackPixel++;
                    if (i - 1 > 0 && j + 1 < image.Height && image.GetPixel(i - 1, j + 1).Name == "ff000000")
                        countBlackPixel++;
                    if (i - 1 > 0 && j - 1 > 0 && image.GetPixel(i - 1, j - 1).Name == "ff000000")
                        countBlackPixel++;
                    if (countBlackPixel < pixelAround)
                        image.SetPixel(i, j, Color.White);
                }
            }
            return image;
        }

        #endregion

        #region Удаляет пустые ряды на изображения

        /// <summary>
        /// Удаляет ряды на изображения, содержащие больше, чем заданное количество пикселей заданного цвета в ряду
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="numberOfPixelsInRow">Количество пикселей в ряду</param>
        /// <param name="colorName">Цвет, на который следует ориентироваться при удалении</param>
        /// <returns>Изображение без пустых рядов</returns>
        public static Bitmap DeleteLinesInRow(Bitmap image, int numberOfPixelsInRow = 3, string colorName = "ff000000")
        {
            var resultImg = new Bitmap(image.Width, image.Height);
            var newHeight = 0;

            for (int i = 0; i < image.Height; i++)
            {
                var numberOfBlackPixelsInRow = 0;
                for (int j = 0; j < image.Width; j++)
                {
                    if (image.GetPixel(j, i).Name == colorName)
                        numberOfBlackPixelsInRow++;
                }
                if (numberOfBlackPixelsInRow > numberOfPixelsInRow)
                {
                    for (int j = 0; j < image.Width; j++)
                        resultImg.SetPixel(j, newHeight, image.GetPixel(j, i));
                    newHeight++;
                }
            }

            resultImg = cropImage(resultImg, new Rectangle(0, 0, resultImg.Width, newHeight));

            return resultImg;
        }

        #endregion

        #region Обрезает изображение

        /// <summary>
        /// Обрезает изображение по размеру квадрата
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <param name="cropArea">Квадрат для обрезания</param>
        /// <returns>Обрезанное изображение</returns>
        public static Bitmap cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        #endregion

        #region Удаляет пустые столбцы на изображения

        /// <summary>
        /// Удаляет столбцы на изображения, содержащие больше, чем заданное количество пикселей заданного цвета в столбце
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="numberOfPixelsInColumn">Количество пикселей в столбце</param>
        /// <param name="colorName">Цвет, на который следует ориентироваться при удалении</param>
        /// <returns>Изображение без пустых столбцёв</returns>
        public static Bitmap DeleteLinesInColumn(Bitmap image, int numberOfPixelsInColumn = 2,
                                                 string colorName = "ff000000")
        {
            var resultImage = new Bitmap(image.Width, image.Height);

            var newWidth = 0;

            for (int i = 0; i < image.Width; i++)
            {
                var numberOfBlackPixelsInRow = 0;
                for (int j = 0; j < image.Height; j++)
                {
                    if (image.GetPixel(i, j).Name == colorName)
                        numberOfBlackPixelsInRow++;
                }
                if (numberOfBlackPixelsInRow > numberOfPixelsInColumn)
                {
                    for (int j = 0; j < image.Height; j++)
                        resultImage.SetPixel(newWidth, j, image.GetPixel(i, j));
                    newWidth++;
                }
            }

            resultImage = cropImage(resultImage, new Rectangle(0, 0, newWidth, resultImage.Height));
            return resultImage;
        }

        #endregion

        #region Разбивает изображение на части

        /// <summary>
        /// Разбивает изображение на части
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="numberOfPixelsInColumn">Количество пикселей в столбце, которое учитывает столбец, как подходящий</param>
        /// <param name="numberOfPixelsInRow">Количество пикселей, в которых располагается символ</param>
        /// <param name="colorName">Цвет, которым написаны символы</param>
        /// <returns>Массив изображений, содержащий все символы</returns>
        public static Bitmap[] CutImageIntoPieces(Bitmap image, int numberOfPixelsInColumn = 1,
                                                  int numberOfPixelsInRow = 2, string colorName = "ff000000")
        {
            int[] distanceBetweenColumn = new int[image.Width];
            var numberOfColumnWithBlackPixelsInColumn = 0;

            for (int i = 0; i < image.Width; i++)
            {
                var numberOfBlackPixelsInColumn = 0;
                for (int j = 0; j < image.Height; j++)
                {
                    if (image.GetPixel(i, j).Name == colorName)
                        numberOfBlackPixelsInColumn++;
                }
                if (numberOfBlackPixelsInColumn > numberOfPixelsInColumn) continue;

                distanceBetweenColumn[numberOfColumnWithBlackPixelsInColumn] = i;
                numberOfColumnWithBlackPixelsInColumn++;
            }

            //Логика такая у coodinatesAndWidthOfBlackColumn - в столбце 0 всегда находится "ширина" участка, содержащего больше чем numberOfPixelsInRow,
            //а в столбце 1 находится координата начала участка
            //т.е. |"ширина" участка|координата начала участка|

            int[,] coodinatesAndWidthOfBlackColumn = new int[11, 2];
            var numberOfSymbols = 0;
            for (int i = 0; i < numberOfColumnWithBlackPixelsInColumn; i++)
            {
                if (i + 1 < numberOfColumnWithBlackPixelsInColumn &&
                    distanceBetweenColumn[i + 1] - distanceBetweenColumn[i] > numberOfPixelsInRow)
                {
                    coodinatesAndWidthOfBlackColumn[numberOfSymbols, 0] = distanceBetweenColumn[i + 1] -
                                                                          distanceBetweenColumn[i];
                    coodinatesAndWidthOfBlackColumn[numberOfSymbols, 1] = distanceBetweenColumn[i];
                    numberOfSymbols++;
                }
            }

            Bitmap[] cuttedImages = new Bitmap[numberOfSymbols];

            for (int i = 0; i < numberOfSymbols; i++)
            {
                cuttedImages[i] = new Bitmap(coodinatesAndWidthOfBlackColumn[i, 0] + 1, image.Height);
            }


            for (int i = 0; i < numberOfSymbols; i++)
            {
                var numberOfRow = 0;
                for (int j = coodinatesAndWidthOfBlackColumn[i, 1];
                     j <= coodinatesAndWidthOfBlackColumn[i, 0] + coodinatesAndWidthOfBlackColumn[i, 1];
                     j++)
                {
                    for (int k = 0; k < image.Height; k++)
                    {
                        cuttedImages[i].SetPixel(numberOfRow, k, image.GetPixel(j, k));
                    }
                    numberOfRow++;
                }
            }
            return cuttedImages;
        }

        #endregion

        #region Изменение размера изображания

        /// <summary>
        /// Подгоняет изображение под заданный размер обрезанием.
        /// </summary>
        /// <param name="sourceBmp">Картинка, которую нужно переделать.</param>
        /// <param name="width">Нужная ширина</param>
        /// <param name="height">Нужная высота</param>
        /// <returns>Изображение нужного размера</returns>
        public static Bitmap ResizeBitmap(Bitmap sourceBmp, int width, int height)
        {
            if (sourceBmp.Width != width || sourceBmp.Height != height)
            {
                var result = new Bitmap(width, height);

                if (sourceBmp.Width < width && sourceBmp.Height <= height)
                {
                    for (int i = 0; i < result.Width; i++)
                    {
                        for (int j = 0; j < sourceBmp.Height; j++)
                        {
                            result.SetPixel(i, j, sourceBmp.Width > i ? sourceBmp.GetPixel(i, j) : Color.White);
                        }
                    }
                }

                if (sourceBmp.Height < height && sourceBmp.Width <= width)
                {
                    for (int i = 0; i < result.Height; i++)
                    {
                        for (int j = 0; j < sourceBmp.Width; j++)
                        {
                            result.SetPixel(j, i, sourceBmp.Height > i ? sourceBmp.GetPixel(j, i) : Color.White);
                        }
                    }
                }

                if (sourceBmp.Width > width && sourceBmp.Height <= height)
                {
                    for (int i = 0; i < result.Width; i++)
                    {
                        for (int j = 0; j < sourceBmp.Height; j++)
                        {
                            result.SetPixel(i, j, sourceBmp.GetPixel(i, j));
                        }
                    }
                }

                if (sourceBmp.Height > height && sourceBmp.Width <= width)
                {
                    for (int i = 1; i < result.Height + 1; i++)
                    {
                        for (int j = 0; j < sourceBmp.Width; j++)
                        {
                            result.SetPixel(j, i - 1, sourceBmp.GetPixel(j, i));

                        }
                    }
                }

                return result;
            }
            return sourceBmp;
        }

        #endregion

        #region Функция для сохранения весов

        /// <summary>
        /// Сохраняет веса
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="name">Название файла</param>
        /// <param name="bmp">Картинка, из которой делаются веса</param>
        public static void SaveBin(String path, String name, Bitmap bmp)
        {

            var w = bmp.Width;
            var h = bmp.Height;
            var n = w * h;

            var mas = new String[n];

            for (int j = 0, k = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    var val = 0.3 * bmp.GetPixel(i, j).R + 0.59 * bmp.GetPixel(i, j).G + 0.11 * bmp.GetPixel(i, j).B;

                    if (val > 127)
                    {
                        mas[k++] = "-0,5";
                    }
                    else
                    {
                        mas[k++] = "0,5";
                    }
                }
            }

            File.WriteAllLines(path + "\\" + name + ".in.txt", mas);
        }

        #endregion

        #region Распознаёт символ

        /// <summary>
        /// Распознаёт символ с помощью нейронной сети
        /// </summary>
        /// <param name="path">Путь до весов сети</param>
        /// <param name="net">Нейронная сеть</param>
        /// <returns>Рагзаданные символы</returns>
        public static string Recognize(string path, NeuralNW net)
        {
            if (!File.Exists(path))
                return null;

            var x = new double[net.GetX];
            double[] y;

            string[] currFile = File.ReadAllLines(path);

            for (int i = 0; i < net.GetX; i++)
            {
                x[i] = Convert.ToDouble(currFile[i]);
            }

            net.NetOUT(x, out y);
            var numb = Array.IndexOf(y, y.Max());

            switch (numb)
            {
                case 0: return "0";
                case 1: return "1";
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "7";
                case 8: return "8";
                case 9: return "9";
                case 10: return "a";
                case 11: return "b";
                case 12: return "c";
                case 13: return "d";
                case 14: return "e";
                case 15: return "f";

                default:
                    throw new ArgumentException();
            }

        }

        #endregion
    }
}
