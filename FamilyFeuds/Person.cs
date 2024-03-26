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
using JuicyGrapeApps.FamilyFeuds;

/// <summary>
/// This class is the bot representation of a person on screen it stores their details such as date of birth,
/// emotional state, intellegence or family ties it also calculates things like a person's age, movement and
/// even how long it takes to have an idea or recuperate from emotional damage.
/// </summary>
public class Person : IFeudEvent
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
        Eat = 10,
        HaveKids = 20,
    }

    // Person's brain filled with ideas.
    public static readonly Ideas[] Brain = (Ideas[]) Enum.GetValues(typeof(Ideas));

    public int id { get; set; }
    public int family { get; set; }
    public int mother { get; set; } = -1;
    public int father { get; set; } = -1;

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
    public Point[] motherLine = new Point[4];
    public Point[] fatherLine = new Point[4];
    private int m_age;
    private int m_energy = 5;
    private int m_lookat = -1;
    private Emotion m_emotion;
    private int m_intelligence = 10;
    private int m_emotional = 5;
    private bool m_killer = false;
    private int m_grave = 0;
    public int followed = -1;
    public RectangleF bounds = new();
    public bool changeMask = false;
    public int ghost = 255;
    public bool forceMask = false;
    private float m_speed = 0.0005f;
    private bool m_reciveEvents = false;

    public int lookat { 
        get => m_lookat;
        set
        {
            if (m_lookat != -1  && followed == -1) followed = m_lookat;
            m_lookat = value;
        }
    }

    public bool killer
    {
        get => m_killer;
        set => m_killer = value;
    }

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
    public bool isAvailable => lookat == -1 && isActive;

    // Randomize volocity changes direction and speed of the bot on screen.
    public void ChangeVolocity() => volocity = new Point(RandomGenerator.Int(2, 1, true), RandomGenerator.Int(2, 1, true));
    public string fullname => name + " " + surname;
    public int age => (DateTime.Now - dob).Minutes;

    /// <summary>
    /// Amount of energy a person has if energy reaches zero they expire.
    /// </summary>
    public int energy
    {
        get => m_energy;
        set => Energy(value);
    }

    /// <summary>
    /// Returns true person is looking else where.
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    private bool FocusedElsewhere(Person person) =>
        lookat + person.lookat != -2 && id != person.lookat &&
        person.id != lookat;

    /// <summary>
    /// Change a person's emotional state.
    /// </summary>
    public Emotion emotion
    {
        get => m_emotion;
        set
        {
            if (isDead) return;
            if (m_emotion != value) lookat = -1;

            m_emotional = 5;
            m_emotion = value;
            changeMask = true;

            switch (m_emotion)
            {
                case Emotion.None:
                    image = gender ? FamilyFeuds.Properties.Resources.Male : FamilyFeuds.Properties.Resources.Female;
                    m_emotional = 0;
                    break;
                case Emotion.Love:
                    image = FamilyFeuds.Properties.Resources.Love;
                    break;
                case Emotion.Happy:
                    image = FamilyFeuds.Properties.Resources.Happy;
                    break;
                case Emotion.Party:
                    image = FamilyFeuds.Properties.Resources.Party;
                    break;
                case Emotion.Sad:
                    image = FamilyFeuds.Properties.Resources.Sad;
                    break;
                case Emotion.Angry:
                    m_emotional = 20;
                    image = FamilyFeuds.Properties.Resources.Angry;
                    break;
                case Emotion.Jealous:
                    m_emotional = 8;
                    image = FamilyFeuds.Properties.Resources.Jealous;
                    break;
                case Emotion.Injured:
                    image = FamilyFeuds.Properties.Resources.Injured;
                    forceMask = true;
                    break;
                case Emotion.Baby:
                    image = FamilyFeuds.Properties.Resources.Baby;
                    break;
                case Emotion.Angel:
                    m_emotional = 0;
                    image = FamilyFeuds.Properties.Resources.Angel;
                    break;
                case Emotion.Devil:
                    m_emotional = 0;
                    image = FamilyFeuds.Properties.Resources.Devil;
                    break;
            }
        }
    }

    /// <summary>
    /// Sets family members emotions.
    /// </summary>
    /// <param name="id">Family members id</param>
    /// <param name="emotion">The emotion they feel</param>
    private void SetFamilyEmotion(int id, Emotion emotion)
    {
        if (id == -1) return;
        Person? person = ApplicationControl.person(id);
        if (person != null && person.isAvailable)
            person.emotion = emotion;
    }

    /// <summary>
    /// Changes image mask, masks are used to clear the image on screen.
    /// </summary>
    public void ChangeMask()
    {
        switch (m_emotion)
        {
            case Emotion.None:
                mask = gender ? FamilyFeuds.Properties.Resources.Mask : FamilyFeuds.Properties.Resources.FemaleMask;
                break;
            case Emotion.Party:
                mask = FamilyFeuds.Properties.Resources.PartyMask;
                break;
            case Emotion.Sad:
                mask = FamilyFeuds.Properties.Resources.SadMask;
                break;
            case Emotion.Angel:
                mask = FamilyFeuds.Properties.Resources.AngelMask;
                break;
            case Emotion.Devil:
                mask = FamilyFeuds.Properties.Resources.DevilMask;
                break;
            default:
                mask = FamilyFeuds.Properties.Resources.Mask;
                break;
        }
        changeMask = false;
    }

    /// <summary>
    /// Constructor used for a custom person.
    /// </summary>
    /// <param name="name">Forename of person</param>
    /// <param name="surname">Surname/Family name of person</param>
    /// <param name="gender">Gender of person</param>
    /// <param name="family">Family id of the person's family</param>
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
    /// Constructor used for children or a default person.
    /// </summary>
    /// <param name="person">Creates a child of the person passed or a random default person if null</param>
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
        }
        Initialize();
    }

    /// <summary>
    /// Initialize a person with data common to both custom and default people.
    /// </summary>
    private void Initialize()
    {
        id = ApplicationControl.UniqueBotId;
        ApplicationControl.UniqueBotId++;
        ApplicationControl.NumberOfPeople++;

        dob = DateTime.Now;
        ChangeVolocity();
        ChangeMask();
        Subscribe();

        ApplicationControl.Update += Update;
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

        if (lookat > -1)
        {
            Person? person = ApplicationControl.person(lookat);
            if (person == null) lookat = -1;
            else location = Is.Towards(location, person.location, m_speed);
            m_speed += 0.0005f;
        }
        else
        {
            m_speed = 0.0005f;
            location.X += volocity.X;
            location.Y += volocity.Y;
        }

        if (isDead)
        {
            float floating = 1 - (float) Math.Abs(m_grave - location.Y) / m_energy;
            if (floating < 0) floating = 0;

            if (ghost == 0) Trash();
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
        bool hasEmotions = (isEmotional || person.isEmotional) &&
                          !(lookat == spouse && person.lookat == person.spouse);
        
        if (mother == person.id || father == person.id ||
            person.mother == id || person.father == id ||
            person.gender == gender || hasEmotions) return false;

        if (spouse == person.id || person.spouse == id)
        {
            lookat = -1;
            person.lookat = -1;
            return true;
        }
        if (person.family == family || spouse + person.spouse != -2) return false;

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
        if (lookat == bumped) lookat = -1;
        if (person.lookat == id) person.lookat = -1;

        if (person.isDead || person.isBaby || person.family == family ||
            mother == person.id || father == person.id ||
            person.mother == id || person.father == id) return;

        if (person.gender == gender || isAngry || person.isAngry) {            
            if (person.isInjured || (!isInjured && RandomGenerator.Bool()))
            {
                person.energy--;
                if (person.isDead) m_killer = true;
                else person.emotion = Emotion.Injured;
            }
            else
            {
                energy--;
                if (isDead) person.killer = true;
                else emotion = Emotion.Injured;
            }
        }
        else if (!isAngry && !person.isAngry && spouse > -1)
        {
            Person? partner = ApplicationControl.person(spouse);
            if (partner == null) spouse = -1;
            else if (partner.isAvailable)
            {
                partner.emotion = Emotion.Jealous;
                partner.lookat = person.id;
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

        switch (idea) 
        {
            case Ideas.ChangeDirection: ChangeVolocity(); break;
            case Ideas.Eat: if (m_energy < 10) m_energy++; break;
            case Ideas.HaveKids:
                if (spouse > -1 && isAvailable && emotion != Emotion.Love &&
                    !ApplicationControl.OverPopulated)
                {
                    Person? partner = ApplicationControl.person(spouse);
                    if (partner != null && partner.isAvailable)
                    {
                        emotion = Emotion.Love;
                        lookat = spouse;
                        partner.emotion = Emotion.Love;
                        partner.lookat = id;
                    }
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
            m_age = age;
            bumped = -1;
            if (age % 5 == 0) energy--;

            if (!isDead && isAvailable) 
            {
                emotion = Emotion.Party;
                SetFamilyEmotion(spouse, Emotion.Party);
                SetFamilyEmotion(mother, Emotion.Party);
                SetFamilyEmotion(father, Emotion.Party);
            }
        }
    }

    /// <summary>
    /// Set energy level, if level reaches zero person expires.
    /// </summary>
    private int Energy(int value)
    {
        if (isDead) return 0;

        if (ApplicationControl.OverPopulated)
            value = (value < m_energy) ? value - 2 : m_energy;

        m_energy = value;
        if (m_energy > 0) return m_energy;

        if (lookat > -1) lookat = -1;

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
        volocity.X = 0;
        m_grave = location.Y;
        spouse = -1;

        ApplicationControl.FamilyEvents.Invoke(this, bumped == -1 ? Emotion.Sad: Emotion.Angry);
        Unsubscribe();
        return 0;
    }

    /// <summary>
    /// Check for contact with screen bounderies
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
        if (person == this || FocusedElsewhere(person)) return;
        bool ignored = bumped == person.id;

        if (!isDead && !person.isDead && !ignored && 
            Is.Overlap(location, person.location, new Size(43, 43))) {

            bumped = person.id;
            person.bumped = id;

            if (await Marry(person))
            {
                emotion = Emotion.Love;
                person.emotion = Emotion.Love;
                ApplicationControl.family.Add(new Person(this));
            }
            else await Fight(person);

            if (!isDead) {
                volocity.X = location.X < person.location.X ? -1 : 1;
                volocity.Y = location.Y < person.location.Y ? -1 : 1;
            }

            if (!person.isDead)
            {
                person.volocity.X = volocity.X * -1;
                person.volocity.Y = volocity.Y * -1;
            }
        }
        else if (ignored) bumped = -1;
    }

    /// <summary>
    /// Subscrib to collision and family events
    /// </summary>
    private void Subscribe()
    {
        if (m_reciveEvents) return;
        ApplicationControl.Collision += OnCollision;
        ApplicationControl.FamilyEvents.Subscribe(this);
        m_reciveEvents = true;
    }

    /// <summary>
    /// Unsubscrib from collision and family events
    /// </summary>
    private void Unsubscribe()
    {
        if (!m_reciveEvents) return;
        ApplicationControl.Collision -= OnCollision;
        ApplicationControl.FamilyEvents.Unsubscribe(this);
        m_reciveEvents = false;
    }

    /// <summary>
    /// This function is required as it is part of the ITrashable interface
    /// and used in conjuction with the IDisposable to clean up any resources. 
    /// </summary>
    public void Trash()
    {
        ApplicationControl.FamilyEvents.InvokeChildren(this);
        ApplicationControl.Update -= Update;
        Unsubscribe();

        mother = -1;
        father = -1;

        GarbageBin.Bin(this);
    }

    /// <summary>
    /// Clean up the memory used by resources.
    /// </summary>
    public void Dispose()
    {
        try
        {
            GarbageBin.Garbage -= Dispose;

            if (!GarbageBin.Contains(this))
            {
                ApplicationControl.Update -= Update;
                Unsubscribe();

                if (ApplicationControl.family.Remove(this))
                    ApplicationControl.NumberOfPeople--;
            }

            image.Dispose();
            mask.Dispose();
        }
        catch { }
    }

    /// <summary>
    /// This function is called on all members of the same family once the
    /// FamilyEvents event is invoked in FamiyEventManager.
    /// <see cref="FamilyEventManager.Invoke(IFeudEvent)"/>
    /// </summary>
    /// <param name="args">Arguments received from event</param>
    public void FamilyEvent(FeudEventArgs args)
    {
        if (!isActive || (isAngry && lookat > -1)) return;
        Person person = (Person) args.person;
        emotion = args.emotion;
        if (isAngry) lookat = person.bumped;
    }

    /// <summary>
    /// This function is called on every child of a parent once the 
    /// ChildEvent event is invoked in FamiyEventManager.
    /// <see cref="FamilyEventManager.InvokeChildren(IFeudEvent)"/>
    /// </summary>
    /// <param name="args">Arguments received from event</param>
    public void ChildEvent(FeudEventArgs args)
    {
        Person person = (Person)args.person;
        if (mother == person.id) mother = -1;
        if (father == person.id) father = -1;
    }
}