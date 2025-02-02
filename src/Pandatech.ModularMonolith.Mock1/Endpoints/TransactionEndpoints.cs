using FluentMinimalApiMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Pandatech.ModularMonolith.Mock1.Features.Create;
using Pandatech.ModularMonolith.Mock1.Features.Update;
using Pandatech.ModularMonolith.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.Mock1.Endpoints;

public class TransactionEndpoints : IEndpoint
{
   private const string BaseRoute = "/transactions";
   private const string TagName = "transactions";
   private static string RoutePrefix => ApiHelper.GetRoutePrefix(1, BaseRoute);

   public void AddRoutes(IEndpointRouteBuilder app)
   {
      var groupApp = app
                     .MapGroup(RoutePrefix)
                     .WithTags(TagName)
                     .WithGroupName(ApiHelper.GroupNameModular)
                     .WithOpenApi();

      groupApp.MapPost("",
         async ([FromServices] ISender sender) =>
         {
            await sender.Send(new CreateTransactionOrderCommand());
            return TypedResults.Ok();
         });

      groupApp.MapPut("{id}",
         async ([FromRoute] long id, [FromServices] ISender sender) =>
         {
            await sender.Send(new UpdateTransactionOrderCommand
            {
               Id = id
            });
            return TypedResults.Ok();
         });
   }
}