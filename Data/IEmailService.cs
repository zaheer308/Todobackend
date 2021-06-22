using MimeKit;

namespace TodoBackend.Data
{
    public interface IEmailService
    {


        MimeMessage GetMessageObjForAuthLoginEmailEx(string emailID, string emailWording);
        MimeMessage GetMessageObjForEmailEx(string emailID);

       
    }
}