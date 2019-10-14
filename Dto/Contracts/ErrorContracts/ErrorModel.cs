using System.Collections.Generic;

namespace Dto.Contracts.ErrorContracts
{
    public class ErrorModel
    {
        public string FieldName { get; set; }
        public List<string> Messages { get; set; }
    }
}