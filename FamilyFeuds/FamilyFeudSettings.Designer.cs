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
            textForename = new TextBox();
            textSurname = new TextBox();
            labelComputerName = new Label();
            labelPersonName = new Label();
            labelCustomPeople = new Label();
            listFamilyNames = new ListBox();
            buttonAdd = new Button();
            buttonRemove = new Button();
            buttonCancel = new Button();
            radioMale = new RadioButton();
            radioFemale = new RadioButton();
            trackbarDefaultPeople = new TrackBar();
            labelDefaultPeople = new Label();
            labelExcluding = new Label();
            labelPeopleGenerated = new Label();
            ((System.ComponentModel.ISupportInitialize)trackbarDefaultPeople).BeginInit();
            SuspendLayout();
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(172, 549);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(94, 29);
            buttonOk.TabIndex = 4;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // textForename
            // 
            textForename.Location = new Point(171, 296);
            textForename.Name = "textForename";
            textForename.Size = new Size(178, 27);
            textForename.TabIndex = 0;
            textForename.KeyPress += textForename_KeyPress;
            // 
            // textSurname
            // 
            textSurname.Location = new Point(171, 329);
            textSurname.Name = "textSurname";
            textSurname.Size = new Size(178, 27);
            textSurname.TabIndex = 1;
            // 
            // labelComputerName
            // 
            labelComputerName.AutoSize = true;
            labelComputerName.Location = new Point(34, 299);
            labelComputerName.Name = "labelComputerName";
            labelComputerName.Size = new Size(75, 20);
            labelComputerName.TabIndex = 2;
            labelComputerName.Text = "Forename";
            // 
            // labelPersonName
            // 
            labelPersonName.AutoSize = true;
            labelPersonName.Location = new Point(34, 332);
            labelPersonName.Name = "labelPersonName";
            labelPersonName.Size = new Size(67, 20);
            labelPersonName.TabIndex = 3;
            labelPersonName.Text = "Surname";
            // 
            // labelCustomPeople
            // 
            labelCustomPeople.AutoSize = true;
            labelCustomPeople.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelCustomPeople.Location = new Point(30, 21);
            labelCustomPeople.Name = "labelCustomPeople";
            labelCustomPeople.RightToLeft = RightToLeft.Yes;
            labelCustomPeople.Size = new Size(114, 20);
            labelCustomPeople.TabIndex = 6;
            labelCustomPeople.Text = "Custom People";
            // 
            // listFamilyNames
            // 
            listFamilyNames.FormattingEnabled = true;
            listFamilyNames.ItemHeight = 20;
            listFamilyNames.Location = new Point(30, 54);
            listFamilyNames.Name = "listFamilyNames";
            listFamilyNames.Size = new Size(319, 224);
            listFamilyNames.TabIndex = 7;
            listFamilyNames.SelectedIndexChanged += listFamilyNames_SelectedIndexChanged;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(87, 405);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(94, 29);
            buttonAdd.TabIndex = 8;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonRemove
            // 
            buttonRemove.Location = new Point(187, 405);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(94, 29);
            buttonRemove.TabIndex = 9;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(272, 549);
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
            radioMale.Location = new Point(171, 365);
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
            radioFemale.Location = new Point(255, 365);
            radioFemale.Name = "radioFemale";
            radioFemale.Size = new Size(78, 24);
            radioFemale.TabIndex = 12;
            radioFemale.Text = "Female";
            radioFemale.UseVisualStyleBackColor = true;
            // 
            // trackbarDefaultPeople
            // 
            trackbarDefaultPeople.Location = new Point(34, 487);
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
            labelDefaultPeople.Location = new Point(324, 493);
            labelDefaultPeople.Name = "labelDefaultPeople";
            labelDefaultPeople.Size = new Size(40, 32);
            labelDefaultPeople.TabIndex = 14;
            labelDefaultPeople.Text = "10";
            // 
            // labelExcluding
            // 
            labelExcluding.AutoSize = true;
            labelExcluding.Font = new Font("Segoe UI", 8F, FontStyle.Italic, GraphicsUnit.Point);
            labelExcluding.Location = new Point(163, 460);
            labelExcluding.Name = "labelExcluding";
            labelExcluding.Size = new Size(125, 19);
            labelExcluding.TabIndex = 16;
            labelExcluding.Text = "(excluding custom)";
            // 
            // labelPeopleGenerated
            // 
            labelPeopleGenerated.AutoSize = true;
            labelPeopleGenerated.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelPeopleGenerated.Location = new Point(34, 460);
            labelPeopleGenerated.Name = "labelPeopleGenerated";
            labelPeopleGenerated.Size = new Size(133, 20);
            labelPeopleGenerated.TabIndex = 15;
            labelPeopleGenerated.Text = "People Generated";
            // 
            // FamilyFeudSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(378, 590);
            Controls.Add(labelPeopleGenerated);
            Controls.Add(labelExcluding);
            Controls.Add(labelDefaultPeople);
            Controls.Add(trackbarDefaultPeople);
            Controls.Add(radioFemale);
            Controls.Add(radioMale);
            Controls.Add(buttonCancel);
            Controls.Add(buttonRemove);
            Controls.Add(buttonAdd);
            Controls.Add(listFamilyNames);
            Controls.Add(labelCustomPeople);
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
        private TextBox textForename;
        private TextBox textSurname;
        private Label labelComputerName;
        private Label labelPersonName;
        private Label labelCustomPeople;
        private ListBox listFamilyNames;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonCancel;
        private RadioButton radioMale;
        private RadioButton radioFemale;
        private TrackBar trackbarDefaultPeople;
        private Label labelDefaultPeople;
        private Label labelExcluding;
        private Label labelPeopleGenerated;
    }
}