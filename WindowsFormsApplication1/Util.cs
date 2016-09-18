using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TAS_Installer
{
    class Util
    {

        public static Font LoadFont(byte[] buffer, float f)
        {

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                PrivateFontCollection fontCollection = new PrivateFontCollection();
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                fontCollection = new PrivateFontCollection();
                fontCollection.AddMemoryFont(ptr, buffer.Length);
                return new Font(fontCollection.Families[0], f);
            }
            finally
            {
                handle.Free();
            }
        }

        public static Font LoadFont(Stream s,float f)
        {
            return LoadFont(getBytes(s),f);
        }

        public static Font LoadFont(string s, float f)
        {
            return LoadFont(getBytes(getStream(s)), f);
        }

        public static byte[] getBytes(Stream s)
        {
            byte[] ar = new byte[s.Length];
            s.Read(ar,0,ar.Length);
            return ar;
        }

        public static Stream getStream(string s)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            return myAssembly.GetManifestResourceStream(s);
        }

        public static Image getImage(string s)
        {
            return Image.FromStream(getStream(s));
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

        public static System.Threading.Thread creatAndStartThread(Work w)
        {
            System.Threading.ThreadStart threadDelegate = new System.Threading.ThreadStart(w.DoWork);
            System.Threading.Thread newThread = new System.Threading.Thread(threadDelegate);
            newThread.Start();
            return newThread;
        }
    }
}
