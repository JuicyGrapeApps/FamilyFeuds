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
using JuicyGrapeApps.FamilyFueds;

/// <summary>
/// Global random generator
/// </summary>
public static class RandomGenerator
{
    public static int family;

    private static Random rnd = new Random();

    /// <summary>
    /// Random gender generator this function returns either a male/femle gender
    /// depending on parameter passed to it.
    /// </summary>
    /// <returns>Boolean True (Male), False (Female)</returns>
    public static bool Gender => rnd.Next(2) == 0;


    /// <summary>
    /// Random location generator this function returns random point on the screen.
    /// </summary>
    /// <returns>Point (Screen Location)</returns>
    public static Point Location => new Point(rnd.Next(ApplicationControl.MaxWidth + 1), rnd.Next(ApplicationControl.MaxHeight + 1));

    /// <summary>
    /// Random integer generator this function returns a random signed or
    /// unsigned integer value upto the maximu value passed in parameters.
    /// </summary>
    /// <param name="value">The maximum integer value returned</param>
    /// <param name="offset">Add an offset to random result</param>
    /// <param name="unsigned">(Optional) bool True (Unsigned), False (Signed)</param>
    /// <returns>Integer</returns>
    public static int Int(int value, int offset = 0, bool unsigned = false) =>
        (rnd.Next(value + 1) + offset) * (unsigned && rnd.Next(2) == 1 ? -1: 1);

    /// <summary>
    /// Random floating point generator this function returns a random
    /// signed or unsigned floating point value upto the maximum value
    /// passed in parameters.
    /// </summary>
    /// <param name="value">The maximum float value returned</param>
    /// <param name="offset">Add an offset to random result</param>
    /// <param name="unsigned">(Optional) bool True (Unsigned), False (Signed)</param>
    /// <returns>Integer</returns>
    public static double Float(double value, double offset = 0, bool unsigned = false) => 
        (rnd.NextDouble() * value + offset) * (unsigned && rnd.Next(2) == 1 ? -1 : 1);

    /// <summary>
    /// Returns an idea from person 's brain.
    /// </summary>
    /// <returns></returns>
    public static Person.Ideas Idea()
    {
        Person.Ideas idea = Person.Ideas.ChangeDirection;
        idea = Person.Brain[Int(2)];
        int react = (int)idea;
        if (react > 0 && Int(react) != 0) idea = Person.Ideas.ChangeDirection;
        return idea;
    }

    /// <summary>
    /// Random surname generator this property returns either a random surname each time
    /// </summary>
    /// <returns>String (Surname)</returns>
    public static string Surname
    {
        get
        {
            string[] surnames = new string[17];
            surnames[0] = "Smith";
            surnames[1] = "Jones";
            surnames[2] = "Johnson";
            surnames[3] = "Harris";
            surnames[4] = "Baker";
            surnames[5] = "Green";
            surnames[6] = "Williams";
            surnames[7] = "Roberts";
            surnames[8] = "Lewis";
            surnames[9] = "Phillips";
            surnames[10] = "Booth";
            surnames[11] = "Hicks";
            surnames[12] = "Adams";
            surnames[13] = "Richards";
            surnames[14] = "Edwards";
            surnames[15] = "Weston";
            surnames[16] = "Evans";

            family = rnd.Next(17);
            return surnames[family];
        }
    }

    /// <summary>
    /// Random color generator
    /// </summary>
    public static Color Color
    {
        get
        {
            Color color = Color.LightYellow;
            switch (Int(10))
            {
                case 1:
                    color = Color.Tomato;
                    break;
                case 2:
                    color = Color.LightBlue;
                    break;
                case 3:
                    color = Color.LightGreen;
                    break;
                case 4:
                    color = Color.LightSalmon;
                    break;
                case 5:
                    color = Color.LightPink;
                    break;
                case 6:
                    color = Color.Orange;
                    break;
                case 7:
                    color = Color.Lime;
                    break;
                case 8:
                    color = Color.Magenta;
                    break;
                case 9:
                    color = Color.MediumAquamarine;
                    break;
                case 10:
                    color = Color.MediumSpringGreen;
                    break;
            }
            return color;
        }
    }
    /// <summary>
    /// Random name generator this function returns either a male or female forename
    /// depending on parameter passed to it.
    /// </summary>
    /// <param name="male">True = Male Forename, False = Female Forename</param>
    /// <returns>String (Forename)</returns>
    public static string Forename(bool male)
    {
        string name;

        if (male) {
            string[] forename = new string[17];
            forename[0] = "John";
            forename[1] = "Simon";
            forename[2] = "James";
            forename[3] = "Peter";
            forename[4] = "Frank";
            forename[5] = "William";
            forename[6] = "Tony";
            forename[7] = "Bob";
            forename[8] = "Glen";
            forename[9] = "Kevin";
            forename[10] = "Robert";
            forename[11] = "Steven";
            forename[12] = "Stuwart";
            forename[13] = "Philip";
            forename[14] = "Bill";
            forename[15] = "Graham";
            forename[16] = "Michael";
            name = forename[rnd.Next(17)];
        }
        else
        {
            string[] forename = new string[17];
            forename[0] = "Sarah";
            forename[1] = "Jane";
            forename[2] = "Jackie";
            forename[3] = "Sofia";
            forename[4] = "Amy";
            forename[5] = "Alice";
            forename[6] = "Rachel";
            forename[7] = "Anna";
            forename[8] = "Katie";
            forename[9] = "Jill";
            forename[10] = "Lucy";
            forename[11] = "Chloe";
            forename[12] = "Natalie";
            forename[13] = "Susan";
            forename[14] = "Emily";
            forename[15] = "Hallie";
            forename[16] = "Rose";
            name = forename[rnd.Next(17)];
        }
        return name;
    }
}
