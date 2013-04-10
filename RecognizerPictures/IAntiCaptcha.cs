using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecognizerPictures
{
    public interface IAntiCaptcha
    {
        Bitmap Image { get; set; }
        String TextFromImage { get; set; }
        String recognizeImage(Bitmap image);
    }
}
