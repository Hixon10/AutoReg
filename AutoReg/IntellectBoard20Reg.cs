using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognizerPictures;

namespace AutoReg
{
    public class IntellectBoard20Reg : RegBase
    {
        public IntellectBoard20Reg(IAntiCaptcha antiCaptcha) : base(antiCaptcha) { }

        public override bool reg(String url, String email, String password, String nick)
        {
            return false;
        }
    }
}
