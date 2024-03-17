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
            SaveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string name = textForename.Text.Replace(" ", "").ToLower();
            if (!validate(name)) return;
            name = name.Substring(0,1).ToUpper()+ name.Substring(1);
            string surname = textSurname.Text.Replace(" ", "").ToLower();
            if (!validate(surname)) return;
            surname = surname.Substring(0, 1).ToUpper() + surname.Substring(1);
            name += " " + surname + (radioMale.Checked ? " (M)": " (F)");
            name = name.Replace(".", "");
            if (!listFamilyNames.Items.Contains(name)) 
                listFamilyNames.Items.Add(name);
        }

        private bool validate(string name)
        {
            bool empty = String.IsNullOrEmpty(name);
            if (empty)
            {
                MessageBox.Show("Missing details need both the forename and surname.",
                    ApplicationControl.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else listFamilyNames.Items.RemoveAt(listFamilyNames.SelectedIndex);
        }

        private void trackbarDefaultPeople_Scroll(object sender, EventArgs e)
        {
            labelDefaultPeople.Text = trackbarDefaultPeople.Value.ToString();
        }
    }
}
