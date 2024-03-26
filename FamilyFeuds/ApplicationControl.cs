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

namespace JuicyGrapeApps.FamilyFeuds
{
    /// <summary>
    /// Global application controller class.
    /// </summary>
    internal static class ApplicationControl
    {
        // Family Feud Global Constants
        public const string WIN_REG_PATH = "SOFTWARE\\FamilyFeuds";
        public const string REG_KEY_NAMES = "FamilyNames";
        public const string REG_KEY_DEFAULT = "DefaultNames";
        public const int MAX_BOT_COUNT = 50;
        public const int CLEAR_COUNTDOWN = 30;
        public const int POPULATION_LIMIT = 20;

        // MessageBox Title
        public static string messageTitle = "Family Feuds";

        // Family Feud Global Settings
        public static int MaxHeight;  // Screen dimensions.
        public static int MaxWidth;
        public static int MaxDefaultNumber = 10; // Default people generated
        public static int UniqueBotId = 0;       // Bots Unique Id
        public static int NumberOfPeople = 0;    // Number of bots on screen.  
        public static List<Person> family = new List<Person>();   // FamilyFeuds bot data.
        public static List<string> names = new List<string>();    // Custom family names from windows registary.
        public static List<string> surnames = new List<string>(); // Used to generate a custom family id.
        public static ExecuteMode Mode;
        public static FamilyEventManager FamilyEvents = new FamilyEventManager();

        private static int m_elapsed = 0;
        private static DateTime m_time;
        private static int m_clear = CLEAR_COUNTDOWN;
        public static bool fireWorks = false;  // Display fireworks
        public static bool OverPopulated = false;   // Control Population Growth

        // FamilyFeuds Events
        public static event CoreEventHandler? Update;
        public static event PersonEventHandler? Collision;

        public enum ExecuteMode
        {
            FullScreen,
            Preview,
            Configure
        }

        /// <summary>
        ///  Find a person by id.
        /// </summary>
        public static Person? person(int id) => family.Find(x => x.id == id);

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
            return idx + 1000;  // Return value must be greater than length of surnames
                                // array in RandomGenerator to prevent conflicts.
        }

        /// <summary>
        /// Initialize screen saver the settings are retrieved from the Windows registary
        /// at path specified by the gobal constants <see cref="=WIN_REG_PATH"/> and also
        /// <see cref="=REG_KEY_NAMES"/> and <seealso cref="=REG_KEY_DEFAULTS"/>
        /// </summary>
        public static void Initialize()
        {
            try
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(WIN_REG_PATH);

                if (key != null)
                {
                    var regNames = key.GetValue(REG_KEY_NAMES);
                    string familyNames = (regNames == null) ? "" : (string)regNames;

                    if (!String.IsNullOrEmpty(familyNames))
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
                    var regPeople = key.GetValue(REG_KEY_DEFAULT);
                    string defaultPeople = (regPeople == null) ? "" : (string)regPeople;
                    MaxDefaultNumber = String.IsNullOrEmpty(defaultPeople) ? 10 : int.Parse(defaultPeople);
                }
            } catch { }
        }

        /// <summary>
        /// Run in configuration mode allows customization of the FamilyFeud screen saver such as
        /// having own family and friends appear on the screen.
        /// </summary>
        public static void Configure()
        {
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
        /// Main program loop that updates bots and draws them on screen.
        /// </summary>
        /// <param name="form"></param>
        public static void RefreshScreenSaver(Form form)
        {
            try
            {
                FamilyFeudsForm familyFeud = (FamilyFeudsForm)form;

                if (fireWorks) familyFeud.FireworkDisplay();
                else 
                {
                    OverPopulated = NumberOfPeople > POPULATION_LIMIT;

                    int familyId = -1;
                    bool isWinner = true;
                    for (int i = 0; i < NumberOfPeople; i++)
                    {
                        Person person = family[i];
                        familyFeud.Draw(person);
                        Collision?.Invoke(person);

                        if (familyId != person.family)
                        {
                            if (familyId != -1) isWinner = false;
                            familyId = person.family;
                        }
                    }

                    int elapsed = (DateTime.Now - m_time).Seconds;
                    if (elapsed == m_elapsed && m_clear > 0) return;
                    m_time = DateTime.Now;

                    if (isWinner)
                    {
                        fireWorks = true;
                        m_clear = -1;
                        GarbageBin.Dispose();
                    }
                    else
                    {
                        m_clear--;
                        Update?.Invoke();
                        GarbageBin.Empty();
                    }

                    if (m_clear < 0)
                    {
                        m_clear = CLEAR_COUNTDOWN;
                        familyFeud.graphics.Clear(Color.Black);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Restarts the screen saver after fireworks display is over.
        /// </summary>
        public static void Restart()
        {
            family.Clear();
            UniqueBotId = 0;
            NumberOfPeople = 0;
            InitializeBots();
            m_clear = -1;
            fireWorks = false;
        }

        public static void InitializeBots()
        {
            // Create custom bots by invoking Birth event which is subscribed
            // to by the OnCreate in the BotManager.
            foreach (string fullname in names)
            {
                string name = fullname;
                bool gender = name.Contains("(M)");
                name = name.Replace(" (M)", "").Replace(" (F)", "");

                int idx = name.IndexOf(" ");

                if (idx != -1)
                {
                    string forename = name.Substring(0, idx);
                    string surname = name.Substring(idx + 1);
                    family.Add(new Person(forename, surname, gender, familyIndex(surname)));
                }
            }

            int numberOfPeople = NumberOfPeople;

            // Create default bots by invoking Birth event which is subscribed
            // to by the OnCreate in the BotManager.
            for (int i = numberOfPeople; i < numberOfPeople + MaxDefaultNumber; i++)
                family.Add(new Person());
        }

        /// <summary>
        /// Application shutdown requested, this function invokes the Garbage event which
        /// all IDisposable objects should be subscribed to so their Dispose method gets called.
        /// </summary>
        public static void Shutdown()
        {
            GarbageBin.Dispose();
            Application.Exit();
        }
    }
}