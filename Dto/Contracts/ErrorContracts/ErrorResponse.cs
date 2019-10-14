using System.Collections.Generic;

namespace Dto.Contracts.ErrorContracts
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; }
    }
}