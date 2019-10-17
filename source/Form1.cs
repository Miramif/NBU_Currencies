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
            DateTime buf = dateTimePicker1.Value;
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
                }
                if (XmlLoadedCorrectly)
                {
                    label1.Text = parser.CurNameGS;
                    label2.Text = parser.RatioGS.ToString();
                    label3.Text = parser.DateGS.ToString();
                    MysqlConnect db = new MysqlConnect();
                   // db.Insert(db.Choose_Type(parser.CurNameGS), parser.RatioGS, parser.DateGS.ToString("yyyy-MM-dd"));
                  //  label3.Text = "select * from ratio where type_id = " + 2 + "and r_date = '" + parser.DateGS.ToString("yyyy-MM-dd") + "'";
                     List<string>[] a = db.Exists(db.Choose_Type(parser.CurNameGS), parser.DateGS.ToString("yyyy-MM-dd"));
                      label3.Text = string.Join(",", a[0].ToArray());
                    if (string.Join(",", a[0].ToArray()) == "") MessageBox.Show("NOT FOUND", "NO IN DB", MessageBoxButtons.OK);
                }
            }

        }

    }
}
