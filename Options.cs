namespace ConfigWorkbook;

/// <summary>Raised when the command line cannot be turned into a usable <see cref="Options"/>.</summary>
internal sealed class OptionsException : Exception
{
    public OptionsException(string message) : base(message) { }
}

internal sealed class Options
{
    public string Url { get; private set; } = "";
    public string Database { get; private set; } = "";
    public string User { get; private set; } = "";
    public string Password { get; private set; } = "";
    public string ItemType { get; private set; } = "";
    public string Output { get; private set; } = "";
    public bool ShowHelp { get; private set; }

    public const string Usage = """
        ConfigWorkbook - write the configuration workbook of an Aras Innovator ItemType.

        Usage:
          ConfigWorkbook --url <url> --database <db> --user <name> [--password <pw>]
                         --itemtype <name> [--output <file.xlsx>]

        The queries run on the server through ApplySQL, so the user must be allowed to run SQL
        (by default, a member of Administrators).

        Required:
          -u, --url <url>          Innovator server URL, e.g. http://host/InnovatorServer
          -d, --database <name>    Database to connect to.
          -n, --user <name>        Innovator login name.
          -t, --itemtype <name>    Name of the ItemType, e.g. Part.

        Optional:
          -p, --password <pw>      Password (plaintext or MD5). Prompted for if omitted.
          -o, --output <file>      Workbook to write. Default: "<itemtype> Configuration Workbook.xlsx".
          -h, --help               Show this help.

        Exit codes:
          0  the workbook was written
          1  a query failed, or the workbook could not be written
          2  bad usage, could not connect / log in, or the ItemType does not exist
        """;

    public static Options Parse(string[] args)
    {
        var options = new Options();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg is "-h" or "--help" or "-?" or "/?")
            {
                options.ShowHelp = true;
                return options;
            }

            switch (arg)
            {
                case "-u" or "--url":
                    options.Url = NextValue(args, ref i, arg);
                    continue;
                case "-d" or "--database":
                    options.Database = NextValue(args, ref i, arg);
                    continue;
                case "-n" or "--user":
                    options.User = NextValue(args, ref i, arg);
                    continue;
                case "-p" or "--password":
                    options.Password = NextValue(args, ref i, arg);
                    continue;
                case "-t" or "--itemtype":
                    options.ItemType = NextValue(args, ref i, arg);
                    continue;
                case "-o" or "--output":
                    options.Output = NextValue(args, ref i, arg);
                    continue;
            }

            throw new OptionsException(arg.StartsWith('-')
                ? $"Unknown option '{arg}'."
                : $"Unexpected argument '{arg}'.");
        }

        options.Validate();
        return options;
    }

    private static string NextValue(string[] args, ref int index, string option)
    {
        if (index + 1 >= args.Length)
        {
            throw new OptionsException($"Option '{option}' requires a value.");
        }

        return args[++index];
    }

    private void Validate()
    {
        var missing = new List<string>();
        if (Url.Length == 0) missing.Add("--url");
        if (Database.Length == 0) missing.Add("--database");
        if (User.Length == 0) missing.Add("--user");
        if (ItemType.Length == 0) missing.Add("--itemtype");

        if (missing.Count > 0)
        {
            throw new OptionsException($"Missing required option(s): {string.Join(", ", missing)}.");
        }

        if (Output.Length == 0)
        {
            Output = $"{ItemType} Configuration Workbook.xlsx";
        }

        if (!Output.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            throw new OptionsException($"The output file must end in .xlsx: {Output}");
        }
    }

    /// <summary>Prompts for the password when it was not supplied on the command line.</summary>
    public void EnsurePassword()
    {
        if (Password.Length > 0)
        {
            return;
        }

        Console.Error.Write($"Password for {User}: ");
        Password = Console.IsInputRedirected ? Console.ReadLine() ?? "" : ReadMasked();
        Console.Error.WriteLine();

        if (Password.Length == 0)
        {
            throw new OptionsException("No password supplied.");
        }
    }

    private static string ReadMasked()
    {
        var password = new System.Text.StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return password.ToString();
                case ConsoleKey.Backspace when password.Length > 0:
                    password.Length--;
                    Console.Error.Write("\b \b");
                    break;
                case ConsoleKey.Backspace:
                    break;
                default:
                    if (!char.IsControl(key.KeyChar))
                    {
                        password.Append(key.KeyChar);
                        Console.Error.Write('*');
                    }
                    break;
            }
        }
    }
}
