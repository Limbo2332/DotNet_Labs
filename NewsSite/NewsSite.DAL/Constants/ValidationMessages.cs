using NewsSite.DAL.Context.Constants;

namespace NewsSite.DAL.Constants
{
    public static class ValidationMessages
    {
        public const string EMAIL_PROPERTY_NAME = "Email";
        public const string PASSWORD_PROPERTY_NAME = "Password";
        public const string NAME_PROPERTY_NAME = "Name";
        public const string FULL_NAME_PROPERTY_NAME = "FullName";
        public const string SUBJECT_PROPERTY_NAME = "Subject";
        public const string CONTENT_PROPERTY_NAME = "Content";
        public const string PUBLIC_INFORMATION_PROPERTY_NAME = "PublicInformation";

        public static string GetEntityIsEmptyMessage(string propertyName) => $"{propertyName} is required";

        public static string GetEntityWithWrongFormatMessage(string propertyName) => $"{propertyName} is in incorrect format";

        public static string GetEntityWithWrongMinimumLengthMessage(string propertyName, int minLength) =>
            $"{propertyName} must have at least {minLength} symbol";

        public static string GetEntityWithWrongMaximumLengthMessage(string propertyName, int maxLength) =>
            $"{propertyName} must not exceed {maxLength} symbol";

        public static string GetEntityIsNotUniqueMessage(string propertyName) =>
            $"This {propertyName} is already registered";

        public const string PASSWORD_WITH_WRONG_FORMAT_MESSAGE = "Password must have at least 1 number, 1 lowercase, 1 uppercase and 1 special character";
        public const string INVALID_EMAIL_OR_PASSWORD_MESSAGE = "Invalid FullName or password";

        public static readonly string BirthDateLessThanYears = $"You must be at least {ConfigurationConstants.MIN_YEARS_TO_REGISTER}";
    }
}
