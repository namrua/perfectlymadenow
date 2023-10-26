using AutomationSystem.Main.Core.Registrations.AppLogic.Factories;
using AutomationSystem.Main.Model;
using Xunit;

namespace AutomationSystem.Main.Tests.Registrations.AppLogic.Factories
{
    public class RegistrationPaymentFactoryTests
    {

        #region CreateRegistrationPaymentForEditByClassCurrency() tests
        [Fact]
        public void CreateRegistrationPaymentForEditByClassCurrency_Currency_ReturnsRegistrationPaymentForEdit()
        {
            // arragne
            var currency = new Currency
            {
                Name = "Name"
            };
            var factory = CreateFactory();

            // act
            var forEdit = factory.CreateRegistrationPaymentForEditByClassCurrency(currency);

            // assert
            Assert.Equal("Name", forEdit.CurrencyCode);
        }
        #endregion

        #region private methods
        private RegistrationPaymentFactory CreateFactory()
        {
            return new RegistrationPaymentFactory();
        }
        #endregion
    }
}
