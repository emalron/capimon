using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class Group
    {
        public Group(byte[] input, Memory m, int id, int address)
        {
            this.rawdata = input;
            this.pid = id;
            this.address = address;
            this.mem = m;
        }

        Memory mem;

        public byte[] rawdata;
        public int pid;
        private int address;
        private string name;

        private int numFirms;
        public int cityNo;
        private int playerNo_;
        public List<Firm> myFirms;

        public Element Name;
        public Element GroupAddress;

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
            this.name = Encoding.ASCII.GetString(rawdata, startPoint, i-startPoint);

            this.numFirms = BitConverter.ToInt16(this.rawdata, 0xa8);
            int firmArray = BitConverter.ToInt32(this.rawdata, 0x1ec);

            myFirms = new List<Firm>();
            for(int j=0; j<numFirms; j++)
            {
                int firm_ptr = firmArray + j * 0x4 - 0x400000;
                int firm_addr = BitConverter.ToInt32(mem.watching(firm_ptr, 4), 0) - 0x400000;
                int size = 0xcc1;
                byte[] firm = mem.watching(firm_addr, size);

                myFirms.Add(new Firm(firm));
            }

            string addr = this.address.ToString("X").Replace("-", "");
            GroupAddress = new Element("Group Address", addr);

            Name = new Element("Group Name", this.name);
        }
    }
}
