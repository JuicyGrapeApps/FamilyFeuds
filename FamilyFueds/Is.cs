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

using System;

/// <summary>
/// Global expression library.
/// </summary>
public static class Is
{
    /// <summary>
    /// Returns true if value is between from and to values.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static bool Between(int value, int from, int to) => value >= from && value <= to;

    /// <summary>
    /// Returns true if a point is between from and to points.
    /// </summary>
    /// <param name="value">Point to check</param>
    /// <param name="from">Location of first point</param>
    /// <param name="to">Location of second point</param>
    /// <returns></returns>
    public static bool PointBetween(Point value, Point from, Point to)
          => value.X >= from.X && value.Y >= from.Y && value.X <= to.X && value.Y <= to.Y;

    /// <summary>
    /// Returns true if two identical rectangles overlap each other.
    /// </summary>
    /// <param name="value">Point location of first rectangle</param>
    /// <param name="target">Point location of second rectangle</param>
    /// <param name="size">Size of rectangles</param>
    /// <returns></returns>
    public static bool Overlap(Point value, Point target, int size) =>
        Is.PointBetween(value, target, new Point(target.X += size, target.Y += size)) ||
        Is.PointBetween(target, value, new Point(value.X += size, value.Y += size));
}
