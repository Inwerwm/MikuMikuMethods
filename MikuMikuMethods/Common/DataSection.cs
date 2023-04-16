﻿namespace MikuMikuMethods.Common;

public record DataSection(string Name, int? Index, string Description)
{
    internal static string GetOrdinal(int number) => number switch
    {
        int num when num % 100 is 11 or 12 or 13 => $"{number}th",
        int num when num % 10 is 1 => $"{number}st",
        int num when num % 10 is 2 => $"{number}nd",
        int num when num % 10 is 3 => $"{number}rd",
        _ => $"{number}th"
    };

    public override string? ToString()
    {
        return $"## {Name}{(Index is null ? "" : ":")}{Index} - {Description}";
    }
}