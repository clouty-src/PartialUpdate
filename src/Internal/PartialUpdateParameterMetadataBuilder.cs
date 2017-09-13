using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clouty.Helpers.PartialUpdate
{
    internal static class PartialUpdateParameterMetadataBuilder
    {
        public static ParameterFieldInfo[] BuildParameterFields(HttpActionContext actionContext, Type type)
        {
            var jsonFormatter = actionContext.ControllerContext.Configuration.Formatters.JsonFormatter;

            var jContract =
                jsonFormatter.SerializerSettings.ContractResolver.ResolveContract(type) as JsonObjectContract;

            var parameterTypeProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var fields = new List<ParameterFieldInfo>();

            foreach (var propertyInfo in parameterTypeProperties)
            {
                var propertyType = propertyInfo.PropertyType;

                var isAssignable = propertyType.IsGenericType &&
                                   propertyType.GetGenericTypeDefinition() == typeof(Assignable<>);

                ParameterFieldInfo fieldInfo;

                if (isAssignable)
                {
                    fieldInfo = new AssignableParameterFieldInfo()
                    {
                        PropertyType = propertyType,
                        PropertyInfo = propertyInfo,
                        AssignableIsSetPropertyInfo = propertyType.GetProperty(nameof(Assignable<Object>.IsAssigned)),
                        AssignableValuePropertyInfo = propertyType.GetProperty(nameof(Assignable<Object>.Value)),
                        GenericBaseType = propertyType.GenericTypeArguments[0]
                    };
                }
                else
                {
                    fieldInfo = new NonAssignableParameterFieldInfo(pt => pt.IsValueType
                        ? Activator.CreateInstance(pt)
                        : null)
                    {
                        PropertyType = propertyType,
                        PropertyInfo = propertyInfo
                    };
                }

                fieldInfo.JsonFieldName = GetJsonFieldName(propertyInfo, jContract);

                fields.Add(fieldInfo);
            }

            return fields.ToArray();
        }

        private static String GetJsonFieldName(PropertyInfo property, JsonObjectContract jContract)
        {
            var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();

            String jsonFieldName = null;

            if (jsonPropertyAttribute != null)
            {
                jsonFieldName = jsonPropertyAttribute.PropertyName;
            }
            else
            {
                if (jContract != null)
                {
                    var jsonProperty = jContract.Properties.GetClosestMatchProperty(property.Name);

                    jsonFieldName = jsonProperty.PropertyName;
                }
                else
                {
                    jsonFieldName = property.Name;
                }
            }

            return jsonFieldName;
        }
    }
}