using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using CardsaveDotNet.Common;

namespace CardsaveDotNet.Hosted
{
    public class ServerTransactionResult
    {
        public ServerTransactionResult(NameValueCollection formVariables) {
            if (formVariables == null)
                throw new ArgumentNullException("formVariables");

            BuildResult(formVariables);
        }

        public string MerchantID { get; set; }
        public TransactionStatus Status { get; set; }
        public string Message { get; set; }
        public TransactionStatus PreviousStatus { get; set; }
        public string PreviousMessage { get; set; }
        public string CrossReference { get; set; }
        public int Amount { get; set; }
        public int CurrencyCode { get; set; }
        public string OrderID { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string OrderDescription { get; set; }
        public string CustomerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public int CountryCode { get; set; }

        public bool Successful
        {
            get { return this.Status == TransactionStatus.Successful; }
        }

        protected void BuildResult(NameValueCollection formVariables) {
            this.MerchantID = formVariables["MerchantID"];

            var status = TransactionStatus.NotSpecified;
            Enum.TryParse(formVariables["StatusCode"], out status);
            this.Status = status;

            this.Message = formVariables["Message"];

            var prevStatus = TransactionStatus.NotSpecified;
            Enum.TryParse(formVariables["PreviousStatusCode"], out prevStatus);
            this.PreviousStatus = prevStatus;

            this.PreviousMessage = formVariables["PreviousMessage"];
            this.CrossReference = formVariables["CrossReference"];
            this.Amount = int.Parse(formVariables["Amount"]);
            this.CurrencyCode = int.Parse(formVariables["CurrencyCode"]);
            this.OrderID = formVariables["OrderID"];
            this.TransactionType = formVariables["TransactionType"];
            this.TransactionDateTime = DateTime.Parse(formVariables["TransactionDateTime"]);
            this.OrderDescription = formVariables["OrderDescription"];

            this.CustomerName = formVariables["CustomerName"];
            this.Address1 = formVariables["Address1"];
            this.Address2 = formVariables["Address2"];
            this.Address3 = formVariables["Address3"];
            this.Address4 = formVariables["Address4"];
            this.City = formVariables["City"];
            this.State = formVariables["State"];
            this.PostCode = formVariables["PostCode"];
            this.CountryCode = int.Parse(formVariables["CountryCode"]);
        }
    }
}
