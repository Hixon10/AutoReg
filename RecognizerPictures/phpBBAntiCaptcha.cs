using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ClassLibraryNeuralNetworks;

namespace RecognizerPictures
{
    public class phpBBAntiCaptcha: IAntiCaptcha
    {
        #region Основные переменые

        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }
        private NeuralNW _net;
        private static int _globalNumberSymbolsInImage = 0;
        private Bitmap _myBitMap;
        private List<string> _listWithAllShadesGray;
        private string _textFromCapcha = string.Empty;

        #endregion

        #region Конструктор класса phpBBAntiCaptcha, инициализирует НС

        public phpBBAntiCaptcha()
        {
            _net = new NeuralNW(Directory.GetCurrentDirectory() + "\\..\\..\\..\\RecognizerPictures\\nwFiles\\phpbb.nw");
        }

        #endregion

        #region Распознавание картинки

        /// <summary>
        /// Распознавание картинки
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns></returns>

        public String recognizeImage(Bitmap image)
        {

            /*TUT NUZHNUY HTML
            richTextBox2.Clear();
            richTextBox2.AppendText(htmlCode);*/
            _myBitMap = new Bitmap(image);


            _listWithAllShadesGray = GetAllShadesGray(_myBitMap); // function
            var clearEnumerableWithAllShadesGray = _listWithAllShadesGray.Distinct();
            var clearListFromEnumerableListWithAllShadesGray = clearEnumerableWithAllShadesGray.ToList();

            _myBitMap = Filter(clearListFromEnumerableListWithAllShadesGray, _myBitMap); // function


            Bitmap[] test = CutImageIntoPieces(_myBitMap);

            //List<PictureBox> pb = Controls.OfType<PictureBox>().ToList();

            int index = 0;
            int step = _globalNumberSymbolsInImage;
            Bitmap clearBt;
            Bitmap newBitmapForNewImage40Na40;
            _textFromCapcha = "";
            for (int i = 0; i < test.Count(); i++)
            {

                newBitmapForNewImage40Na40 = createNewBitmap40na40(new Bitmap(40, 40));
                clearBt = deleteWhiteStripesInHeight(test[i]);
                newBitmapForNewImage40Na40 = putBlackPixelOnNewBitmap(newBitmapForNewImage40Na40, clearBt);
                var countBlackPixelInOneSmolSquare = new int[4];
                newBitmapForNewImage40Na40 = createSaturatedBitmap(newBitmapForNewImage40Na40, clearBt,
                                                                   countBlackPixelInOneSmolSquare);
                //newBitmapForNewImage40Na40.Save(Environment.CurrentDirectory  + index.ToString() + ".bmp");
                //SaveBin(Environment.CurrentDirectory + "\\temp\\", index.ToString(), newBitmapForNewImage40Na40); // делает файл с весами
                //_textFromCapcha += GetCharFromImage(Environment.CurrentDirectory + "\\temp\\new\\" + index.ToString() + ".in.txt");
                _textFromCapcha +=
                    GetCharFromImage(SaveBin(Environment.CurrentDirectory + "\\temp\\", index.ToString(),
                                             newBitmapForNewImage40Na40));

                /*newBitmapForNewImage40Na40.Save(Environment.CurrentDirectory + index.ToString() + ".bmp");
                SaveBin(Environment.CurrentDirectory, index.ToString(), newBitmapForNewImage40Na40); // делает файл с весами
                _textFromCapcha += GetCharFromImage(Environment.CurrentDirectory + index.ToString() + ".in.txt");
                */
                index++;
                step--;
            }
            //MessageBox.Show(_textFromCapcha);
            return _textFromCapcha;
        }

        #endregion

        #region Создает новый битмап 40 на 40 пикселей

        /// <summary>
        /// Создает новый битмап 40 на 40 пикселей
        /// </summary>
        /// <param name="btmp">Изображение</param>
        /// <returns></returns>

        private Bitmap createNewBitmap40na40(Bitmap btmp)
        {
            for (int v = 0; v < btmp.Width; v++)
            {
                for (int g = 0; g < btmp.Height; g++)
                {
                    btmp.SetPixel(v, g, Color.White);
                }
            }
            return btmp;
        }

        #endregion

        #region Заполняет битмап 40 на 40 пикселей пикселями с картинки

        /// <summary>
        /// Заполняет битмап 40 на 40 пикселей пикселями с картинки
        /// </summary>
        /// <param name="btmp">Новое изображение</param>
        /// <param name="wBitmap">Текущее изображение</param>
        /// <returns></returns>

        private Bitmap putBlackPixelOnNewBitmap(Bitmap btmp, Bitmap wBitmap)
        {
            for (int v = 0; v < wBitmap.Width; v++)
            {
                for (int g = 0; g < wBitmap.Height; g++)
                {
                    btmp.SetPixel(v, g, wBitmap.GetPixel(v, g));
                }
            }
            return btmp;
        }

        #endregion

        #region Создает насущенную картинку для каждого символа с капчи

        /// <summary>
        /// Создает насущенную картинку для каждого символа с капчи
        /// </summary>
        /// <param name="btmp">Новое изображение</param>
        /// <param name="wBitmap">Текущее изображение</param>
        /// <param name="countBlackPixelInOneSmolSquare">Массив для подсчета количества черных пикселей</param>
        /// <returns></returns>

        private Bitmap createSaturatedBitmap(Bitmap btmp, Bitmap wBitmap, int[] countBlackPixelInOneSmolSquare)
        {
            for (int v = 0; v < wBitmap.Width - 1; v++)
            {
                for (int g = 0; g < wBitmap.Height - 1; g++)
                {
                    if (btmp.GetPixel(v, g).Name == "ff000000")
                    {
                        countBlackPixelInOneSmolSquare[0] = 1;
                    }
                    else
                    {
                        countBlackPixelInOneSmolSquare[0] = 0;
                    }
                    if (btmp.GetPixel(v + 1, g).Name == "ff000000")
                    {
                        countBlackPixelInOneSmolSquare[1] = 1;
                    }
                    else
                    {
                        countBlackPixelInOneSmolSquare[1] = 0;
                    }
                    if (btmp.GetPixel(v, g + 1).Name == "ff000000")
                    {
                        countBlackPixelInOneSmolSquare[2] = 1;
                    }
                    else
                    {
                        countBlackPixelInOneSmolSquare[2] = 0;
                    }
                    if (btmp.GetPixel(v + 1, g + 1).Name == "ff000000")
                    {
                        countBlackPixelInOneSmolSquare[3] = 1;
                    }
                    else
                    {
                        countBlackPixelInOneSmolSquare[3] = 0;
                    }
                    if (countBlackPixelInOneSmolSquare.Sum() >= 1)
                    {
                        btmp.SetPixel(v, g, Color.Black);
                    }
                }
            }
            return btmp;
        }

        #endregion

        #region Убирает лишнее пространство с разрезаных картинок

        /// <summary>
        /// Убирает лишнее пространство с разрезаных картинок
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns></returns>

        private Bitmap deleteWhiteStripesInHeight(Bitmap image)
        {
            int newHeight = 0;
            var yCoordinatesForCopy = new List<int>();
            int k;
            for (int i = 0; i < image.Height; i++)
            {
                bool canDeleteRow = true;
                for (int j = 0; j < image.Width - 1; j++)
                {
                    if (image.GetPixel(j, i) != image.GetPixel(j + 1, i))
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
            //MessageBox.Show(newHeight.ToString());
            var resultImage = new Bitmap(image.Width, newHeight);
            k = 0;
            foreach (var y in yCoordinatesForCopy)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, y);
                    resultImage.SetPixel(j, k, c);
                }
                k++;
            }

            return resultImage;
        }

        #endregion

        #region Разрезает капчу на отдельные картинки с цифрами

        /// <summary>
        /// Разрезает капчу на отдельные картинки с цифрами
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="numberOfPixelsInColumn">Количество пикселей в столбце, которое учитывает столбец, как подходящий</param>
        /// <param name="numberOfPixelsInRow">Количество пикселей, в которых располагается символ</param>
        /// <param name="colorName">Цвет, которым написаны символы</param>
        /// <returns>Массив изображений, содержащий все символы</returns>
        public static Bitmap[] CutImageIntoPieces(Bitmap image, int numberOfPixelsInColumn = 1,
                                                  int numberOfPixelsInRow = 2, string colorName = "ff000000")
        {
            var distanceBetweenColumn = new int[image.Width];
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

            var coodinatesAndWidthOfBlackColumn = new int[11, 2];
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

            //moya peremenaya dlya otladki(Artem)
            _globalNumberSymbolsInImage = numberOfSymbols;
            var cuttedImages = new Bitmap[numberOfSymbols];

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

        #region Лист со всеми оттенками серого

        /// <summary>
        /// Лист со всеми оттенками серого
        /// </summary>
        /// <param name="bm">Изображение</param>
        /// <returns></returns>

        private List<string> GetAllShadesGray(Bitmap bm)
        {
            var list = new List<string>();
            Bitmap bmm = bm;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color cl = bmm.GetPixel(i, j);
                    list.Add(cl.Name);
                }
            }

            for (int i = _myBitMap.Width - 10; i < _myBitMap.Width; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color cl = _myBitMap.GetPixel(i, j);
                    list.Add(cl.Name);
                }
            }
            return list;
        }

        #endregion

        #region Преобразует картинку в цифры 0.5 и -0.5, в специальный вид, удобный для сравнения со значением, которое находится в каждом пикселе картинки

        /// <summary>
        /// Преобразует картинку в цифры 0.5 и -0.5, в специальный вид, удобный для сравнения со значением, которое находится в каждом пикселе картинки
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="bmp">Изображение</param>
        /// <returns></returns>

        private static string[] SaveBin(String path, String name, Bitmap bmp)
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

            //File.WriteAllLines(path + "\\" + name + ".in.txt", mas);
            //File.Copy(path + "\\" + name + ".in.txt", path + "\\"+"new"+"\\" + name + ".in.txt");
            return mas;
        }

        #endregion

        #region Возвращает символ с полученной картинки

        /// <summary>
        /// Возвращает символ с полученной картинки
        /// </summary>
        /// <param name="mas">Массив весов</param>
        /// <returns></returns>

        private string GetCharFromImage(string[] mas)
        {
            /*if (!File.Exists(path))
                return null;*/

            var x = new double[_net.GetX];
            double[] y;

            string[] currFile = mas;//File.ReadAllLines(path);

            for (int i = 0; i < _net.GetX; i++)
            {
                x[i] = Convert.ToDouble(currFile[i]);
            }

            _net.NetOUT(x, out y);
            var numb = Array.IndexOf(y, y.Max());
            switch (numb)
            {
                case 0:
                    return "1";
                case 1:
                    return "L";
                case 2:
                    return "K";
                case 3:
                    return "M";
                case 4:
                    return "J";
                case 5:
                    return "N";
                case 6:
                    return "I";
                case 7:
                    return "P";
                case 8:
                    return "H";
                case 9:
                    return "Q";
                case 10:
                    return "G";
                case 11:
                    return "R";
                case 12:
                    return "F";
                case 13:
                    return "S";
                case 14:
                    return "E";
                case 15:
                    return "T";
                case 16:
                    return "D";
                case 17:
                    return "U";
                case 18:
                    return "C";
                case 19:
                    return "V";
                case 20:
                    return "B";
                case 21:
                    return "W";
                case 22:
                    return "A";
                case 23:
                    return "X";
                case 24:
                    return "9";
                case 25:
                    return "Y";
                case 26:
                    return "8";
                case 27:
                    return "Z";
                case 28:
                    return "7";
                case 29:
                    return "6";
                case 30:
                    return "5";
                case 31:
                    return "4";
                case 32:
                    return "3";
                case 33:
                    return "2";
                default:
                    return "ERROR";
            }
        }

        #endregion

        #region Превращает картинку в черно-белую

        /// <summary>
        /// Превращает картинку в черно-белую
        /// </summary>
        /// <param name="list">Лист</param>
        /// <param name="bm">Изображение</param>
        /// <returns></returns>

        private Bitmap Filter(List<string> list, Bitmap bm)
        {
            Bitmap bmm = bm;
            for (int i = 0; i < bmm.Width; i++)
            {
                for (int j = 0; j < bmm.Height; j++)
                {
                    Color cl = bmm.GetPixel(i, j);
                    if (colr(list, cl.Name))
                    {
                        bmm.SetPixel(i, j, Color.White);
                    }
                }
            }
            for (int i = 0; i < bmm.Width; i++)
            {
                for (int j = 0; j < bmm.Height; j++)
                {
                    Color cl = bmm.GetPixel(i, j);
                    if (String.Compare(cl.Name.ToString(), "ffffffff") != 0)
                    {
                        bmm.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return bmm;
        }

        #endregion

        #region Помогает закрасить картинку в черно-белый цвет

        /// <summary>
        /// Помогает закрасить картинку в черно-белый цвет
        /// </summary>
        /// <param name="li">Лист</param>
        /// <param name="name">Имя</param>
        /// <returns></returns>

        private bool colr(List<string> li, string name)
        {
            int f = 0;

            for (int i = 0; i < li.Count; i++)
            {
                if (String.Compare(li[i], name) == 0)
                {
                    f = 1;
                    i = li.Count + 1;
                }
                else
                {
                    f = 0;
                }
            }
            if (f == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
