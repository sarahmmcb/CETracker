using DbUp;
using System.Reflection;

var connectionString =
    args.FirstOrDefault()
    ?? "Server=localhost, 1433;User Id=sa;Password=7Qt*_~kMMHB6;Database=CASCETracker;Encrypt=False;";
    //?? "Data Source=localhost\\SQLEXPRESS;Initial Catalog=CASCETracker;Integrated Security=True;Connect Timeout=30;Encrypt=False";

EnsureDatabase.For.SqlDatabase(connectionString);

var upgrader =
    DeployChanges.To
        .SqlDatabase(connectionString, "ce")
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;