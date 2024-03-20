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
    /// <summary>
    /// Handles any family events that happen.
    /// </summary>
    public class FamilyEventManager
    {
        static Dictionary<int, Action<FeudEventArgs>> FamilyEvents = new();

        public void Subscribe(IFamilyEvents handler)
        {
            if (FamilyEvents.ContainsKey(handler.family))
                FamilyEvents[handler.family] += handler.FamilyEvent;
            else
                FamilyEvents.Add(handler.family, handler.FamilyEvent);
        }
        public void Unsubscribe(IFamilyEvents handler)
        {
            if (FamilyEvents.ContainsKey(handler.family))
                FamilyEvents[handler.family] -= handler.FamilyEvent;
        }

        public void Invoke(Person person) => FamilyEvents[person.family].Invoke(new FeudEventArgs(person));
    }
}