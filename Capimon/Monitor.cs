using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Capimon
{
    public class Monitor
    {
        public Monitor(OnUpdateChart f)
        {
            mem = new Memory();

            this.callback = f;
            this.monitor = new Thread(() => { });
        }

        Memory mem;
        public Thread monitor;

        public delegate void OnUpdateChart(Data f);
        OnUpdateChart callback;

        public void startMon()
        {
            monitor = new Thread(() =>
            {
                int daySpent_;
                int prev_daySpent = 0;
                double totalMoney_ = 0;

                while (true)
                {
                    // watching Day spent which is located at MainModule + 0x003d75ac, size is 4 bytes
                    daySpent_ = BitConverter.ToInt32(mem.watching(0x003d75ac, 4), 0);

                    totalMoney_ = BitConverter.ToDouble(mem.watching(0x0d6eeb80, 8), 0);

                    // store it to the list
                    if (daySpent_ > prev_daySpent)
                    {
                        Data d_ = new Data(daySpent_, totalMoney_);
                        prev_daySpent = daySpent_;

                        this.callback(d_);
                    }
                }
            });

            monitor.Start();
        }
    }
}
