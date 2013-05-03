using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using RecognizerPictures;
using System.Net;

namespace AutoReg
{
    public class IntellectBoard22Reg : RegBase
    {
        public IntellectBoard22Reg(IAntiCaptcha antiCaptcha) : base(antiCaptcha) { }

        public override Status reg(String url, String email, String password, String nick)
        {
            return Status.IncorrectCaptcha;
        }

        public String getHtmlFromUrl(String url)
        {
            using (WebClient client = new WebClient()) 
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0");
                client.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                client.Headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");

                string htmlCode = client.DownloadString(url);
                return htmlCode;
            }
        }

        public Status getStatusRegestration(String responeHtml)
        {
            if (responeHtml.Contains("защитный код"))
            {
                return Status.IncorrectCaptcha; 
            }

            if (responeHtml.Contains("уже существует на"))
            {
                return Status.UserAlreadyExists;
            }

            if (responeHtml.Contains("пароль не совпадает"))
            {
                return Status.PasswordsDoNotMatch;
            }

            if (responeHtml.Contains("Не указан адрес"))
            {
                return Status.EmptyEmail;
            }

            if (responeHtml.Contains("Пароль пользователя не может"))
            {
                return  Status.EmptyPasswords;
            }

            if (responeHtml.Contains("Вы не ввели имя"))
            {
                return Status.EmptyLogin;
            }

            if (responeHtml.Contains("успешно зарегистрирован"))
            {
                return Status.SuccessfulRegistration;
            }

            throw new ArgumentException("Не существует такого статуса регистрации");
        }

        public String getSidDdos(String html)
        {
            Match match = Regex.Match(html, @"sid_ddos"" value=""(.+)""><img", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string key = match.Groups[1].Value;
                return key;
            }
            return String.Empty;
        }

        public Bitmap getCaptchaFromSiDdos(String domen, String sidDdos)
        {
            string localFilename = Directory.GetCurrentDirectory() + @"\" + sidDdos.GetHashCode() + ".jpg";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(domen + "agent.php?a=code&sid=" + sidDdos, localFilename);
                Bitmap image = new Bitmap(localFilename);
                return image;
            }
        }

        public String sendDataWithPost(String domen, String email, String password, String nick, String captcha, String siDdos)
        {
            using (var wb = new WebClient())
            {
                String url = domen + "index.php";
                var data = new NameValueCollection();
                data["u__name"] = nick;
                data["password1"] = password;
                data["password2"] = password;
                data["u__email"] = email;
                data["sid_ddos"] = siDdos;
                data["code"] = captcha;
                data["refpage"] = url;
                data["u_showmail"] = "3";
                data["u_lnid"] = "1";
                data["u_stid"] = "4";
                data["u_signature"] = "	";
                data["u_usesignature"] = "1";
                data["u_detrans"] = "0";
                data["u_multilang"] = "1";
                data["u_extform"] = "1";
                data["claim"] = "0";
                data["u"] = "3";
                data["m"] = "profile";
                data["a"] = "do_register";

                var response = wb.UploadValues(url, "POST", data);
                return Encoding.Default.GetString(response);
            }
        }
    }
}
