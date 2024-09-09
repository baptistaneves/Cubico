namespace RealEstate.Application.Commands.Municipalities.DeleteMunicipality;
public record DeleteMunicipalityResult(bool IsSuccess);

public record DeleteMunicipalityCommand(Guid Id) : ICommand<DeleteMunicipalityResult>;
