namespace RealEstate.Application.Exceptions;

public class RealEstateNotFoundException : NotFoundException
{
    public RealEstateNotFoundException(string message) : base(message) {  }
}
