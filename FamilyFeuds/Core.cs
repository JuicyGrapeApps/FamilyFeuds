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
    /// Core library functions.
    /// </summary>
    public static class Scroll
    {
        private static Dictionary<int, int> arrayIndex = new Dictionary<int, int>();
        public static int lastIndex = 0;
        public static int length = 0;
        private static Action<int, int>? scan;

        // Scrolling action to perform.
        public enum ScrollingAction
        {
            Continuous,
            Directional,
            Targeted,
        }

        /// <summary>
        /// A singular function to scan any collection types like arrays and lists in either direction. 
        /// this function remembers the index of each collection scanned by storing it in a dictionary above.
        /// 
        /// Overloading methods <see cref="Collection(object, bool, ScrollingAction)"/>
        /// </summary>
        /// 
        /// <param name="collection"></param> -  The collection being scanned. 
        /// <param name="step"></param> - Scan through collections indicies at this rate, negative values scan backwards and a zero value will return current index. 
        /// <param name="action"></param> - Scrolling action to perform, Continuous - Scrolls continuosly looping from start to end or vica-versa, Directional stops when beginning or end reached.
        ///                                 and Targeted targets a specific index specified by step value.  <see cref="SetIndex(object, int)"/>
        ///
        /// Useage: Use this function instead of specifiying the index of a collection. 
        /// 
        /// <returns>Intager value of a collections index</returns>
        public static int Collection(object collection, int step = 1, ScrollingAction action = ScrollingAction.Continuous)
        {
            try
            {
                Type type = collection.GetType();
                length = ((int)(type.GetProperty("Length") ?? type.GetProperty("Count"))?.GetValue(collection)) - 1;

                if (length < 0) return 0;

                // Retrive collection's index from 'arrayIndex' dictionary or set index to zero
                int store = retriveIndex(collection, action);
                int index = store + step;

                if (step == 0 || (action != ScrollingAction.Continuous && (index < 0 || index > length))) return store;

                index = (index < 0) ? length : (index > length) ? 0 : index;
                // Save current collection's index in 'arrayIndex' dictionary
                storeIndex(collection, index);

                if (scan != null)
                    for (int idx = 0; idx < length; idx++) scan?.Invoke(index, idx);

                lastIndex = index;
                return index;
            }
            catch { return 0; }
        }

        internal static void OnRescanListener(Action<int, int> value) => scan = value;

        private static void storeIndex(object collection, int index)
        {
            // Store the objects HashCode only to prevent memory leaks
            int Id = collection.GetHashCode();
            arrayIndex[Id] = index;
        }

        private static int retriveIndex(object collection, ScrollingAction action)
        {
            if (action == ScrollingAction.Targeted) return 0;
            int Id = collection.GetHashCode();
            arrayIndex.TryGetValue(Id, out int index);
            return index;
        }

        #region - Simplified overload 
        /// Usage: Use this overload to simply scan upwards/backward though indexes.
        public static int Up(object collection) => Collection(collection, -1, ScrollingAction.Continuous);
        /// Usage: Use this overload to simply scan down/forward though indexes.
        public static int Down(object collection) => Collection(collection, 1, ScrollingAction.Continuous);
        /// Usage: Use this overload to simply scan downwards/forward though the children of a GameObject.
        public static int Collection(object collection, bool forward, ScrollingAction action = ScrollingAction.Continuous) => Collection(collection, forward ? 1 : -1, action);
        #endregion

        #region - Get/set the indexes of a collection
        /// Usage: Use this function to target an index in a collection.
        public static int GetIndex(object collection) => Collection(collection, 0);

        /// Usage: Use this function to target an index in a collection, use with lastIndex to sync different arrays with same lengths.
        public static void SetIndex(object collection, int index) => Collection(collection, index, ScrollingAction.Targeted);
    }
    #endregion

    /// <summary>
    /// Global expressions
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
        public static Point Towards(Point origin, Point target, float move, bool cap = true)
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
        public static bool Overlap(Point value, Point target, Size size) =>
            !Rectangle.Intersect(new Rectangle(value, size),
                                 new Rectangle(target, size)).IsEmpty;
    }
}