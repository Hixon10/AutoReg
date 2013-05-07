using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using AutoReg;
using RecognizerPictures;

namespace Gui
{
    public partial class MainForm : Form
    {
        private readonly IAntiCaptcha _intellectBoard22AntiCaptcha;
        private readonly IAntiCaptcha _intellectBoard20AntiCaptcha;
        private readonly IAntiCaptcha _phpBBAntiCaptcha;
        private readonly IntellectBoardReg _intellectBoard22Reg;
        private readonly IntellectBoardReg _intellectBoard20Reg;
        private readonly phpBBReg _phpBbReg;

        private List<String> _nicks;
        private List<String> _passwords;
        private List<String> _emails;
        private int _numberAcc = 0;
        private bool _isGeneratePassword;
        private int _possiblenumberAcc = 0;
        private String _urlIntellectBoard22Forum;
        private String _urlIntellectBoard20Forum;
        private String _urlphpBBForum;

        public MainForm()
        {
            InitializeComponent();
            backgroundWorkerIB22.WorkerReportsProgress = true;
            backgroundWorkerPHPBB.WorkerReportsProgress = true;
            _intellectBoard22AntiCaptcha = new IntellectBoard22AntiCaptcha();
            _intellectBoard20AntiCaptcha = new IntellectBoard20AntiCaptcha();
            _phpBBAntiCaptcha = new phpBBAntiCaptcha();
            _intellectBoard22Reg = new IntellectBoardReg(_intellectBoard22AntiCaptcha);
            _intellectBoard20Reg = new IntellectBoardReg(_intellectBoard20AntiCaptcha);
            _phpBbReg = new phpBBReg(_phpBBAntiCaptcha);
        }

        private void start_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                _possiblenumberAcc = calculatePossibleNumberAcc(_nicks.Count, _emails.Count, _passwords.Count,
                                                                _numberAcc);
                labelMaxPossilbeRegistrationIB20.Text = "/" + _possiblenumberAcc;
                labelMaxPossilbeRegistrationIB22.Text = "/" + _possiblenumberAcc;
                labelMaxPossilbeRegistrationPhpBB.Text = "/" + _possiblenumberAcc;
                labelIntellectBoard22Stat.Text = "0";
                labelIntellectBoard20Stat.Text = "0";
                labelIntellectBoard22Stat.Text = "0";
                this.Refresh();

                if (checkBoxUrlIB22.Checked)
                {
                    if (backgroundWorkerIB22.IsBusy != true)
                    {
                        // Start the asynchronous operation.
                        backgroundWorkerIB22.RunWorkerAsync();
                    }
                }

                if (checkBoxUrlphpBB.Checked)
                {
                    if (backgroundWorkerPHPBB.IsBusy != true)
                    {
                        // Start the asynchronous operation.
                        backgroundWorkerPHPBB.RunWorkerAsync();
                    }
                }

                for (int i = 0; i < _possiblenumberAcc; i++)
                {
                    if (checkBoxUrlIB20.Checked)
                    {
                        //Важно, используются левые данные для регистрации, для того чтобы не проходила регистрация
                        if (_intellectBoard20Reg.reg(_urlIntellectBoard20Forum, _emails[0], _passwords[0], _nicks[0]) !=
                            RegBase.Status.IncorrectCaptcha)
                        {
                            int digit = int.Parse(labelIntellectBoard20Stat.Text);
                            labelIntellectBoard20Stat.Text = (digit + 1).ToString();
                        }
                    }

                    this.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Не заполнены все поля!");
            }
        }

        private bool Validation()
        {
            int.TryParse(textBoxNumberAcc.Text, out _numberAcc);
            if (radioButtonNo.Checked)
            {
                _isGeneratePassword = false;

                if (_passwords == null) return false;

                if (_passwords.Count == 0)
                {
                    return false;
                }
            }
            else
            {
                _isGeneratePassword = true;
                _passwords = getPasswords(_numberAcc, 7);
            }

            if (_nicks.Count == 0) return false;
            if (_emails.Count == 0) return false;
            if (_numberAcc == 0) return false;

            if (String.IsNullOrWhiteSpace(textBoxUrlIB22.Text) && checkBoxUrlIB22.Checked) return false;
            _urlIntellectBoard22Forum = textBoxUrlIB22.Text;

            if (String.IsNullOrWhiteSpace(textBoxUrlIB20.Text) && checkBoxUrlIB20.Checked) return false;
            _urlIntellectBoard20Forum = textBoxUrlIB20.Text;

            if (String.IsNullOrWhiteSpace(textBoxUrlPhpBB.Text) && checkBoxUrlphpBB.Checked) return false;
            _urlphpBBForum = textBoxUrlPhpBB.Text;

            return true;
        }

        private int calculatePossibleNumberAcc(int nicksNumber, int emailsNumber, int passwordNumber, int accNumber)
        {
            List<int> digitals = new List<int>(4);
            digitals.Add(nicksNumber);
            digitals.Add(emailsNumber);
            digitals.Add(passwordNumber);
            digitals.Add(accNumber);
            return digitals.Min(); 
        }

        private List<String> getPasswords(int passwordsNumber, int passwordLength)
        {
            List<String> passwords = new List<string>(passwordsNumber);
            for (int i = 0; i < passwordsNumber; i++)
            {
                passwords.Add(PasswordGenerator.createRandomPassword(passwordLength));
            }

            return passwords;
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

        private void backgroundWorkerIB22_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int progress = 0;

            for (int i = 0; i < _possiblenumberAcc; i++)
            {
                //Важно, используются левые данные для регистрации, для того чтобы не проходила регистрация
                if (_intellectBoard22Reg.reg(_urlIntellectBoard22Forum, _emails[0], _passwords[0], _nicks[0]) !=
                    RegBase.Status.IncorrectCaptcha)
                {
                    progress++;
                    worker.ReportProgress(progress);
                }
            }
        }

        private void backgroundWorkerIB22_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            labelIntellectBoard22Stat.Text = e.ProgressPercentage.ToString();
        }

        private void backgroundWorkerIB22_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Регистрация на форуме IntellectBoard22 завершена!");
        }

        private void backgroundWorkerPHPBB_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int progress = 0;

            for (int i = 0; i < _possiblenumberAcc; i++)
            {
                //Важно, используются левые данные для регистрации, для того чтобы не проходила регистрация
                if (_phpBbReg.reg(_urlphpBBForum, _emails[0], _passwords[0], _nicks[0]) !=
                    RegBase.Status.IncorrectCaptcha)
                {
                    progress++;
                    worker.ReportProgress(progress);
                }
            }
        }

        private void backgroundWorkerPHPBB_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelphpBBStat.Text = e.ProgressPercentage.ToString();
        }

        private void backgroundWorkerPHPBB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Регистрация на форуме phpBB завершена!");
        }



    }
}