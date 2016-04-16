using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Map
{
    public partial class Form1 : Form
    {
        DatabaseConnection objConnect;
        string conString;
        DataSet ds;
        DataRow dRow;
        int inc = 0;
               
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<string, string[]> data = new Dictionary<string, string[]>();

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(Label))
                {
                    try
                    {
                        if (c.Name == "label_UstKamenogorsk")
                        {
                            string ust = "label_Ust-Kamenogorsk";
                            c.Name = ust;
                        }
                        string name = c.Name.Split('_')[1].ToLower();
                        string url = String.Format(@"http://www.kazhydromet.kz/rss-pogoda.php?id={0}", name);
                        XmlReader reader = XmlReader.Create(url);

                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();

                        if (feed.Items.Count() > 0)
                        {
                            string text = feed.Items.ElementAt(0).Summary.Text;
                            string[] arr = text.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr.Length >= 4)
                            {
                                for (int i = 0; i < arr.Length; ++i)
                                {
                                    arr[i] = arr[i].Trim();
                                }
                                data[name] = arr;
                            }
                        }
                        c.Text = data[name][0];
                        inc = 0;
                        DataRow row = ds.Tables[0].Rows[inc];
                        string string_table = ds.Tables[0].Rows[inc].ItemArray.GetValue(0).ToString();
                        if ( string_table == name)
                        {
                            row[1] = data[name][0];
                            ds.Tables[0].Rows.Add(row);
                        }
                        
                        inc++;
                    }

                    catch(Exception ee)
                    {
                        inc = 0;
                        dRow = ds.Tables[0].Rows[inc];
                        inc++;
                        c.Text = dRow.ItemArray.GetValue(1).ToString();
                    }
                    
                }
            }

            objConnect.UpdateDatabase(ds);

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objConnect = new DatabaseConnection();
            conString = Properties.Settings.Default.WeatherConnectionString;
            objConnect.connection_string = conString;
            objConnect.Sql = Properties.Settings.Default.SQL;

            ds = objConnect.GetConnection;
        }
    }
}
