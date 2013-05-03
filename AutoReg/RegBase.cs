using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognizerPictures;

namespace AutoReg
{
    public abstract class RegBase
    {
        protected IAntiCaptcha antiCaptcha;
        protected String Email { get; set; }
        protected String Password { get; set; }
        protected String Nick { get; set; }

        public enum Status
        {
            IncorrectCaptcha, UserAlreadyExists, PasswordsDoNotMatch, EmptyEmail, EmptyPasswords, EmptyLogin, SuccessfulRegistration
        };

        protected RegBase(IAntiCaptcha antiCaptcha)
        {
            this.antiCaptcha = antiCaptcha;
        }

        public virtual Status reg(String url, String email, String password, String nick)
        {
            return Status.IncorrectCaptcha;
        }
    }
}
