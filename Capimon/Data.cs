using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class Data
    {
        public Data(int day, double value)
        {
            this.daySpent = day;
            this.interestedValue = value;
        }

        public int daySpent;
        public double interestedValue;
    }
}
