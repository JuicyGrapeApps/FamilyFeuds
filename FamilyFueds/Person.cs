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
public class Person : IFamilyEvents
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
    public int family { get; set; }
    public string name;
    public string surname;
    public DateTime dob;
    public Point location;
    public Point volocity;
    public Image image;
    public Image mask;
    public bool gender;
    public int married = -1;
    public int father = -1;
    public int mother = -1;
    public int bumped = -1;
    public int lookat = -1;
    public Point[] motherLine = new Point[4];
    public Point[] fatherLine = new Point[4];
    private int m_age;
    private int m_energy = 5;
    private Emotion m_emotion;
    private int m_intelligence = 10;
    private int m_emotional = 5;
    private bool m_killer = false;
    private bool m_follow = false;
    public int followed = -1;
    public RectangleF bounds = new();
    public bool changeMask = false;

    // Boolean repesentations of emotional states
    public bool isInjured => m_emotion == Emotion.Injured;
    public bool isEmotional => m_emotion != Emotion.None && 
                               m_emotion != Emotion.Happy &&
                               m_emotion != Emotion.Party;
    public bool isAngry => m_emotion == Emotion.Angry;
    public bool isBaby => m_emotion == Emotion.Baby;
    public bool isDead => m_emotion == Emotion.Angel ||
                          m_emotion == Emotion.Devil;

    public bool isActive => m_emotion != Emotion.Angel &&
                            m_emotion != Emotion.Devil && 
                            m_emotion != Emotion.Injured;

    // Randomize volocity changes direction and speed of the bot on screen.
    public void ChangeVolocity() => volocity = new Point(RandomGenerator.Int(2, 1, true), RandomGenerator.Int(2, 1, true));
    public string fullname => name + " " + surname;
    public int age => (DateTime.Now - dob).Minutes;

    public bool follow
    {
        get => m_follow && lookat > -1 && !isDead;
        set
        {
            followed = lookat;
            if (value) value = lookat > -1;
            else lookat = -1;
            m_follow = value;
        }
    }

    /// <summary>
    /// Amount of energy a person has if energy reaches zero they expire.
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
    /// Change a person's emotional state.
    /// </summary>
    public Emotion emotion
    {
        get => m_emotion;
        set
        {
            if (isDead) return;
            follow = false;
            m_emotional = 5;
            m_emotion = value;
            changeMask = true;

            switch (m_emotion)
            {
                case Emotion.None:
                    image = gender ? FamilyFueds.Properties.Resources.Male : FamilyFueds.Properties.Resources.Female;
                    m_emotional = 0;
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
                    m_emotional = 8;
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
        }
    }

    /// <summary>
    /// Changes image mask, masks are used to clear image on screen.
    /// </summary>
    public void ChangeMask()
    {
        switch (m_emotion)
        {
            case Emotion.None:
                mask = gender ? FamilyFueds.Properties.Resources.Mask : FamilyFueds.Properties.Resources.FemaleMask;
                break;
            case Emotion.Party:
                mask = FamilyFueds.Properties.Resources.PartyMask;
                break;
            case Emotion.Sad:
                mask = FamilyFueds.Properties.Resources.SadMask;
                break;
            case Emotion.Angel:
                mask = FamilyFueds.Properties.Resources.AngelMask;
                break;
            case Emotion.Devil:
                mask = FamilyFueds.Properties.Resources.DevilMask;
                break;
            default:
                mask = FamilyFueds.Properties.Resources.Mask;
                break;
        }
        changeMask = false;
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
        this.gender = gender;
        this.name = name;
        this.surname = surname;
        this.family = family;
        emotion = Emotion.None;
        location = RandomGenerator.Location;
        Initialize();
    }

    /// <summary>
    /// Constructor used for a default person
    /// </summary>
    /// <param name="person"></param>
    public Person(Person? person = null)
    {
        gender = RandomGenerator.Gender;
        name = RandomGenerator.Forename(gender);

        if (person == null)
        {
            surname = RandomGenerator.Surname;
            family = RandomGenerator.family;
            emotion = Emotion.None;
            location = RandomGenerator.Location;
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

            person = ApplicationControl.person(mother);
            if (person == null) location = RandomGenerator.Location;
            else location = person.location;
            emotion = Emotion.Baby;

            if (ApplicationControl.DEBUG_MODE) Debug.Print(person.fullname + " gave birth to " + fullname);
        }

        Initialize();
    }

    /// <summary>
    /// Initialise a person with data common to both custom and default people.
    /// </summary>
    private void Initialize()
    {
        id = ApplicationControl.NumberOfPeople;
        ApplicationControl.NumberOfPeople++;

        dob = DateTime.Now;
        m_intelligence = RandomGenerator.Int(20, 5);
        ChangeVolocity();
        ChangeMask();
        Birthday();

        ApplicationControl.Update += Update;
        ApplicationControl.FamilyEvents.Subscribe(this);
        GarbageBin.Garbage += Dispose;
    }

    /// <summary>
    /// Called on every screen update <see cref="FamilyFeudsForm.Update(Person)"/>
    /// </summary>
    public void Update()
    {
        if (isDead) return;
        Birthday();
        Recover();
        Idea();
    }

    /// <summary>
    /// Moves a bot on screen according to it's volocity.
    /// </summary>
    public void Move()
    {
        if (isInjured) return;

        if (follow)
        {
            Person person = ApplicationControl.person(lookat);
            if (person == null) follow = false;
            else
            {
                if (location.X < person.location.X) volocity.X = 2;
                if (location.X > person.location.X) volocity.X = -2;
                if (location.Y < person.location.Y) volocity.Y = 2;
                if (location.Y > person.location.Y) volocity.Y = -2;
            }
        }

        location.X += volocity.X;
        location.Y += volocity.Y;

        if (!isDead) Contact();
        else if (location.Y < -50 || location.Y > ApplicationControl.MaxHeight + 100) Trash();
    }

    /// <summary>
    /// Called after a collition event, this function preforms a marrage between the people
    /// if certain criteria is met.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public async Task<bool> Marry(Person person)
    {
        if (isEmotional || person.isEmotional || person.mother == id ||
            person.father == id || person.family == family || person.gender == gender ||
            married + person.married != -2) return !isEmotional && !person.isEmotional && married == person.id;

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
    /// Called after a collition event, this function preforms a argument between the people who met depending
    /// on certain criteria.
    /// </summary>
    /// <param name="person"></param>
    public async Task<bool> Fight(Person person)
    {
        if (lookat == bumped) follow = false;

        if (!isActive || person.isDead || person.isBaby || person.family == family ||
            person.mother == id || person.father == id) return true;

        if (person.gender == gender || person.isAngry) {
            person.energy--;
            if (person.energy == 0)
            {
                m_killer = true;
                return false;
            }
            else person.emotion = Emotion.Injured;
        }
        else if (person.married > -1)
        {
            Person spouse = ApplicationControl.person(person.married);
            if (spouse == null) person.married = -1;
            else if (!spouse.isActive)
            {
                spouse.emotion = Emotion.Jealous;
                spouse.lookat = id;
                spouse.follow = true;
            }
        }
        return true;
    }

    /// <summary>
    /// Called when a person is over their emotion.
    /// </summary>
    private void Recover()
    {
        if (m_emotional == 0) return;
        m_emotional--;
        if (m_emotional < 0) emotion = isBaby ? Emotion.Happy: Emotion.None;
    }

    /// <summary>
    /// Called every time a person has an idea, idea's occure after the person has been thinking,
    /// the amount of time it takes in seconds for a person to think depends on their intelligence.
    /// </summary>
    private void Idea()
    {
        m_intelligence--;
        if (m_intelligence < 0)
        {
            if (!isEmotional) ChangeVolocity();
            m_intelligence = RandomGenerator.Int(20, 5);
        }
    }

    /// <summary>
    /// Called on a persons birthday, the time factor is one year is equivalent to one minute.
    /// </summary>
    private void Birthday()
    {
        if (m_age != age)
        {
            if (ApplicationControl.DEBUG_MODE) Debug.Print("It's " + fullname + " " + (m_age+1) + " birthday!");

            if (m_age == 5) { energy--; if (!isEmotional) emotion = Emotion.Party; }
            else if (m_age == 10) { energy--; if (!isEmotional) emotion = Emotion.Party; }
            else if (m_age == 15) { energy--; if (!isEmotional) emotion = Emotion.Party; }
            else if (m_age == 20) { energy--; if (!isEmotional) emotion = Emotion.Party; }
            else if (m_age == 25) energy--;
            m_age = age;
        }
    }

    /// <summary>
    /// Called when a person expires, factors that could cause this include old age or injuries sustained
    /// during an argument.
    /// </summary>
    private void Died()
    {
        volocity.X = 0;
        follow = false;
        if (m_killer)
        {
            emotion = Emotion.Devil;
            volocity.Y = 3;
        }
        else
        {
            emotion = Emotion.Angel;
            volocity.Y = -2;
        }
        if (ApplicationControl.DEBUG_MODE) Debug.Print(fullname + " has died");
        ApplicationControl.FamilyEvents.Invoke(this);
    }

    /// <summary>
    /// Check for contact with screen bounderies
    /// 
    /// <remarks>
    /// If event ever needed for screen collition use following:
    /// </summary>
    private void Contact()
    {
        if (location.X < 0) { location.X = 0; volocity.X = 1; }
        if (location.Y < 0) { location.Y = 0; volocity.Y = 1; }
        if (location.X > ApplicationControl.MaxWidth) { location.X = ApplicationControl.MaxWidth; volocity.X = -1; }
        if (location.Y > ApplicationControl.MaxHeight) { location.Y = ApplicationControl.MaxHeight; volocity.Y = -1; }
    }

    /// <summary>
    /// Check for contact with other bots.
    /// </summary>
    /// <param name="person"></param>
    public async void Contact(Person person)
    {
        if (person == this) return;
        bool ignored = bumped == person.id;

        if (!isDead && !person.isDead && !ignored && Is.Overlap(location, person.location, 50)) {
            bumped = person.id;
            person.bumped = id;

            if (await Marry(person))
            {
                if (ApplicationControl.DEBUG_MODE) Debug.Print(name + " got married to " + person.name);
                emotion = Emotion.Love;
                person.emotion = Emotion.Love;
                ApplicationControl.family.Add(new Person(this));
            }
            else await Fight(person);

            volocity.X = location.X < person.location.X ? -1 : 1;
            volocity.Y = location.Y < person.location.Y ? -1 : 1;
            
            if (!person.isDead)
            {
                person.volocity.X = volocity.X * -1;
                person.volocity.Y = volocity.Y * -1;
            }
    }
        else if (ignored) bumped = -1;
    }

    public void Trash()
    {
        ApplicationControl.Update -= Update;
        ApplicationControl.FamilyEvents.Unsubscribe(this);

        if (ApplicationControl.family.Remove(this))
            ApplicationControl.NumberOfPeople--;

        GarbageBin.Add(this);
    }

    public void Dispose()
    {
        GarbageBin.Garbage -= Dispose;
        image.Dispose();
        mask.Dispose();
    }

    public void FamilyEvent(FeudEventArgs args)
    {
        if (!isActive || (isAngry && follow)) return;

        Person person = args.person;

        if (person.isDead)
        {
            if (person.bumped == -1) emotion = Emotion.Sad;
            else
            {
                emotion = Emotion.Angry;
                lookat = person.bumped;
                follow = true;
            }
        }
        else emotion = Emotion.Party;
    }
}