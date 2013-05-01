using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AForge.Imaging.Filters;

namespace RecognizerPictures
{
    /// <summary>
    /// Класс отвечает за распознание символов "0 1 2 3 4 5 6 7 8 9 a b c d e f" для переданной картинки
    /// Класс работает с картиной, размер которой = 64 на 14
    /// </summary>
    public class IntellectBoard22AntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }
        private const int requiredWidth = 64;
        private const int requiredHeight = 14;
        private const int charactersNumber = 8;

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }

        //http://www.navigator61.ru/user/register
        //http://birja-mo.ru/script/si/securimage_show.php?0.6290400929924447
        //http://www.freelancejob.ru/js/captcha/captcha.php
        //http://freelance-tomsk.ru/cms_input/captcha.php?session=178
        //https://www.qiwi.ru/register/captcha.action?random=0.058002821041386765

        #region Обрезаем белые полосы по краям  изображения

        public Bitmap deleteWhiteStripes(Bitmap image)
        {
            int newWidth = 0;
            int newHeight = 0;
            List<int> xCoordinatesForCopy = new List<int>();
            List<int> yCoordinatesForCopy = new List<int>();

            for (int i = 0; i < image.Width; i++)
            {
                bool canDeleteRow = true;

                for (int j = 0; j < image.Height - 1; j++)
                {
                    if (image.GetPixel(i, j).GetHashCode() != image.GetPixel(i, j + 1).GetHashCode())
                    {
                        canDeleteRow = false;
                        break;
                    }
                }

                if (!canDeleteRow)
                {
                    newWidth++;
                    xCoordinatesForCopy.Add(i);
                }
            }

            Bitmap newImage = new Bitmap(newWidth, image.Height);
            int k = 0;
            foreach (int x in xCoordinatesForCopy)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color color = image.GetPixel(x, j);
                    newImage.SetPixel(k, j, color);
                }
                k++;
            }

            for (int i = 0; i < newImage.Height; i++)
            {
                bool canDeleteRow = true;
                for (int j = 0; j < newImage.Width - 1; j++)
                {
                    if (newImage.GetPixel(j, i) != newImage.GetPixel(j + 1, i))
                    {
                        canDeleteRow = false;
                        break;
                    }
                }

                if (!canDeleteRow)
                {
                    newHeight++;
                    yCoordinatesForCopy.Add(i);
                }
            }

            Bitmap resultImage = new Bitmap(newWidth, newHeight);
            k = 0;
            foreach (var y in yCoordinatesForCopy)
            {
                for (int j = 0; j < newImage.Width; j++)
                {
                    Color c = newImage.GetPixel(j, y);
                    resultImage.SetPixel(j, k, c);
                }
                k++;
            }

            Bitmap newImageWithRequiredSize = new Bitmap(requiredWidth, requiredHeight);
            Graphics.FromImage(newImageWithRequiredSize).DrawImage(resultImage, 0, 0, requiredWidth, requiredHeight);

            return newImageWithRequiredSize;
        }

        #endregion

        #region Биномиризация изображения

        public Bitmap binarizationImage(Bitmap image)
        {
            Grayscale filterGrayscale = Grayscale.CommonAlgorithms.BT709;
            //Grayscale filterGrayscale = new Grayscale(0.5, 0.419, 0.081); // R-Y
            Bitmap grayImage = filterGrayscale.Apply(image);
            Threshold filter = new Threshold(200);
            filter.ApplyInPlace(grayImage);
            return grayImage;
        }

        #endregion

        #region разрезание капчи на отдельные буквы

        public List<Bitmap> splitImageIntoChars(Bitmap image)
        {
            List<Bitmap> chars = new List<Bitmap>(charactersNumber);
            int width = (int) requiredWidth/charactersNumber;

            int shift = 0;
            for (int d = 0; d < width; d++)
            {
                Bitmap charImage = new Bitmap(width, requiredHeight);
                int k = 0;
                for (int i = shift; i < shift + width; i++)
                {
                    for (int j = 0; j < requiredHeight; j++)
                    {
                        Color color = image.GetPixel(i, j);
                        charImage.SetPixel(k, j, color);
                    }
                    k++;
                }
                chars.Add(charImage);
                shift += width;
            }

            return chars;
        }

        #endregion
        
    }
}
