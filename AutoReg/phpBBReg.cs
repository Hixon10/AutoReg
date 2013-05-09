using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RecognizerPictures;

namespace AutoReg
{
    public class phpBBReg : RegBase
    {
        public phpBBReg(IAntiCaptcha antiCaptcha) : base(antiCaptcha) { }

        #region Основные переменые

        private string _sid = string.Empty;
        private string _htmlCode = string.Empty;
        private string _pathToCaptcha = string.Empty;
        private string _replaceAmp = string.Empty;
        private const string PathToRegistration = "http://forum.ipadiz.ru/ucp.php?mode=register";
        private string _answer = string.Empty;
        private Bitmap _newBtmp;

        #endregion

        #region Регистрирует

        public override Status reg(String url, String email, String password, String nick)
        {
            //string strr = "href=\"\\./index.php\\?sid=(.*?)\"";
            string strr = "src=\"\\./ucp.php?(.*?)\"";
            //string confirm_id = "confirm_id=(.*?)&amp";src=\"./ucp.php?(.*?)sid
            string linkToCaptcha = "src=\"./ucp.php?(.*?)\" alt=";


            _htmlCode = GetHtmlCodeFromPage(url);
            _pathToCaptcha = getLinkToCapthcaFromUcp(linkToCaptcha, _htmlCode);
            _sid = GetSidFromPage(_pathToCaptcha);
            _replaceAmp = getLinkToCapthcaFromUcp(linkToCaptcha, _htmlCode).Replace("amp;", "");
            _newBtmp = new Bitmap(GetCapthcaFromPage("http://forum.ipadiz.ru/ucp.php" + _replaceAmp));
            _answer = antiCaptcha.recognizeImage(_newBtmp);
            return getStatusRegestration(_answer, _htmlCode);
        }

        #endregion

        #region Проверяет статус регистрации

        private Status getStatusRegestration(string textFromCaptcha, string html)
        {
            if (html.Contains(textFromCaptcha.Trim()))
            {
                return Status.SuccessfulRegistration;
            }
            else
            {
                return Status.IncorrectCaptcha;
            }
            throw new ArgumentException("Не существует такого статуса регистрации");
        }

        #endregion
        
        #region Получает картинку с капчей

        private Bitmap GetCapthcaFromPage(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            CookieContainer cookieJar = new CookieContainer();
            cookieJar.Add(new Uri("http://forum.ipadiz.ru/"), new Cookie("phpbb3_jornv_k", ""));
            cookieJar.Add(new Uri("http://forum.ipadiz.ru/"), new Cookie("phpbb3_jornv_u", "1"));
            cookieJar.Add(new Uri("http://forum.ipadiz.ru/"), new Cookie("style_cookie", "null"));
            cookieJar.Add(new Uri("http://forum.ipadiz.ru/"), new Cookie("phpbb3_jornv_sid", _sid));
            request.CookieContainer = cookieJar;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream st = response.GetResponseStream();
            Bitmap bitmap = new Bitmap(st);
            return bitmap;
        }

        #endregion

        #region Получает sid - coockie

        private string GetSidFromPage(string path)
        {
            string str = string.Empty;
            for (int i = 81; i < path.Length; i++)
            {
                str = str + path[i].ToString();
            }
            return str;
        }

        #endregion
        
        #region Получает html код страницы

        public static String GetHtmlCodeFromPage(string Url)
        {

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "POST";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

        #endregion

        #region Получается полный путь до картинки

        private string getLinkToCapthcaFromUcp(string strr, string htmlCode)
        {
            Regex re = new Regex(strr);
            Match match = re.Match(htmlCode);
            return match.Groups[1].Value;
        }

        #endregion    
    }
}
