﻿namespace R3M.Financas.Api.Domain;

public class Institution : Registry
{
    public string Name { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; }
    public decimal Balance { get; set; }
}
