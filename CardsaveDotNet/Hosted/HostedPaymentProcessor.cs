using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Collections.Specialized;
using CardsaveDotNet.Utils;
using CardsaveDotNet.Common;

namespace CardsaveDotNet.Hosted
{
    public class HostedPaymentProcessor : IHostedPaymentProcessor
    {
        private readonly string _merchantId;
        private readonly HttpContextBase _context;

        /// <summary>
        /// Creates a new instance of the payment processor
        /// </summary>
        /// <param name="merchantId">The merchant ID that corresponds to the gateway account the transaction will be run through.</param>
        /// <param name="context">The current http context</param>
        public HostedPaymentProcessor(string merchantId, HttpContextBase context) {
            if (string.IsNullOrEmpty(merchantId))
                throw new ArgumentNullException("merchantId");

            _context = context;
            _merchantId = merchantId;
        }

        /// <summary>
        /// The merchant ID that corresponds to the gateway account the transaction will be run through.
        /// </summary>
        public string MerchantId { get { return _merchantId; } }

        /// <summary>
        /// Submits a payment request to the hosted payment page
        /// </summary>
        /// <param name="request">The request to submit</param>
        /// <param name="merchantPassword">The merchant password that corresponds to the gateway account the transaction will be run through.</param>
        /// <param name="preSharedKey">The merchant gateway account pre shared key</param>
        /// <param name="postUrl">The url of the hosted payment page</param>
        public void SubmitTransaction(HostedTransactionRequest request, string merchantPassword, string preSharedKey, string postUrl)
        {
            if (CommonUtils.AreNullOrEmpty(merchantPassword, preSharedKey, postUrl))
                throw new ArgumentNullException();
            
            if (request == null)
                throw new ArgumentNullException("request");

            var hashInputs = new NameValueCollection();

            var hashMethod = HashMethod.SHA1;

            if (hashMethod == HashMethod.SHA1 || hashMethod == HashMethod.MD5)
            {
                // only add if using standard hash method (MD5 or SHA1)
                hashInputs.Add("PreSharedKey", preSharedKey);
            }
            
            hashInputs.Add("MerchantID", _merchantId);
            hashInputs.Add("Password", merchantPassword);

            var requestInputs = request.ToNameValueCollection();
            foreach (var k in requestInputs.AllKeys)
                hashInputs.Add(k, requestInputs.GetValues(k)[0]);
            
            var hashString = hashInputs.ToQueryString(encode: false);
            var hash = HashUtil.ComputeHashDigest(hashString, preSharedKey, hashMethod);
            
            // ready to post - just return the NameValue Collection

            var remotePost = new RemotePost(_context, postUrl, FormMethod.POST);
            remotePost.AddInput("HashDigest", hash);
            remotePost.AddInput("MerchantID", _merchantId);

            // add the rest of the form variables
            foreach (var k in requestInputs.AllKeys)
                remotePost.AddInput(k, requestInputs.GetValues(k)[0]);

            remotePost.Post("CardsavePaymentForm");
        }

        /// <summary>
        /// Validates the request to ensure it came from a valid source
        /// </summary>
        /// <param name="urls">Optional source urls</param>
        /// <returns>Boolean</returns>
        public bool ValidateRequest(string[] urls = null) {
            string[] validUrls = new[] {
                "slayer.thepaymentgateway.net",
                "destroyer.thepaymentgateway.net"
            };

            if (urls != null) {
                var arr = new string[validUrls.Length + urls.Length];
                validUrls.CopyTo(arr, 0);
                urls.CopyTo(arr, validUrls.Length);
                validUrls = arr;
            }

            var validIpAddresses = new List<IPAddress>();
            foreach (var url in validUrls)
                validIpAddresses.AddRange(Dns.GetHostAddresses(url));

            string requestIp = _context.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(requestIp)) {
                return false;
            }

            if (!validIpAddresses.Contains(IPAddress.Parse(requestIp))) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Performs basic validation of the transaction result (you should also implement your own e.g. check amounts against order)
        /// </summary>
        /// <param name="result">Transaction result</param>
        public void ValidateResult(ServerTransactionResult result) {
            if (!(result.MerchantID.Equals(result.MerchantID, StringComparison.InvariantCultureIgnoreCase)))
                throw new Exception("The merchant id of the result is invalid");
        }

        /// <summary>
        /// Creates a response message confirming delivery of transaction result
        /// </summary>
        /// <param name="status">Result of delivery (note this is to confirm that you have received the result, not the result itself)</param>
        /// <param name="message">Optional message for example, any exceptions that may have occurred</param>
        /// <returns>String</returns>
        public string CreateServerResponseString(TransactionStatus status, string message = "") {
            
            var response = new NameValueCollection();
            response.Add("StatusCode", (int)status);
            if (!string.IsNullOrEmpty(message)) {
                response.Add("Message", message);
            }

            return response.ToQueryString();
        }
    }
}
