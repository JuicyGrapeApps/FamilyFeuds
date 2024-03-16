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
        public static string ComputerName = "";
        public static string PersonName = "";
        public static int NumberOfPeople = 0;
        public static bool previewMode = false;
        public static bool configMode = false;
        public static int MaxHeight;
        public static int MaxWidth;
        public static List<Person> family = new List<Person>();
        public static List<string> names = new List<string>();
        public static List<string> surnames = new List<string>();

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

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string? secondArgument = null;

                // Handle cases where arguments are separated by colon. example: /c:[attribute] or /P:[attribute]
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                    secondArgument = args[1];

                if (firstArgument == "/c") Configure();  // Configuration mode
                else if (firstArgument == "/p") Preview(secondArgument);     // Preview mode
                else if (firstArgument == "/s") Run();     // Full-screen mode
                else    // Undefined argument
                {
                    MessageBox.Show("Command line argument \"" + firstArgument +"\" is not valid.",
                        messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else Configure();   // No arguments found
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
            configMode = true;
            previewMode = false;
            LoadSettings();
            Application.Run(new FamilyFeudSettings());
        }

        /// <summary>
        /// Run in preview mode allows someone to preview FamilyFeuds screen saver before actually 
        /// changing the windows screen saver.
        /// </summary>
        static void Preview(string argument)
        {
            previewMode = true;
            configMode = false;

            if (argument == null)
            {
                MessageBox.Show("Expected window handle was not provided.",
                    messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            LoadSettings();
            IntPtr previewWndHandle = new IntPtr(long.Parse(argument));
            Application.Run(new FamilyFeudsForm(previewWndHandle));
        }

        /// <summary>
        /// Run the FamilyFeuds screen saver.  This function gets called automatically by Windows
        /// when it activates the screen saver.
        /// </summary>
        static void Run()
        {
            previewMode = false;
            configMode = false;
            LoadSettings();

            foreach (Screen screen in Screen.AllScreens)
            {
                FamilyFeudsForm screensaver = new FamilyFeudsForm(screen.Bounds);
                screensaver.Show();
            }
            Application.Run();
        }
    }
}