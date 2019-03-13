using System.Threading;

namespace Project
{
    public class TimeKeeping
    {
        public static int time;

        public TimeKeeping()
        {
            time = 0;
        }

        public void Timeloop()
        {
            while (Application.running)
            {
                Thread.Sleep(1000);
                time++;
            }
            time = 0;
        }

        public int getTime()
        {
            return time;
        }
    }
}
