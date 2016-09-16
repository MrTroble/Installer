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

namespace WindowsFormsApplication1
{
    public partial class TAS : Form
    {
        public static Font MainFont;

        public TAS()
        {
            InitializeComponent();
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(get("CONSOLAB.TTF"));
            FontFamily fontFamily = new FontFamily("Consolas", collection);
            MainFont = new Font(fontFamily, 18.0F);
            Icon = new Icon(get("back2.ico"));
            FormBorderStyle = FormBorderStyle.None;
        }

        private void TAS_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Start Programm");
            Rectangle b = Screen.FromControl(this).Bounds;
            SetBounds((b.Width/2 - 250), (b.Height / 2 - 200),500,400);

            Label lab = new Label();
            lab.Text = "Welcome to TAS\nInstaller";
            lab.Font = MainFont;
            lab.SetBounds(((Bounds.Width/2) - 100), ((Bounds.Height/2) - 125), 220, 60);
            lab.BorderStyle = BorderStyle.None;
            lab.TextAlign = ContentAlignment.MiddleCenter;
            lab.BackColor = Color.FromKnownColor(KnownColor.Transparent);

            Button next = new Button();
            next.Text = "Next";
            buttonBuild(next);
            next.Click += Next_Click; 

            AcceptButton = next;

            Controls.Add(next);
            Controls.Add(lab);
            cancelButton();
            BackGround();
        }


        TextBox box;

        private void Next_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Next-->");
            clear();

            box = new TextBox();
            box.Multiline = true;
            box.AcceptsReturn = true;
            box.Lines = new string[] { "Write downe the names","in each Line one name"};
            box.SetBounds(2, 2, Bounds.Width - 4, Bounds.Height - 100);
            box.Font = new Font(MainFont.FontFamily,12.0F);

            Button btn = new Button();
            btn.Text = "Next";
            btn.Click += Btn_Click;
            buttonBuild(btn);

            AcceptButton = btn;

            Controls.Add(btn);
            Controls.Add(box);

            cancelButton();
            BackGround();
            btn.Focus();
        }

        string[] names;
        string folderName;

        private void Btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Next-->2");
            names = box.Lines;
            clear();

            BackGround();

            FolderBrowserDialog fld = new FolderBrowserDialog();
            if(fld.ShowDialog() == DialogResult.OK)
            {
                folderName = fld.SelectedPath;
            }
            else
            {
                Canc_Click(sender, e);
                if(Visible) Btn_Click(sender, e);
            }


        }

        private void clear()
        {
            foreach(Control cl in Controls)
            {
                cl.Visible = false;
            }
        }

        public string get(string name)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @name);
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
            labd.Image = Image.FromFile(get("Back.png"));
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
            if (MessageBox.Show("Do you real wan't to cancel the Setup", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
        }
    }
}
