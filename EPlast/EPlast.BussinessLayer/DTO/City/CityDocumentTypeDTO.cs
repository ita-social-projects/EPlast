﻿using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.City
{
    public class CityDocumentTypeDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<CityDocumentsDTO> CityDocuments { get; set; }
    }
}
