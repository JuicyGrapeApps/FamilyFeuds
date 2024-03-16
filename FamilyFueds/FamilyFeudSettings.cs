using Microsoft.Win32;

namespace FamilyFueds
{
    public partial class FamilyFeudSettings : Form
    {
        public FamilyFeudSettings()
        {
            InitializeComponent();

            foreach (string name in Program.names) listFamilyNames.Items.Add(name);
        }

        private void SaveSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Program.WIN_REG_PATH);
            string listOfNames = "";
            foreach (string name in listFamilyNames.Items) listOfNames += name+".";
            key.SetValue("FamilyNames", listOfNames);
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
                    Program.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return !empty;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listFamilyNames.SelectedIndex == -1)
            {
                MessageBox.Show("Select a name from the list first.",
                Program.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else listFamilyNames.Items.RemoveAt(listFamilyNames.SelectedIndex);
        }
    }
}
