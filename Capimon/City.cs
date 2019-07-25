using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class City
    {
        public City(byte[] input, Memory m, int cityNo)
        {
            this.rawdata = input;
            mem = m;

            this.cityNo = cityNo;
        }

        Memory mem;

        byte[] rawdata;
        private string name_;
        private double population_;
        private double realWageRate_;
        public int idListBase;
        public int cityNo;

        public Element Name;
        public Element Population;
        public Element RealWageRate;

        public void setProperties()
        {
            // get name
            int i = 0;
            byte temp_ = rawdata[i];

            while(temp_ != 0)
            {
                i++;
                temp_ = rawdata[i];
            }
            this.name_ = Encoding.ASCII.GetString(rawdata, 0, i);

            // get population
            this.population_ = BitConverter.ToDouble(rawdata, 0x40);

            // get real wage rate
            this.realWageRate_ = BitConverter.ToDouble(rawdata, 0x220);

            // get id list
            this.idListBase = BitConverter.ToInt32(rawdata, 0x410);

            Name = new Element("City Name", this.name_);
            Population = new Element("Population", this.population_.ToString());
            RealWageRate = new Element("Real Wage Rate", this.realWageRate_.ToString());
        }
    }
}
