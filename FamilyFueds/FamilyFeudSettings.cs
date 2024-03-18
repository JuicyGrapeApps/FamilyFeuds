/*
 * Copyright (c) 2024 JuicyGrape Apps.
 *
 * Licensed under the MIT License, (the "License");
 * you may not use any file by JuicyGrape Apps except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     https://www.juicygrapeapps.com/terms
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Microsoft.Win32;
using System.Xml.Linq;

namespace JuicyGrapeApps.FamilyFueds
{
    public partial class FamilyFeudSettings : Form
    {
        public FamilyFeudSettings()
        {
            InitializeComponent();

            foreach (string name in ApplicationControl.names) listFamilyNames.Items.Add(name);

            trackbarDefaultPeople.Value = ApplicationControl.MaxDefaultNumber;
            labelDefaultPeople.Text = ApplicationControl.MaxDefaultNumber.ToString();
        }

        private void SaveSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(ApplicationControl.WIN_REG_PATH);
            string listOfNames = "";
            foreach (string name in listFamilyNames.Items) listOfNames += name+".";
            key.SetValue(ApplicationControl.REG_KEY_NAMES, listOfNames);
            key.SetValue(ApplicationControl.REG_KEY_DEFAULT, labelDefaultPeople.Text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (textForename.Text != string.Empty && textSurname.Text != string.Empty) {
                string name = GetName(false);
                string ext = " (M)";
                string gender = "male";

                if (!radioMale.Checked)
                {
                    ext = " (F)";
                    gender = "female";
                }

                if (!listFamilyNames.Items.Contains(name + ext))
                {

                    DialogResult result = MessageBox.Show($"The {gender} name \"{name}\" has been entered but not added to list.  Do you wish to save this as well?",
                        ApplicationControl.messageTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes) AddNameToList();
                    else if (result == DialogResult.Cancel) return;
                }
            }
            SaveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddNameToList();
        }

        /// <summary>
        /// Build a formatted fullname from the names entered, validation checks are made on the
        /// names if a boolean value of true is passed to this function. <see cref="validate(string, int)"/>
        /// </summary>
        /// <param name="validation"></param>
        /// <returns></returns>
        private string GetName(bool validation)
        {
            string name = textForename.Text.Replace(" ", "").ToLower();

            if (validation && !validate(name, 1)) return "";

            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            string surname = textSurname.Text.Replace(" ", "").ToLower();

            if (validation && !validate(surname, 2)) return "";

            surname = surname.Substring(0, 1).ToUpper() + surname.Substring(1);
            name = name += " " + surname;
            name = name.Replace(".", "");

            if (validation && !validate(name, 3)) return "";

            return name;
        }

        /// <summary>
        /// Adds the fullname plus gender identifier to list.
        /// </summary>
        private void AddNameToList()
        {
            string name = GetName(true);

            if (string.IsNullOrEmpty(name)) return;

            name += (radioMale.Checked ? " (M)" : " (F)");

            if (!listFamilyNames.Items.Contains(name))
                listFamilyNames.Items.Add(name);

            textForename.Text = "";
            textSurname.Text = "";
        }

        /// <summary>
        /// Validate the names for null and empty values invalid names will not be added to list
        /// the function also checks the length of the fullname as it need to fit into an allocated
        /// space, a long name may get truncated to achive this.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool validate(string name, int i)
        {
            bool empty = String.IsNullOrEmpty(name);
            if (empty)
            {
                string s = (i == 1 ? "forename": (i == 2 ? "surname" : "details"));
                MessageBox.Show($"Missing {s} you'll need to enter both the forename and surname.",
                    ApplicationControl.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (i == 3 && name.Length > 23)
            {
                MessageBox.Show($"\"{name}\" is quite a long name, it might be best to preview the screen saver, the name may need shortening to display correctly.",
                    ApplicationControl.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return !empty;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listFamilyNames.SelectedIndex == -1)
            {
                MessageBox.Show("Select a name from the list first.",
                ApplicationControl.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string name = GetName(false) + (radioMale.Checked ? " (M)" : " (F)");
                bool clear = listFamilyNames.Text.Equals(name);

                listFamilyNames.Items.RemoveAt(listFamilyNames.SelectedIndex);

                if (clear)
                {
                    textForename.Text = "";
                    textSurname.Text = "";
                }
            }
        }

        private void trackbarDefaultPeople_Scroll(object sender, EventArgs e)
        {
            labelDefaultPeople.Text = trackbarDefaultPeople.Value.ToString();
        }

        private void listFamilyNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = listFamilyNames.Text;
            bool gender = name.Contains("(M)");
            radioMale.Checked = gender;
            radioFemale.Checked = !gender;

            name = name.Replace(" (M)", "").Replace(" (F)", "");
            int idx = name.IndexOf(" ");

            if (idx != -1)
            {
                textForename.Text = name.Substring(0, idx);
                textSurname.Text = name.Substring(idx + 1);
            }
        }
    }
}
