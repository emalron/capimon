using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace Capimon
{
    public class DataHandler
    {
        public DataHandler(DataFactory df)
        {
            dFactory = df;
        }

        DataFactory dFactory;

        public DataSet ds;
        DataTable dbTable;
        delegate T finder<T>(List<T> list, int index);

        List<Element> getColumns(int i)
        {
            List<Element> cols = new List<Element>();

            Item item_ = dFactory.Items[i];
            ItemDB db_ = dFactory.ItemDBs[i];
            ItemInfo info_ = dFactory.ItemInfos[i];
            int cityNo_ = item_.cityNo - 1;
            City city_ = dFactory.Cities[cityNo_];
            int firmNo_ = item_.firmNo;

            finder<Firm> finder = (firms, index) => {
                // Firm result = firms.FirstOrDefault(x => x.firmNo == index);

                Firm result = firms.SingleOrDefault(x => x.firmNo == index);

                return result;
            };

            Firm firm_ = finder(dFactory.Firms, firmNo_);
            int groupNo_;
            if(firm_ == null)
            {
                groupNo_ = 0;
            }
            else
            {
                groupNo_ = firm_.groupNo - 1;
            }
            Group group_ = dFactory.Groups[groupNo_];

            cols.Add(item_.ItemAddress);
            cols.Add(item_.ItemID);
            cols.Add(db_.Name);
            cols.Add(db_.Address);
            cols.Add(info_.Address);
            cols.Add(item_.FirmNo);
            cols.Add(city_.Name);
            cols.Add(group_.Name);

            return cols;
        }

        void initDatabase()
        {
            ds = new DataSet();
            dbTable = new DataTable("Items");

            ds.Tables.Add(dbTable);

            DataColumn col = new DataColumn();
            col = new DataColumn();
            col.DataType = System.Type.GetType("System.Int32");
            col.ColumnName = "ID";
            col.AutoIncrement = true;
            dbTable.Columns.Add(col);

            List<Element> cols = getColumns(0);

            foreach (var e in cols)
            {
                DataColumn column = convertToDataColumn(e);
                dbTable.Columns.Add(column);
            }
        }

        DataColumn convertToDataColumn(Element e)
        {
            DataColumn dc = new DataColumn();
            dc.DataType = typeof(string);
            dc.ColumnName = e.name;
            return dc;
        }

        public void makeDatabase()
        {
            initDatabase();

            DataRow row;
            List<Element> elements;

            for (int i = 0; i < dFactory.Items.Count; i++)
            {
                row = dbTable.NewRow();

                elements = getColumns(i);

                foreach(var e in elements)
                {
                    addRow(ref row, e);
                }

                dbTable.Rows.Add(row);
            }
        }

        void addRow(ref DataRow row, Element e)
        {
            row[e.name] = e.value;
        }
    }
}
