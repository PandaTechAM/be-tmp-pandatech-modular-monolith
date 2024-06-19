using BaseConverter;
using BaseConverter.Extensions;
using Microsoft.AspNetCore.Builder;
using Pandatech.Crypto;
using Pandatech.ModularMonolith.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCryptoAndFilters(this WebApplicationBuilder builder)
   {
      builder.ConfigureBaseConverter(builder.Configuration[ConfigurationPaths.Base36Chars]!);
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoArgon2Id();


      return builder;
   }
}
