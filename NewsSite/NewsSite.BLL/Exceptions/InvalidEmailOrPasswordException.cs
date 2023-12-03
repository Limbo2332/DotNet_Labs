using NewsSite.DAL.Constants;

namespace NewsSite.BLL.Exceptions
{
    public class InvalidEmailOrPasswordException : Exception
    {
        public InvalidEmailOrPasswordException() : base(ValidationMessages.INVALID_EMAIL_OR_PASSWORD_MESSAGE) { }
    }
}
