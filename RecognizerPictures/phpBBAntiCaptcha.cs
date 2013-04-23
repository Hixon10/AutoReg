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

        private Bitmap MyBitMap;
        private byte[] MyArrByte;

        private void button1_Click(object sender, EventArgs e)
        {
            MyBitMap = new Bitmap("img1.jpg");
            /*pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.ClientSize = new Size(320, 50);
            pictureBox1.Image = (Image)MyBitMap;*/
            List<string> col = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color cl = MyBitMap.GetPixel(i, j);
                    col.Add(cl.Name);
                }
            }

            for (int i = MyBitMap.Width - 10; i < MyBitMap.Width; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color cl = MyBitMap.GetPixel(i, j);
                    col.Add(cl.Name);
                }
            }
            //MessageBox.Show(col.Count.ToString());
            var we = col.Distinct();
            var qq = we.ToList();

            //MessageBox.Show(we.Count().ToString());
            for (int i = 0; i < we.Count(); i++)
            {
                //richTextBox1.Text += qq[i].ToString() + "\n";
            }
            for (int i = 0; i < MyBitMap.Width; i++)
            {
                for (int j = 0; j < MyBitMap.Height; j++)
                {
                    Color cl = MyBitMap.GetPixel(i, j);
                    if (colr(qq, cl.Name))
                        MyBitMap.SetPixel(i, j, Color.White);


                    //if (String.Compare(cl.Name.ToString(), "d0d0d0") == 0 || String.Compare(cl.Name.ToString(), "9e9e9e") == 0 || String.Compare(cl.Name.ToString(), "949494") == 0 || String.Compare(cl.Name.ToString(), "a1a1a1") == 0)
                    //MessageBox.Show(cl.Name.ToString());
                    //if ((String.Compare(cl.Name.ToString(), "ff040404") != 0) && (String.Compare(cl.Name.ToString(), "fffafafa") != 0))
                    //if (String.Compare(cl.Name.ToString(), "ffd0d0d0") == 0 || String.Compare(cl.Name.ToString(), "ff9e9e9e") == 0 || String.Compare(cl.Name.ToString(), "ff949494") == 0 || String.Compare(cl.Name.ToString(), "ffa1a1a1") == 0)
                    /*{
                        //MessageBox.Show(cl.Name.ToString());
                        MyBitMap.SetPixel(i, j, Color.Red);
                    }*/
                }
            }
            for (int i = 0; i < MyBitMap.Width; i++)
            {
                for (int j = 0; j < MyBitMap.Height; j++)
                {
                    Color cl = MyBitMap.GetPixel(i, j);
                    if (String.Compare(cl.Name.ToString(), "ffffffff") != 0)
                    {
                        MyBitMap.SetPixel(i, j, Color.Black);
                    }
                }
            }
            /*pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.ClientSize = new Size(320, 50);
            pictureBox2.Image = (Image)MyBitMap;*/
            MyBitMap.Save(Environment.CurrentDirectory + @"/ds.jpg");
            NumberOne(MyBitMap);
            /*pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.ClientSize = new Size(320, 50);
            pictureBox3.Image = (Image)NumberOne(MyBitMap);*/
            //GetClearImg(MyBitMap);
        }

        private Bitmap NumberOne(Bitmap bt)
        {
            Bitmap nbt = new Bitmap(100, 100);
            int buf = 0;
            int count = 0;
            int myblack = 0;
            int mywhite = 0;
            int re = 0;
            string[,] blwh = new string[100, 50];
            Color cl = new Color();
            int[] shir = new int[MyBitMap.Width];
            int ggg = 0;
            for (int i = 0; i < MyBitMap.Width; i++)
            {
                var xxx = 0;
                for (int j = 0; j < MyBitMap.Height; j++)
                {
                    if (bt.GetPixel(i, j).Name == "ff000000")
                    {
                        xxx++;
                    }
                }
                if (xxx < 2)
                {
                    shir[ggg] = i;
                    ggg++;
                }
            }
            int[,] h = new int[4, 2];

            var xc = 0;

            for (int i = 0; i < ggg; i++)
            {
                if (i + 1 < ggg && shir[i + 1] - shir[i] > 1)
                {
                    h[xc, 0] = shir[i + 1] - shir[i];
                    h[xc, 1] = shir[i];
                    xc++;
                }
            }
            Bitmap b1 = new Bitmap(h[0, 0] + 1, MyBitMap.Height);
            Bitmap b2 = new Bitmap(h[1, 0] + 1, MyBitMap.Height);
            Bitmap b3 = new Bitmap(h[2, 0] + 1, MyBitMap.Height);
            Bitmap b4 = new Bitmap(h[3, 0] + 1, MyBitMap.Height);

            Bitmap[] bb = { b1, b2, b3, b4 };

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
            /*pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.ClientSize = new Size(30, 50);
            pictureBox3.Image = (Image)bb[0];

            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.ClientSize = new Size(30, 50);
            pictureBox4.Image = (Image)bb[1];*/
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

            return nbt;
        }


        private bool colr(List<string> Li, string name)
        {
            int f = 0;
            for (int i = 0; i < Li.Count; i++)
            {
                if (String.Compare(Li[i], name) == 0)
                {
                    f = 1;
                    i = Li.Count + 1;
                    //MessageBox.Show(Li[i].ToString());
                }
                else
                {
                    f = 0;
                }
            }
            if (f == 1)
                return true;
            else
            {
                return false;
            }
        }


        private void GetClearImg(Bitmap MBT)
        {
            /*Bitmap qw = MBT;
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
        }

        //------END GAVNOKOD IZ PROBNOGO PROEKTA-----

    }
}
