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
using System.Numerics;

namespace JuicyGrapeApps.Core
{
    /// <summary>
    /// Global expressions library
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
        internal static bool Between(int value, int from, int to) => value >= from && value <= to;

        /// <summary>
        /// Moves a point a certain distance towards another point, the
        /// discance covered is passed in the move parameter as a decimal
        /// pacentage 0 - (0%) equals origin point and 1 - (100%) equals
        /// target point, for example: 0.5f - (50%) would equal halfway
        /// between the origin point and target point. 
        /// </summary>
        /// <param name="origin">Point of origin</param>
        /// <param name="target">Point of destination</param>
        /// <param name="move">The distance range from 0 to 1</param>
        /// <param name="cap">Cap the rage between 0 and 1</param>
        /// <returns>Any given point beween the two points</returns>
        internal static Point Towards(Point origin, Point target, float move, bool cap = true)
        {
            if (cap) move = (move < 0) ? 0 : (move > 1) ? 1 : move;
            Vector2 vector = Vector2.Lerp(new Vector2(origin.X, origin.Y),
                                          new Vector2(target.X, target.Y), move);
            return new Point((int)vector.X, (int)vector.Y);
        }

        /// <summary>
        /// Returns true if two identical rectangles overlap each other.
        /// </summary>
        /// <param name="value">Point location of first rectangle</param>
        /// <param name="target">Point location of second rectangle</param>
        /// <param name="size">Size of rectangles</param>
        /// <returns></returns>
        internal static bool Overlap(Point value, Point target, Size size) =>
            !Rectangle.Intersect(new Rectangle(value, size),
                                 new Rectangle(target, size)).IsEmpty;
    }
}