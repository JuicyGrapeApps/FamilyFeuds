using FamilyFueds;

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
    public static Point Location => new Point(rnd.Next(Program.MaxHeight + 1), rnd.Next(Program.MaxWidth + 1));

    /// <summary>
    /// Random integer generator this function returns a random signed or unsigned integer value upto the maximum
    /// value passed in parrameters
    /// </summary>
    /// <param name="value">The maximum integer value returned</param>
    /// <param name="offset">Add an offset to random result</param>
    /// <param name="unsigned">(Optional) bool True (Unsigned), False (Signed)</param>
    /// <returns>Integer</returns>
    public static int Int(int value, int offset, bool unsigned = false) => (rnd.Next(value + 1) + offset) * (unsigned && rnd.Next(2) == 1 ? -1: 1);

    /// <summary>
    /// Random surname generator this property returns either a random surname each time
    /// </summary>
    /// <returns>String (Surname)</returns>
    public static string Surname
    {
        get
        {
            string[] surnames = new string[17];
            surnames[0] = "Rogers";
            surnames[1] = "Jones";
            surnames[2] = "Peters";
            surnames[3] = "Harris";
            surnames[4] = "Baker";
            surnames[5] = "Smith";
            surnames[6] = "Williams";
            surnames[7] = "Shepard";
            surnames[8] = "Hills";
            surnames[9] = "Jacobs";
            surnames[10] = "Booth";
            surnames[11] = "Hicks";
            surnames[12] = "Adams";
            surnames[13] = "Johnson";
            surnames[14] = "Adle";
            surnames[15] = "Weston";
            surnames[16] = "Samson";

            family = rnd.Next(17);
            return surnames[family];
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
            forename[0] = "Mark";
            forename[1] = "Paul";
            forename[2] = "John";
            forename[3] = "James";
            forename[4] = "Frank";
            forename[5] = "Peter";
            forename[6] = "Will";
            forename[7] = "Bob";
            forename[8] = "Glen";
            forename[9] = "Kevin";
            forename[10] = "Rob";
            forename[11] = "Steven";
            forename[12] = "Stuwart";
            forename[13] = "Phil";
            forename[14] = "Tom";
            forename[15] = "Graham";
            forename[16] = "Michel";
            name = forename[rnd.Next(17)];
        }
        else
        {
            string[] forename = new string[17];
            forename[0] = "Sarah";
            forename[1] = "Jane";
            forename[2] = "Jackie";
            forename[3] = "Karen";
            forename[4] = "Amy";
            forename[5] = "Alison";
            forename[6] = "Rachel";
            forename[7] = "Debby";
            forename[8] = "Kate";
            forename[9] = "Jill";
            forename[10] = "Clare";
            forename[11] = "Helen";
            forename[12] = "Natalie";
            forename[13] = "Iris";
            forename[14] = "Paula";
            forename[15] = "Hillary";
            forename[16] = "Kathy";
            name = forename[rnd.Next(17)];
        }
        return name;
    }
}
