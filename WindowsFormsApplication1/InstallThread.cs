using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace TAS_Installer
{
    public class Install : Work
    {
        string loc;
        string[] names;
        Control cl;

        public Install(Thread th,string loc,string[] names,Control cl)
        : base(th){
            this.loc = loc;
            this.names = names;
            this.cl = cl;
        }

        public override void DoWork()
        {
            try
            {
                Directory.CreateDirectory(loc);
            } catch (ArgumentNullException){
                exitBadState("0x01",2,"No File acces\nPlease use an other Path");
            }
            FileStream fl = File.Create(loc + @"\Class.lis");
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("TAS");
            key.SetValue("Path", loc + @"\Class.lis");
            key.Close();
            foreach (string s in names)
            {
                byte[] bt = GetBytes(s);
                fl.Write(bt,0,bt.Length);
                byte[] ub = GetBytes(Environment.NewLine);
                fl.Write(ub,0,ub.Length);
            }
            byte[] ex = GetBytes("<EXCEPTION>");
            byte[] bn = GetBytes("<BANNLIST>");
            fl.Write(ex,0,ex.Length);
            byte[] usb = GetBytes(Environment.NewLine);
            fl.Write(usb, 0, usb.Length);
            fl.Write(bn,0,bn.Length);
            fl.Flush();
            fl.Close();

            final();
        }

        public void final()
        {
            if (TAS.p1.InvokeRequired)
            {
                cl.Invoke((Action)final);
                return;
            }
            TAS.p1.Visible = false;
            TAS.p2.Visible = false;
            TAS.p3.Visible = false;

            TAS.tm1.Stop();
            TAS.tm2.Stop();
            TAS.tm3.Stop();

            TAS.tm12.Stop();
            TAS.tm22.Stop();
            TAS.tm32.Stop();

            TAS.pw.Text = "Finished";
            TAS.fn.Enabled = true;
            return;
        }

        public int exitBadState(string state,int s,string st)
        {
            if (TAS.p1.InvokeRequired)
            {
                cl.Invoke((Action)(() => exitBadState(state,s,st)));
                return s;
            }

            if (MessageBox.Show(st,"Error " + state,MessageBoxButtons.OKCancel,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false) == DialogResult.Cancel) {
                Form.ActiveForm.Close();
                Application.Exit();
                return s;
            }
            Form fr = cl.FindForm();
            TAS ts = (TAS)fr;
            ts.initStep(s);
            ts.BringToFront();
            ts.Focus();
            return 0;
        }
        

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
