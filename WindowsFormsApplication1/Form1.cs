using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Runtime.InteropServices;

namespace TAS_Installer
{
    public partial class TAS : Form
    {
        public static Font MainFont;
        public static Rectangle b = new Rectangle();

        public TAS()
        {
            b = Screen.FromControl(this).Bounds;
            InitializeComponent();
            Stream s = getStream("Resources.CONSOLAB.TTF");
            byte[] ba = new byte[s.Length];
            int i = 0;
            foreach (int iss in ba)
            {
                ba[i] = (byte)s.ReadByte();
                i++;
            }
            PrivateFontCollection vcol = new PrivateFontCollection();
            FontFamily collection = LoadFontFamily(ba, out vcol);
            MainFont = new Font(collection,18F);
            FormBorderStyle = FormBorderStyle.None;
        }

        private Label lab;
        private Button next;

        public static FontFamily LoadFontFamily(byte[] buffer, out PrivateFontCollection fontCollection)
        {
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                     var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                     fontCollection = new PrivateFontCollection();
                     fontCollection.AddMemoryFont(ptr, buffer.Length);
                     return fontCollection.Families[0];
            }
            finally
            {
            handle.Free();
            }
        }

        private void TAS_Load(object sender, EventArgs e)
        {
            this.Icon = new Icon(getStream("back2 (1).ico"));
            ShowIcon = true;
            Console.WriteLine("Start Programm");
            SetBounds((b.Width / 2 - 250), (b.Height / 2 - 200), 500, 400);

            lab = new Label();
            lab.Text = "Welcome to TAS\nInstaller";
            lab.Font = MainFont;
            lab.SetBounds(((Bounds.Width / 2) + i), ((Bounds.Height / 2) - 125), 220, 60);
            lab.BorderStyle = BorderStyle.None;
            lab.TextAlign = ContentAlignment.MiddleCenter;
            lab.BackColor = Color.FromKnownColor(KnownColor.Transparent);

            Timer t = new Timer();
            t.Interval = 20;
            t.Tick += T_Tick;
            t.Start();

            next = new Button();
            next.Enabled = false;
            next.Text = "Next";
            buttonBuild(next);
            next.Click += Next_Click; 

            AcceptButton = next;

            Controls.Add(next);
            Controls.Add(lab);
            cancelButton();
            BackGround();
        }

        private int i = 250;
        private int pt = 20;
        private int tick;

        private void T_Tick(object sender, EventArgs e)
        {
            lab.SetBounds(((Bounds.Width / 2) + i), ((Bounds.Height / 2) - 125), 220, 60);
            if (i <= -100)
            {
                next.Enabled = true;
                next.Focus();
                ((Timer)sender).Stop();
            }
            if (tick >= 3)
            {
                if (pt > 2)
                {
                    pt -= 2;
                }
                tick = 0;
            }
            if (i - pt <= -100)
            {
                i = -100;
            }
            else { i -= pt; }
            tick++;
        }

        TextBox box;

        private void Next_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Next-->");
            clear();
            box = new TextBox();
            box.Multiline = true;
            box.AcceptsReturn = true;
            box.Lines = new string[] { "Write the names down,", "only one name for each line." };
            box.SetBounds(2, 2, Bounds.Width - 4, Bounds.Height - 100);
            box.Font = MainFont;

            Button btn = new Button();
            btn.Text = "Next";
            btn.Click += Btn_Click;
            buttonBuild(btn);

            Button back = new Button();
            back.Text = "Back";
            buttonBuild(back);
            back.Click += Back_Click;
            back.SetBounds((Bounds.Width - 164), (Bounds.Height - 42), 80, 40);

            AcceptButton = btn;

            Controls.Add(btn);
            Controls.Add(box);
            Controls.Add(back);

            cancelButton();
            step(1);
            BackGround();
            btn.Focus();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            clear();
            TAS_Load(sender,e);
        }

        string[] names;
        string folderName;

        private TextBox bx;

        private void Btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Next-->2");
            names = box.Lines;
            clear();

            Button back = new Button();
            back.Text = "Back";
            buttonBuild(back);
            back.Click += Back2_Click;
            back.SetBounds((Bounds.Width - 164), (Bounds.Height - 42), 80, 40);

            bx = new TextBox();
            bx.Text = "C:\\Program Files\\TAS";
            bx.Font = MainFont;
            bx.Multiline = true;
            bx.SetBounds((Bounds.Width/2 - 155), (Bounds.Height/2 - 20), 250, 40);
            bx.BorderStyle = BorderStyle.None;

            Button DDD = new Button();
            DDD.Text = "...";
            buttonBuild(DDD);
            DDD.Click += DDD_Click;
            DDD.SetBounds((Bounds.Width/2 + 97), (Bounds.Height/2 - 20), 60, 40);

            Button btn = new Button();
            btn.Text = "Next";
            btn.Click += Next3_Click;
            buttonBuild(btn);

            Label lab = new Label();
            lab.Text = "Choose Install Location";
            lab.Font = MainFont;
            lab.TextAlign = ContentAlignment.MiddleCenter;
            lab.SetBounds((Bounds.Width/2 - 175), (Bounds.Height / 2 - 60), 350, 40);

            Controls.Add(btn);
            Controls.Add(DDD);
            Controls.Add(back);
            Controls.Add(bx);
            Controls.Add(lab);

            cancelButton();
            step(2);

            BackGround();
        }

        private void step(int i)
        {
            Label stp = new Label();
            stp.Text = "Step " + i + "/3";
            stp.ForeColor = Color.White;
            stp.TextAlign = ContentAlignment.MiddleCenter;
            stp.Font = MainFont;
            stp.SetBounds((122),(Bounds.Height - 42),200,40);

            Controls.Add(stp);
        }

        Label p1 = new Label(), p2 = new Label(),p3 = new Label();
        Timer tm1 = new Timer(),tm2 = new Timer(),tm3 = new Timer();
        Timer tm12 = new Timer(), tm22 = new Timer(), tm32 = new Timer();

        private void Next3_Click(object sender, EventArgs e)
        {
            clear();
            Console.WriteLine("Next-->3");

            tm1.Tick += Tm1_Tick;
            tm2.Tick += Tm2_Tick;
            tm3.Tick += Tm3_Tick;

            tm12.Tick += Tm12_Tick;
            tm22.Tick += Tm22_Tick;
            tm32.Tick += Tm32_Tick;

            foreach (Timer l in new Timer[] { tm1, tm2, tm3 })
            {
                l.Interval = 1;
                l.Start();
            }

            foreach (Timer l in new Timer[] { tm12, tm22, tm32 })
            {
                l.Interval = 1;
            }

            Label pw = new Label();
            pw.Font = MainFont;
            pw.Text = "Please Wait";
            pw.TextAlign = ContentAlignment.MiddleCenter;
            pw.SetBounds((Bounds.Width/2 - 90), (Bounds.Height/2 - 60), 180, 40);

            Button btn = new Button();
            btn.Text = "Finish";
            btn.Click += Fin_Click;
            buttonBuild(btn);
            btn.Enabled = false;
            btn.SetBounds((Bounds.Width - 102), (Bounds.Height - 42), 100, 40);

            Controls.Add(btn);

            cancelButton();

            step(3);

            foreach (Label l in new Label[] { p1, p2, p3 })
            {
                l.Font = MainFont;
                l.Text = ".";
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.BackColor = Color.Transparent;
            }

            Controls.Add(p3);
            Controls.Add(p2);
            Controls.Add(p1);
            Controls.Add(pw);

            Label lsb = BackGround();
            p1.Parent = lsb;
            p2.Parent = lsb;
            p3.Parent = lsb;

            System.Threading.ThreadStart threadDelegate = new System.Threading.ThreadStart(Work.DoWork);
            System.Threading.Thread newThread = new System.Threading.Thread(threadDelegate);
            newThread.Start();
        }

        private void Tm32_Tick(object sender, EventArgs e)
        {
            if (ts3 <= -(Bounds.Width + 20))
            {
                tm32.Stop();
                ts3 = 5;
                ts2 = 5;
                ts1 = 5;
                foreach (Timer l in new Timer[] { tm1, tm2, tm3 })
                {
                    l.Start();
                }
                return;
            }
            p3.SetBounds(Bounds.Width + ts3, (Bounds.Height / 2 - 20), 20, 40);
            ts3 -= 2;
        }

        private void Tm22_Tick(object sender, EventArgs e)
        {
            if (ts2 <= -(Bounds.Width + 20))
            {
                tm22.Stop();
                return;
            }
            p2.SetBounds(Bounds.Width + ts2, (Bounds.Height / 2 - 20), 20, 40);
            ts2 -= 3;
        }

        private void Tm12_Tick(object sender, EventArgs e)
        {
            if (ts1 <= -(Bounds.Width + 20))
            {
                tm12.Stop();
                return;
            }
            p1.SetBounds(Bounds.Width + ts1, (Bounds.Height / 2 - 20), 20, 40);
            ts1 -= 4;
        }

        private int ts1 = 5, ts2 = 5, ts3 = 5;

        private void Tm3_Tick(object sender, EventArgs e)
        {
            if (ts3 <= -((Bounds.Width / 2) - 20))
            {
                tm3.Stop();
                foreach (Timer l in new Timer[] {tm12, tm22, tm32 })
                {
                    l.Start();
                }
                return;
            }
            p3.SetBounds(Bounds.Width + ts3, (Bounds.Height / 2 - 20), 20, 40);
            ts3 -= 1;
        }

        private void Tm2_Tick(object sender, EventArgs e)
        {
            if (ts2 <= -((Bounds.Width / 2)))
            {
                tm2.Stop();
                return;
            }
            p2.SetBounds(Bounds.Width + ts2, (Bounds.Height / 2 - 20), 20, 40);
            ts2 -= 2;
        }

        private void Tm1_Tick(object sender, EventArgs e)
        {
            if (ts1 <= -((Bounds.Width / 2) + 20))
            {
                tm1.Stop();
                return;
            }
            p1.SetBounds(Bounds.Width + ts1, (Bounds.Height / 2 - 20), 20, 40);
            ts1 -= 3;
        }

        private void Fin_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void CreateShortcut(string shortcutPath, string shortcutDest)
        {
            StreamWriter sw = new StreamWriter(shortcutPath);
            sw.WriteLine("[InternetShortcut]");
            sw.WriteLine("URL=file:///" + shortcutDest);
            sw.WriteLine("IconIndex=0");
            sw.WriteLine("IconFile=" + shortcutDest);
            sw.Close();
        }

        private void Back2_Click(object sender, EventArgs e)
        {
            clear();
            Next_Click(sender,e);
        }

        public static Stream getStream(string s)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            return myAssembly.GetManifestResourceStream("TAS_Installer." + s);
        }

        private void DDD_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fld = new FolderBrowserDialog();
            if (fld.ShowDialog() == DialogResult.OK)
            {
                folderName = fld.SelectedPath;
                if(folderName.Last() == '\\')
                {
                    folderName = folderName.Remove(folderName.Length - 1,1);
                }
                bx.Text = folderName + @"\TAS";
                folderName = bx.Text;
            }
        }

        private void clear()
        {
            foreach (Control cl in Controls)
            {
                cl.Visible = false;
            }
        }

        public void buttonBuild(Button next)
        {
            next.BackColor = Color.White;
            next.Font = MainFont;
            next.SetBounds((Bounds.Width - 82), (Bounds.Height - 42), 80, 40);
            next.FlatStyle = FlatStyle.Flat;
            next.FlatAppearance.BorderColor = Color.Black;
            next.FlatAppearance.BorderSize = 0;
        }

        private Label BackGround()
        {
            Label labd = new Label();
            labd.SetBounds(0,0,Bounds.Width,Bounds.Height);
            labd.Image = Image.FromStream(getStream("Resources.Back.png"));
            Controls.Add(labd);

            foreach(Control l in Controls)
            {
                if (l is Label && l != labd) {
                    l.Parent = labd;
                    l.BackColor = Color.Transparent;
                }
            }
            return labd;
        }

        public void cancelButton()
        {
            Button canc = new Button();
            canc.Text = "Cancel";
            buttonBuild(canc);
            canc.Click += Canc_Click;
            canc.BackColor = Color.White;
            canc.SetBounds(2, (Bounds.Height - 42), 100, 40);
            CancelButton = canc;

            Controls.Add(canc);
        }

        private void Canc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to cancel the setup?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
        }
    }

    class Work
    {
        Work() { }

        public static void DoWork()
        {

        }
    }
}
