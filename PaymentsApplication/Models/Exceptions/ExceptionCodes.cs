namespace PaymentsApplication.Models.Exceptions
{
    public static class ExceptionCodes
    {
        public const string NotFound = "data.not.found";
        public const string InternalServerError = "internal.server.error";
        public const string ValidationError = "validation.error";
        public const string UnexpectedError = "unexpected.error";
        public const string FileError = "file.error";
        public const string DublicateData = "dublicate.data";
        public const string InvalidCredentials = "invalid.credentials";
        public const string UnauthorizedError = "unauthorized.error";
        public const string DuplicateCustomer = "duplicate.customer";
        public const string TimeError = "time.error";
        public const string Blocked = "warning.blocked";
    }
}
