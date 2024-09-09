namespace RealEstate.Application.Commands.Municipalities.CreateMunicipality;

public record CreateMunicipalityResult(bool ISSuccess);

public record CreateMuniciplaityCommand(string Name, Guid ProvinceId, string CreatedBy) : ICommand<CreateMunicipalityResult>;
