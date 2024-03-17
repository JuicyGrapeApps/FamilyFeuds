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
using System.Diagnostics;
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

        private Graphics graphics;

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

            // Create custom people
            foreach (string fullname in ApplicationControl.names)
            {
                string name = fullname;
                bool gender = name.Contains("(M)");
                name = name.Replace(" (M)", "").Replace(" (F)", "");

                int idx = name.IndexOf(" ");

                if (idx != -1)
                {
                    string forename = name.Substring(0, idx);
                    string surname = name.Substring(idx + 1);
                    ApplicationControl.family.Add(new Person(forename, surname, gender, ApplicationControl.familyIndex(surname)));
                }
            }

            int numberOfPeople = ApplicationControl.NumberOfPeople;

            // Create default people
            for (int i = numberOfPeople; i < numberOfPeople + ApplicationControl.MaxDefaultNumber; i++)
                ApplicationControl.family.Add(new Person());

            ApplicationControl.Events.Collision += OnCollision;
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
            ApplicationControl.Events.Collision -= OnCollision;
            ApplicationControl.Shutdown();
        }

        /// <summary>
        /// Called by the Execute timer on the FamiltFeudForm form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Execute_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < ApplicationControl.NumberOfPeople; i++)
                {
                    Person person = ApplicationControl.family[i];
                    Update(person);
                    for (int j = 0; j < ApplicationControl.NumberOfPeople; j++) person.Contact(ApplicationControl.family[j]);
                }
            }
            catch (System.Exception ex)
            {
                if (ApplicationControl.DEBUG_MODE) Debug.Print("Exception: "+ex.Message);
            }
        }

        /// <summary>
        /// Called every tick count of the Execute timer on the FamiltFeudForm form. This
        /// function clears and paints all the graphics on screen.
        /// <see cref="Execute_Tick(object, EventArgs)"/>
        /// </summary>
        /// <param name="person"></param>
        private void Update(Person person)
        {
            person.Update();

            graphics.FillRectangle(Brushes.Black, new Rectangle(person.location, new Size(200, 50)));

            if (person.mother > -1)
                graphics.DrawBezier(new Pen(Brushes.Black), person.motherLine[0], person.motherLine[1], person.motherLine[2], person.motherLine[3]);

            if (person.father > -1)
                graphics.DrawBezier(new Pen(Brushes.Black), person.fatherLine[0], person.fatherLine[1], person.fatherLine[2], person.fatherLine[3]);

            DrawArrow(person, true);

            person.Move();

            Point point = person.location;
            point.X += 55;
            point.Y += 18;

            graphics.DrawImage(person.image, new Rectangle(person.location, new Size(50, 50)));
            graphics.DrawString(person.fullname, Font, Brushes.WhiteSmoke, point);

            if (person.mother > -1)
            {
                if (ApplicationControl.family[person.mother].isDead) person.mother = -1;
                else
                {
                    int offset = 40;
                    person.motherLine[0] = person.location;
                    person.motherLine[0].X += offset;
                    person.motherLine[0].Y += offset;
                    person.motherLine[3] = ApplicationControl.family[person.mother].location;
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

                    graphics.DrawBezier(new Pen(Brushes.Pink), person.motherLine[0], person.motherLine[1], person.motherLine[2], person.motherLine[3]);
                }
            }

            if (person.father > -1)
            {
                if (ApplicationControl.family[person.father].isDead) person.father = -1;
                else
                {
                    int offset = 40;
                    person.fatherLine[0] = person.location;
                    person.fatherLine[0].X += offset;
                    person.fatherLine[0].Y += offset;
                    person.fatherLine[3] = ApplicationControl.family[person.father].location;
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

                    graphics.DrawBezier(new Pen(Brushes.LightBlue), person.fatherLine[0], person.fatherLine[1], person.fatherLine[2], person.fatherLine[3]);
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
            if (person.lookat+person.followed == -2) return;

            Person target = ApplicationControl.family[(person.followed > -1) ? person.followed: person.lookat];

            Double scale = 10;
            Pen pen;
            if (clear)
            {
                scale = 11;
                pen = new Pen(Brushes.Black, 3.0f);
            }
            else
            {
                pen = new Pen(Brushes.NavajoWhite, 1.0f);
            }

            Point center = new Point(person.location.X, person.location.Y);

            double v = center.X - target.location.X;
            double h = target.location.Y - center.Y;

            Double t1 = Math.Atan2(v, h) - 99.75;
            Double x1 = (Math.Cos(t1) - Math.Sin(t1)) * scale;
            Double y1 = (Math.Sin(t1) + Math.Cos(t1)) * scale;

            graphics.DrawLine
            (
                pen,
                (Single) center.X,
                (Single) center.Y,
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
                (Single) center.X,
                (Single) center.Y,
                (Single)(center.X + x2),
                (Single)(center.Y + y2)
            );
            graphics.DrawLine
            (
                pen,
                (Single) center.X,
                (Single) center.Y,
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

            if (clear)
            {
                if (target.isDead) person.emotion = Person.Emotion.Sad;
                else if (target.family == person.family) person.emotion = Person.Emotion.None;
                person.followed = -1;
            }
        }

        /// <summary>
        /// Collision event handler.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="collider"></param>
        private void OnCollision(Person person, Person collider)
        {
            if (ApplicationControl.DEBUG_MODE) Debug.Print(person.fullname + " bumped into " + collider.fullname);
            
            person.volocity.X = (person.location.X < collider.location.X) ? -1: 1;
            person.volocity.Y = (person.location.Y < collider.location.Y) ? -1 :1;
            Update(person);

            collider.volocity.X = (collider.location.X < person.location.X) ? -1 : 1;
            collider.volocity.Y = (collider.location.Y < person.location.Y) ? -1 : 1;
            Update(collider);

            if (person.Marry(collider))
            {
                if (ApplicationControl.DEBUG_MODE) Debug.Print(person.name + " got married to " + collider.name);
                person.emotion = Person.Emotion.Love;
                collider.emotion = Person.Emotion.Love;

                ApplicationControl.family.Add(new Person(person));

                person.FamilyEmotional(Person.Emotion.Party, true);
            }
            else person.Fight(collider);
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
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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