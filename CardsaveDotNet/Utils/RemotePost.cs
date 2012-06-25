using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;

namespace CardsaveDotNet.Utils
{
    public class RemotePost : IRemotePost
    {
        private readonly HttpContextBase _httpContext;
        private NameValueCollection _inputValues;

        public RemotePost(HttpContextBase httpContext, string url, FormMethod method) : this(httpContext) {
            this.Url = url;
            this.Method = method;
        }
        
        public RemotePost(HttpContextBase httpContext) {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            
            _inputValues = new NameValueCollection();
            _httpContext = httpContext;
        }

        public string Url { get; set; }
        public FormMethod Method { get; set; }
        public NameValueCollection InputValues { get { return _inputValues; } set { _inputValues = value; } }

        public void AddInput(string name, object value) {
            _inputValues.Add(name, value.ToString());
        }

        public void Post(string formName)
        {
            if (string.IsNullOrEmpty(formName))
                throw new ArgumentNullException("formName");
            
            var formBuilder = new StringBuilder();
            formBuilder.AppendLine("<html><head>");
            formBuilder.AppendLineFormat("</head><body onload=\"document.{0}.submit()\">", formName);
            formBuilder.AppendLineFormat("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", formName, Method.ToString(), Url);
            for (int i = 0; i < _inputValues.Keys.Count; i++) {
                formBuilder.AppendLineFormat("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", 
                    HttpUtility.HtmlEncode(_inputValues.Keys[i]), HttpUtility.HtmlEncode(_inputValues[_inputValues.Keys[i]]));
            }
            formBuilder.AppendLine("</form>");
            formBuilder.AppendLine("</body></html>");

            
            #if DEBUG
                Debug.Write(formBuilder.ToString());
            #endif

            _httpContext.Response.Clear();
            _httpContext.Response.Write(formBuilder.ToString());
            _httpContext.Response.End();
        }
    }

    public enum FormMethod {
        GET,
        POST
    }
}