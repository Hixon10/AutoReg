using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Gui
{
    public partial class MainForm : Form
    {
        private List<String> _nicks;
        private List<String> _passwords;
        private List<String> _emails;
        private int _numberAcc = 0;
        private bool _isGeneratePassword;

        public MainForm()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                //TO DO 
            }
        }

        private bool Validation()
        {
            int.TryParse(textBoxNumberAcc.Text, out _numberAcc);
            if (radioButtonNo.Checked)
            {
                _isGeneratePassword = false;

                if (_passwords.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                _isGeneratePassword = true;
            }

            if (_nicks.Count == 0) return false;
            if (_emails.Count == 0) return false;
            if (_numberAcc == 0) return false;

            return true;
        }

        private void textBoxNickList_Click(object sender, EventArgs e)
        {
            _nicks = Opendialog(textBoxNickList);
        }

        private void textBoxPassList_Click(object sender, EventArgs e)
        {
            _passwords = Opendialog(textBoxPassList);
        }

        private void textBoxEmailList_Click(object sender, EventArgs e)
        {
            _emails = Opendialog(textBoxEmailList);
        }

        private static List<string> Opendialog(TextBox text)
        {
            var list = new List<string>();
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open text";
                dlg.Filter = "Text document (.txt)|*.txt";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    text.Text = dlg.FileName;
                    list.AddRange(File.ReadAllText(text.Text).Split(new[] { "\r\n", "\n" }, StringSplitOptions.None));
                }
            }
            return list;
        }

    }
}