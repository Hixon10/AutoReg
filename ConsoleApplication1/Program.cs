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
            Bitmap image = new Bitmap(@"C:\Users\Денис\Desktop\capchta\996.jpg");
            Bitmap newImage = intellectBoard22AntiCaptcha.deleteWhiteStripes(image);
            Bitmap binarizationImage = intellectBoard22AntiCaptcha.binarizationImage(newImage);
            List<Bitmap> chars = intellectBoard22AntiCaptcha.splitImageIntoChars(binarizationImage);
            foreach (var charImage in chars)
            {
                charImage.Save(@"C:\Users\Денис\Desktop\capchta\chars\" + charImage.GetHashCode() + ".jpg", ImageFormat.Jpeg);
            }

            Console.ReadKey();
        }
    }
}
