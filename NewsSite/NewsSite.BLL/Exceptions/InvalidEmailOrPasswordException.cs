using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO;

namespace NewsSite.BLL.Exceptions
{
    public class InvalidEmailOrPasswordException() : Exception(ValidationMessages.INVALID_EMAIL_OR_PASSWORD_MESSAGE);
}
