namespace BOGOGMATCH_DOMAIN.MODELS.UserManagement
{
    public class Email
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Email(string to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }

    public class EmailResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public EmailResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
