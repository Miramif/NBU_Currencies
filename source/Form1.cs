using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace CurrencyDisp
{
    public partial class Form1 : Form
    {
        bool XmlLoadedCorrectly = false;
        XmlParser parser;
        public Form1()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            InitializeComponent();
            Cur_Type.SelectedIndex = 3;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DateTime buf = DateTime.Now;
            if (dateTimePicker1.Value > buf)
            {
                MessageBox.Show("This date has not yet arrived. Date will be set to today.","Inccorect date",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                dateTimePicker1.Value = buf;
            }
            MysqlConnect db = new MysqlConnect();
            List<string>[] a = db.Exists(db.Choose_Type(Cur_Type.Text), dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            if (string.Join(",", a[0].ToArray()) == "")
            {
                buf = dateTimePicker1.Value;
                string path = "https://old.bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode="
                    + Cur_Type.Text + "&date=" + buf.ToString("yyyyMMdd");
                byte[] data;
                using (WebClient webClient = new WebClient())
                    data = webClient.DownloadData(path);
                string str = Encoding.GetEncoding("Windows-1252").GetString(data);
                XDocument XmlCur = XDocument.Parse(str);
                foreach (XElement record in XmlCur.Element("exchange").Elements("currency"))
                {

                    XElement name = record.Element("cc");
                    XElement ratio = record.Element("rate");
                    XElement date = record.Element("exchangedate");

                    if (name != null && ratio != null && date != null)
                    {
                        parser = new XmlParser(name.Value, float.Parse(ratio.Value, CultureInfo.InvariantCulture),
                             Convert.ToDateTime(date.Value));
                        XmlLoadedCorrectly = true;
                        if (XmlLoadedCorrectly)
                        {
                            label1.Text = "Type: " + parser.CurNameGS;
                            label2.Text = "Rate: " + parser.RatioGS.ToString();
                            label3.Text = "Date: " + parser.DateGS.ToString("dd.MM.yyyy");
                            db = new MysqlConnect();
                            db.Insert(db.Choose_Type(parser.CurNameGS), parser.RatioGS.ToString(), parser.DateGS.ToString("yyyy-MM-dd"));
                        }

                    }
                }
            }
            else
            {
                label1.Text = "Type: " + db.Choose_TypeR(Convert.ToInt32(string.Join(",", a[1].ToArray())));
                label2.Text = "Rate: " + string.Join(",", a[2].ToArray());
                buf = Convert.ToDateTime(string.Join(",", a[3].ToArray()));
                label3.Text = "Date: " + buf.ToString("dd.MM.yyyy");
            }



        }

    }
}
