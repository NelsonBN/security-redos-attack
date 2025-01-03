using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();

app.MapPost("/register", (RegisterRequest request) =>
{
    if(string.IsNullOrWhiteSpace(request.Email))
    {
        return Results.BadRequest("Email is required");
    }

    if(!request.Email.IsValidEmail())
    {
        return Results.BadRequest("Email is invalid");
    }

    if(string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest("Password is required");
    }

    return Results.Created();
});

await app.RunAsync();


public record RegisterRequest(string Email, string Password);

public static class Validator
{
    public static bool IsValidEmail(this string email)
        => Regex.IsMatch(
            email,
            //@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", // Good Regex - Linear Time Complexity
            @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", // Bad Regex - Exponential Time Complexity
            RegexOptions.None,
            TimeSpan.FromMilliseconds(500));
}
