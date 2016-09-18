using System.Threading;

namespace TAS_Installer
{
    public abstract class Work
    {
        public Thread th;

        public Work(Thread th) {
            this.th = th;
        }

        public abstract void DoWork();
    }
}
