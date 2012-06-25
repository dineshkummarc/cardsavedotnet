using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardsaveDotNet.Hosted;

namespace ASPNetDemo
{
    /// <summary>
    /// Summary description for CallbackHandler
    /// </summary>
    public class CallbackHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = new CallbackResult(context.Request.QueryString);
            if (result != null)
            {
                Action<string> write = s => context.Response.Write(s + "<br/>");

                write("MerchantID : " + result.MerchantID);
                write("HashDigest : " + result.HashDigest);
                write("OrderID : " + result.OrderID);
                write("CrossReference : " + result.CrossReference);
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