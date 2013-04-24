using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecognizerPictures
{
    public class IntellectBoard20AntiCaptcha : IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }

        #region Делает изображение чёрно-белым

        /// <summary>
        /// Делает изображение чёрно-белым, ориентируясь на "небелые" пиксели
        /// </summary>
        /// <param name="img">Изображение</param>
        /// <returns></returns>
        public static Bitmap MakeBlackAndWhitePicture(Bitmap img)
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color cl = img.GetPixel(i, j);

                    if (cl.Name != "ffffffff")
                        img.SetPixel(i, j, Color.Black);
                }
            }

            return img;
        }

        #endregion

        #region Удаляет пискельный шум

        /// <summary>
        /// Удаляет пискельный шум, ориентируется на количество пикселей вокруг
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="pixelAround">Количество пикселей вокруг</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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

    }
}
