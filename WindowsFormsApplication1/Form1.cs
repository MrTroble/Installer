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
            box.Lines = new string[] { "Write the names down", "only one name for each line" };
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

            Button DDD = new Button();
            DDD.Text = "...";
            buttonBuild(DDD);
            DDD.Click += DDD_Click;

            Button btn = new Button();
            btn.Text = "Next";
            btn.Click += Btn_Click;
            buttonBuild(btn);

            bx = new TextBox();
            bx.Text = "C:\\Program Files\\TAS";
            bx.Font = MainFont;

            Controls.Add(btn);
            Controls.Add(DDD);
            Controls.Add(back);
            Controls.Add(bx);

            cancelButton();

            BackGround();

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
                bx.Text = folderName + "\\TAS";
            }
            else
            {
                Canc_Click(sender, e);
                if (Visible) Btn_Click(sender, e);
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

        private void BackGround()
        {
            Label labd = new Label();
            labd.SetBounds(0,0,Bounds.Width,Bounds.Height);
            labd.Image = Properties.Resources.Back;
            Controls.Add(labd);

            foreach(Control l in Controls)
            {
                if (l is Label && l != labd) {
                    l.Parent = labd;
                    l.BackColor = Color.Transparent;
                }
            }
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
}
