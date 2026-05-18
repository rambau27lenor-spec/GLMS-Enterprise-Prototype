using GLMS_ST10104382.Services;

namespace GLMS_ST10104382.Tests
{
    public class BusinessRulesServiceTests
    {
        [Fact]
        public void CanCreateServiceRequest_WhenContractIsActive_ReturnsTrue()
        {
            var service = new BusinessRulesService();

            var result = service.CanCreateServiceRequest("Active");

            Assert.True(result);
        }

        [Fact]
        public void CanCreateServiceRequest_WhenContractIsExpired_ReturnsFalse()
        {
            var service = new BusinessRulesService();

            var result = service.CanCreateServiceRequest("Expired");

            Assert.False(result);
        }

        [Fact]
        public void CanCreateServiceRequest_WhenContractIsOnHold_ReturnsFalse()
        {
            var service = new BusinessRulesService();

            var result = service.CanCreateServiceRequest("On Hold");

            Assert.False(result);
        }

        [Fact]
        public void ConvertUsdToZar_WithValidRate_ReturnsCorrectAmount()
        {
            var service = new BusinessRulesService();

            var result = service.ConvertUsdToZar(100, 18.50m);

            Assert.Equal(1850, result);
        }

        [Fact]
        public void IsValidAgreementFile_WhenPdf_ReturnsTrue()
        {
            var service = new BusinessRulesService();

            var result = service.IsValidAgreementFile("contract.pdf");

            Assert.True(result);
        }

        [Fact]
        public void IsValidAgreementFile_WhenExe_ReturnsFalse()
        {
            var service = new BusinessRulesService();

            var result = service.IsValidAgreementFile("virus.exe");

            Assert.False(result);
        }
    }
}