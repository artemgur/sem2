using System;
using Authentication.Infrastructure;

namespace sem2.Infrastructure.Services
{
    public class FakePaymentService : IPaymentService
    {
        private Random _random;

        public FakePaymentService()
        {
            _random = new Random();
        }

        public Result Pay(decimal money, CreditCardInfo info)
        {
            return _random.Next(100) >= 50 ? Result.Success() : Result.Failure("Недостаточно денег");
        }
    }
}