namespace Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxNickList = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonYes = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonNo = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassList = new System.Windows.Forms.TextBox();
            this.textBoxEmailList = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelphpBBStat = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelIntellectBoard22Stat = new System.Windows.Forms.Label();
            this.labelIntellectBoard20Stat = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxNumberAcc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxUrlIB22 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxUrlPhpBB = new System.Windows.Forms.TextBox();
            this.textBoxUrlIB20 = new System.Windows.Forms.TextBox();
            this.labelMaxPossilbeRegistrationIB22 = new System.Windows.Forms.Label();
            this.labelMaxPossilbeRegistrationPhpBB = new System.Windows.Forms.Label();
            this.labelMaxPossilbeRegistrationIB20 = new System.Windows.Forms.Label();
            this.checkBoxUrlIB22 = new System.Windows.Forms.CheckBox();
            this.checkBoxUrlIB20 = new System.Windows.Forms.CheckBox();
            this.checkBoxUrlphpBB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxNickList
            // 
            this.textBoxNickList.Location = new System.Drawing.Point(217, 3);
            this.textBoxNickList.Name = "textBoxNickList";
            this.textBoxNickList.ReadOnly = true;
            this.textBoxNickList.Size = new System.Drawing.Size(128, 20);
            this.textBoxNickList.TabIndex = 0;
            this.textBoxNickList.Text = "кликните для загрузки";
            this.textBoxNickList.Click += new System.EventHandler(this.textBoxNickList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Список ников";
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.Location = new System.Drawing.Point(217, 42);
            this.radioButtonYes.Name = "radioButtonYes";
            this.radioButtonYes.Size = new System.Drawing.Size(37, 17);
            this.radioButtonYes.TabIndex = 2;
            this.radioButtonYes.TabStop = true;
            this.radioButtonYes.Text = "да";
            this.radioButtonYes.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Генерировать пароли автоматически";
            // 
            // radioButtonNo
            // 
            this.radioButtonNo.AutoSize = true;
            this.radioButtonNo.Checked = true;
            this.radioButtonNo.Location = new System.Drawing.Point(260, 42);
            this.radioButtonNo.Name = "radioButtonNo";
            this.radioButtonNo.Size = new System.Drawing.Size(42, 17);
            this.radioButtonNo.TabIndex = 4;
            this.radioButtonNo.TabStop = true;
            this.radioButtonNo.Text = "нет";
            this.radioButtonNo.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Список паролей";
            // 
            // textBoxPassList
            // 
            this.textBoxPassList.Location = new System.Drawing.Point(217, 65);
            this.textBoxPassList.Name = "textBoxPassList";
            this.textBoxPassList.ReadOnly = true;
            this.textBoxPassList.Size = new System.Drawing.Size(128, 20);
            this.textBoxPassList.TabIndex = 6;
            this.textBoxPassList.Text = "кликните для загрузки";
            this.textBoxPassList.Click += new System.EventHandler(this.textBoxPassList_Click);
            // 
            // textBoxEmailList
            // 
            this.textBoxEmailList.Location = new System.Drawing.Point(217, 91);
            this.textBoxEmailList.Name = "textBoxEmailList";
            this.textBoxEmailList.ReadOnly = true;
            this.textBoxEmailList.Size = new System.Drawing.Size(128, 20);
            this.textBoxEmailList.TabIndex = 7;
            this.textBoxEmailList.Text = "кликните для загрузки";
            this.textBoxEmailList.Click += new System.EventHandler(this.textBoxEmailList_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Список емейлов";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Статистика разгаданных капч";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 297);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "phpBB";
            // 
            // labelphpBBStat
            // 
            this.labelphpBBStat.AutoSize = true;
            this.labelphpBBStat.Location = new System.Drawing.Point(125, 297);
            this.labelphpBBStat.Name = "labelphpBBStat";
            this.labelphpBBStat.Size = new System.Drawing.Size(13, 13);
            this.labelphpBBStat.TabIndex = 11;
            this.labelphpBBStat.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 324);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "IntellectBoard-2-20";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 348);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "IntellectBoard-2-22";
            // 
            // labelIntellectBoard22Stat
            // 
            this.labelIntellectBoard22Stat.AutoSize = true;
            this.labelIntellectBoard22Stat.Location = new System.Drawing.Point(125, 348);
            this.labelIntellectBoard22Stat.Name = "labelIntellectBoard22Stat";
            this.labelIntellectBoard22Stat.Size = new System.Drawing.Size(13, 13);
            this.labelIntellectBoard22Stat.TabIndex = 14;
            this.labelIntellectBoard22Stat.Text = "0";
            // 
            // labelIntellectBoard20Stat
            // 
            this.labelIntellectBoard20Stat.AutoSize = true;
            this.labelIntellectBoard20Stat.Location = new System.Drawing.Point(125, 324);
            this.labelIntellectBoard20Stat.Name = "labelIntellectBoard20Stat";
            this.labelIntellectBoard20Stat.Size = new System.Drawing.Size(13, 13);
            this.labelIntellectBoard20Stat.TabIndex = 15;
            this.labelIntellectBoard20Stat.Text = "0";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(17, 233);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(149, 23);
            this.start.TabIndex = 16;
            this.start.Text = "Начать регистрацию";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 119);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(121, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Количество аккаунтов";
            // 
            // textBoxNumberAcc
            // 
            this.textBoxNumberAcc.Location = new System.Drawing.Point(217, 116);
            this.textBoxNumberAcc.Name = "textBoxNumberAcc";
            this.textBoxNumberAcc.Size = new System.Drawing.Size(128, 20);
            this.textBoxNumberAcc.TabIndex = 18;
            this.textBoxNumberAcc.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "URL IB22";
            // 
            // textBoxUrlIB22
            // 
            this.textBoxUrlIB22.Location = new System.Drawing.Point(217, 142);
            this.textBoxUrlIB22.Name = "textBoxUrlIB22";
            this.textBoxUrlIB22.Size = new System.Drawing.Size(128, 20);
            this.textBoxUrlIB22.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 171);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "URL IB20";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 197);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "URL phpBB";
            // 
            // textBoxUrlPhpBB
            // 
            this.textBoxUrlPhpBB.Location = new System.Drawing.Point(217, 194);
            this.textBoxUrlPhpBB.Name = "textBoxUrlPhpBB";
            this.textBoxUrlPhpBB.Size = new System.Drawing.Size(128, 20);
            this.textBoxUrlPhpBB.TabIndex = 23;
            // 
            // textBoxUrlIB20
            // 
            this.textBoxUrlIB20.Location = new System.Drawing.Point(217, 168);
            this.textBoxUrlIB20.Name = "textBoxUrlIB20";
            this.textBoxUrlIB20.Size = new System.Drawing.Size(128, 20);
            this.textBoxUrlIB20.TabIndex = 24;
            // 
            // labelMaxPossilbeRegistrationIB22
            // 
            this.labelMaxPossilbeRegistrationIB22.AutoSize = true;
            this.labelMaxPossilbeRegistrationIB22.Location = new System.Drawing.Point(139, 348);
            this.labelMaxPossilbeRegistrationIB22.Name = "labelMaxPossilbeRegistrationIB22";
            this.labelMaxPossilbeRegistrationIB22.Size = new System.Drawing.Size(18, 13);
            this.labelMaxPossilbeRegistrationIB22.TabIndex = 25;
            this.labelMaxPossilbeRegistrationIB22.Text = "/0";
            // 
            // labelMaxPossilbeRegistrationPhpBB
            // 
            this.labelMaxPossilbeRegistrationPhpBB.AutoSize = true;
            this.labelMaxPossilbeRegistrationPhpBB.Location = new System.Drawing.Point(139, 297);
            this.labelMaxPossilbeRegistrationPhpBB.Name = "labelMaxPossilbeRegistrationPhpBB";
            this.labelMaxPossilbeRegistrationPhpBB.Size = new System.Drawing.Size(18, 13);
            this.labelMaxPossilbeRegistrationPhpBB.TabIndex = 26;
            this.labelMaxPossilbeRegistrationPhpBB.Text = "/0";
            // 
            // labelMaxPossilbeRegistrationIB20
            // 
            this.labelMaxPossilbeRegistrationIB20.AutoSize = true;
            this.labelMaxPossilbeRegistrationIB20.Location = new System.Drawing.Point(139, 324);
            this.labelMaxPossilbeRegistrationIB20.Name = "labelMaxPossilbeRegistrationIB20";
            this.labelMaxPossilbeRegistrationIB20.Size = new System.Drawing.Size(18, 13);
            this.labelMaxPossilbeRegistrationIB20.TabIndex = 27;
            this.labelMaxPossilbeRegistrationIB20.Text = "/0";
            // 
            // checkBoxUrlIB22
            // 
            this.checkBoxUrlIB22.AutoSize = true;
            this.checkBoxUrlIB22.Location = new System.Drawing.Point(352, 144);
            this.checkBoxUrlIB22.Name = "checkBoxUrlIB22";
            this.checkBoxUrlIB22.Size = new System.Drawing.Size(108, 17);
            this.checkBoxUrlIB22.TabIndex = 28;
            this.checkBoxUrlIB22.Text = "Регистрировать";
            this.checkBoxUrlIB22.UseVisualStyleBackColor = true;
            // 
            // checkBoxUrlIB20
            // 
            this.checkBoxUrlIB20.AutoSize = true;
            this.checkBoxUrlIB20.Location = new System.Drawing.Point(352, 170);
            this.checkBoxUrlIB20.Name = "checkBoxUrlIB20";
            this.checkBoxUrlIB20.Size = new System.Drawing.Size(108, 17);
            this.checkBoxUrlIB20.TabIndex = 29;
            this.checkBoxUrlIB20.Text = "Регистрировать";
            this.checkBoxUrlIB20.UseVisualStyleBackColor = true;
            // 
            // checkBoxUrlphpBB
            // 
            this.checkBoxUrlphpBB.AutoSize = true;
            this.checkBoxUrlphpBB.Location = new System.Drawing.Point(352, 196);
            this.checkBoxUrlphpBB.Name = "checkBoxUrlphpBB";
            this.checkBoxUrlphpBB.Size = new System.Drawing.Size(108, 17);
            this.checkBoxUrlphpBB.TabIndex = 30;
            this.checkBoxUrlphpBB.Text = "Регистрировать";
            this.checkBoxUrlphpBB.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 377);
            this.Controls.Add(this.checkBoxUrlphpBB);
            this.Controls.Add(this.checkBoxUrlIB20);
            this.Controls.Add(this.checkBoxUrlIB22);
            this.Controls.Add(this.labelMaxPossilbeRegistrationIB20);
            this.Controls.Add(this.labelMaxPossilbeRegistrationPhpBB);
            this.Controls.Add(this.labelMaxPossilbeRegistrationIB22);
            this.Controls.Add(this.textBoxUrlIB20);
            this.Controls.Add(this.textBoxUrlPhpBB);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxUrlIB22);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxNumberAcc);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.start);
            this.Controls.Add(this.labelIntellectBoard20Stat);
            this.Controls.Add(this.labelIntellectBoard22Stat);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelphpBBStat);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxEmailList);
            this.Controls.Add(this.textBoxPassList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButtonNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButtonYes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxNickList);
            this.Name = "MainForm";
            this.Text = "AutoReg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNickList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonYes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassList;
        private System.Windows.Forms.TextBox textBoxEmailList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelphpBBStat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelIntellectBoard22Stat;
        private System.Windows.Forms.Label labelIntellectBoard20Stat;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxNumberAcc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxUrlIB22;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxUrlPhpBB;
        private System.Windows.Forms.TextBox textBoxUrlIB20;
        private System.Windows.Forms.Label labelMaxPossilbeRegistrationIB22;
        private System.Windows.Forms.Label labelMaxPossilbeRegistrationPhpBB;
        private System.Windows.Forms.Label labelMaxPossilbeRegistrationIB20;
        private System.Windows.Forms.CheckBox checkBoxUrlIB22;
        private System.Windows.Forms.CheckBox checkBoxUrlIB20;
        private System.Windows.Forms.CheckBox checkBoxUrlphpBB;
    }
}

