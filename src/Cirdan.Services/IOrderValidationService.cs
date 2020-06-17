using Cirdan.Models;

namespace Cirdan.Services
{
    public interface IOrderValidationService
    {
        Order ParseInputAndConvertToEntity(string orderDetails);
    }
}