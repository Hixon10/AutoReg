using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecognizerPictures
{
    public class phpBBAntiCaptcha: IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }

        private static int GlobalNumberSymbolsInImage = 0;
        private Bitmap _myBitMap;
        private List<string> ListWithAllShadesGray;


        #region Конпка, для проекта, в котором происходит отладка

        private void button1_Click(object sender, EventArgs e)
        {
            _myBitMap = new Bitmap("img121.jpg");

            /*
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.ClientSize = new Size(320, 50);
            pictureBox1.Image = (Image)_myBitMap;
            */

            ListWithAllShadesGray = GetAllShadesGray(_myBitMap); // function
            var clearEnumerableWithAllShadesGray = ListWithAllShadesGray.Distinct();
            var clearListFromEnumerableListWithAllShadesGray = clearEnumerableWithAllShadesGray.ToList();

            _myBitMap = Filter(clearListFromEnumerableListWithAllShadesGray, _myBitMap); // function

            /*
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.ClientSize = new Size(320, 50);
            pictureBox2.Image = (Image)_myBitMap;
            */
            _myBitMap.Save(Environment.CurrentDirectory + @"/ds.jpg");

            Bitmap[] test = CutImageIntoPieces(_myBitMap);

            //List<PictureBox> PB = this.Controls.OfType<PictureBox>().ToList();

            int index = 0;
            int step = GlobalNumberSymbolsInImage;
            Bitmap clearBt;
            Bitmap cc;
            for (int i = 0; i < GlobalNumberSymbolsInImage; i++)
            {

                cc = new Bitmap(50, 50);
                for (int v = 0; v < cc.Width; v++)
                {
                    for (int g = 0; g < cc.Height; g++)
                    {
                        cc.SetPixel(v, g, Color.BlueViolet);
                    }
                }
                clearBt = deleteWhiteStripesInHeight(test[index]);
                for (int v = 0; v < clearBt.Width; v++)
                {
                    for (int g = 0; g < clearBt.Height; g++)
                    {
                        cc.SetPixel(v, g, clearBt.GetPixel(v, g));
                    }
                }

                //Bitmap s = new Bitmap((Image)clearBt);
                //Bitmap v = s.Clone(new Rectangle(0, 0, 20, s.Height), s.PixelFormat);
                //PB[step].SizeMode = PictureBoxSizeMode.StretchImage;
                //PB[step].ClientSize = new Size(clearBt.Width, clearBt.Height);
                
                //PB[step].ClientSize = new Size(cc.Width, cc.Height);
                
                //cc = clearBt.Clone(new Rectangle(0, 0, clearBt.Width, clearBt.Height), clearBt.PixelFormat);
                
                //PB[step].Image = (Image)cc;

                //MessageBox.Show(test[index].Width.ToString());

                cc.Save(Environment.CurrentDirectory + @"/result" + index + ".jpg");
                //PB[step].ClientSize = new Size(v.Width, v.Height);
                //PB[step].Image = (Image)v;
                //MessageBox.Show(test[index].Height.ToString());
                //v.Save(Environment.CurrentDirectory + @"/result" + index + ".jpg");
                index++;
                step--;
            }

        }

        #endregion


        #region Убирает лишнее пространство с разрезаных картинок

        private Bitmap deleteWhiteStripesInHeight(Bitmap image)
        {
            int newHeight = 0;
            var yCoordinatesForCopy = new List<int>();
            int k = 0;
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


        #region Вовин метод

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

            //moya peremenaya dlya otladki(Artem)
            GlobalNumberSymbolsInImage = numberOfSymbols;
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
