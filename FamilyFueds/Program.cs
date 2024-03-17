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

namespace JuicyGrapeApps.FamilyFueds
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            ApplicationControl.Initialize();

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
                    case "/c": ApplicationControl.Configure(); break;    // Configuration mode
                    case "/p": ApplicationControl.Preview(arg2); break;  // Preview mode
                    case "/s": ApplicationControl.Run(); break;          // Full-screen mode
                    default:                          // Undefined argument
                        {
                        MessageBox.Show("Command line argument \"" + arg1 + "\" is not valid.",
                            ApplicationControl.messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                }
            } else 
                ApplicationControl.Configure(); // No arguments found
        }
    }
}