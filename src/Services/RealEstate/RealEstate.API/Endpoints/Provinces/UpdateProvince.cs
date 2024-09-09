namespace RealEstate.API.Endpoints.Provinces;

public record UpdateProvinceRequest(Guid Id, string Name, string LastModifiedBy);

public record UpdateProvinceResponse(bool IsSuccess);

public class UpdateProvince : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/pronvices", async ([FromBody] UpdateProvinceRequest updateProvince, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateProvinceCommand(updateProvince.Id, updateProvince.Name, updateProvince.LastModifiedBy);

            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<UpdateProvinceResponse>();

            return Results.Ok(response);
        })
          .WithName("UpdateProvince")
          .WithDescription("Update Description")
          .Produces<UpdateProvinceResponse>()
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status404NotFound);
    }
}