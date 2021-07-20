using System.Collections.Generic;
using System.Linq;
using EPlast.BLL.DTO;

namespace EPlast.BLL.Services
{
    public static class DocumentsSorter<T> where T : IDocumentDTO
    {
        public static IEnumerable<T> SortDocumentsBySubmitDate(IEnumerable<T> documents)
        {
            if (documents == null || documents.Count() <= 1)
            {
                return documents;
            }

            var sortedDocuments = documents.OrderBy(doc => doc.SubmitDate).ToList();

            int lastDocWithoutDate = 0;
            while (lastDocWithoutDate < sortedDocuments.Count && sortedDocuments[lastDocWithoutDate].SubmitDate == null)
            {
                ++lastDocWithoutDate;
            }

            if (lastDocWithoutDate != sortedDocuments.Count)
            {
                var docsWithNullDate = sortedDocuments.GetRange(0, lastDocWithoutDate);
                sortedDocuments.RemoveRange(0, lastDocWithoutDate);
                sortedDocuments.AddRange(docsWithNullDate);
            }

            return sortedDocuments;
        }
    }
}