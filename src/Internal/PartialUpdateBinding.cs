using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using Newtonsoft.Json.Linq;

namespace Clouty.Helpers.PartialUpdate
{
    internal class PartialUpdateBinding : HttpParameterBinding
    {
        public PartialUpdateBinding(HttpParameterDescriptor descriptor) : base(descriptor) { }
        
        public override Boolean WillReadBody => true;
        
        public override async Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var parameterType = Descriptor.ParameterType;

            var requestJson = await actionContext
                .ControllerContext
                .Request
                .Content
                .ReadAsStringAsync();

            var requestJsonObject = JObject.Parse(requestJson);

            var parameter = Activator.CreateInstance(parameterType);

            var fields = PartialUpdateParameterMetadata.GetParameterFields(actionContext, parameterType);

            foreach (var fieldInfo in fields)
            {
                var fieldJsonObject = requestJsonObject[fieldInfo.JsonFieldName];

                switch (fieldInfo)
                {
                    case AssignableParameterFieldInfo assignableFieldInfo:
                    {
                        var assignableObject = BuildAssignableObject(assignableFieldInfo, fieldJsonObject);

                        assignableFieldInfo.PropertyInfo.SetValue(parameter, assignableObject);

                        break;
                    }
                    case NonAssignableParameterFieldInfo nonAssignableFieldInfo:
                    {
                        var fieldValue = fieldJsonObject?.ToObject(nonAssignableFieldInfo.PropertyType) ??
                                         nonAssignableFieldInfo.EmptyValue;

                        nonAssignableFieldInfo.PropertyInfo.SetValue(parameter, fieldValue);

                        break;
                    }
                }
            }

            SetValue(actionContext, parameter);
        }

        private static Object BuildAssignableObject(AssignableParameterFieldInfo fieldInfo, JToken fieldJsonObject)
        {
            var assignableObject = Activator.CreateInstance(fieldInfo.PropertyType);

            var isFieldSet = fieldJsonObject != null;

            fieldInfo.AssignableIsSetPropertyInfo.SetValue(assignableObject, isFieldSet);

            var fieldValue = fieldJsonObject?.ToObject(fieldInfo.GenericBaseType);

            if (fieldValue != null)
            {
                fieldInfo.AssignableValuePropertyInfo.SetValue(assignableObject, fieldValue);
            }

            return assignableObject;
        }
    }
}