using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using ClassLibraryNeuralNetworks;

namespace RecognizerPictures
{
    public class phpBBAntiCaptcha: IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }
        private NeuralNW _net;
        private static int _globalNumberSymbolsInImage = 0;
        private Bitmap _myBitMap;
        private List<string> _listWithAllShadesGray;

        public phpBBAntiCaptcha()
        {
            _net = new NeuralNW(Directory.GetCurrentDirectory() + "\\..\\..\\..\\RecognizerPictures\\nwFiles\\phpbb.nw");
        }

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }

        #region Конпка, для проекта, в котором происходит отладка

        private void button1_Click(object sender, EventArgs e)
        {
            //for (int t = 1; t < 128; t++)
            //{

            using (var client = new WebClient())
            {
                client.DownloadFile(
                    "http://localhost/forum/www/ucp.php?mode=confirm&confirm_id=a95b95b4d425f334f1a4e502036ea75c&type=1&sid=4b3e600d9335c450c146aed5bcf622b8",
                    "asde.bmp");
            }


            _myBitMap = new Bitmap("asde.bmp");
            //_myBitMap = new Bitmap("img121.jpg");
            //_myBitMap = new Bitmap("img" + t.ToString() + ".jpg");

            /*
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.ClientSize = new Size(320, 50);
            pictureBox1.Image = _myBitMap;
            */

            _listWithAllShadesGray = GetAllShadesGray(_myBitMap); // function
            var clearEnumerableWithAllShadesGray = _listWithAllShadesGray.Distinct();
            var clearListFromEnumerableListWithAllShadesGray = clearEnumerableWithAllShadesGray.ToList();

            _myBitMap = Filter(clearListFromEnumerableListWithAllShadesGray, _myBitMap); // function

            /*
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.ClientSize = new Size(320, 50);
            pictureBox2.Image = _myBitMap;
            */
            
            _myBitMap.Save(Environment.CurrentDirectory + @"/ds.jpg");
            Bitmap[] test = CutImageIntoPieces(_myBitMap);

            //List<PictureBox> pb = Controls.OfType<PictureBox>().ToList();

            int index = 0;
            int step = _globalNumberSymbolsInImage;
            Bitmap clearBt;
            Bitmap newBitmapForNewImage40Na40;
            for (int i = 0; i < _globalNumberSymbolsInImage; i++)
            {

                newBitmapForNewImage40Na40 = new Bitmap(40, 40);
                for (int v = 0; v < newBitmapForNewImage40Na40.Width; v++)
                {
                    for (int g = 0; g < newBitmapForNewImage40Na40.Height; g++)
                    {
                        newBitmapForNewImage40Na40.SetPixel(v, g, Color.White);
                    }
                }
                clearBt = deleteWhiteStripesInHeight(test[index]);
                for (int v = 0; v < clearBt.Width; v++)
                {
                    for (int g = 0; g < clearBt.Height; g++)
                    {
                        newBitmapForNewImage40Na40.SetPixel(v, g, clearBt.GetPixel(v, g));
                    }
                }
                //pb[step].ClientSize = new Size(newBitmapForNewImage40Na40.Width, newBitmapForNewImage40Na40.Height);
                //pb[step].Image = newBitmapForNewImage40Na40;
                var countBlackPixelInOneSmolSquare = new int[4];
                for (int v = 0; v < clearBt.Width - 1; v++)
                {
                    for (int g = 0; g < clearBt.Height - 1; g++)
                    {
                        if (newBitmapForNewImage40Na40.GetPixel(v, g).Name == "ff000000")
                        {
                            countBlackPixelInOneSmolSquare[0] = 1;
                        }
                        else
                        {
                            countBlackPixelInOneSmolSquare[0] = 0;
                        }
                        if (newBitmapForNewImage40Na40.GetPixel(v + 1, g).Name == "ff000000")
                        {
                            countBlackPixelInOneSmolSquare[1] = 1;
                        }
                        else
                        {
                            countBlackPixelInOneSmolSquare[1] = 0;
                        }
                        if (newBitmapForNewImage40Na40.GetPixel(v, g + 1).Name == "ff000000")
                        {
                            countBlackPixelInOneSmolSquare[2] = 1;
                        }
                        else
                        {
                            countBlackPixelInOneSmolSquare[2] = 0;
                        }
                        if (newBitmapForNewImage40Na40.GetPixel(v + 1, g + 1).Name == "ff000000")
                        {
                            countBlackPixelInOneSmolSquare[3] = 1;
                        }
                        else
                        {
                            countBlackPixelInOneSmolSquare[3] = 0;
                        }
                        if (countBlackPixelInOneSmolSquare.Sum() >= 1)
                        {
                            newBitmapForNewImage40Na40.SetPixel(v, g, Color.Black);
                        }
                    }
                }
                newBitmapForNewImage40Na40.Save(Environment.CurrentDirectory + "\\2progona\\" + index.ToString() + "AAA.bmp");
                SaveBin(Environment.CurrentDirectory + "\\temp\\", index.ToString(), newBitmapForNewImage40Na40); // делает файл с весами
                //*****************
                //richTextBox1.AppendText(GetCharFromImage(Environment.CurrentDirectory + "\\temp\\" + index.ToString() + ".in.txt")); // распознаёт файл с весами
                //*****************
                index++;
                step--;
            }

            //------------------------------------------------------------
            //Вызов функций, происходит в цикле:
            //------------------------------------------------------------
            //for (int i = 0; i < symbols.Length; i++)
            //{
            //  newsymbols[i] = ResizeBitmap(DeleteLinesInColumn(DeleteLinesInRow(symbols[i], 1), 1), 8, 10); //подгоняет под размер
            //newsymbols[i].Save(Environment.CurrentDirectory + "\\temp\\" + nameoffile + string.Format("{0:00}", i) + ".bmp"); // сохраняет файл  с символов

            //}




            //}
            //MessageBox.Show("The end");
            /*
             * 
             * sss = richTextBox1.Text.Trim();
                // Формируем строку с параметрами
            string secondStepForm = "username=Trishasha" +
                                "&email=Trisha@gmail.com" +
                                "&email_confirm=Trisha@gmail.com" +
                                "&new_password=Trishacom" +
                                "&password_confirm=Trishacom" +
                                "&confirm_code=" + (sss);
            //refresh_vc
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost/forum/www/ucp.php?mode=register");

                // Настраиваем параметры запроса
                request.UserAgent = "Mozilla/5.0";
                request.Method = "POST";

                // Указываем метод отправки данных скрипту
                request.AllowAutoRedirect = true;
                request.Referer = "http://localhost/forum/www/index.php";
                //request.CookieContainer = cookieCont;

                // Указываем тип отправляемых данных
                request.ContentType = "application/x-www-form-urlencoded";

                // Преобразуем данные к соответствующую кодировку
                byte[] EncodedPostParams = Encoding.ASCII.GetBytes(secondStepForm);
                request.ContentLength = EncodedPostParams.Length;

                // Записываем данные в поток
                request.GetRequestStream().Write(EncodedPostParams,
                                                 0,
                                                 EncodedPostParams.Length);

                request.GetRequestStream().Close();

                // Получаем ответ
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Получаем html-код страницы
                string html = new StreamReader(response.GetResponseStream(),
                                               Encoding.UTF8).ReadToEnd();

                richTextBox2.AppendText(html);

                //Regex re = new Regex("href=\"(.*?)\" id=\"lowres\"");
            string strr = "href=\"\\./index.php\\?sid=(.*?)\"";
            Regex re = new Regex(strr);
            Match match = re.Match(html);
            string url = match.Groups[1].Value; // Вот тут url из href
            MessageBox.Show(url);


            string secondStepForm1 = "sid=" + url.Trim() + 
                                "&username=Trishasha" +
                                "&email=Trisha@gmail.com" +
                                "&email_confirm=Trisha@gmail.com" +
                                "&new_password=Trishacom" +
                                "&password_confirm=Trishacom" +
                                "&confirm_code=" + (sss);
            //refresh_vc
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create("http://localhost/forum/www/ucp.php?mode=register");

            // Настраиваем параметры запроса
            request1.UserAgent = "Mozilla/5.0";
            request1.Method = "POST";

            // Указываем метод отправки данных скрипту
            request1.AllowAutoRedirect = true;
            request1.Referer = "http://localhost/forum/www/index.php";
            //request.CookieContainer = cookieCont;

            // Указываем тип отправляемых данных
            request1.ContentType = "application/x-www-form-urlencoded";

            // Преобразуем данные к соответствующую кодировку
            byte[] EncodedPostParams1 = Encoding.ASCII.GetBytes(secondStepForm1);
            request1.ContentLength = EncodedPostParams1.Length;

            // Записываем данные в поток
            request1.GetRequestStream().Write(EncodedPostParams1,
                                             0,
                                             EncodedPostParams1.Length);

            request1.GetRequestStream().Close();

            // Получаем ответ
            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();

            // Получаем html-код страницы
            string html1 = new StreamReader(response1.GetResponseStream(),
                                           Encoding.UTF8).ReadToEnd();


            richTextBox3.AppendText(html1);


            string strr1 = "src=\"(.*?)\"";
            Regex re1 = new Regex(strr1);
            Match match1 = re.Match(html1);
            string url1 = match1.Groups[1].Value; // Вот тут url из href
            MessageBox.Show(url1);
             * 
             */
            //}
        }

        #endregion

        #region Убирает лишнее пространство с разрезаных картинок

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

        private List<string> GetAllShadesGray(Bitmap bm)
        {
            var list = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color cl = bm.GetPixel(i, j);
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

        private static void SaveBin(String path, String name, Bitmap bmp)
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

        #region Возвращает символ с полученной картинки

        private string GetCharFromImage(string path)
        {
            if (!File.Exists(path))
                return null;

            var x = new double[_net.GetX];
            double[] y;

            string[] currFile = File.ReadAllLines(path);

            for (int i = 0; i < _net.GetX; i++)
            {
                x[i] = Convert.ToDouble(currFile[i]);
            }

            _net.NetOUT(x, out y);
            var numb = Array.IndexOf(y, y.Max());
            //richTextBox2.AppendText(numb.ToString() + "\n"); //КОММЕНТАРИЙ ДОБАВИЛ ДЕНИС - НЕ НАЙДЕН ЭЛЕМЕНТ richTextBox2
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

        private Bitmap Filter(List<string> list, Bitmap bm)
        {
            for (int i = 0; i < _myBitMap.Width; i++)
            {
                for (int j = 0; j < _myBitMap.Height; j++)
                {
                    Color cl = _myBitMap.GetPixel(i, j);
                    if (colr(list, cl.Name))
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                }
            }
            for (int i = 0; i < _myBitMap.Width; i++)
            {
                for (int j = 0; j < _myBitMap.Height; j++)
                {
                    Color cl = _myBitMap.GetPixel(i, j);
                    if (String.Compare(cl.Name.ToString(), "ffffffff") != 0)
                    {
                        bm.SetPixel(i, j, Color.Black);
                    }
                }
            }
            return bm;
        }

        #endregion

        #region помогает закрасить картинку в черно-белый цвет

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

        #region Тут остатки разного кода, на всякий случай

        private void SHLAK(Bitmap MBT)
        {
            /*
             * private Bitmap[] GetCutImage(Bitmap bt)
        {
            var numberSymbolsInImage = 0;
            int[] arrayWithIndexBlackPixelFromImage = new int[bt.Width];
            int indexBlackPixelOnImage = 0;

            for (int i = 0; i < bt.Width; i++)
            {
                var countBlackPixelOnImage = 0;

                for (int j = 0; j < bt.Height; j++)
                {
                    if (bt.GetPixel(i, j).Name == "ff000000")
                    {
                        countBlackPixelOnImage++;
                    }
                }
                if (countBlackPixelOnImage < 2)
                {
                    arrayWithIndexBlackPixelFromImage[indexBlackPixelOnImage] = i;
                    indexBlackPixelOnImage++;
                }
            }

            int[,] numberSymbolsInImageArr = new int[10, 2]; //BOLSHE 10 simvolov ne budet -> 10 i 2

            for (int i = 0; i < indexBlackPixelOnImage; i++)
            {
                if (i + 1 < indexBlackPixelOnImage && arrayWithIndexBlackPixelFromImage[i + 1] - arrayWithIndexBlackPixelFromImage[i] > 1)
                {
                    numberSymbolsInImageArr[numberSymbolsInImage, 0] = arrayWithIndexBlackPixelFromImage[i + 1] - arrayWithIndexBlackPixelFromImage[i];
                    numberSymbolsInImageArr[numberSymbolsInImage, 1] = arrayWithIndexBlackPixelFromImage[i];
                   
                    numberSymbolsInImage++;
                }
            }
            
            GlobalNumberSymbolsInImage = numberSymbolsInImage;
            var bitmapWithCutPictures = new Bitmap[numberSymbolsInImage];

            for (int i = 0; i < numberSymbolsInImage; i++)
            {
                bitmapWithCutPictures[i] = new Bitmap(numberSymbolsInImageArr[i, 0] + 1, bt.Height);
            }

            for (int i = 0; i < numberSymbolsInImage; i++)
            {
                var count = 0;
                for (int j = numberSymbolsInImageArr[i, 1]; j <= numberSymbolsInImageArr[i, 0] + numberSymbolsInImageArr[i, 1]; j++)
                {
                    for (int k = 0; k < bt.Height; k++)
                    {
                        bitmapWithCutPictures[i].SetPixel(count, k, bt.GetPixel(j, k));
                    }
                    count++;
                }
            }
            return bitmapWithCutPictures;
        }
             * 
             * 
             * 
             * 
             * Bitmap qw = MBT;
            for (int i = 0; i < qw.Width; i++)
            {
                for (int j = 0; j < qw.Height; j++)
                {
                    Color cl = MBT.GetPixel(i, j);

                    //if (String.Compare(cl.Name.ToString(), "d0d0d0") == 0 || String.Compare(cl.Name.ToString(), "9e9e9e") == 0 || String.Compare(cl.Name.ToString(), "949494") == 0 || String.Compare(cl.Name.ToString(), "a1a1a1") == 0)
                    //MessageBox.Show(cl.Name.ToString());
                    if (String.Compare(cl.Name.ToString(), "040404") != 0 && String.Compare(cl.Name.ToString(), "fafafa") != 0)
                    {
                        qw.SetPixel(i, j, Color.Red);
                    }
                }
            }
            

            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.ClientSize = new Size(320, 50);
            pictureBox2.Image = (Image)qw;*/

            /*Bitmap b1 = new Bitmap(h[0, 0]+1, MyBitMap.Height);
            Bitmap b2 = new Bitmap(h[1, 0] + 1, MyBitMap.Height);
            Bitmap b3 = new Bitmap(h[2, 0] + 1, MyBitMap.Height);
            Bitmap b4 = new Bitmap(h[3, 0] + 1, MyBitMap.Height);

            Bitmap[] bb = {b1,b2,b3,b4 };

            for (int i = 0; i < 4; i++)
            {
                var jjj = 0;
                for (int j = h[i, 1]; j <= h[i, 0] + h[i, 1]; j++)
                {
                    for (int k = 0; k < MyBitMap.Height; k++)
                    {
                        bb[i].SetPixel(jjj, k, bt.GetPixel(j, k));
                    }
                    jjj++;
                }
            }
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.ClientSize = new Size(NumberSymbolsInImageArr[0, 0] + 1, MyBitMap.Height);
            pictureBox3.Image = (Image)BitmapWithCutPictures[0];

            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.ClientSize = new Size(NumberSymbolsInImageArr[0, 0] + 1, MyBitMap.Height);
            pictureBox4.Image = (Image)BitmapWithCutPictures[1];

            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.ClientSize = new Size(NumberSymbolsInImageArr[0, 0] + 1, MyBitMap.Height);
            pictureBox5.Image = (Image)BitmapWithCutPictures[2];

            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.ClientSize = new Size(NumberSymbolsInImageArr[0, 0] + 1, MyBitMap.Height);
            pictureBox6.Image = (Image)BitmapWithCutPictures[3];*/
            /*for (int i = 0; i < MyBitMap.Width; i++)
            {
                for (int j = 0; j < MyBitMap.Height; j++)
                {
                    cl = bt.GetPixel(i, j);
                    if (String.Compare(cl.Name.ToString(), "ffffffff") != 0)
                    {
                        myblack = myblack + 1;
                        if (myblack == 2)
                        {
                            buf = i;
                        }
                    }
                    else
                    {
                        mywhite = mywhite + 1;
                    }
                }
                if (myblack >= 2 && mywhite < MyBitMap.Height)
                {
                    buf = i;
                    for (int r = 0; r < MyBitMap.Height; r++)
                    {
                        cl = bt.GetPixel(buf, r);
                        blwh[count, r] = cl.Name.ToString();
                    }
                    count = count + 1;
                    //MessageBox.Show(count.ToString());
                }
                else
                {
                    re = count;
                    
                    nbt = new Bitmap(100, MyBitMap.Height);
                    for (int ii = 0; ii < 30; ii++)
                    {
                        for (int j = 0; j < MyBitMap.Height; j++)
                        {
                            //richTextBox1.Text += blwh[i, j] + "\n";
                            if (String.Compare(blwh[ii, j], "ffffffff") != 0)
                            {
                                nbt.SetPixel(ii, j, Color.Black);
                            }
                            else
                            {
                                nbt.SetPixel(ii, j, Color.White);
                            }
                            //nbt.SetPixel(i, j, blwh[i, j]);
                        }
                    }
                    count = 0;
                }
                myblack = 0;
                mywhite = 0;


                


               *count = buf;
                for (int w = buf; w < count; w++)
                {

                }
            }*/

            /*
                MessageBox.Show(PB.Count.ToString());
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox3.ClientSize = new Size(test[0].Width, test[0].Height);
                pictureBox3.Image = (Image)test[0];

                pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox4.ClientSize = new Size(test[1].Width, test[1].Height);
                pictureBox4.Image = (Image)test[1];

                pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox5.ClientSize = new Size(test[2].Width, test[2].Height);
                pictureBox5.Image = (Image)test[2];

                pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox6.ClientSize = new Size(test[3].Width, test[3].Height);
                pictureBox6.Image = (Image)test[3];
            */

            /*pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.ClientSize = new Size(320, 50);
            pictureBox3.Image = (Image)NumberOne(MyBitMap);*/
            //GetClearImg(MyBitMap);
        }

        #endregion
    }
}
