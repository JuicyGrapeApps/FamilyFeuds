using System;

public static class Is
{
    public static bool Between(int value, int from, int to) => value >= from && value <= to;
    public static bool AnyEqual(int value, int match, int value2, int match2) => value == match || value2 == match2;
}
