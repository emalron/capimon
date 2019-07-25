using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class ItemInfo
    {
        public ItemInfo(int address, Item item, City city, Memory m)
        {
            this.baseAddr = address;
            this.item = item;
            this.city = city;
            this.size = 0x928;

            mem = m;

            this.pid = item.itemID;
        }

        Memory mem;
        Item item;
        City city;
        int baseAddr;
        int size;
        public int pid;
        private int address;

        public Element Address;

        public void setProperties()
        {
            int itemID_ = item.itemID;
            int id_addr = city.idListBase + 0x2 * (itemID_ - 1) - 0x400000;

            int id_ = BitConverter.ToInt16(mem.watching(id_addr,2), 0);

            if(id_ != 0)
            {
                this.address = this.baseAddr + this.size * (id_ - 1);
            }
            else
            {
                this.address = 0x0;
            }

            string addr = this.address.ToString("X").Replace("-", "");

            Address = new Element("ItemInfo Address", addr);
        }
    }
}
