namespace GLMS_ST10104382.Services
{
    public class BusinessRulesService
    {
        public bool CanCreateServiceRequest(string contractStatus)
        {
            return contractStatus != "Expired" && contractStatus != "On Hold";
        }

        public decimal ConvertUsdToZar(decimal usdAmount, decimal exchangeRate)
        {
            return usdAmount * exchangeRate;
        }

        public bool IsValidAgreementFile(string fileName)
        {
            return Path.GetExtension(fileName).ToLower() == ".pdf";
        }
    }
}