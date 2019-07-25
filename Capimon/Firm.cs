using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class Firm
    {
        public Firm(byte[] input)
        {
            this.rawdata = input;
        }

        public byte[] rawdata;

        public int isRetail;
        public int firmNo;
        public int cityNo;
        public int groupNo;
        public int howManyItems;
        public string name;

        public void setProperties()
        {
            // get name
            int i = 0xa;
            int startPoint = 0xa;
            byte temp_ = rawdata[i];

            while (temp_ != 0x00)
            {
                i++;
                temp_ = rawdata[i];
            }
            this.name = Encoding.ASCII.GetString(rawdata, startPoint, i - startPoint);

            this.isRetail = BitConverter.ToInt32(rawdata, 0x74);
            this.firmNo = BitConverter.ToInt32(rawdata, 0xc);
            this.cityNo = BitConverter.ToInt16(rawdata, 0x10);
            this.groupNo = BitConverter.ToInt32(rawdata, 0xec);
            this.howManyItems = BitConverter.ToInt32(rawdata, 0xe8);
        }
    }
}
