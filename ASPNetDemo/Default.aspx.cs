using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CardsaveDotNet.Hosted;
using System.Web.Configuration;
using CardsaveDotNet.Common;

namespace ASPNetDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            decimal amount = 0M;
            Decimal.TryParse(txtAmount.Text, out amount);
            if (amount == 0) {
                return;
            }

            // create transaction

            // get settings from web.config
            string merchantId = WebConfigurationManager.AppSettings["MerchantID"];
            string merchantPassword = WebConfigurationManager.AppSettings["MerchantPassword"];
            string preSharedKey = WebConfigurationManager.AppSettings["PreSharedKey"];
            string serverCallBack = WebConfigurationManager.AppSettings["ServerCallBackUrl"];
            string callBackUrl = WebConfigurationManager.AppSettings["CallbackUrl"];
            string postUrl = "https://mms.cardsaveonlinepayments.com/Pages/PublicPages/PaymentForm.aspx";

            var request = new HostedTransactionRequest {
                Amount = Convert.ToInt32(amount * 100),
                OrderID = "123456",
                OrderDescription = "Test Order",
                TransactionType = TransactionType.SALE,
                CallbackURL = callBackUrl,
                ServerResultURL = serverCallBack
            };
                
            var processor = new HostedPaymentProcessor(merchantId, new HttpContextWrapper(HttpContext.Current));

            processor.SubmitTransaction(request, merchantPassword, preSharedKey, postUrl);
        }
    }
}