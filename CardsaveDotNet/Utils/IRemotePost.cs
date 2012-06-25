using System;
namespace CardsaveDotNet.Utils
{
    public interface IRemotePost
    {
        void AddInput(string name, object value);
        System.Collections.Specialized.NameValueCollection InputValues { get; set; }
        FormMethod Method { get; set; }
        void Post(string formName);
        string Url { get; set; }
    }
}
