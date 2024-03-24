namespace JuicyGrapeApps.FamilyFeuds
{
    partial class FamilyFeudSettings
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
            buttonOk = new Button();
            label2 = new Label();
            textForename = new TextBox();
            textSurname = new TextBox();
            labelComputerName = new Label();
            labelPersonName = new Label();
            label3 = new Label();
            listFamilyNames = new ListBox();
            buttonAdd = new Button();
            buttonRemove = new Button();
            buttonCancel = new Button();
            radioMale = new RadioButton();
            radioFemale = new RadioButton();
            trackbarDefaultPeople = new TrackBar();
            labelDefaultPeople = new Label();
            labelDefaultSlider = new Label();
            labelNotIncluding = new Label();
            ((System.ComponentModel.ISupportInitialize)trackbarDefaultPeople).BeginInit();
            SuspendLayout();
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(172, 584);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(94, 29);
            buttonOk.TabIndex = 4;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(29, 24);
            label2.Name = "label2";
            label2.Size = new Size(236, 20);
            label2.TabIndex = 5;
            label2.Text = "Setup Family Feuds Screen Saver";
            // 
            // textForename
            // 
            textForename.Location = new Point(170, 333);
            textForename.Name = "textForename";
            textForename.Size = new Size(178, 27);
            textForename.TabIndex = 0;
            textForename.KeyPress += textForename_KeyPress;
            // 
            // textSurname
            // 
            textSurname.Location = new Point(170, 366);
            textSurname.Name = "textSurname";
            textSurname.Size = new Size(178, 27);
            textSurname.TabIndex = 1;
            // 
            // labelComputerName
            // 
            labelComputerName.AutoSize = true;
            labelComputerName.Location = new Point(33, 336);
            labelComputerName.Name = "labelComputerName";
            labelComputerName.Size = new Size(75, 20);
            labelComputerName.TabIndex = 2;
            labelComputerName.Text = "Forename";
            // 
            // labelPersonName
            // 
            labelPersonName.AutoSize = true;
            labelPersonName.Location = new Point(33, 369);
            labelPersonName.Name = "labelPersonName";
            labelPersonName.Size = new Size(67, 20);
            labelPersonName.TabIndex = 3;
            labelPersonName.Text = "Surname";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 58);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.Yes;
            label3.Size = new Size(155, 20);
            label3.TabIndex = 6;
            label3.Text = "Custom Family Names";
            // 
            // listFamilyNames
            // 
            listFamilyNames.FormattingEnabled = true;
            listFamilyNames.ItemHeight = 20;
            listFamilyNames.Location = new Point(29, 91);
            listFamilyNames.Name = "listFamilyNames";
            listFamilyNames.Size = new Size(319, 224);
            listFamilyNames.TabIndex = 7;
            listFamilyNames.SelectedIndexChanged += listFamilyNames_SelectedIndexChanged;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(86, 442);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(94, 29);
            buttonAdd.TabIndex = 8;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(186, 442);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(94, 29);
            buttonRemove.TabIndex = 9;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(272, 584);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(94, 29);
            buttonCancel.TabIndex = 10;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // radioMale
            // 
            radioMale.AutoSize = true;
            radioMale.Checked = true;
            radioMale.Location = new Point(170, 402);
            radioMale.Name = "radioMale";
            radioMale.Size = new Size(63, 24);
            radioMale.TabIndex = 11;
            radioMale.TabStop = true;
            radioMale.Text = "Male";
            radioMale.UseVisualStyleBackColor = true;
            // 
            // radioFemale
            // 
            radioFemale.AutoSize = true;
            radioFemale.Location = new Point(254, 402);
            radioFemale.Name = "radioFemale";
            radioFemale.Size = new Size(78, 24);
            radioFemale.TabIndex = 12;
            radioFemale.Text = "Female";
            radioFemale.UseVisualStyleBackColor = true;
            // 
            // trackbarDefaultPeople
            // 
            trackbarDefaultPeople.Location = new Point(33, 522);
            trackbarDefaultPeople.Maximum = 20;
            trackbarDefaultPeople.Name = "trackbarDefaultPeople";
            trackbarDefaultPeople.Size = new Size(288, 56);
            trackbarDefaultPeople.TabIndex = 13;
            trackbarDefaultPeople.TickStyle = TickStyle.Both;
            trackbarDefaultPeople.Value = 10;
            trackbarDefaultPeople.Scroll += trackbarDefaultPeople_Scroll;
            // 
            // labelDefaultPeople
            // 
            labelDefaultPeople.AutoSize = true;
            labelDefaultPeople.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            labelDefaultPeople.Location = new Point(323, 528);
            labelDefaultPeople.Name = "labelDefaultPeople";
            labelDefaultPeople.Size = new Size(40, 32);
            labelDefaultPeople.TabIndex = 14;
            labelDefaultPeople.Text = "10";
            // 
            // labelDefaultSlider
            // 
            labelDefaultSlider.AutoSize = true;
            labelDefaultSlider.Location = new Point(33, 494);
            labelDefaultSlider.Name = "labelDefaultSlider";
            labelDefaultSlider.Size = new Size(127, 20);
            labelDefaultSlider.TabIndex = 15;
            labelDefaultSlider.Text = "People Generated";
            // 
            // labelNotIncluding
            // 
            labelNotIncluding.AutoSize = true;
            labelNotIncluding.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
            labelNotIncluding.Location = new Point(158, 496);
            labelNotIncluding.Name = "labelNotIncluding";
            labelNotIncluding.Size = new Size(149, 19);
            labelNotIncluding.TabIndex = 16;
            labelNotIncluding.Text = "(not including custom)";
            labelNotIncluding.Click += labelNotIncluding_Click;
            // 
            // FamilyFeudSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(378, 625);
            Controls.Add(labelNotIncluding);
            Controls.Add(labelDefaultSlider);
            Controls.Add(labelDefaultPeople);
            Controls.Add(trackbarDefaultPeople);
            Controls.Add(radioFemale);
            Controls.Add(radioMale);
            Controls.Add(buttonCancel);
            Controls.Add(buttonRemove);
            Controls.Add(buttonAdd);
            Controls.Add(listFamilyNames);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(buttonOk);
            Controls.Add(labelPersonName);
            Controls.Add(labelComputerName);
            Controls.Add(textSurname);
            Controls.Add(textForename);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FamilyFeudSettings";
            Text = "Family Feuds Settings";
            ((System.ComponentModel.ISupportInitialize)trackbarDefaultPeople).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonOk;
        private Label label2;
        private TextBox textForename;
        private TextBox textSurname;
        private Label labelComputerName;
        private Label labelPersonName;
        private Label label3;
        private ListBox listFamilyNames;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonCancel;
        private RadioButton radioMale;
        private RadioButton radioFemale;
        private TrackBar trackbarDefaultPeople;
        private Label labelDefaultPeople;
        private Label labelDefaultSlider;
        private Label labelNotIncluding;
    }
}