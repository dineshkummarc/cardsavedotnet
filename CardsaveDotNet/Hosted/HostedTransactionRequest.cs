using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;
using CardsaveDotNet.Common;
using CardsaveDotNet.Utils;

namespace CardsaveDotNet.Hosted
{
    /// <summary>
    /// Represents a transaction request for Cardsave's hosted payment page
    /// <see cref="https://mms.cardsaveonlinepayments.com/Pages/PublicPages/PaymentForm.aspx"/>
    /// </summary>
    public class HostedTransactionRequest
    {
        public HostedTransactionRequest()
        {
            // defaults
            this.TransactionType = TransactionType.SALE;
            this.ResultDeliveryMethod = ResultDeliveryMethod.SERVER;
            this.CV2Mandatory = true;
            this.TransactionDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzzz");
            this.CountryCode = "826";//Customer’s billing country code. ISO 3166-1 e.g. United Kingdom: 826
            this.CurrencyCode = "826";//The currency of the transaction. ISO 4217 e.g. GBP: 826
            
            this.ServerResultURLCookieVariables = new NameValueCollection();
            this.ServerResultURLFormVariables = new NameValueCollection();
            this.ServerResultURLQueryStringVariables = new NameValueCollection();
        }
        
        /// <summary>
        /// The transaction amount in minor currency – e.g. for £10.00, it must be submitted as 1000
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// The currency of the transaction. ISO 4217 e.g. GBP: 826
        /// </summary>
        public string CurrencyCode { get; set; }
        
        /// <summary>
        /// A merchant side ID for the order – primarily used to for determining duplicate transactions. 
        /// </summary>
        public string OrderID { get; set; }
        
        /// <summary>
        /// Must be either SALE or PREAUTH
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The date & time (as seen by the merchant's server) of the transaction.
        /// </summary>
        public string TransactionDateTime { get; set; }

        /// <summary>
        /// The URL of the page on the merchant's site that the results of the transaction will be posted back to (see section below)
        /// </summary>
        public string CallbackURL { get; set; }

        /// <summary>
        /// A description for the order. Note: make sure that special characters in the OrderDescription are properly escaped, otherwise the hash digest will not match
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// The name of the customer
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Customer’s billing address line 1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Customer’s billing address line 2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Customer’s billing address line 3
        /// </summary>
        public string Address3 { get; set; }

        /// <summary>
        /// Customer’s billing address line 4
        /// </summary>
        public string Address4 { get; set; }

        /// <summary>
        /// Customer’s billing address city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Customer’s billing address state
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// ustomer’s billing address post code
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Customer’s billing country code. ISO 3166-1 e.g. United Kingdom: 826
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Control variable that determines whether the CV2 field on the payment form will be mandatory
        /// </summary>
        public bool CV2Mandatory { get; set; }

        /// <summary>
        /// Control variable that determines whether the Address1 field on the payment form will be mandatory
        /// </summary>
        public bool Address1Mandatory { get; set; }

        /// <summary>
        /// Control variable that determines whether the City field on the payment form will be mandatory
        /// </summary>
        public bool CityMandatory { get; set; }

        /// <summary>
        /// Control variable that determines whether the PostCode field on the payment form will be mandatory
        /// </summary>
        public bool PostCodeMandatory { get; set; }

        /// <summary>
        /// Control variable that determines whether the State field on the payment form will be mandatory
        /// </summary>
        public bool StateMandatory { get; set; }

        /// <summary>
        /// Control variable that determines whether the Country field on the payment form will be mandatory
        /// </summary>
        public bool CountryMandatory { get; set; }

        /// <summary>
        /// The delivery method of the payment result, either POST, SERVER or SERVER_PULL. 
        /// POST will deliver the full results via the customer's browser as a form post back to the CallbackURL. 
        /// With SERVER, the results are PUSHED TO the ServerResultURL on the merchant's webshop BEFORE the customer is redirected back to the webshop.
        /// </summary>
        public ResultDeliveryMethod ResultDeliveryMethod { get; set; }

        /// <summary>
        /// The merchant's external server URL used for SERVER result delivery method
        /// </summary>
        public string ServerResultURL { get; set; }

        /// <summary>
        /// Boolean that determines whether the payment result will be displayed on the PaymentForm page, 
        /// or redirected to the merchant's site after a response from the merchant's 
        /// external server (ServerResultURL)
        /// </summary>
        public bool PaymentFormDisplaysResult { get; set; }

        /// <summary>
        /// Cookie variables to deliver to the merchant's external server (ServerResultURL), when SERVER result delivery method is used.
        /// </summary>
        public NameValueCollection ServerResultURLCookieVariables { get; set; }

        /// <summary>
        /// Form variables to post across to the merchant's external server (ServerResultURL), when SERVER result delivery method is used.
        /// </summary>
        public NameValueCollection ServerResultURLFormVariables { get; set; }

        /// <summary>
        /// QueryString variables to post across to the merchant's external server (ServerResultURL), when SERVER result delivery method is used.
        /// </summary>
        public NameValueCollection ServerResultURLQueryStringVariables { get; set; }

        /// <summary>
        /// Converts the transaction request into a NameValueCollection ready for posting to the hosted payment page
        /// </summary>
        /// <returns>NameValueCollection</returns>
        public NameValueCollection ToNameValueCollection()
        {
            var collection = new NameValueCollection();
            collection.AddProperty(this, r => r.Amount);
            collection.AddProperty(this, r => r.CurrencyCode);
            collection.AddProperty(this, r => r.OrderID);
            collection.AddProperty(this, r => r.TransactionType);
            collection.AddProperty(this, r => r.TransactionDateTime);
            collection.AddProperty(this, r => r.CallbackURL);
            collection.AddProperty(this, r => r.OrderDescription);
            collection.AddProperty(this, r => r.CustomerName);
            collection.AddProperty(this, r => r.Address1);
            collection.AddProperty(this, r => r.Address2);
            collection.AddProperty(this, r => r.Address3);
            collection.AddProperty(this, r => r.Address4);
            collection.AddProperty(this, r => r.City);
            collection.AddProperty(this, r => r.State);
            collection.AddProperty(this, r => r.PostCode);
            collection.AddProperty(this, r => r.CountryCode);
            collection.AddProperty(this, r => r.CV2Mandatory);
            collection.AddProperty(this, r => r.Address1Mandatory);
            collection.AddProperty(this, r => r.CityMandatory);
            collection.AddProperty(this, r => r.PostCodeMandatory);
            collection.AddProperty(this, r => r.StateMandatory);
            collection.AddProperty(this, r => r.CountryMandatory);
            collection.AddProperty(this, r => r.ResultDeliveryMethod);
            collection.AddProperty(this, r => r.ServerResultURL);
            collection.AddProperty(this, r => r.PaymentFormDisplaysResult);

            // TODO Temporary
            collection.Add("ServerResultURLCookieVariables", "");
            collection.Add("ServerResultURLFormVariables", "");
            collection.Add("ServerResultURLQueryStringVariables", "");

            return collection;
        }
    }
}
