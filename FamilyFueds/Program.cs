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

namespace FamilyFueds
{
    internal static class Program
    {
        // Family Feud Global Constants
        public const string WIN_REG_PATH = "SOFTWARE\\FamilyFeuds";

        // MessageBox Title
        public static string messageTitle = "Family Feuds";

        // Family Feud Global Settings
        public static int MaxHeight;  // Screen dimensions.
        public static int MaxWidth;
        public static int NumberOfPeople = 0;                     // Number of bots on screen.  
        public static List<Person> family = new List<Person>();   // FamilyFeuds bot data.
        public static List<string> names = new List<string>();    // Custom family names from windows registary.
        public static List<string> surnames = new List<string>(); // Used to generate a custom family id.
        public static ExecuteMode Mode;

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
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            LoadSettings();

            if (args.Length > 0)
            {
                string arg1 = args[0].ToLower().Trim();
                string arg2 = "";

                // Handle cases where arguments are separated by colon. example: /c:[attribute] or /P:[attribute]
                if (arg1.Length > 2)
                {
                    arg2 = arg1.Substring(3).Trim();
                    arg1 = arg1.Substring(0, 2);
                } else if (args.Length > 1) 
                    arg2 = args[1];

                switch (arg1)
                {
                    case "/c": Configure(); break;    // Configuration mode
                    case "/p": Preview(arg2); break;  // Preview mode
                    case "/s": Run(); break;          // Full-screen mode
                    default:                          // Undefined argument
                        {
                        MessageBox.Show("Command line argument \"" + arg1 + "\" is not valid.",
                            messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                }
            } else 
                Configure(); // No arguments found
        }

        /// <summary>
        /// The settings are retrieved from the Windows registary at path specified by the gobal
        /// constant <see cref="=WIN_REG_PATH"/>
        /// </summary>
        static void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(WIN_REG_PATH);

            if (key != null)
            {
                string familyNames = (string) key.GetValue("FamilyNames");

                if (familyNames != null && familyNames.Length > 0) {
                    int idx = familyNames.IndexOf(".");
                    do
                    {
                        string name = familyNames.Substring(0, idx);
                        names.Add(name);
                        familyNames = familyNames.Substring(idx + 1);
                        idx = familyNames.IndexOf(".");
                    } while (idx > 0);

                }
            }
        }

        /// <summary>
        /// Run in configuration mode allows customization of the FamilyFeud screen saver such as
        /// having own family and friends appear on the screen.
        /// </summary>
        static void Configure()
        {
            Mode = ExecuteMode.Configure;
            Application.Run(new FamilyFeudSettings());
        }

        /// <summary>
        /// Run in preview mode allows someone to preview FamilyFeuds screen saver before actually 
        /// changing the windows screen saver.
        /// </summary>
        static void Preview(string argument)
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
        static void Run()
        {
            Mode = ExecuteMode.FullScreen;

            foreach (Screen screen in Screen.AllScreens)
                new FamilyFeudsForm(screen.Bounds).Show();

            Application.Run();
        }
    }
}