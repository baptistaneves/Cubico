namespace RealEstate.API.Endpoints.Provinces;

public record GetProvinceByIdResponse(ProvinceDto ProvinceDto);

public class GetProvinceById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/provinces/{id}", async(Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProvinceByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            var response = result.Adapt<GetProvinceByIdResponse>();

            return Results.Ok(response);
        })
          .WithName("GetProvinceById")
          .WithDescription("Get province by id")
          .Produces<UpdateProvinceResponse>();
    }
}