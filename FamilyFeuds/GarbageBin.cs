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
namespace JuicyGrapeApps.FamilyFeuds
{
    /// <summary>
    /// Global garbage collector and disposal class.
    /// </summary>
    public static class GarbageBin
    {
        public static event CoreEventHandler Garbage;
        private static List<IDisposable> trash = new List<IDisposable>();   // Store FamilyFeuds bot data until bin is emptied.
        public static bool isDirty = trash.Count > 0;
        
        public static void Add(Person person) => trash.Add(person);

        public static void Empty()
        {
            if (isDirty)
            {
                foreach (Person person in trash) person.Dispose();
                trash.Clear();
            }
        }

        /// <summary>
        /// Called on application shutdown and disposes of all relevant
        /// objects.
        /// </summary>
        public static void Dispose()
        {
            Empty();
            Garbage?.Invoke();
        }
    }
}