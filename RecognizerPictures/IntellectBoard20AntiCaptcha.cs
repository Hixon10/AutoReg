using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RecognizerPictures
{
    public class IntellectBoard20AntiCaptcha : IAntiCaptcha
    {
        public Bitmap Image { get; set; }
        public String TextFromImage { get; set; }

        public String recognizeImage(Bitmap image)
        {
            return String.Empty;
        }
    }
}
