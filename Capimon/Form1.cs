using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Capimon
{
    public partial class Form1 : Form
    {
        Memory memory;
        Monitor mon;
        DataFactory dFactory;
        DataHandler dHandle;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controlInit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mon.monitor.IsAlive)
            {
                mon.monitor.Abort();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int resultNum;
            string msg;

            init();

            resultNum = memory.init();
            msg = memory.setMsg(resultNum);

            label1.Text = msg;

            bool loadSuccess = resultNum == 0;
            if(loadSuccess)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mon.startMon();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dFactory.makeLists();
            dHandle.makeDatabase();

            dataGridView1.DataSource = dHandle.ds.Tables[0];
        }

        void controlInit()
        {
            label1.Text = "";
            label2.Text = "";
            chart1.Series.Add("total");
            chart1.Series["total"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            chart1.Series["total"].BorderWidth = 2;
        }

        int init()
        {
            memory = new Memory();

            Monitor.OnUpdateChart f = new Monitor.OnUpdateChart(updateChart);
            mon = new Monitor(f);

            dFactory = new DataFactory(memory);
            dHandle = new DataHandler(dFactory);

            int msg;
            msg = memory.init();

            return msg;
        }

        private void updateChart(Data d)
        {
            // test: thread safe
            if (chart1.InvokeRequired)
            {
                chart1.BeginInvoke(new Action(() => {
                    // chart1.Series[0].Points.AddY(data.DaySpent);
                    chart1.Series["total"].Points.AddY(d.interestedValue);
                }));
            }
            else
            {
                // chart1.Series[0].Points.AddY(data.DaySpent);
                chart1.Series["total"].Points.AddY(d.interestedValue);
            }

        }

    }
}
