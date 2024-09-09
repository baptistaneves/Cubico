namespace RealEstate.API.Endpoints.Provinces;

public record GetAllProvincesResponse(IEnumerable<ProvinceDto> ProvincesDto);

public class GetProvinces : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/provinces", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProvincesQuery();

            var result = await sender.Send(query, cancellationToken);

            var response = result.Adapt<GetAllProvincesResponse>();

            return Results.Ok(response);
        })
          .WithName("GetProvinces")
          .WithDescription("Get all provinces")
          .Produces<UpdateProvinceResponse>();
    }
}