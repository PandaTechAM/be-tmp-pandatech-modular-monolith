using BaseConverter;
using FinHub.SharedKernel.Helpers;
using Microsoft.AspNetCore.Builder;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;

namespace FinHub.SharedKernel.Extensions;

public static class CryptoExtensions
{
   public static WebApplicationBuilder AddPandaCryptoAndFilters(this WebApplicationBuilder builder)
   {
      builder.ConfigureBaseConverter(builder.Configuration[ConfigurationPaths.Base36Chars]!);
      builder.ConfigureEncryptedConverter(builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration[ConfigurationPaths.AesKey]!);
      builder.Services.AddPandatechCryptoArgon2Id();


      return builder;
   }
}
