# OpLabNetWrapper

This project contains a lightweight OpLab REST API wrapper for .NET.

Quick start example

```csharp
using System;
using OpLabNetWrapper;

class Program
{
    static async System.Threading.Tasks.Task Main()
    {
        using var client = new OpLabClient();
        // authenticate
        var user = await client.AuthenticateAsync("you@example.com", "your-password");
        Console.WriteLine($"Hello {user?.Name}, token len: {user?.AccessToken?.Length}");

        // fetch quotes
        var quotes = await client.GetQuotesAsync("PETR4,ABEV3");
        foreach (var q in quotes ?? Array.Empty<OpLabNetWrapper.Models.Quote>())
            Console.WriteLine($"{q.Symbol} = {q.Close}");
    }
}
```

Files added by the wrapper generator:

- OpLabNetWrapper/OpLabClient.cs
- OpLabNetWrapper/Models/UserInfo.cs
- OpLabNetWrapper/Models/Quote.cs
- OpLabNetWrapper/Models/Instrument.cs
- OpLabNetWrapper/Models/OptionInstrument.cs
- OpLabNetWrapper/Models/Portfolio.cs
- OpLabNetWrapper/Models/Order.cs

Next steps

- Run `dotnet build` on the solution to validate the project and generate XML docs.
- Extend models and add methods for any additional endpoints you need (portfolios, orders, watchlists, etc.).
# OpLabNetWrapper
.NET Wrapper for the OpLab API (https://apidocs.oplab.com.br/)
