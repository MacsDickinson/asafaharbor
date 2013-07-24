using System.Net;
using HtmlAgilityPack;


namespace asafaweb.api.Logic
{
    public class HtmlGetLogic
    {
        public HtmlDocument LoadHtmlResponse(string uri)
        {
            WebClient webclient = new WebClient();
            string html = webclient.DownloadString(uri);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
    }
}
