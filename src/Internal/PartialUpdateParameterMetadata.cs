using System;
using System.Collections.Concurrent;
using System.Web.Http.Controllers;

namespace Clouty.Helpers.PartialUpdate
{
    internal static class PartialUpdateParameterMetadata
    {
        private static readonly ConcurrentDictionary<String, ParameterFieldInfo[]> FieldsCache =
            new ConcurrentDictionary<String, ParameterFieldInfo[]>();

        public static ParameterFieldInfo[] GetParameterFields(
            HttpActionContext actionContext, Type type)
        {
            var fieldsInfo = FieldsCache.GetOrAdd(type.FullName,
                s => PartialUpdateParameterMetadataBuilder.BuildParameterFields(actionContext, type));

            return fieldsInfo;
        }   
    }
}