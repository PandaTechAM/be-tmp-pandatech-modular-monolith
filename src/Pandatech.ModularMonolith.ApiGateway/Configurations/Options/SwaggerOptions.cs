﻿namespace Pandatech.ModularMonolith.ApiGateway.Configurations.Options;

public class SwaggerOptions
{
   public Dictionary<string, SwaggerVersionOptions> Versions { get; set; } = null!;
}