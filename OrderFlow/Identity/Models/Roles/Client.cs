﻿namespace OrderFlow.Identity.Models.Roles;

public sealed class Client : Role
{
    public Client()
    {
        Name = "Client";
        NormalizedName = Name.ToUpper();
        ParentRole = new Employee().ToString();
    }
}