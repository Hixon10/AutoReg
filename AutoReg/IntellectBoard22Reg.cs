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

        public override bool reg(String url, String email, String password, String nick)
        {
            return false;
        }

        public String getHtmlFromUrl(String url)
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                string htmlCode = client.DownloadString(url);
                return htmlCode;

                Stream data = client.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s;
            }

//            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
//            myRequest.Method = "GET";
//            WebResponse myResponse = myRequest.GetResponse();
//            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.Default);
//            string result = sr.ReadToEnd();
//            sr.Close();
//            myResponse.Close();
//
//            return result;
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
                client.DownloadFile(domen + "agent.php?a=code&amp;sid=" + sidDdos, localFilename);
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
