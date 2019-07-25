using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class ItemDB
    {
        public ItemDB(byte[] rawdata, int address, int size, int id, int pid)
        {
            this.baseAddr = address;
            this.size = size;
            this.itemID = id;
            this.rawdata = rawdata;

            this.pid = pid;
        }

        byte[] rawdata;
        int baseAddr;
        int size;
        public int itemID;
        public int pid;

        private int addr;
        string name;

        public Element Address;
        public Element Name;

        public void setProperties()
        {
            addr = baseAddr + size * (itemID - 1);
            string address_ = this.addr.ToString("X").Replace("-", "");

            // get name
            int i = 0x23, s = 0x23;
            int delta = 0;
            byte temp_ = rawdata[i];

            while (temp_ != 0x00)
            {
                i++;
                delta++;
                temp_ = rawdata[i];
                
            }
            this.name = Encoding.ASCII.GetString(rawdata, s, delta);

            Address = new Element("Item DB Address", address_);
            Name = new Element("Item Name", this.name);
        }
    }
}
