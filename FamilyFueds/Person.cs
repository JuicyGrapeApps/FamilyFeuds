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
using JuicyGrapeApps.Core;
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

    // Peron's ideas equal to the impulse to react on them.
    public enum Ideas
    {
        ChangeDirection = 0,
        Eat = 5,
        HaveKids = 8,
    }

    // Person's brain filled with ideas.
    public static readonly Ideas[] Brain = (Ideas[]) Enum.GetValues(typeof(Ideas));

    public int id { get; set; }
    public int family { get; set; }
    public string name;
    public string surname;
    public DateTime dob;
    public Point location;
    public Point volocity;
    public Image image;
    public Image mask;
    public bool gender;
    public int spouse = -1;
    public int bumped = -1;
    public int mother { get; set; } = -1;
    public int father { get; set; } = -1;
    public int lookat = -1;
    public Point[] motherLine = new Point[4];
    public Point[] fatherLine = new Point[4];
    private int m_age;
    private int m_energy = 5;
    private Emotion m_emotion;
    private int m_intelligence = 10;
    private int m_emotional = 5;
    private bool m_killer = false;
    private int m_grave = 0;
    private bool m_follow = false;
    public int followed = -1;
    public RectangleF bounds = new();
    public bool changeMask = false;
    public int ghost = 255;

    public int emotional { get; set; }

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
    public bool isAvailable => !follow && isActive;

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
        set => Energy(value);
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
            if (m_emotion != value) follow = false;

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
                    m_emotional = 0;
                    image = FamilyFueds.Properties.Resources.Angel;
                    break;
                case Emotion.Devil:
                    m_emotional = 0;
                    image = FamilyFueds.Properties.Resources.Devil;
                    break;
            }
        }
    }

    /// <summary>
    /// Sets family members emotions or all children's if a negative two
    /// is passed to function.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="emotion"></param>
    private void SetFamilyEmotion(int id, Emotion emotion)
    {
        if (id == -2) ApplicationControl.FamilyEvents.InvokeChildren(this);  
        else if (id > -1)
        {
            Person person = ApplicationControl.person(id);
            if (person != null && person.isAvailable)
            {
                person.emotion = emotion;
                person.lookat = lookat;
                person.follow = follow;
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
        m_intelligence = RandomGenerator.Int(20, 5);
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
            m_intelligence = RandomGenerator.Int(20, 5);
        }
        else
        {
            surname = person.surname;
            family = person.family;

            if (person.gender)
            {
                mother = person.spouse;
                father = person.id;
            }
            else
            {
                mother = person.id;
                father = person.spouse;
            }
            location = person.location;
            m_intelligence = 5;
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
        ChangeVolocity();
        ChangeMask();

        ApplicationControl.Update += Update;
        ApplicationControl.Collision += OnCollision;
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
        Recovery();
        Thinking();
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

        if (isDead)
        {
            float floating = 1 - (float) Math.Abs(m_grave - location.Y) / m_energy;
            if (floating <= 0) Trash();
            else ghost = (int) (255 * floating);
        }
        else OnCollision();
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
            spouse + person.spouse != -2) return !isEmotional && !person.isEmotional && spouse == person.id;

        spouse = person.id;
        person.spouse = id;

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
        SetFamilyEmotion(mother, Emotion.Happy);
        SetFamilyEmotion(father, Emotion.Happy);
        SetFamilyEmotion(person.mother, Emotion.Happy);
        SetFamilyEmotion(person.father, Emotion.Happy);
        return true;
    }

    /// <summary>
    /// Called after a collition event, this function preforms a argument between the people who met depending
    /// on certain criteria.
    /// </summary>
    /// <param name="person"></param>
    public async Task Fight(Person person)
    {
        if (lookat == bumped) follow = false;

        if (!isActive || person.isDead || person.isBaby || person.family == family ||
            person.mother == id || person.father == id) return;

        if (person.gender == gender || isAngry) {
            person.energy--;
            if (person.energy == 0)
            {
                m_killer = true;
                return;
            }
            else person.emotion = Emotion.Injured;
        }
        else if (person.spouse > -1)
        {
            Person partner = ApplicationControl.person(person.spouse);
            if (partner == null) person.spouse = -1;
            else if (partner.isActive)
            {
                partner.emotion = Emotion.Jealous;
                partner.lookat = id;
                partner.follow = true;
            }
        }
    }

    /// <summary>
    /// Called when a person is over their emotion.
    /// </summary>
    private void Recovery()
    {
        if (m_emotional == 0) return;
        m_emotional--;
        if (m_emotional <= 0) emotion = isBaby ? Emotion.Happy: Emotion.None;
    }

    /// <summary>
    /// Calculates the amount of time it takes in seconds for a person to 
    /// have an idea depending on their intelligence.
    /// </summary>
    private void Thinking()
    {
        m_intelligence--;
        if (m_intelligence < 0)
        {
            if (!isEmotional) Idea();
            m_intelligence = RandomGenerator.Int(20, 5);
        }
    }

    /// <summary>
    /// Called every time a person has an idea and returning the idea
    /// they came up with.
    /// </summary>
    private void Idea()
    {
        Ideas idea = RandomGenerator.Idea();

        if (ApplicationControl.DEBUG_MODE) Debug.Print(fullname+" has the idea to "+idea.ToString());

        switch (idea) 
        {
            case Ideas.ChangeDirection: ChangeVolocity(); break;
            case Ideas.Eat: if (m_energy < 10) m_energy++; break;
            case Ideas.HaveKids:
                if (spouse > -1)
                {
                    emotion = Emotion.Love;
                    m_emotional = 20;
                    lookat = spouse;
                    follow = true;
                }
                else ChangeVolocity();
            break;
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

            m_age = age;
            if (age % 5 == 0) energy--;

            if (!isDead)
            {
                if (isAvailable) emotion = Emotion.Party;
                SetFamilyEmotion(spouse, emotion);
                SetFamilyEmotion(mother, emotion);
                SetFamilyEmotion(father, emotion);
            }
        }
    }

    /// <summary>
    /// Set energy level, if level reaches zero person expires.
    /// </summary>
    private int Energy(int value)
    {
        if (isDead) return 0;
        m_energy = value;
        if (m_energy > 0) return m_energy;

        volocity.X = 0;
        follow = false;
        if (m_killer)
        {
            emotion = Emotion.Devil;
            volocity.Y = 3;
            m_energy = ApplicationControl.MaxHeight - location.Y;
        }
        else
        {
            emotion = Emotion.Angel;
            volocity.Y = -2;
            m_energy = location.Y;
        }
        m_grave = location.Y;
        spouse = -1;

        if (ApplicationControl.DEBUG_MODE) Debug.Print(fullname + " has died");

        ApplicationControl.FamilyEvents.Invoke(this);
        return 0;
    }

    /// <summary>
    /// Check for contact with screen bounderies
    /// 
    /// <remarks>
    /// If event ever needed for screen collition use following:
    /// </summary>
    private void OnCollision()
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
    public async void OnCollision(Person person)
    {
        if (person == this) return;
        bool ignored = bumped == person.id;

        if (!isDead && !person.isDead && !ignored && 
            Is.Overlap(location, person.location, new Size(43, 43))) {
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
        ApplicationControl.FamilyEvents.InvokeChildren(this);
        mother = -1;
        father = -1;
        ApplicationControl.Update -= Update;
        ApplicationControl.Collision -= OnCollision;
        ApplicationControl.FamilyEvents.Unsubscribe(this);
        
        if (ApplicationControl.family.Remove(this))
            ApplicationControl.NumberOfPeople--;

        GarbageBin.Add(this);
    }

    public void Dispose()
    {
        try
        {
            GarbageBin.Garbage -= Dispose;
            image.Dispose();
            mask.Dispose();
        } catch { };
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
        else if (person.emotion == Emotion.Love)
        {
            if (emotion != Emotion.Love && emotion != Emotion.Baby)
                emotion = Emotion.Happy;
        }
        else emotion = Emotion.Party;
    }

    public void ChildEvent(FeudEventArgs args)
    {
        Person person = args.person;
        Debug.Print(fullname + " lost parent " + person.name);

        if (person.isDead)
        {
            if (mother == person.id) mother = -1;
            if (father == person.id) father = -1;
        } 
        else if (isAvailable)
        {
            emotion = person.emotion;
            lookat = person.lookat;
            follow = person.follow;
        }
    }
}