namespace Library.Management.System.API.DTOs
{
    public class AuthResponseDTO
    {
        public bool isSuccess { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; } = string.Empty;
    }
}
