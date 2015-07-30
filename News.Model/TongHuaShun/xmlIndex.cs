using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace News.Model.TongHuaShun
{
    public class xmlIndex
    {
        [XmlElement]
        public List<xmlIndexItem> item { get; set; }
    }
    public class xmlIndexItem
    {
        public string name { get; set; }
        public int today { get; set; }
        public string url { get; set; }
        public string columnId { get; set; }
    }
}
