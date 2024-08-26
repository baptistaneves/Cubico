namespace Cubico.Identity.Users.Landlords.Create;

public record CreateLandlordResponse(Guid Id, string Token, string Email, string Name);

public record CreateLandlordRequest(string Email, string Name, string PhoneNumber, string Address, string Password);

public class CreateLandlordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/create-landlord", async ([FromBody] CreateLandlordRequest createLandlordRequest, ISender sender) =>
        {
            var command = createLandlordRequest.Adapt<CreateLandlordCommand>();

            var result = await sender.Send(command);

            var response =  result.Adapt<CreateLandlordResponse>();

            return Results.Ok(response);
        })
         .WithName("CreateLandlord")
         .Produces<CreateLandlordResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Create Lanllord");
    }
}
