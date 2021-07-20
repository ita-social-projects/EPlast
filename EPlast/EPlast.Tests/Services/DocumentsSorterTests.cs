using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Services;
using NUnit.Framework;
using Moq;

namespace EPlast.Tests.Services
{
    public class DocumentsSorterTests
    {
        private List<SectorDocumentsDTO> _inputDocuments;
        private List<SectorDocumentsDTO> _sortedDocuments;

        [SetUp]
        public void SetUp()
        {
            _inputDocuments = new List<SectorDocumentsDTO>()
            {
                new SectorDocumentsDTO() {SubmitDate = DateTime.MaxValue},
                new SectorDocumentsDTO() {SubmitDate = null},
                new SectorDocumentsDTO() {SubmitDate = DateTime.MinValue},
                new SectorDocumentsDTO() {SubmitDate = DateTime.Now}
            };

            _sortedDocuments = new List<SectorDocumentsDTO>()
            {
                new SectorDocumentsDTO() {SubmitDate = DateTime.MinValue},
                new SectorDocumentsDTO() {SubmitDate = DateTime.Now},
                new SectorDocumentsDTO() {SubmitDate = DateTime.MaxValue},
                new SectorDocumentsDTO() {SubmitDate = null}
            };
        }

        [Test]
        public void SortDocumentsBySubmitDate_ReturnsSortedList()
        {
            //Act
            var result = DocumentsSorter<SectorDocumentsDTO>.SortDocumentsBySubmitDate(_inputDocuments);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(AreDocumentsListsEqual(_sortedDocuments.ToList(), result.ToList()));
        }

        private bool AreDocumentsListsEqual(List<SectorDocumentsDTO> list1, List<SectorDocumentsDTO> list2)
        {
            for (int i = 0; i < list1.Count; ++i)
            {
                if (!AreDatesEqualUpToSeconds(list1[i].SubmitDate, list2[i].SubmitDate))
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreDatesEqualUpToSeconds(DateTime? dt1, DateTime? dt2)
        {
            if (dt1 == null || dt2 == null)
            {
                return true;
            }

            DateTime date1 = dt1.Value;
            DateTime date2 = dt2.Value;
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day &&
                   date1.Hour == date2.Hour && date1.Minute == date2.Minute && date1.Second == date2.Second;
        }
    }
}