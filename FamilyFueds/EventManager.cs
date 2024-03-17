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

        public event CollisionEventHandler Collision;
        public event CollisionEventHandler Argument;
        public event CollisionEventHandler Marrage;
        public event FamilyEventHandler Death;
        public event PersonEventHandler Birthday;
        public event EmotionEventHandler Emotion;
        public event PersonEventHandler Idea;

        public enum Event
        {
            Collision,
            Argument,
            Marrage,
            Death,
            Birthday,
            Emotion,
            Idea,
        }

        public void Invoke(Event events, Person person) => Invoke(events, person, null, false);
        public void Invoke(Event events, Person person, bool feeling) => Invoke(events, person, null, feeling);

        public void Invoke(Event events, Person person, Person? collider, bool feeling = false)
        {
            switch (events) 
            {
                case Event.Collision: Collision?.Invoke(person, collider); break;
                case Event.Argument: Argument?.Invoke(person, collider); break;
                case Event.Marrage: Marrage?.Invoke(person, collider); break;
                case Event.Death: Death?.Invoke(person); break;
                case Event.Birthday: Birthday?.Invoke(person); break;
                case Event.Emotion: Emotion?.Invoke(person, feeling); break;
                case Event.Idea: Idea?.Invoke(person); break;
            }
        }
    }
}