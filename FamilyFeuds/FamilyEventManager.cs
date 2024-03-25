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
    /// Manages any family event that occures.
    /// </summary>
    public class FamilyEventManager
    {
        static Dictionary<int, Action<FeudEventArgs>> FamilyEvents = new();
        static Dictionary<int, Action<FeudEventArgs>> ChildEvents = new();

        // Not implimented but benificial for future development.
        [Flags]
        public enum Flag
        {
            None     = 0x00000000,
            IncludeSiblings = 0x00000001,
            IncludeUnavailable = 0x00000002,
        }

        public void Subscribe(IFeudEvent person)
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
        public void Unsubscribe(IFeudEvent person)
        {
            if (FamilyEvents.ContainsKey(person.family))
            {
                try { FamilyEvents[person.family] -= person.FamilyEvent; } catch { }
            }

            if (person.mother > -1 && ChildEvents.ContainsKey(person.mother))
            {
                try { ChildEvents[person.mother] -= person.ChildEvent; } catch { }
            }

            if (person.father > -1 && ChildEvents.ContainsKey(person.father))
            {
                try { ChildEvents[person.father] -= person.ChildEvent; } catch { }
            }
        }

        /// <summary>
        /// Invoke the family event which calls the FamilyEvent function.
        /// on all family members.
        /// </summary>
        /// <param name="person">The person invoking the event</param>
        /// <param name="flag">Attach a flag to the event</param>
        public void Invoke(IFeudEvent person, Flag flag = Flag.None)
        {
            if (FamilyEvents.ContainsKey(person.family))
                FamilyEvents[person.family]?.Invoke(new FeudEventArgs(person, flag));
        }

        /// <summary>
        /// Invokes the child event which calls the ChildEvent function
        /// on all children of parent.
        /// </summary>
        /// <param name="person">The parent invoking the event</param>
        /// <param name="flag">Attach a flag to the event</param>
        public void InvokeChildren(IFeudEvent person, Flag flag = Flag.None)
        {
            if (ChildEvents.ContainsKey(person.id))
                ChildEvents[person.id]?.Invoke(new FeudEventArgs(person, flag));
        }
    }
}