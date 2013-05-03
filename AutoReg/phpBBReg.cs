using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognizerPictures;

namespace AutoReg
{
    public class phpBBReg : RegBase
    {
        public phpBBReg(IAntiCaptcha antiCaptcha) : base(antiCaptcha) { }

        public void test213()
        {
            //dsdssds
        }

        public override Status reg(String url, String email, String password, String nick)
        {
            return Status.IncorrectCaptcha;
        }
    }
}
