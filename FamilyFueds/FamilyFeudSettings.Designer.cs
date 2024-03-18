namespace JuicyGrapeApps.FamilyFueds
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textForename = new System.Windows.Forms.TextBox();
            this.textSurname = new System.Windows.Forms.TextBox();
            this.labelComputerName = new System.Windows.Forms.Label();
            this.labelPersonName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listFamilyNames = new System.Windows.Forms.ListBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.radioMale = new System.Windows.Forms.RadioButton();
            this.radioFemale = new System.Windows.Forms.RadioButton();
            this.trackbarDefaultPeople = new System.Windows.Forms.TrackBar();
            this.labelDefaultPeople = new System.Windows.Forms.Label();
            this.labelDefaultSlider = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarDefaultPeople)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(172, 568);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(94, 29);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(29, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Setup Family Feuds screen saver";
            // 
            // textForename
            // 
            this.textForename.Location = new System.Drawing.Point(170, 340);
            this.textForename.Name = "textForename";
            this.textForename.Size = new System.Drawing.Size(178, 27);
            this.textForename.TabIndex = 0;
            // 
            // textSurname
            // 
            this.textSurname.Location = new System.Drawing.Point(170, 373);
            this.textSurname.Name = "textSurname";
            this.textSurname.Size = new System.Drawing.Size(178, 27);
            this.textSurname.TabIndex = 1;
            // 
            // labelComputerName
            // 
            this.labelComputerName.AutoSize = true;
            this.labelComputerName.Location = new System.Drawing.Point(33, 343);
            this.labelComputerName.Name = "labelComputerName";
            this.labelComputerName.Size = new System.Drawing.Size(75, 20);
            this.labelComputerName.TabIndex = 2;
            this.labelComputerName.Text = "Forename";
            // 
            // labelPersonName
            // 
            this.labelPersonName.AutoSize = true;
            this.labelPersonName.Location = new System.Drawing.Point(33, 376);
            this.labelPersonName.Name = "labelPersonName";
            this.labelPersonName.Size = new System.Drawing.Size(67, 20);
            this.labelPersonName.TabIndex = 3;
            this.labelPersonName.Text = "Surname";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 66);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(155, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Custom Family Names";
            // 
            // listFamilyNames
            // 
            this.listFamilyNames.FormattingEnabled = true;
            this.listFamilyNames.ItemHeight = 20;
            this.listFamilyNames.Location = new System.Drawing.Point(29, 100);
            this.listFamilyNames.Name = "listFamilyNames";
            this.listFamilyNames.Size = new System.Drawing.Size(319, 224);
            this.listFamilyNames.TabIndex = 7;
            this.listFamilyNames.SelectedIndexChanged += new System.EventHandler(this.listFamilyNames_SelectedIndexChanged);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(86, 455);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(94, 29);
            this.buttonAdd.TabIndex = 8;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(186, 455);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(94, 29);
            this.buttonRemove.TabIndex = 9;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(272, 568);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 29);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // radioMale
            // 
            this.radioMale.AutoSize = true;
            this.radioMale.Checked = true;
            this.radioMale.Location = new System.Drawing.Point(170, 411);
            this.radioMale.Name = "radioMale";
            this.radioMale.Size = new System.Drawing.Size(63, 24);
            this.radioMale.TabIndex = 11;
            this.radioMale.TabStop = true;
            this.radioMale.Text = "Male";
            this.radioMale.UseVisualStyleBackColor = true;
            // 
            // radioFemale
            // 
            this.radioFemale.AutoSize = true;
            this.radioFemale.Location = new System.Drawing.Point(254, 411);
            this.radioFemale.Name = "radioFemale";
            this.radioFemale.Size = new System.Drawing.Size(78, 24);
            this.radioFemale.TabIndex = 12;
            this.radioFemale.Text = "Female";
            this.radioFemale.UseVisualStyleBackColor = true;
            // 
            // trackbarDefaultPeople
            // 
            this.trackbarDefaultPeople.Location = new System.Drawing.Point(93, 506);
            this.trackbarDefaultPeople.Maximum = 20;
            this.trackbarDefaultPeople.Name = "trackbarDefaultPeople";
            this.trackbarDefaultPeople.Size = new System.Drawing.Size(228, 56);
            this.trackbarDefaultPeople.TabIndex = 13;
            this.trackbarDefaultPeople.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackbarDefaultPeople.Value = 10;
            this.trackbarDefaultPeople.Scroll += new System.EventHandler(this.trackbarDefaultPeople_Scroll);
            // 
            // labelDefaultPeople
            // 
            this.labelDefaultPeople.AutoSize = true;
            this.labelDefaultPeople.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDefaultPeople.Location = new System.Drawing.Point(327, 512);
            this.labelDefaultPeople.Name = "labelDefaultPeople";
            this.labelDefaultPeople.Size = new System.Drawing.Size(40, 32);
            this.labelDefaultPeople.TabIndex = 14;
            this.labelDefaultPeople.Text = "10";
            // 
            // labelDefaultSlider
            // 
            this.labelDefaultSlider.AutoSize = true;
            this.labelDefaultSlider.Location = new System.Drawing.Point(33, 521);
            this.labelDefaultSlider.Name = "labelDefaultSlider";
            this.labelDefaultSlider.Size = new System.Drawing.Size(58, 20);
            this.labelDefaultSlider.TabIndex = 15;
            this.labelDefaultSlider.Text = "Default";
            // 
            // FamilyFeudSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 609);
            this.Controls.Add(this.labelDefaultSlider);
            this.Controls.Add(this.labelDefaultPeople);
            this.Controls.Add(this.trackbarDefaultPeople);
            this.Controls.Add(this.radioFemale);
            this.Controls.Add(this.radioMale);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listFamilyNames);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelPersonName);
            this.Controls.Add(this.labelComputerName);
            this.Controls.Add(this.textSurname);
            this.Controls.Add(this.textForename);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FamilyFeudSettings";
            this.Text = "Family Feuds Settings";
            ((System.ComponentModel.ISupportInitialize)(this.trackbarDefaultPeople)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}