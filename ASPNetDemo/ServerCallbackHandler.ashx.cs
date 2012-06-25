using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using CardsaveDotNet.Hosted;
using CardsaveDotNet.Common;

namespace ASPNetDemo
{
    /// <summary>
    /// Summary description for ServerCallBackHandler
    /// </summary>
    public class ServerCallbackHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string merchantId = WebConfigurationManager.AppSettings["MerchantId"];

            var processor = new HostedPaymentProcessor(merchantId, new HttpContextWrapper(context));

            try
            {
                if (!processor.ValidateRequest()) {
                    throw new InvalidOperationException("Request came from an invalid source");
                }

                var result = new ServerTransactionResult(context.Request.Form);

                processor.ValidateResult(result); // will throw if merchant ids do not match

                // at this point we can get order and work with the result 
                if (result.Successful)
                {
                    // update our order to say payment sucessful
                }

                // now we need to let Cardsave know we've received the result
                context.Response.Write(
                    processor.CreateServerResponseString(TransactionStatus.Successful));

            }
            catch (Exception ex)
            {
                // let cardsave know there was a problem
                context.Response.Write(
                    processor.CreateServerResponseString(TransactionStatus.Exception, ex.Message));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}