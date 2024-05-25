namespace la_mia_pizzeria_static.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public string Message { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public ErrorViewModel() { }

        public ErrorViewModel(string msg)
        {
            Message = msg;
        }
    }
}