using AutoWrapper;

namespace CustomerResource.Models
{
    public class AutoWrapperResponse
    {
        [AutoWrapperPropertyMap(Prop.ResponseException)]
        public object Error { get; set; }

        [AutoWrapperPropertyMap(Prop.ResponseException_ExceptionMessage)]
        public string Message { get; set; }

        [AutoWrapperPropertyMap(Prop.ResponseException_Details)]
        public string StackTrace { get; set; }
    }
}
