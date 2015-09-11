using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using News.DataAccess.Business;
using PanGu;
using PanGu.HighLight;

namespace News.Service.PanGuTool
{
    public class Index
    {
        public static String INDEX_DIR
        {
            get
            {
                return PanGu.Framework.Path.GetAssemblyPath() + @"NewsIndex";
            }
        }

        private static IndexWriter writer = null;

        public static int MaxMergeFactor
        {
            get
            {
                if (writer != null)
                {
                    return writer.GetMergeFactor();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (writer != null)
                {
                    writer.SetMergeFactor(value);
                }
            }
        }

        public static int MaxMergeDocs
        {
            get
            {
                if (writer != null)
                {
                    return writer.GetMaxMergeDocs();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (writer != null)
                {
                    writer.SetMaxMergeDocs(value);
                }
            }
        }

        public static int MinMergeDocs
        {
            get
            {
                if (writer != null)
                {
                    return writer.GetMaxBufferedDocs();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (writer != null)
                {
                    writer.SetMaxBufferedDocs(value);
                }
            }
        }

        public static void CreateIndex(String indexDir)
        {

            try
            {
                writer = new IndexWriter(indexDir, new PanGuAnalyzer(), false);
            }
            catch
            {
                writer = new IndexWriter(indexDir, new PanGuAnalyzer(), true);
            }

            //writer.Optimize();
            //writer.Close();
        }

        public static void Rebuild(String indexDir)
        {
            writer = new IndexWriter(indexDir, new PanGuAnalyzer(), true);
            writer.Optimize();
            writer.Close();
        }

        public static int IndexString(String indexDir, string url, string title, DateTime time, string content, string id)
        {
            //IndexWriter writer = new IndexWriter(indexDir, new Lucene.Net.Analysis.KTDictSeg.KTDictSegAnalyzer(), false);

            Document doc = new Document();
            Field field = new Field("url", url, Field.Store.YES, Field.Index.NO);
            doc.Add(field);
            field = new Field("title", title, Field.Store.YES, Field.Index.ANALYZED);
            doc.Add(field);
            field = new Field("time", time.ToString("yyyyMMdd"), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS);
            doc.Add(field);
            field = new Field("contents", content, Field.Store.YES, Field.Index.ANALYZED);
            doc.Add(field);
            field = new Field("id", id, Field.Store.YES, Field.Index.NO);
            doc.Add(field);

            writer.AddDocument(doc);
            int num = writer.MaxDoc();
            //writer.Optimize();
            //writer.Close();
            return num;
        }

        public static void CloseWithoutOptimize()
        {
            writer.Close();
        }

        public static void Close()
        {
            writer.Optimize();
            writer.Close();
        }

        static public List<string> SplitKeyWords(string keywords, Analyzer analyzer)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(PanGu.Framework.Stream.WriteStringToStream(keywords,
                Encoding.UTF8), Encoding.UTF8);

            TokenStream tokenStream = analyzer.TokenStream("", reader);

            Lucene.Net.Analysis.Token token = tokenStream.Next();

            List<string> result = new List<string>();

            while (token != null)
            {
                result.Add(keywords.Substring(token.StartOffset(), token.EndOffset() - token.StartOffset()));
                token = tokenStream.Next();
            }

            return result;

        }

        static public string GetKeyWordsSplitBySpace(string keywords, PanGuTokenizer ktTokenizer)
        {
            StringBuilder result = new StringBuilder();

            ICollection<WordInfo> words = ktTokenizer.SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }

                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }

            return result.ToString().Trim();
        }


        public static List<SeekWebNewsItem> Search(String indexDir, String q, int pageLen, int pageNo, out int recCount)
        {
            string keywords = q;

            IndexSearcher search = new IndexSearcher(indexDir);
            q = GetKeyWordsSplitBySpace(q, new PanGuTokenizer());
            QueryParser queryParser = new QueryParser("contents", new PanGuAnalyzer(true));
            Query query = queryParser.Parse(q);
            QueryParser titleQueryParser = new QueryParser("title", new PanGuAnalyzer(true));
            Query titleQuery = titleQueryParser.Parse(q);
            BooleanQuery bq = new BooleanQuery();
            bq.Add(query, BooleanClause.Occur.SHOULD);
            bq.Add(titleQuery, BooleanClause.Occur.SHOULD);

            Hits hits = search.Search(bq);

            List<SeekWebNewsItem> result = new List<SeekWebNewsItem>();

            recCount = hits.Length();
            int i = (pageNo - 1) * pageLen;

            while (i < recCount && result.Count < pageLen)
            {
                SeekWebNewsItem news = null;
                try
                {
                    news = new SeekWebNewsItem();
                    news.NewsId = long.Parse(hits.Doc(i).Get("id"));
                    string Title = hits.Doc(i).Get("title");
                    string NewsText = hits.Doc(i).Get("contents");
                    String strTime = hits.Doc(i).Get("time");
                    PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                        new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"red\">", "</font>");

                    PanGu.HighLight.Highlighter highlighter =
                        new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                        new Segment());
                    highlighter.FragmentSize = 50;
                    news.Abstract = highlighter.GetBestFragment(keywords, NewsText);
                    news.TitleHighLighter = highlighter.GetBestFragment(keywords, Title);
                    if (string.IsNullOrEmpty(news.TitleHighLighter))
                    {
                        news.TitleHighLighter = Title;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    result.Add(news);
                    i++;
                }
            }

            search.Close();
            return result;
        }
    }
}
