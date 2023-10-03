public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Details { get; set; } // Novamente, pense bem antes de enviar detalhes de erros em ambientes de produção
}
