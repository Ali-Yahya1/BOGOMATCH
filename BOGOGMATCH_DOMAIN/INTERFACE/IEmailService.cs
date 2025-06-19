using BOGOGMATCH_DOMAIN.MODELS.UserManagement;

namespace BOGOGMATCH_DOMAIN.INTERFACE
{
    public interface IEmailService
    {
        Task<EmailResult> SendEmailAsync(Email emailModel);

    }
}
