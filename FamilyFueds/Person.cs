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
using JuicyGrapeApps.FamilyFueds;
using System.Diagnostics;


/// <summary>
/// This class is the bot representation of a person on screen it stores their details such as date of birth,
/// emotional state, intellegence or family ties it also calculates things like a person's age, movement and
/// even how long it takes to have an idea or recuperate from emotional damage.
/// </summary>
public class Person
{
    // Emotional states
    public enum Emotion
    {
        None,
        Love,
        Happy,
        Party,
        Sad,
        Angry,
        Jealous,
        Injured,
        Baby,
        Angel,
        Devil
    }

    public int id;
    public int family;
    public string name;
    public string surname;
    public DateTime dob;
    public Point location;
    public Point volocity;
    public Image image;
    public bool gender;
    public int married = -1;
    public int father = -1;
    public int mother = -1;
    public int ignore = -1;
    public int lookat = -1;
    public Point[] motherLine = new Point[4];
    public Point[] fatherLine = new Point[4];
    private int m_age;
    private int m_energy = 5;
    private Emotion m_emotion;
    private DateTime m_recoverTime;
    private DateTime m_brainActivity;
    private bool m_recovered = true;
    private int m_intellegence = 10;
    private int m_emotional = 5;
    private bool m_thinking = false;
    private bool m_killer = false;
    private bool m_follow = false;
    public int followed = -1;

    // Boolean repesentations of emotional states
    public bool isInjured => m_emotion == Emotion.Injured;
    public bool isEmotional => m_emotion != Emotion.None && 
                               m_emotion != Emotion.Happy &&
                               m_emotion != Emotion.Party;
    public bool isAngry => m_emotion == Emotion.Angry;
    public bool isBaby => m_emotion == Emotion.Baby;
    public bool isDead => m_emotion == Emotion.Angel ||
                          m_emotion == Emotion.Devil;

    // Randomize volocity changes direction and speed of the bot on screen.
    public void ChangeVolocity() => volocity = new Point(RandomGenerator.Int(2, 1, true), RandomGenerator.Int(2, 1, true));
    public string fullname => name + " " + surname;
    public int age => (DateTime.Now - dob).Minutes;

    public bool follow
    {
        get => m_follow && lookat > -1;
        set
        {
            followed = lookat;
            if (value) value = lookat > -1;
            else lookat = -1;
            m_follow = value;
        }
    }

    /// <summary>
    /// Used to calculate how long it takes in seconds to recover from emotional damage
    /// </summary>
    public bool recovered
    {
        get
        {
            if (!m_recovered) m_recovered = m_emotional - (DateTime.Now - m_recoverTime).Seconds < 0;
            if (m_recovered && ignore == -2) ignore = -1;
            return m_recovered;
        }
        set
        {
            m_recovered = value;
            if (!m_recovered) m_recoverTime = DateTime.Now;
            if (value) follow = false;
        }
    }

    /// <summary>
    /// Used to calculate how long it takes in seconds for an idea to occure
    /// </summary>
    public bool idea
    {
        get
        {
            if (m_thinking) m_thinking = m_intellegence - (DateTime.Now - m_brainActivity).Seconds > 0;
            return !m_thinking;
        }
        set
        {
            m_thinking = value;
            if (m_thinking) m_brainActivity = DateTime.Now;
        }
    }

    /// <summary>
    /// Energy of the person a person will expire if energy reaches zero.
    /// </summary>
    public int energy
    {
        get => m_energy;
        set
        {
            m_energy = value;
            if (m_energy < 1) Died();
        }
    }

    /// <summary>
    /// Change a persons emotional state.
    /// </summary>
    public Emotion emotion
    {
        get => m_emotion;
        set
        {
            if (isDead) return;
            recovered = true;
            m_emotional = 5;
            m_emotion = value;

            switch (m_emotion)
            {
                case Emotion.None:
                    image = gender ? FamilyFueds.Properties.Resources.Male : FamilyFueds.Properties.Resources.Female;
                    break;
                case Emotion.Love:
                    image = FamilyFueds.Properties.Resources.Love;
                    break;
                case Emotion.Happy:
                    image = FamilyFueds.Properties.Resources.Happy;
                    break;
                case Emotion.Party:
                    image = FamilyFueds.Properties.Resources.Party;
                    break;
                case Emotion.Sad:
                    image = FamilyFueds.Properties.Resources.Sad;
                    break;
                case Emotion.Angry:
                    m_emotional = 20;
                    image = FamilyFueds.Properties.Resources.Angry;
                    break;
                case Emotion.Jealous:
                    image = FamilyFueds.Properties.Resources.Jealous;
                    break;
                case Emotion.Injured:
                    image = FamilyFueds.Properties.Resources.Injured;
                    break;
                case Emotion.Baby:
                    image = FamilyFueds.Properties.Resources.Baby;
                    break;
                case Emotion.Angel:
                    image = FamilyFueds.Properties.Resources.Angel;
                    break;
                case Emotion.Devil:
                    image = FamilyFueds.Properties.Resources.Devil;
                    break;
            }
            if (isEmotional) recovered = false;
        }
    }

    /// <summary>
    /// Constructor used for a custom person
    /// </summary>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="gender"></param>
    /// <param name="family"></param>
    public Person(string name, string surname, bool gender, int family)
    {
        id = ApplicationControl.NumberOfPeople;
        ApplicationControl.NumberOfPeople++;

        this.gender = gender;
        this.name = name;
        dob = DateTime.Now;

        this.surname = surname;
        this.family = family;

        location = RandomGenerator.Location;
        emotion = Emotion.None;

        ChangeVolocity();
        m_intellegence = RandomGenerator.Int(20, 5);
        idea = true;
    }

    /// <summary>
    /// Constructor used for a default person
    /// </summary>
    /// <param name="person"></param>
    public Person(Person person = null)
    {
        id = ApplicationControl.NumberOfPeople;
        ApplicationControl.NumberOfPeople++;
        gender = RandomGenerator.Gender;
        name = RandomGenerator.Forename(gender);
        dob = DateTime.Now;

        if (person == null)
        {
            surname = RandomGenerator.Surname;
            family = RandomGenerator.family;
            location = RandomGenerator.Location;
            emotion = Emotion.None;
        }
        else
        {
            surname = person.surname;
            family = person.family;

            if (person.gender)
            {
                mother = person.married;
                father = person.id;
            }
            else
            {
                mother = person.id;
                father = person.married;
            }

            location = ApplicationControl.family[mother].location;

            emotion = Emotion.Baby;

            if (ApplicationControl.DEBUG_MODE) Debug.Print(ApplicationControl.family[mother].fullname + " gave birth to " + fullname);

            Birthday();
        }

        ChangeVolocity();
        m_intellegence = RandomGenerator.Int(20, 5);
        idea = true;
    }

    /// <summary>
    /// Called after a collition event, this function preforms a marrage between the people who met depending
    /// on certain criteria.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public bool Marry(Person person)
    {
        if (isEmotional || person.isEmotional || isInjured || person.isInjured ||
            person.family == family || person.gender == gender || married + person.married != -2) return !isEmotional && !person.isEmotional && married == person.id;

        married = person.id;
        person.married = id;

        if (gender)
        {
            person.family = family;
            person.surname = surname;
        }
        else
        {
            family = person.family;
            surname = person.surname;
        }
        return true;
    }

    /// <summary>
    /// Change entire families emotional state and sets any reaction to the emotion.
    /// </summary>
    /// <param name="familyEmotion"></param>
    /// <param name="excludeAllEmotions"></param>
    /// <param name="familyId"></param>
    public void FamilyEmotional(Emotion familyEmotion, bool excludeAllEmotions = false, int familyId = -1)
    {
        bool retribution = familyId != -1;
        if (!retribution) familyId = family;

        List<Person> mob;

        if (excludeAllEmotions) mob = ApplicationControl.family.FindAll(x => x.family == familyId && !isEmotional);
        else mob = ApplicationControl.family.FindAll(x => x.family == familyId && !x.isDead && !x.isInjured && !x.isBaby);

        foreach (Person person in mob)
        {
            person.emotion = familyEmotion;

            if (retribution)
            {
                person.lookat = id;
                person.follow = true;
            }
        }
    }

    /// <summary>
    /// Called on every screen update <see cref="FamilyFeudsForm.Update(Person)"/>
    /// </summary>
    public void Update()
    {
        if (isDead) return;
        if (m_age != age)
        {
            m_age = age;
            Birthday();
        }
        if (!m_recovered && recovered) Recovered();
        if (m_thinking && idea) Idea();
    }

    /// <summary>
    /// Called after a collition event, this function preforms a argument between the people who met depending
    /// on certain criteria.
    /// </summary>
    /// <param name="person"></param>
    public void Fight(Person person)
    {
        if (isDead || isInjured || person.isBaby || person.family == family) return;

        if (lookat == person.id) follow = false;

        if (person.gender == gender) {
            if (person.energy == 1)
            {
                person.energy = -1;
                emotion = Emotion.Sad;
                m_killer = true;
                FamilyEmotional(Emotion.Angry, false, person.family);
            }
            else
            {
                person.energy--;
                person.emotion = Emotion.Injured;
            }
        }
        else if (person.married > -1)
        {
            Person spouse = ApplicationControl.family[person.married];
            if (!spouse.isInjured)
            {
                spouse.emotion = Emotion.Jealous;
                spouse.lookat = id;
                spouse.follow = true;
            }
        }
    }

    /// <summary>
    /// Called when a person is over their emotion.
    /// </summary>
    private void Recovered()
    {
        emotion = isBaby ? Emotion.Happy: Emotion.None;
    }

    /// <summary>
    /// Called every time a person has an idea, idea's occure after the person has been thinking,
    /// the amount of time it takes in seconds for a person to think depends on their intelligence.
    /// </summary>
    private void Idea()
    {
        if (ApplicationControl.DEBUG_MODE) Debug.Print(name + " had an idea!");
        if (isDead) return;
        else if (!isEmotional) ChangeVolocity();
        m_intellegence = RandomGenerator.Int(20, 5);
        idea = true;
    }

    /// <summary>
    /// Moves a bot on screen according to it's volocity.
    /// </summary>
    public void Move()
    {
        if (isInjured || ignore == -4) return;

        if (follow)
        {
            Person person = ApplicationControl.family[lookat];
            if (location.X < person.location.X) volocity.X = 2;
            if (location.X > person.location.X) volocity.X = -2;
            if (location.Y < person.location.Y) volocity.Y = 2;
            if (location.Y > person.location.Y) volocity.Y = -2;
        }

        location.X += volocity.X;
        location.Y += volocity.Y;

        if (ignore != -3) Contact();
        else if (location.Y < -50 || location.Y > ApplicationControl.MaxHeight + 100) ignore = -4;
    }

    /// <summary>
    /// Called on a persons birthday, the time factor is one year is equivalent to one minute.
    /// </summary>
    private void Birthday()
    {
        if (ApplicationControl.DEBUG_MODE) Debug.Print("It's " + fullname + " " + (m_age+1) + " birthday!");
        if (m_age == 0) ignore = -2;
        else if (m_age == 5) { energy--; if (!isEmotional) emotion = Emotion.Party; }
        else if (m_age == 10) { energy--; if (!isEmotional) emotion = Emotion.Party; }
        else if (m_age == 15) { energy--; if (!isEmotional) emotion = Emotion.Party; }
        else if (m_age == 20) { energy--; if (!isEmotional) emotion = Emotion.Party; }
        else if (m_age == 25) energy--;
    }

    /// <summary>
    /// Called when a person expires, factors that could cause this include old age or injuries sustained
    /// during an argument.
    /// </summary>
    private void Died()
    {
        volocity.X = 0;

        if (m_killer)
        {
            emotion = Emotion.Devil;
            volocity.Y = 3;
        }
        else
        {
            emotion = Emotion.Angel;
            volocity.Y = -3;
        }

        ignore = -3;

        if (married > -1) ApplicationControl.family[married].married = -1;
        married = -1;
        mother = -1;
        father = -1;

        if (energy == 0) FamilyEmotional(Emotion.Sad);
    }

    /// <summary>
    /// Check for contact with screen bounderies
    /// 
    /// <remarks>
    /// If event ever needed for screen collition use following:
    /// Send: A byte using 0 << bit, set the bits for each screen contact.
    /// Receive: The byte and decode with bool hasBit = (byte & (1 << bit)) != 0;
    /// </summary>
    private void Contact()
    {
        byte impact = 0;
        if (location.X < 0) { location.X = 0; volocity.X = 1; impact = 1 << 1; }
        if (location.Y < 0) { location.Y = 0; volocity.Y = 1; impact = 1 << 2; }
        if (location.X > ApplicationControl.MaxWidth) { location.X = ApplicationControl.MaxWidth; volocity.X = -1; impact = 1 << 3; }
        if (location.Y > ApplicationControl.MaxHeight) { location.Y = ApplicationControl.MaxHeight; volocity.Y = -1; impact = 1 << 4; }
    }

    /// <summary>
    /// Check for contact with other bots.
    /// </summary>
    /// <param name="person"></param>
    public void Contact(Person person)
    {
        if (person.id != id && !Is.AnyEqual(ignore, person.id, person.ignore, id) && !isDead && !person.isDead &&
           (Is.Between(person.location.X, location.X, location.X + 50) &&
            Is.Between(person.location.Y, location.Y, location.Y + 50) ||
            Is.Between(person.location.X + 50, location.X, location.X + 50) &&
            Is.Between(person.location.Y + 50, location.Y, location.Y + 50)))
        {
            ignore = person.id;
            ApplicationControl.Events.Invoke(EventManager.Event.Collision, this, person);
        }
        else if (ignore > -1 && Is.AnyEqual(ignore, person.id, person.ignore, id)) ignore = -1;
    }
}