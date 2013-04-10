using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognizerPictures;

namespace AutoReg
{
    public class IntellectBoard22Reg : RegBase
    {
        public IntellectBoard22Reg(IAntiCaptcha antiCaptcha) : base(antiCaptcha) { }

        public override bool reg(String email, String password, String nick)
        {
            return false;
        }
    }
}
