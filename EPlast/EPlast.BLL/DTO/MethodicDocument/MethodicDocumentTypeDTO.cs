using System.ComponentModel;

namespace EPlast.BLL.DTO
{
    public enum MethodicDocumentTypeDto
    {
        [Description("Нормативний акт")]
        legislation,

        [Description("Методичний документ")]
        Methodics,

        [Description("Різне")]
        Other
    }
}
