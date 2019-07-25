using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class Item
    {
        public Item(byte[] input, int id, int address)
        {
            this.rawdata = input;

            this.pid = id;

            this.address = address;
        }

        public byte[] rawdata;

        public int pid;
        private int address;

        public int itemID;
        public int firmNo;
        public int cityNo;
        private int playerNo_;

        public Element ItemID;
        public Element FirmNo;
        public Element CityNo;
        public Element PlayerNo;
        public Element ItemAddress;

        public void setProperties()
        {
            this.itemID = BitConverter.ToInt16(this.rawdata, 0);
            this.firmNo = BitConverter.ToInt16(this.rawdata, 0x10);
            this.cityNo = BitConverter.ToInt16(this.rawdata, 0x12);
            this.playerNo_ = BitConverter.ToInt16(this.rawdata, 0x16);

            string addr = this.address.ToString("X").Replace("-","");

            ItemID = new Element("Item ID", itemID.ToString());
            FirmNo = new Element("Firm #", this.firmNo.ToString());
            CityNo = new Element("City #", this.cityNo.ToString());
            PlayerNo = new Element("Player #", this.playerNo_.ToString());
            ItemAddress = new Element("Item Address", addr);

        }
    }
}
