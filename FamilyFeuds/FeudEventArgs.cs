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
    public class FeudEventArgs
    {
        public IFeudEvent person;
        private FamilyEventManager.Flag flags;

        public FeudEventArgs(IFeudEvent person, FamilyEventManager.Flag flags = FamilyEventManager.Flag.None)
        {
            this.person = person;
            this.flags = flags;
        }

        public FeudEventArgs AddFlag(FamilyEventManager.Flag flag)
        {
            flags |= flag;
            return this;
        }
        public FeudEventArgs RemoveFlag(FamilyEventManager.Flag flag)
        {
            flags &= ~flag;
            return this;
        }

        public bool HasFlag(FamilyEventManager.Flag flag) => flags.HasFlag(flag);
    }
}