namespace RealEstate.Application.Commands.Provinces.CreateProvince;

public record CreateProvinceResult(bool IsSucesss);

public record CreateProvinceCommand(string Name, string CreatedBy) : ICommand<CreateProvinceResult>;
