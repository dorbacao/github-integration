// See https://aka.ms/new-console-template for more information


using github_integration;

try
{
    await Github.CreateRepository();
}
catch (Exception ex)
{
    var currentColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine(ex.Message);

    Console.ForegroundColor = currentColor;
}