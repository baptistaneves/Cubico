namespace RealEstate.Application.Commands.Provinces.DeleteProvince;

public record DeleteProvinceResult(bool IsSuccess);

public record DeleteProvinceCommand(Guid Id) : ICommand<DeleteProvinceResult>;
