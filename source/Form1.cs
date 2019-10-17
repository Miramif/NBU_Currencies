using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DateTime buf;
                bool TryDate = DateTime.TryParse(textBox2.Text, out buf);
            if (!TryDate) 
            {
                MessageBox.Show("Incorrect format of date. Should be dd.mm.yyyy. Date will be set to current.", "Date error", MessageBoxButtons.OK);
                buf = DateTime.Now;
                //label1.Text = buf.ToString("yyyyMMdd");
            }

            string path = "https://old.bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode="
                + textBox1.Text + "&date=" + buf.ToString("yyyyMMdd");

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
                    parser = new XmlParser(name.Value, double.Parse(ratio.Value, CultureInfo.InvariantCulture),
                         Convert.ToDateTime(date.Value));
                    XmlLoadedCorrectly = true;
                }
                if (XmlLoadedCorrectly)
                {
                    label1.Text = parser.CurNameGS;
                    label2.Text = parser.RatioGS.ToString();
                    label3.Text = Convert.ToString(parser.DateGS.Date);
                }
            }

        }
    }
}
