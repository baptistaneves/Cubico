namespace Cubico.Identity.Users.Tenants.GetById;

public record GetTenantByIdResponse(UserTenantDto UserTenantDto);

public class GetTenantByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-tenant-by-id/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetTenantByIdQuery(id));

            var response = result.Adapt<GetTenantByIdResponse>();

            return Results.Ok(response);
        })
         .WithName("GetTenantById")
         .Produces<GetTenantByIdResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Get Tenant By Id");
    }
}
