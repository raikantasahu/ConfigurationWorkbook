using Aras.IOM;

namespace ConfigWorkbook;

internal static class Program
{
    private const int ExitSuccess = 0;
    private const int ExitFailure = 1;
    private const int ExitUsage = 2;

    private static int Main(string[] args)
    {
        Console.OutputEncoding = new System.Text.UTF8Encoding(false);

        Options options;
        try
        {
            options = Options.Parse(args);
            if (options.ShowHelp)
            {
                Console.WriteLine(Options.Usage);
                return ExitSuccess;
            }

            options.EnsurePassword();
        }
        catch (OptionsException ex)
        {
            Console.Error.WriteLine($"error: {ex.Message}");
            Console.Error.WriteLine();
            Console.Error.WriteLine(Options.Usage);
            return ExitUsage;
        }

        HttpServerConnection connection;
        Innovator innovator;
        try
        {
            connection = IomFactory.CreateHttpServerConnection(
                options.Url, options.Database, options.User, options.Password);

            var login = connection.Login();
            if (login.isError())
            {
                Console.Error.WriteLine($"error: login failed: {login.getErrorString()}");
                return ExitUsage;
            }

            innovator = IomFactory.CreateInnovator(connection);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: could not connect to {options.Url}: {ex.Message}");
            return ExitUsage;
        }

        Console.WriteLine($"Connected to {options.Url} [{options.Database}] as {options.User}.");

        try
        {
            var itemTypeId = FindItemType(innovator, options.ItemType);
            if (itemTypeId is null)
            {
                Console.Error.WriteLine($"error: ItemType '{options.ItemType}' not found.");
                return ExitUsage;
            }

            var output = Path.GetFullPath(options.Output);
            Console.WriteLine($"Writing the sheets for ItemType '{options.ItemType}' ({itemTypeId}):");
            new WorkbookBuilder(new SqlRunner(innovator), itemTypeId).Build(output);
            Console.WriteLine($"Wrote {output}");
            return ExitSuccess;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"error: {ex.Message}");
            return ExitFailure;
        }
        finally
        {
            TryLogout(connection);
        }
    }

    /// <summary>Returns the id of the ItemType with this name, or null when there is none.</summary>
    private static string? FindItemType(Innovator innovator, string name)
    {
        var query = innovator.newItem("ItemType", "get");
        query.setAttribute("select", "id");
        query.setProperty("name", name);
        var result = query.apply();
        if (result.isEmpty())
        {
            return null;
        }

        if (result.isError())
        {
            throw new InvalidOperationException($"could not read ItemType '{name}': {result.getErrorString()}");
        }

        return result.getID();
    }

    private static void TryLogout(HttpServerConnection connection)
    {
        try
        {
            connection.Logout();
        }
        catch (Exception ex)
        {
            // The workbook is already written at this point, so a failed logout is not worth failing over.
            Console.Error.WriteLine($"warning: logout failed: {ex.Message}");
        }
    }
}
