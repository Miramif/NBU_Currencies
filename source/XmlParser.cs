using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Globalization;

namespace CurrencyDisp
{
    class XmlParser
    {
        string CurName;
        float Ratio;
        DateTime Date;
        public XmlParser(string name, float ratio, DateTime date)
        {
                    CurName = name;
                    Ratio = ratio;
                    Date = date;
        }
        public string CurNameGS
        {
            get { return CurName; }
            set { CurName = value; }
        }
        public float RatioGS
        {
            get { return Ratio; }
            set { Ratio = value; }
        }
        public DateTime DateGS
        {
            get { return Date; }
            set { Date = value; }
        }
    }
}
