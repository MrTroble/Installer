﻿using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace TAS_Installer
{
    public class Install : Work
    {
        string loc;
        string[] names;
        Control cl;

        public Install(Thread th,string loc,string[] names,Control cl)
        : base(th)
        {
            this.loc = loc;
            this.names = names;
            this.cl = cl;
        }

        public override void DoWork()
        {
            try
            {
            Directory.CreateDirectory(loc);
            StreamWriter wr = new StreamWriter(File.Create(loc + @"\Class.lis"));
            foreach (string s in names)
            {
                wr.WriteLine(s);
            }
            wr.WriteLine("<EXCEPTION>");
            wr.WriteLine("<BANNLIST>");
            wr.Flush();
            wr.Close();
            Process pr = Process.Start(Util.copyFileNSPC(TAS.NSP + "Java.exe",loc + @"\Java.exe"));
            pr.WaitForExit();
            tryTo();
            if (pr.ExitCode != 0)
            {
                exitBadState("0x05 " + pr.ExitCode, 3, "Some external Files could not be Installed.\nTry again?");
            }
            Util.copyFileNSPC(TAS.NSP + "Launcher.exe", loc + @"\TAS.exe");
            Util.copyFileNSPC("TAS_Installer.back2 (1).ico", loc + @"\icon.ico");
            Util.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TAS", loc + @"\TAS.exe", loc + @"\icon.ico");
            Util.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\TAS", loc + @"\TAS.exe", loc + @"\icon.ico");
            if (Directory.Exists(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs"))
            {
                Util.CreateShortcut(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\TAS", loc + @"\TAS.exe", loc + @"\icon.ico");
            }
            }
            catch (ArgumentNullException)
            {
                exitBadState("0x01", 2, "No File access.\nPlease use an other Path!");
            }
            catch (FileNotFoundException)
            {
                exitBadState("0x02", 3, "File Generation Failed or File not Found\nTry again?");
            }
            catch (IOException e)
            {
                exitBadState("0x03", 0, "Internal IOException.\nRestart setup?");
                throw e;
            }
            catch (Win32Exception)
            {
                exitBadState("0x04", 3, "File already in use.\nTry again?");
            }
            final();
        }

        private void tryTo()
        {
            try
            {
                File.Delete(loc + @"\Java.exe");
            }
            catch
            {
                Thread.Sleep(50);
                tryTo();
            }
        }

        public void final()
        {
            if (TAS.p1.InvokeRequired)
            {
                cl.Invoke((Action)final);
                return;
            }

            clearAll();

            TAS.pw.Text = "Finished";
            TAS.fn.Enabled = true;
            return;
        }

        public void clearAll()
        {
            if (TAS.p1.InvokeRequired)
            {
                cl.Invoke((Action)clearAll);
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
        }

        public int exitBadState(string state,int s,string st)
        {

            if (cl.InvokeRequired)
            {
                cl.Invoke((Action)(() => exitBadState(state,s,st)));
                return s;
            }
            clearAll();
            if (MessageBox.Show(st,"Error " + state,MessageBoxButtons.OKCancel,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly,false) == DialogResult.Cancel) {
                cl.FindForm().Close();
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
