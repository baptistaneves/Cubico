namespace RealEstate.Application.Commands.Provinces.UpdateProvince;

public record UpdateProvinceResult(bool IsSuccess);

public record UpdateProvinceCommand(Guid Id, string Name, string LastModifiedBy) : ICommand<UpdateProvinceResult>;
