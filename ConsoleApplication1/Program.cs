using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using AutoReg;
using RecognizerPictures;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
//            IntellectBoard22AntiCaptcha intellectBoard22AntiCaptcha = new IntellectBoard22AntiCaptcha();
//            Bitmap image = new Bitmap(@"C:\Users\Денис\Desktop\capchta\997.jpg");
//            String result = intellectBoard22AntiCaptcha.recognizeImage(image);
//            Console.WriteLine(result);
            IAntiCaptcha antiCaptcha = new IntellectBoard22AntiCaptcha();
            IntellectBoard22Reg intellectBoard22Reg = new IntellectBoard22Reg(antiCaptcha);
            String html = intellectBoard22Reg.getHtmlFromUrl(@"http://forum.vgd.ru/index.php?a=register&m=profile&q=1");
            String sidDdos = intellectBoard22Reg.getSidDdos(html);
            Bitmap image = intellectBoard22Reg.getCaptchaFromSiDdos(sidDdos);
            image.Save(@"C:\Users\Денис\Desktop\capchta\" + image.GetHashCode() + ".jpg");

            Console.ReadKey();
        }
    }
}
