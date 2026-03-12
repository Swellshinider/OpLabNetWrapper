using System;

namespace OpLabNetWrapper.Debug;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var environmentVariable = Environment.GetEnvironmentVariable("OPLAB_API_KEY", EnvironmentVariableTarget.Machine) 
            ?? throw new InvalidOperationException("OPLAB_API_KEY environment variable is not set.");

        // Default Instance of OpLabClient
        using var client = new OpLabClient();

        // Set the access token for authentication, you can also use your email and password to authenticate
        // client.AuthenticateAsync("email", "password");
        client.SetAccessToken(environmentVariable);

        var response = await client.GetQuotesAsync("PETR4");
        Console.WriteLine($"Result: {response}");
    }
}