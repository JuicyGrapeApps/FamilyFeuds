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
    public class EventManager
    {
        public delegate void CollisionEventHandler(Person person, Person collider);
        public delegate void PersonEventHandler(Person person);
        public delegate void EmotionEventHandler(Person person, bool feeling);
        public delegate void FamilyEventHandler(Person person);
        public delegate void GarbageEventHandler();

        public event CollisionEventHandler Collision;
        public event CollisionEventHandler Argument;
        public event CollisionEventHandler Marrage;
        public event FamilyEventHandler Death;
        public event FamilyEventHandler Birth;
        public event PersonEventHandler Birthday;
        public event EmotionEventHandler Emotion;
        public event PersonEventHandler Idea;
        public event GarbageEventHandler Garbage;

        public enum Event
        {
            Collision,
            Argument,
            Marrage,
            Birth,
            Death,
            Birthday,
            Emotion,
            Idea,
        }

        public void Invoke(Event events, Person person) => Invoke(events, person, null, false);
        public void Invoke(Event events, Person person, bool feeling) => Invoke(events, person, null, feeling);

        /// <summary>
        /// Invoke event from the EventManager.
        /// </summary>
        /// <param name="events"></param>
        /// <param name="person"></param>
        /// <param name="collider"></param>
        /// <param name="feeling"></param>
        public void Invoke(Event events, Person person, Person? collider, bool feeling = false)
        {
            switch (events) 
            {
                case Event.Collision: Collision?.Invoke(person, collider); break; // Handled by FamilyFeudsForm.OnCollition
                case Event.Argument: Argument?.Invoke(person, collider); break; // Not handled or consumed
                case Event.Marrage: Marrage?.Invoke(person, collider); break; // Not handled or consumed
                case Event.Birth: Birth?.Invoke(person); break; // Handled by BotManager.OnCreateBot
                case Event.Death: Death?.Invoke(person); break; // Handled by BotManager.OnDestroyBot
                case Event.Birthday: Birthday?.Invoke(person); break; // Not handled or consumed
                case Event.Emotion: Emotion?.Invoke(person, feeling); break; // Not handled or consumed
                case Event.Idea: Idea?.Invoke(person); break; // Not handled or consumed
            }
        }

        /// <summary>
        /// Called on by ApplicationControl.Shutdown, this function invokes the Garbage event which
        /// all IDisposable objects should be subscribed to so their Dispose method gets called.
        /// </summary>
        public void Dispose() => Garbage?.Invoke();
    }
}