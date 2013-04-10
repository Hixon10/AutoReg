using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gui
{
    public partial class MainForm : Form
    {
        List<String> nicks = null;
        List<String> passwords = null;
        List<String> emails = null;
        int numberAcc = 0;
        bool isGeneratePassword = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (validation())
            {

            }
        }

        private bool validation()
        {
            int.TryParse(this.textBoxNumberAcc.Text, out numberAcc);
            if (radioButtonNo.Checked)
            {
                isGeneratePassword = false;

                if (passwords.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                isGeneratePassword = true;
            }

            if (nicks.Count == 0) return false;
            if (emails.Count == 0) return false;
            if (numberAcc == 0) return false;

            return true;
        }

        private void textBoxNickList_Click(object sender, EventArgs e)
        {
            nicks = opendialog(this.textBoxNickList);
        }

        private void textBoxPassList_Click(object sender, EventArgs e)
        {
            passwords = opendialog(this.textBoxPassList);
        }

        private void textBoxEmailList_Click(object sender, EventArgs e)
        {
            emails = opendialog(this.textBoxEmailList);
        }

        private List<string> opendialog(TextBox text)
        {
            var list = new List<string>();
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open text";
                dlg.Filter = "Text document (.txt)|*.txt";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    text.Text = dlg.FileName;
                    list.AddRange(File.ReadAllText(text.Text).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));
                }
            }
            return list;
        }

    }
}