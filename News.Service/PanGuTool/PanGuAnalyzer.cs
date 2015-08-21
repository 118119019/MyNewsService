using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;

namespace News.Service.PanGuTool
{
    public class PanGuAnalyzer : Analyzer
    {
        private bool _OriginalResult = false;

        public PanGuAnalyzer()
        {
        }

        /// <summary>
        /// Return original string.
        /// Does not use only segment
        /// </summary>
        /// <param name="originalResult"></param>
        public PanGuAnalyzer(bool originalResult)
        {
            _OriginalResult = originalResult;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            TokenStream result = new PanGuTokenizer(reader, _OriginalResult);
            result = new LowerCaseFilter(result);
            return result;
        }
    }
}
