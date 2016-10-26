namespace Sudoku.Types.Configuration
{
    public class Configuration
    {
        public Logging Logging { get; set; }
    }

    public class Logging
    {
        public string LogglyApiKey { get; set; }
        public string LogglyBaseUrl { get; set; }
    }
}
