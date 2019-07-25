using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class DataFactory
    {
        public DataFactory(Memory m)
        {
            this.mem = m;
        }
        Memory mem;

        public List<Item> Items;
        public List<ItemDB> ItemDBs;
        public List<City> Cities;
        public List<ItemInfo> ItemInfos;
        public List<Group> Groups;
        public List<Firm> Firms;

        void init()
        {
            Items = new List<Item>();
            ItemDBs = new List<ItemDB>();
            Cities = new List<City>();
            ItemInfos = new List<ItemInfo>();
            Groups = new List<Group>();
            Firms = new List<Firm>();
        }

        public void makeLists()
        {
            init();
            makeListItems();
            makeListItemDB();
            makeListCity();
            makeListItemInfo();
            makeListGroup();
            makeListFirm();
        }

        void makeListItems()
        {
            // get information of "Item" from its pointers
            int ItemMain_addr = 0x007d3fc8 - 0x400000;
            int numItems_ptr = ItemMain_addr + 0xc;
            int sizeItem_ptr = ItemMain_addr + 0x10;
            int itemBase_ptr = ItemMain_addr + 0x1c;

            // get real addresses of information of "Item"
            int numItems = BitConverter.ToInt32(mem.watching(numItems_ptr, 4), 0);
            int sizeItem = BitConverter.ToInt32(mem.watching(sizeItem_ptr, 4), 0);
            int itemBase = BitConverter.ToInt32(mem.watching(itemBase_ptr, 4), 0) - 0x400000;
            
            for (int i = 0; i < numItems; i++)
            {
                int address = itemBase + i * sizeItem;
                IntPtr baseAddr = mem.Target.MainModule.BaseAddress;

                bool itemValid = BitConverter.ToInt16(mem.watching(address, 2), 0) != 0;

                if(itemValid)
                {
                    this.Items.Add(new Item(mem.watching(address, sizeItem), i, address + (int)baseAddr));
                }
            }

            this.Items.ForEach(o => { o.setProperties(); });
        }

        void makeListItemDB()
        {
            // get information of "item DB"
            int itemDBbase_ptr = 0x7cc03c - 0x400000;
            int itemDBbase = BitConverter.ToInt32(mem.watching(itemDBbase_ptr, 4), 0);

            int sizeItemDB = 0x9f8;

            bool items_isnt_empty = this.Items.Count != 0;
            if(items_isnt_empty)
            {
                this.Items.ForEach(o =>
                {
                    int id = o.itemID;
                    int addr = itemDBbase + sizeItemDB * (id - 1) - 0x400000;
                    this.ItemDBs.Add(new ItemDB(mem.watching(addr, sizeItemDB), itemDBbase, sizeItemDB, id, o.pid));
                });
                
                this.ItemDBs.ForEach(o => { o.setProperties(); });
            }
        }

        void makeListCity()
        {
            int cityMain_addr = 0x007727c8 - 0x400000;
            int numCity_ptr = cityMain_addr + 0x14;
            int sizeCity_ptr = cityMain_addr + 0x18;
            int listCity_ptr = cityMain_addr + 0x24;

            int numCity = BitConverter.ToInt32(mem.watching(numCity_ptr, 4), 0);
            int sizeCity = BitConverter.ToInt32(mem.watching(sizeCity_ptr, 4), 0);
            int listCity = BitConverter.ToInt32(mem.watching(listCity_ptr, 4), 0);
            bool items_isnt_empty = this.Items.Count != 0;
            if (items_isnt_empty)
            {
                for (int i = 0; i < numCity; i++)
                {
                    int city_ptr = listCity + i * 0x4 - 0x400000;
                    int city_addr = BitConverter.ToInt32(mem.watching(city_ptr, sizeCity), 0) - 0x400000;
                    int size = 0x9c40;
                    int cityNo_ = i + 1;
                    byte[] city = mem.watching(city_addr, size);

                    this.Cities.Add(new City(city, mem, cityNo_));
                }

                this.Cities.ForEach(o => { o.setProperties(); });
            }
        }

        void makeListItemInfo()
        {
            int itemInfoBase_addr = 0x77280c - 0x400000;
            int itemInfoBase = BitConverter.ToInt32(mem.watching(itemInfoBase_addr, 4), 0);

            bool items_isnt_empty = this.Items.Count != 0;
            if (items_isnt_empty)
            {
                foreach (var i in this.Items)
                {
                    int cityNo = i.cityNo - 1;
                    City city_ = Cities[cityNo];

                    this.ItemInfos.Add(new ItemInfo(itemInfoBase, i, city_, mem));
                }

                this.ItemInfos.ForEach(o => { o.setProperties(); });
            }
        }

        void makeListGroup()
        {
            int MainTable_addr = 0x007e23f8 - 0x400000;
            int numGroup_addr = MainTable_addr + 0xc;
            int groupArray_ptr = MainTable_addr + 0x1c;

            int numGroup = BitConverter.ToInt32(mem.watching(numGroup_addr, 4), 0);
            int groupArray = BitConverter.ToInt32(mem.watching(groupArray_ptr, 4), 0);

            for (int i = 0; i < numGroup; i++)
            {
                int group_ptr = groupArray + i * 0x4 - 0x400000;
                int group_addr = BitConverter.ToInt32(mem.watching(group_ptr, 4), 0) - 0x400000;
                int size = 0x35b8;
                byte[] group = mem.watching(group_addr, size);

                this.Groups.Add(new Group(group, mem, i, group_addr));
            }

            this.Groups.ForEach(o => { o.setProperties(); });
        }

        void makeListFirm()
        {
            foreach (var g in Groups)
            {
                foreach (var f in g.myFirms)
                {
                    this.Firms.Add(f);
                }
            }

            //int MainTable_addr = 0x007dc8a8 - 0x400000;
            //int numFirm_addr = MainTable_addr + 0xc;
            //int firmArray_ptr = MainTable_addr + 0x1c;

            //int numFirm = BitConverter.ToInt32(mem.watching(numFirm_addr, 4), 0);
            //int firmArray = BitConverter.ToInt32(mem.watching(firmArray_ptr, 4), 0);

            //for (int i = 0; i < numFirm; i++)
            //{
            //    int firm_ptr = firmArray + i * 0x4 - 0x400000;
            //    int firm_addr = BitConverter.ToInt32(mem.watching(firm_ptr, 4), 0) - 0x400000;
            //    int size = 0xcc0;
            //    byte[] firm = mem.watching(firm_addr, size);

            //    this.Firms.Add(new Firm(firm));
            //}

            this.Firms.ForEach(o =>
            {
                o.setProperties();
            });
        }
    }
}
