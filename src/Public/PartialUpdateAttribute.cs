using System.Web.Http;
using System.Web.Http.Controllers;

namespace Clouty.Helpers.PartialUpdate
{
    public class PartialUpdateAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new PartialUpdateBinding(parameter);
        }
    }
}