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

        //------NIZHE GAVNOKOD IZ PROBNOGO PROEKTA-----

        private int _globalNumberSymbolsInImage = 0;
        private Bitmap _myBitMap;

        private void button1_Click(object sender, EventArgs e)
        {
            _myBitMap = new Bitmap("img43.jpg");

            /*pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.ClientSize = new Size(320, 50);
            pictureBox1.Image = (Image)_myBitMap;
            */
            List<string> ListWithAllShadesGray = new List<string>();

            ListWithAllShadesGray = GetAllShadesGray(_myBitMap);// function
            var clearEnumerableWithAllShadesGray = ListWithAllShadesGray.Distinct();
            var clearListFromEnumerableListWithAllShadesGray = clearEnumerableWithAllShadesGray.ToList();

            for (int i = 0; i < clearEnumerableWithAllShadesGray.Count(); i++)
            {
                //richTextBox1.Text += clearListFromEnumerableListWithAllShadesGray[i].ToString() + "\n";
            }

            _myBitMap = Filter(clearListFromEnumerableListWithAllShadesGray, _myBitMap);// function
            /*
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.ClientSize = new Size(320, 50);
            pictureBox2.Image = (Image)_myBitMap;*/
            _myBitMap.Save(Environment.CurrentDirectory + @"/ds.jpg");
            
            Bitmap[] test = GetCutImage(_myBitMap);
            //List<PictureBox> PB = new List<PictureBox>();
            /*
            foreach (Control c in this.Controls)
            {
                var current = c as PictureBox;

                if (current != null)
                    PB.Add(current);
            }*/

            int index = 0;
            int step = _globalNumberSymbolsInImage;

            for (int i = 0; i < _globalNumberSymbolsInImage; i++)
            {
                /*PB[step].SizeMode = PictureBoxSizeMode.StretchImage;
                PB[step].ClientSize = new Size(test[index].Width, test[index].Height);
                PB[step].Image = (Image)test[index];*/

                index++;
                step--;
            }

        }

        private Bitmap[] GetCutImage(Bitmap bt)
        {
            var numberSymbolsInImage = 0;
            int[] arrayWithIndexBlackPixelFromImage = new int[_myBitMap.Width];
            int indexBlackPixelOnImage = 0;

            for (int i = 0; i < _myBitMap.Width; i++)
            {
                var countBlackPixelOnImage = 0;

                for (int j = 0; j < _myBitMap.Height; j++)
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

            _globalNumberSymbolsInImage = numberSymbolsInImage;
            var bitmapWithCutPictures = new Bitmap[numberSymbolsInImage];

            for (int i = 0; i < numberSymbolsInImage; i++)
            {
                bitmapWithCutPictures[i] = new Bitmap(numberSymbolsInImageArr[i, 0] + 1, _myBitMap.Height);
            }

            for (int i = 0; i < numberSymbolsInImage; i++)
            {
                var count = 0;
                for (int j = numberSymbolsInImageArr[i, 1]; j <= numberSymbolsInImageArr[i, 0] + numberSymbolsInImageArr[i, 1]; j++)
                {
                    for (int k = 0; k < _myBitMap.Height; k++)
                    {
                        bitmapWithCutPictures[i].SetPixel(count, k, bt.GetPixel(j, k));
                    }
                    count++;
                }
            }
            return bitmapWithCutPictures;
        }

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

        //------END GAVNOKOD IZ PROBNOGO PROEKTA-----

    }
}
