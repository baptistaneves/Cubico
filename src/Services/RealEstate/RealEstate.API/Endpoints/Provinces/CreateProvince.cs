namespace RealEstate.API.Endpoints.Provinces;

public record CreateProvinceResponse(bool IsSuccess);

public record CreateProvinceRequest(string Name, string CreatedBy);

public class CreateProvince : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/provinces", async([FromBody] CreateProvinceRequest createProvince, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateProvinceCommand(createProvince.Name, createProvince.CreatedBy);

            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<CreateProvinceResponse>();

            return Results.Ok(response);
        })
        .WithName("CreateProvince")
        .WithDescription("Create Province")
        .Produces<CreateProvinceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}