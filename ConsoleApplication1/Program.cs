using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using RecognizerPictures;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IntellectBoard22AntiCaptcha intellectBoard22AntiCaptcha = new IntellectBoard22AntiCaptcha();
            Bitmap image = new Bitmap(@"C:\Users\Денис\Desktop\capchta\997.jpg");
            String result = intellectBoard22AntiCaptcha.recognizeImage(image);
            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
