using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace News.Model.TongHuaShun
{
    public class xmlColumn
    {

        public string columnName { get; set; }

        public int total { get; set; }

        public int pages { get; set; }

        public string nextPage { get; set; }

        public int currentPage { get; set; }
        [XmlElement]
        public xmlColumnPageItems pageItems { get; set; }
    }
    public class xmlColumnPageItems
    {
        [XmlElement]
        public List<xmlColumnItem> item { get; set; }
    }
    public class xmlColumnItem
    {
        public long seq { get; set; }
        public string title { get; set; }
        public string ctime { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string hot { get; set; }
        public string imgurl { get; set; }
    }
}
