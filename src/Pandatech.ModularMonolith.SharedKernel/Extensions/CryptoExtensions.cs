using Microsoft.AspNetCore.Builder;
using Pandatech.Crypto;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCryptoAndFilters(this WebApplicationBuilder builder)
   {
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration.GetAesKey());
      builder.Services.AddPandatechCryptoArgon2Id();


      return builder;
   }
}