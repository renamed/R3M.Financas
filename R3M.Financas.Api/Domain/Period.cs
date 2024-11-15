﻿namespace R3M.Financas.Api.Domain;

public class Period : Registry
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public string Description { get; set; } = string.Empty;
}
