namespace RealEstate.Application.Commands.Municipalities.UpdateMunicipality;
public record UpdateMunicipalityResult(bool IsSuccess);

public record UpdateMunicipalityCommand(Guid Id, string Name, Guid ProvinceId, string LastModifiedBy) 
    : ICommand<UpdateMunicipalityResult>;
