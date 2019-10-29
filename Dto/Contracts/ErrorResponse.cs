namespace Dto.Contracts
{
    public class ErrorResponse : IResponse

    {
    public string Message { get; set; }
    public string Type { get; set; }
    }
}