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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace JuicyGrapeApps.FamilyFueds
{
    public partial class FamilyFeudsForm : Form
    {
        [DllImport("user32.dll")] static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", SetLastError = true)] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [Flags]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        // Firework variables
        const int MAX_FIREWORKS = 3;
        const int EXPLOSION_PARTICALS = 20;

        public Graphics graphics;
        private float[] fireworkThrust = new float[MAX_FIREWORKS] { 0.02f, 0.05f, 0.03f };
        bool[] fireworkExplode = new bool[MAX_FIREWORKS] { false, false, false };
        bool[] fireworkLit = new bool[MAX_FIREWORKS] { false, false, false };
        Single[] thrust = new Single[MAX_FIREWORKS] { 0f, 0f, 0f };
        Single[] fireworkFuse = new float[MAX_FIREWORKS] { 1.2f, 2f, 1.8f };
        Partical[] fireworkPartical = new Partical[MAX_FIREWORKS];
        Partical[,] explosion = new Partical[MAX_FIREWORKS, EXPLOSION_PARTICALS];
        int[] fireworkHeight = new int[] { 0, 0, 0 };
        int[] fireworkDistance =new int[] { 0, 0, 0 };
        static Point origin = new Point(ApplicationControl.MaxHeight - 200,
                                        ApplicationControl.MaxWidth / 2);

        /// <summary>
        /// Firwork partical stucture used for storing a firework's data
        /// and drawing it to screen.
        /// </summary>
        struct Partical
        {
            public Point origin;
            public Point target;
            private Color color;
            private double decay = 0;
            Trail[] trail;

            internal struct Trail
            {
                public Point coord;
                public Color color;
                public Trail(Point coord, Color color)
                {
                    this.coord = coord;
                    this.color = color;
                }
            }

            public Point coord
            {
                get => trail[0].coord;
                set => Trails(value);
            }

            public Partical(Point origin, Point target, int trails, Color color)
            {
                this.origin = origin;
                this.target = target;
                this.color = color;
                trail = new Trail[trails];
                trail[0] = new Trail(origin, color);
                SetAlphas();
            }

            public Partical(Point origin, int trails, Color color)
            {
                this.origin = origin;
                this.target = origin;
                this.color = color;
                trail = new Trail[trails];
                trail[0] = new Trail(origin, color);
                SetAlphas();
            }

            private void SetAlphas()
            {
                int length = trail.Length-1;
                for (int i = 0; i < trail.Length; i++)
                    trail[i].color = Color.FromArgb(255 - (int)(255 * ((double)i / length)), color);
            }

            public void Draw(Graphics g, Point point)
            {
                coord = point;
                using (Pen clear = new Pen(Brushes.Black, 2))
                {
                    Point a = new Point(point.X, point.Y+1);
                    foreach (Trail t in trail)
                    {
                        if (t.coord.X == 0 || t.coord == origin) continue;
                        using (Pen pen = new Pen(t.color, 2))
                        {
                            g.DrawLine(clear, a, t.coord);
                            g.DrawLine(pen, a, t.coord);
                            a = t.coord;
                        }
                    }
                }
                Decay();
            }

            private void Trails(Point value)
            {
                for (int i = trail.Length - 2; i > -1; i--)
                    trail[i+1].coord = trail[i].coord;

                trail[0] = new Trail(value, color);
            }

            public void Decay()
            {
                double rate = Math.Sin(decay);
                if (rate < 1f) decay += 0.01f;
                target.Y += (int)(100 * rate);
            }
        }

        public FamilyFeudsForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            GetClientRect(PreviewWndHandle, out Rectangle ParentRect);

            Size = ParentRect.Size;
            ApplicationControl.MaxHeight = Size.Height;
            ApplicationControl.MaxWidth = Size.Width;
            Location = new Point(0, 0);
        }

        public FamilyFeudsForm(Rectangle bounds)
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED /*| EXECUTION_STATE.ES_AWAYMODE_REQUIRED*/);

            InitializeComponent();

            Bounds = bounds;

            Size = Bounds.Size;
            ApplicationControl.MaxHeight = Size.Height - 50;
            ApplicationControl.MaxWidth = Size.Width - 200;
            Location = new Point(0, 0);
        }

        private void FamilyFeudsForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            graphics = CreateGraphics();

            ApplicationControl.InitializeBots();
        }

        private Point mouseLocation;

        private void FamilyFeudsForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (ApplicationControl.Mode == ApplicationControl.ExecuteMode.Preview) return;

            if (!mouseLocation.IsEmpty)
            {
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                    Math.Abs(mouseLocation.Y - e.Y) > 5)
                    ApplicationControl.Shutdown();
            }

            // Update current mouse location
            mouseLocation = e.Location;
        }

        private void FamilyFeudsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ApplicationControl.Mode == ApplicationControl.ExecuteMode.Preview) return;
            FamilyFeudsForm_Unload();
        }

        private void FamilyFeudsForm_Click(object sender, EventArgs e)
        {
            if (ApplicationControl.Mode == ApplicationControl.ExecuteMode.Preview) return;
            FamilyFeudsForm_Unload();
        }

        private void FamilyFeudsForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (ApplicationControl.Mode == ApplicationControl.ExecuteMode.Preview) return;
            FamilyFeudsForm_Unload();
        }


        private void FamilyFeudsForm_Unload()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            graphics.Dispose();
            ApplicationControl.Shutdown();
        }

        /// <summary>
        /// Called by the Execute timer on the FamiltFeudForm form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Execute_Tick(object sender, EventArgs e) => ApplicationControl.RefreshScreenSaver(this);

        public void FamilyWinner()
        {
            for (int i = 0; i < MAX_FIREWORKS; i++)
                DrawFireworks(i);
        }

        /// <summary>
        /// Called for each bot from the ApplicationControl, this function
        /// clears and paints all the graphics on screen.
        /// <see cref="ApplicationControl.RefreshScreenSaver(Form)"/>
        /// </summary>
        /// <param name="person"></param>
        public void Draw(Person person)
        {
            ImageAttributes imageAttributes = new ImageAttributes();

            if (person.ghost != 255f)
            {
                ColorMatrix colorMatrix = new ColorMatrix 
                    { Matrix33 = person.ghost / 255f };
                imageAttributes.SetColorMatrix(colorMatrix);
            }

            if (!person.isInjured)
            {
                graphics.FillRectangle(Brushes.Black, person.bounds);
                graphics.DrawImage(person.mask, new Rectangle(person.location, new Size(50, 50)));
            }

            using (Pen pen = new Pen(Brushes.Black))
            {
                if (person.mother > -1)
                    graphics.DrawBezier(pen, person.motherLine[0], person.motherLine[1], person.motherLine[2], person.motherLine[3]);
                if (person.father > -1)
                    graphics.DrawBezier(pen, person.fatherLine[0], person.fatherLine[1], person.fatherLine[2], person.fatherLine[3]);
            }

            DrawArrow(person, true);

            person.Move();

            RectangleF personalSpace = new RectangleF(new Point(person.location.X + 56, person.location.Y + 12), new Size(300, 30));
            StringFormat format = new StringFormat();
            format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, person.fullname.Length) });

            graphics.DrawImage(person.image, new Rectangle(person.location, new Size(50, 50)), 0, 0, person.image.Width, person.image.Height, GraphicsUnit.Pixel, imageAttributes);

            using (Brush brush = new SolidBrush(Color.FromArgb(person.ghost, Color.WhiteSmoke)))
                graphics.DrawString(person.fullname, Font, brush, personalSpace, format);

            person.bounds = graphics.MeasureCharacterRanges(person.fullname, Font, personalSpace, format)[0].GetBounds(graphics);
            person.bounds.X -= 1;
            person.bounds.Width += 5;

            if (person.changeMask) person.ChangeMask();

            if (person.mother > -1)
            {
                Person parent = ApplicationControl.person(person.mother);
                if (parent == null || parent.isDead) person.mother = -1;
                else
                {
                    Color color = Color.FromArgb(person.ghost, Color.Pink);
                    int offset = 40;
                    person.motherLine[0] = person.location;
                    person.motherLine[0].X += offset;
                    person.motherLine[0].Y += offset;
                    person.motherLine[3] = parent.location;
                    person.motherLine[3].X += offset;
                    person.motherLine[3].Y += offset;
                    int x = (person.motherLine[3].X - person.motherLine[0].X) / 4;
                    int y = ((person.motherLine[3].Y - person.motherLine[0].Y) / 4) + 100;
                    person.motherLine[1] = new Point();
                    person.motherLine[1].X = person.motherLine[0].X + x;
                    person.motherLine[1].Y = person.motherLine[0].Y + y;
                    person.motherLine[2] = new Point();
                    person.motherLine[2].X = person.motherLine[3].X - x;
                    person.motherLine[2].Y = person.motherLine[3].Y - y;

                    using (Pen pen = new Pen(color))
                        graphics.DrawBezier(pen, person.motherLine[0], person.motherLine[1], person.motherLine[2], person.motherLine[3]);
                }
            }

            if (person.father > -1)
            {
                Person parent = ApplicationControl.person(person.father);
                if (parent == null || parent.isDead) person.father = -1;
                else
                {
                    Color color = Color.FromArgb(person.ghost, Color.LightBlue);
                    int offset = 40;
                    person.fatherLine[0] = person.location;
                    person.fatherLine[0].X += offset;
                    person.fatherLine[0].Y += offset;
                    person.fatherLine[3] = parent.location;
                    person.fatherLine[3].X += offset;
                    person.fatherLine[3].Y += offset;
                    int x = (person.fatherLine[3].X - person.fatherLine[0].X) / 4;
                    int y = ((person.fatherLine[3].Y - person.fatherLine[0].Y) / 4) + 100;
                    person.fatherLine[1] = new Point();
                    person.fatherLine[1].X = person.fatherLine[0].X + x;
                    person.fatherLine[1].Y = person.fatherLine[0].Y + y;
                    person.fatherLine[2] = new Point();
                    person.fatherLine[2].X = person.fatherLine[3].X - x;
                    person.fatherLine[2].Y = person.fatherLine[3].Y - y;

                    using (Pen pen = new Pen(color))
                        graphics.DrawBezier(pen, person.fatherLine[0], person.fatherLine[1], person.fatherLine[2], person.fatherLine[3]);
                }
            }

            DrawArrow(person);
        }

        /// <summary>
        /// Draws the arrow that points to the person who's being looked at.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="clear"></param>
        public void DrawArrow(Person person, bool clear = false)
        {
            if (person.lookat + person.followed == -2) return;

            Person target = ApplicationControl.person(person.followed > -1 ? person.followed : person.lookat);

            if (target == null)
            {
                person.lookat = -1;
                person.followed = -1;
                return;
            }

            Double scale = clear ? 11 : 10;

            using (Pen pen = clear ? new Pen(Brushes.Black, 3.0f) : new Pen(Brushes.NavajoWhite, 1.0f))
            {
                Point center = new Point(person.location.X, person.location.Y);

                double v = center.X - target.location.X;
                double h = target.location.Y - center.Y;

                Double t1 = Math.Atan2(v, h) - 99.75;
                Double x1 = (Math.Cos(t1) - Math.Sin(t1)) * scale;
                Double y1 = (Math.Sin(t1) + Math.Cos(t1)) * scale;

                graphics.DrawLine
                (
                    pen,
                    (Single)center.X,
                    (Single)center.Y,
                    (Single)(center.X + x1),
                    (Single)(center.Y + y1)
                );

                Double t2 = t1 - 90;
                Double x2 = (Math.Cos(t2) - Math.Sin(t2)) * scale;
                Double y2 = (Math.Sin(t2) + Math.Cos(t2)) * scale;

                Double t3 = t1 + 90;
                Double x3 = (Math.Cos(t3) - Math.Sin(t3)) * scale;
                Double y3 = (Math.Sin(t3) + Math.Cos(t3)) * scale;

                graphics.DrawLine
                (
                    pen,
                    (Single)center.X,
                    (Single)center.Y,
                    (Single)(center.X + x2),
                    (Single)(center.Y + y2)
                );
                graphics.DrawLine
                (
                    pen,
                    (Single)center.X,
                    (Single)center.Y,
                    (Single)(center.X + x3),
                    (Single)(center.Y + y3)
                );
                graphics.DrawLine
                (
                    pen,
                    (Single)(center.X + x1),
                    (Single)(center.Y + y1),
                    (Single)(center.X + x2),
                    (Single)(center.Y + y2)
                );
                graphics.DrawLine
                (
                    pen,
                    (Single)(center.X + x1),
                    (Single)(center.Y + y1),
                    (Single)(center.X + x3),
                    (Single)(center.Y + y3)
                );
            }

            if (clear)
            {
                if (target.isDead) person.emotion = Person.Emotion.Sad;
                else if (target.family == person.family) person.emotion = Person.Emotion.None;
                person.followed = -1;
            }
        }

        /// <summary>
        /// Shows firework display when only one family are left.
        /// </summary>
        /// <param name="firework"></param>
        public void DrawFireworks(int firework)
        {
            if (!fireworkLit[firework])
            {
                fireworkLit[firework] = true;
                fireworkHeight[firework] = RandomGenerator.Int(750, 100);
                fireworkDistance[firework] = RandomGenerator.Int(150, 75, true);
                thrust[firework] = 0;
                fireworkExplode[firework] = false;
                fireworkPartical[firework] = new Partical(origin, RandomGenerator.Int(25,10), RandomGenerator.Color);
            }

            thrust[firework] += fireworkThrust[firework];

            if (thrust[firework] < fireworkFuse[firework] && !fireworkExplode[firework])
            {
                Point point = fireworkPartical[firework].origin;
                point.X += (int)(Math.Cos(thrust[firework]) * fireworkDistance[firework]);
                point.Y -= (int)(Math.Sin(thrust[firework]) * fireworkHeight[firework]);
                fireworkPartical[firework].Draw(graphics, point);
            }
            else if (!fireworkExplode[firework])
            {
                fireworkExplode[firework] = true;
                thrust[firework] = 0;
                fireworkThrust[firework] = (Single)RandomGenerator.Float(0.1f, 0.01f);
                for (int i = 0; i < EXPLOSION_PARTICALS; i++)
                {
                    fireworkPartical[firework].target = fireworkPartical[firework].coord;
                    fireworkPartical[firework].target.X += (int)(Math.Cos(RandomGenerator.Float(6.3)) * 500);
                    fireworkPartical[firework].target.Y -= (int)(Math.Sin(RandomGenerator.Float(6.3)) * 500);
                    explosion[firework, i] = new Partical(fireworkPartical[firework].coord, fireworkPartical[firework].target, RandomGenerator.Int(10, 5), RandomGenerator.Color);
                }
            }
            else
            {
                for (int i = 0; i < EXPLOSION_PARTICALS; i++)
                    explosion[firework, i].Draw(graphics, Is.Towards(explosion[firework, i].origin, explosion[firework, i].target, thrust[firework], false));

                if (thrust[firework] > 2) fireworkLit[firework] = false;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Execute = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Execute
            // 
            this.Execute.Enabled = true;
            this.Execute.Interval = 12;
            this.Execute.Tick += new System.EventHandler(this.Execute_Tick);
            // 
            // FamilyFeudsForm
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Font = new System.Drawing.Font("Segoe Print", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FamilyFeudsForm";
            this.Load += new System.EventHandler(this.FamilyFeudsForm_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FamilyFeudsForm_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FamilyFeudsForm_MouseMove);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer Execute;
        private System.ComponentModel.IContainer components;
    }
}