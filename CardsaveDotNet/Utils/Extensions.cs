using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using CardsaveDotNet.Hosted;

namespace CardsaveDotNet.Utils
{
    public static class Extensions
    {
        public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object[] args) {
            sb.AppendLine(string.Format(format, args));
            return sb;
        }
        
        public static void Add(this NameValueCollection collection, string name, object value) {
            collection.Add(name, value == null ? "" : value.ToString());
        }

        public static string ToQueryString(this NameValueCollection collection, string delimiter = "&", bool omitEmpty = false, bool encode = true) {
            if (String.IsNullOrEmpty(delimiter))
                delimiter = "&";

            Char equals = '=';
            List<String> items = new List<String>();

            for (int i = 0; i < collection.Count; i++) {
                foreach (String value in collection.GetValues(i)) {
                    Boolean addValue = (omitEmpty) ? !String.IsNullOrEmpty(value) : true;
                    if (addValue)
                        items.Add(String.Concat(collection.GetKey(i), equals, (encode) ? HttpUtility.UrlEncode(value) : value));
                }
            }
            return String.Join(delimiter, items.ToArray());
        }

        public static void AddProperty<TEntity, TProperty>(this NameValueCollection collection, TEntity entity, Expression<Func<TEntity, TProperty>> e, object value = null)
        {
            var memberExpression = e.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Expression does not refer to a member");

            var propertyInfo = memberExpression.Member as PropertyInfo;
                       
           if (propertyInfo == null)
                throw new ArgumentException("Expression does not refer to a property");

            collection.Add(
                propertyInfo.Name, value ?? propertyInfo.GetValue(entity, null)
            );
        }
    }
}
