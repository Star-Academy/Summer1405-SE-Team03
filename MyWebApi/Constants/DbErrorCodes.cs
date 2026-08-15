namespace MyWebApi.Constants;

public static class DbErrorCodes
{
    public static class Postgres
    {
        public const string UniqueViolation = "23505";
    }

    public static class SqlServer
    {
        public const int CannotInsertDuplicateKeyRow = 2601;
        public const int ViolationOfUniqueConstraint = 2627; 
    }
}