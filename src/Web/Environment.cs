namespace Web
{
    public static class Environment
    {
        public static string Api => !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("Api"))
            ? System.Environment.GetEnvironmentVariable("Api")
            : "http://localhost:5002";
    }
}
