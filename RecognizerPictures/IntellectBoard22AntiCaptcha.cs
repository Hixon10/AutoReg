﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecognizerPictures
{
    public class IntellectBoard22AntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }

        #region Обрезаем белые полосы по краям изображения

        private Bitmap deleteWhiteStripes(Bitmap image)
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

            return resultImage;
        }

        #endregion
    }
}
