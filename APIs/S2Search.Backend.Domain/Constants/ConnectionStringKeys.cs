namespace S2Search.Backend.Domain.Constants;

public static class ConnectionStringKeys
{
    public const string SqlDatabase = "SqlDatabase";
    public const string AzureStorage = "AzureStorage";
    public const string Redis = "Redis";
}

public static class ConnectionStringFunctionKeys
{
    public const string SqlDatabase = "ConnectionStrings:SqlDatabase";
    public const string AzureStorage = "ConnectionStrings:AzureStorage";
}

