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
    /// Manages any family event that occures.
    /// </summary>
    public class FamilyEventManager
    {
        static Dictionary<int, Action<FeudEventArgs>> FamilyEvents = new();
        static Dictionary<int, Action<FeudEventArgs>> ChildEvents = new();

        public void Subscribe(IFamilyEvents person)
        {
            if (FamilyEvents.ContainsKey(person.family))
                FamilyEvents[person.family] += person.FamilyEvent;
            else
                FamilyEvents.Add(person.family, person.FamilyEvent);
            
            if (person.mother > -1)
            {
                if (ChildEvents.ContainsKey(person.mother))
                    ChildEvents[person.mother] += person.ChildEvent;
                else
                    ChildEvents.Add(person.mother, person.ChildEvent);
            }
            
            if (person.father > -1)
            {
                if (ChildEvents.ContainsKey(person.father))
                    ChildEvents[person.father] += person.ChildEvent;
                else
                    ChildEvents.Add(person.father, person.ChildEvent);
            }
        }
        public void Unsubscribe(IFamilyEvents person)
        {
            try
            {
                if (FamilyEvents.ContainsKey(person.family))
                    FamilyEvents[person.family] -= person.FamilyEvent;
            } 
            catch { }
            if (person.mother > -1)
            {
                try
                {
                    if (ChildEvents.ContainsKey(person.mother))
                        ChildEvents[person.mother] -= person.ChildEvent;
                } catch { }
            }
            if (person.father > -1)
            {
                try
                {
                    if (ChildEvents.ContainsKey(person.father))
                        ChildEvents[person.father] -= person.ChildEvent;
                } catch { }
            }
        }

        public void Invoke(Person person)
        {
            if (FamilyEvents.ContainsKey(person.family))
                FamilyEvents[person.family]?.Invoke(new FeudEventArgs(person));
        }
            
        public void InvokeChildren(Person person)
        {
            if (ChildEvents.ContainsKey(person.id))
                ChildEvents[person.id]?.Invoke(new FeudEventArgs(person));
        }
    }
}