using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
using AForge.Imaging.Filters;
using ClassLibraryNeuralNetworks;
using RecognizerPictures.Properties;

namespace RecognizerPictures
{
    /// <summary>
    /// Класс отвечает за распознание символов "0 1 2 3 4 5 6 7 8 9 a b c d e f" для переданной картинки
    /// Класс работает с картиной, размер которой = 64 на 14
    /// </summary>
    public class IntellectBoard22AntiCaptcha : IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }
        private const int requiredWidth = 64;
        private const int requiredHeight = 14;
        private const int charactersNumber = 8;
        private NeuralNW _net;

        public IntellectBoard22AntiCaptcha()
        {
            _net = new NeuralNW(Directory.GetCurrentDirectory() + "\\..\\..\\..\\RecognizerPictures\\nwFiles\\IntellectBoard22-4.nw");
        }
        
        public String recognizeImage(Bitmap image)
        {
            Bitmap imageWithoutWhiteStripes = deleteWhiteStripes(image);
            Bitmap blackAndWhiteImage = binarizationImage(imageWithoutWhiteStripes);
            List<Bitmap> symbols = splitImageIntoChars(blackAndWhiteImage);
            StringBuilder recognizedImage = new StringBuilder();
            String path = Environment.CurrentDirectory + "";
            String fileName = "weight";

            for (int i = 0; i < symbols.Count; i++)
            {
                String resultPath = path + fileName + string.Format("{0:00}",i) + ".bmp";
                symbols[i].Save(resultPath,ImageFormat.Bmp); // сохраняет файл  с символов
                SaveBin(path, fileName + string.Format("{0:00}", i), symbols[i]); // делает файл с весами
                recognizedImage.Append(Test(path + "\\" + fileName + string.Format("{0:00}", i) + ".in.txt")); // распознаёт файл с весами
            }
            return recognizedImage.ToString();
        }

        public Bitmap pressImageToFooter(Bitmap image)
        {
            int shift = 0;
            bool canShift = true;
            for (int i = image.Height - 1; i >= 0; i--)
            {
                if (!canShift)
                {
                    shift--;
                    break;
                }
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        canShift = false;
                        break;
                    }
                }
                shift++;
            }
            
            Bitmap resultImage = new Bitmap(image.Width, image.Height);
            int k = image.Height - 1;
            for (int i = image.Height - 1 - shift; i >= 0; i--)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);
                    resultImage.SetPixel(j,k,pixel);
                }
                k--;
            }

            for (int i = 0; i < shift; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    resultImage.SetPixel(j, i, Color.White);
                }
            }

            return resultImage;
        }

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
            return doBlackAndWhiteImage(grayImage);
        }

        private Bitmap doBlackAndWhiteImage(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    if (pixel.R < 23 && pixel.G < 23 && pixel.B < 23)
                    {
                        newImage.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        newImage.SetPixel(i,j,Color.White);
                    }
                }
            }
            return newImage;
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

        #region Работа с нейронной сетью

        private static void SaveBin(String path, String name, Bitmap bmp)
        {
            var w = bmp.Width;
            var h = bmp.Height;
            var n = w*h;

            var mas = new String[n];

            for (int j = 0, k = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    var val = 0.3*bmp.GetPixel(i, j).R + 0.59*bmp.GetPixel(i, j).G + 0.11*bmp.GetPixel(i, j).B;

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

            String resultPath = path + "\\" + name + ".in.txt";
            File.WriteAllLines(resultPath, mas);
        }

        private string Test(string path)
        {
            if (!File.Exists(path))
                return null;

            var x = new double[_net.GetX];
            double[] y;

            // Загружаем текущий входной файл
            string[] currFile = File.ReadAllLines(path);

            for (int i = 0; i < _net.GetX; i++)
            {
                x[i] = Convert.ToDouble(currFile[i]);
            }

            _net.NetOUT(x, out y);
            var numb = Array.IndexOf(y, y.Max());

            switch (numb)
            {
                case 0:
                    return "0";
                case 1:
                    return "1";
                case 2:
                    return "2";
                case 3:
                    return "3";
                case 4:
                    return "4";
                case 5:
                    return "5";
                case 6:
                    return "6";
                case 7:
                    return "7";
                case 8:
                    return "8";
                case 9:
                    return "9";
                case 10:
                    return "a";
                case 11:
                    return "b";
                case 12:
                    return "c";
                case 13:
                    return "d";
                case 14:
                    return "e";
                case 15:
                    return "f";
                default:
                    throw new ArgumentException();
            }
        }

        #endregion
    }
}
