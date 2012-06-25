using System;
using CardsaveDotNet.Common;
namespace CardsaveDotNet.Hosted
{
    public interface IHostedPaymentProcessor {
        string MerchantId { get; }
        string CreateServerResponseString(TransactionStatus status, string message = "");
        void SubmitTransaction(HostedTransactionRequest request, string merchantPassword, string preSharedKey, string postUrl);
        bool ValidateRequest(string[] urls = null);
        void ValidateResult(ServerTransactionResult result);
    }
}
