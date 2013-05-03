using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoReg;
using RecognizerPictures;

namespace WindowsFormsApplicationTest
{
    public partial class Form1 : Form
    {
        private readonly IntellectBoard22Reg intellectBoard22Reg;
        private String sidDdos;
        private String domen = "http://forum.vgd.ru/";
        public Form1()
        {
            InitializeComponent();

            IAntiCaptcha antiCaptcha = new IntellectBoard22AntiCaptcha();
            intellectBoard22Reg = new IntellectBoard22Reg(antiCaptcha);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
//            String html = intellectBoard22Reg.getHtmlFromUrl(domen + "index.php?a=register&m=profile&q=1");
//            sidDdos = intellectBoard22Reg.getSidDdos(html);
//            Bitmap image = intellectBoard22Reg.getCaptchaFromSiDdos(domen, sidDdos);
//            pictureBox1.Image = image;
//            label5.Text = sidDdos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int succesfulRecognizeCaptcha = 0;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 500; i++)
            {

                String html = intellectBoard22Reg.getHtmlFromUrl(domen + "index.php?a=register&m=profile&q=1");
                sidDdos = intellectBoard22Reg.getSidDdos(html);
                Bitmap image = intellectBoard22Reg.getCaptchaFromSiDdos(domen, sidDdos);
                //pictureBox1.Image = image;
                //label5.Text = sidDdos;

                IAntiCaptcha antiCaptcha = new IntellectBoard22AntiCaptcha();
                textBox4.Text = antiCaptcha.recognizeImage(image);
                sb.Append(textBox4.Text);
                sb.Append("\r\n");

                String response = intellectBoard22Reg.sendDataWithPost(domen,
                                                                       textBox3.Text, textBox2.Text,
                                                                       textBox1.Text, textBox4.Text, sidDdos);

                if (intellectBoard22Reg.getStatusRegestration(response) != RegBase.Status.IncorrectCaptcha)
                {
                    succesfulRecognizeCaptcha += 1;
                }
            }
            File.WriteAllText(@"C:\Users\Денис\Desktop\capchta\chars\1.txt", sb.ToString());
            MessageBox.Show(succesfulRecognizeCaptcha.ToString());
        }
    }
}
