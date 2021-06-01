using Authentication.Infrastructure;

namespace sem2.Infrastructure.Services
{
    public interface IPaymentService
    {
        public Result Pay(decimal money, CreditCardInfo info);
    }
}