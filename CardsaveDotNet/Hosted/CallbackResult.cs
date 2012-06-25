using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CardsaveDotNet.Hosted
{
    /// <summary>
    /// Provides a wrapper around the callback result sent when using SERVER postback
    /// </summary>
    public class CallbackResult
    {
        public CallbackResult(NameValueCollection formVariables) {
            if (formVariables == null)
                throw new ArgumentNullException("formVariables");

            BuildResult(formVariables);
        }
       
        public string HashDigest { get; set; }
        public string MerchantID { get; set; }
        public string CrossReference { get; set; }
        public string OrderID { get; set; }

        protected void BuildResult(NameValueCollection formVariables) {
            this.HashDigest = formVariables["HashDigest"];
            this.MerchantID = formVariables["MerchantID"];
            this.CrossReference = formVariables["CrossReference"];
            this.OrderID = formVariables["OrderID"];
        }
    }
}
