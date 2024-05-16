using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Mappers
{
    public class RegisterToFlightOwner
    {
        FlightOwner flightowner;
        public RegisterToFlightOwner(RegisterFlightOwnerDTO register)
        {
            flightowner = new FlightOwner();
            flightowner.Name = register.Name;
            flightowner.Email = register.Email;
            flightowner.CompanyName = register.CompanyName;
            flightowner.Phone = register.Phone;
            flightowner.Address = register.Address;
            flightowner.BusinessRegistrationNumber = register.BusinessRegistrationNumber;
            flightowner.Username = register.Username;
        }
        public FlightOwner GetFlightOwner()
        {

            return flightowner;
        }
    }
}
