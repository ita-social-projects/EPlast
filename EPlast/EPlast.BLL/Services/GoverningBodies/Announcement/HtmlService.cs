using HtmlAgilityPack;

namespace EPlast.BLL.Services.GoverningBodies.Announcement
{
    public class HtmlService : IHtmlService
    {
        public bool IsHtmlTextEmpty(string htmlText)
        {
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlText);
                return string.IsNullOrWhiteSpace(htmlDocument.DocumentNode.InnerText);
            }
            catch
            {
                return true;
            }  
        }
    }
}

