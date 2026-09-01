namespace Gamana_Muttopalvelu_Backend.DTO.Admin
{
    public class AdminResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public AdminResponse() { }

        public AdminResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
