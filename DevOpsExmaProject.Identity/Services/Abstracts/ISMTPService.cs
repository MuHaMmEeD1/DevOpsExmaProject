namespace DevOpsExmaProject.Identity.Services.Abstracts
{
    public interface ISMTPService
    {

        void SendSMTP(string recipientEmail, int randomCheckCode);

    }
}
