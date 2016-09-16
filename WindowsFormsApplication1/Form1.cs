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
            SetDesktopLocation((b.Width/2 - Bounds.Width/2), (b.Height / 2 - Bounds.Height / 2));

            Label lab = new Label();
            lab.Text = "Welcome to TAS\nInstaller";
            lab.Font = MainFont;
            lab.SetBounds(((Bounds.Width/2) - 100), ((Bounds.Height/2) - 125), 220, 50);


            Button next = new Button();
            next.Text = "Next";
            buttonBuild(next);
            next.Click += Next_Click; 

            AcceptButton = next;

            Controls.Add(next);
            Controls.Add(lab);
            cancelButton();
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
            btn.Focus();
        }

        string[] names;
        string folderName;

        private void Btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Next-->2");
            names = box.Lines;
            clear();

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
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Visible = false;
            }
        }

        public string get(string name)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @name);
        }

        public void buttonBuild(Button next)
        {
            next.BackgroundImage = Image.FromFile(get("Button.png"));
            next.Font = MainFont;
            next.SetBounds((Bounds.Width - 82), (Bounds.Height - 42), 80, 40);
            next.FlatStyle = FlatStyle.Flat;
            next.FlatAppearance.BorderColor = Color.Black;
            next.FlatAppearance.BorderSize = 0;
        }

        public void cancelButton()
        {
            Button canc = new Button();
            canc.Text = "Cancel";
            buttonBuild(canc);
            canc.Click += Canc_Click;
            canc.BackColor = Color.White;
            canc.SetBounds(5, (Bounds.Height - 42), 100, 40);

            Controls.Add(canc);
            CancelButton = canc;
        }

        private void Canc_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you real wan't to cancel the Setup", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
        }
    }
}
