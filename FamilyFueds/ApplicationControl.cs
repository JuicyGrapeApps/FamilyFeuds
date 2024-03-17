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
    internal static class ApplicationControl
    {
        // Family Feud Global Constants
        public const string WIN_REG_PATH = "SOFTWARE\\FamilyFeuds";
        public const string REG_KEY_NAMES = "FamilyNames";
        public const string REG_KEY_DEFAULT = "DefaultNames";
        public const bool DEBUG_MODE = false;

        // MessageBox Title
        public static string messageTitle = "Family Feuds";

        // Family Feud Global Settings
        public static int MaxHeight;  // Screen dimensions.
        public static int MaxWidth;
        public static int MaxDefaultNumber = 10;
        public static int NumberOfPeople = 0;                     // Number of bots on screen.  
        public static List<Person> family = new List<Person>();   // FamilyFeuds bot data.
        public static List<string> names = new List<string>();    // Custom family names from windows registary.
        public static List<string> surnames = new List<string>(); // Used to generate a custom family id.
        public static ExecuteMode Mode;
        public static EventManager Events = new EventManager();

        public enum ExecuteMode
        {
            FullScreen,
            Preview,
            Configure
        }

        /// <summary>
        ///  Retrive the unique family id or create one if family has none.
        /// </summary>
        public static int familyIndex(string surname)
        {
            int idx = surnames.IndexOf(surname);
            if (idx == -1)
            {
                idx = surnames.Count;
                surnames.Add(surname);
            }
            return idx + 1000;  // This must be greater than lenght of surnames array in RandomGenerator.
        }

        /// <summary>
        /// Initialise screen saver the settings are retrieved from the Windows registary
        /// at path specified by the gobal constants <see cref="=WIN_REG_PATH"/> and also
        /// <see cref="=REG_KEY_NAMES"/> and <see cref="=REG_KEY_DEFAULTS"/>
        /// </summary>
        public static void Initialize()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(WIN_REG_PATH);

                if (key != null)
                {
                    string familyNames = (string)key.GetValue(REG_KEY_NAMES);

                    if (familyNames != null && familyNames.Length > 0)
                    {
                        int idx = familyNames.IndexOf(".");
                        do
                        {
                            string name = familyNames.Substring(0, idx);
                            names.Add(name);
                            familyNames = familyNames.Substring(idx + 1);
                            idx = familyNames.IndexOf(".");
                        } while (idx > 0);

                    }

                    string defaultPeople = (string)key.GetValue(REG_KEY_DEFAULT);
                    MaxDefaultNumber = (defaultPeople == null) ? 10 : int.Parse(defaultPeople);
                }
            } catch { }
        }

        /// <summary>
        /// Run in configuration mode allows customization of the FamilyFeud screen saver such as
        /// having own family and friends appear on the screen.
        /// </summary>
        public static void Configure()
        {
            if (DEBUG_MODE) { Run(); return; }
            
            Mode = ExecuteMode.Configure;
            Application.Run(new FamilyFeudSettings());
        }

        /// <summary>
        /// Run in preview mode allows someone to preview FamilyFeuds screen saver before actually 
        /// changing the windows screen saver.
        /// </summary>
        public static void Preview(string argument)
        {
            if (argument == null)
            {
                MessageBox.Show("Expected window handle was not provided.",
                    messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Mode = ExecuteMode.Preview;
            IntPtr previewWndHandle = new IntPtr(long.Parse(argument));
            Application.Run(new FamilyFeudsForm(previewWndHandle));
        }

        /// <summary>
        /// Run in fullscreen mode the FamilyFeuds screen saver.  This function gets called automatically by 
        /// Windows when it activates the screen saver.
        /// </summary>
        public static void Run()
        {
            Mode = ExecuteMode.FullScreen;

            foreach (Screen screen in Screen.AllScreens)
                new FamilyFeudsForm(screen.Bounds).Show();

            Application.Run();
        }

        /// <summary>
        /// Application shutdown requested.
        /// </summary>
        public static void Shutdown()
        {
            Application.Exit();
        }
    }
}