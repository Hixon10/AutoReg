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
            //dsds
        }

        public override bool reg(String email, String password, String nick)
        {
            return false;
        }
    }
}
