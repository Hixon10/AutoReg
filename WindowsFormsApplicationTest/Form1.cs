using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoReg;
using RecognizerPictures;

namespace WindowsFormsApplicationTest
{
    public partial class Form1 : Form
    {
        private IntellectBoard22Reg intellectBoard22Reg;
        private String sidDdos;
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
            String html = intellectBoard22Reg.getHtmlFromUrl(@"http://forum.vgd.ru/index.php?a=register&m=profile&q=1");
            sidDdos = intellectBoard22Reg.getSidDdos(html);
            Bitmap image = intellectBoard22Reg.getCaptchaFromSiDdos(sidDdos);
            this.pictureBox1.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String html = intellectBoard22Reg.sendDataWithPost("http://forum.vgd.ru/index.php",
                                                               "denisdenis20000@mail.ru", "denisdenis200000",
                                                               "denisdenis20000", textBox4.Text, sidDdos);
            MessageBox.Show(html);
        }
    }
}
