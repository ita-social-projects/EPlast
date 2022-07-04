using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Services;
using Moq;
using NUnit.Framework;

namespace EPlast.Tests.Services
{
    public class DocumentsSorterTests
    {
        private List<SectorDocumentsDto> _inputDocuments;
        private List<SectorDocumentsDto> _sortedDocuments;

        [SetUp]
        public void SetUp()
        {
            _inputDocuments = new List<SectorDocumentsDto>()
            {
                new SectorDocumentsDto() {SubmitDate = DateTime.MaxValue},
                new SectorDocumentsDto() {SubmitDate = null},
                new SectorDocumentsDto() {SubmitDate = DateTime.MinValue},
                new SectorDocumentsDto() {SubmitDate = DateTime.Now}
            };

            _sortedDocuments = new List<SectorDocumentsDto>()
            {
                new SectorDocumentsDto() {SubmitDate = DateTime.MinValue},
                new SectorDocumentsDto() {SubmitDate = DateTime.Now},
                new SectorDocumentsDto() {SubmitDate = DateTime.MaxValue},
                new SectorDocumentsDto() {SubmitDate = null}
            };
        }

        [Test]
        public void SortDocumentsBySubmitDate_ReturnsSortedList()
        {
            //Act
            var result = DocumentsSorter<SectorDocumentsDto>.SortDocumentsBySubmitDate(_inputDocuments);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(AreDocumentsListsEqual(_sortedDocuments.ToList(), result.ToList()));
        }

        private bool AreDocumentsListsEqual(List<SectorDocumentsDto> list1, List<SectorDocumentsDto> list2)
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