﻿/*
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

using JuicyGrapeApps.FamilyFueds;

public interface IFeudEvent : ITrashable
{
    int id { get; set; }
    int family { get; set; }
    int mother { get; set; }
    int father { get; set; }
    void FamilyEvent(FeudEventArgs args);
    void ChildEvent(FeudEventArgs args);
}