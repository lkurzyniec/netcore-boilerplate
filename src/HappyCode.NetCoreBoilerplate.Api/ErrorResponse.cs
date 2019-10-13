using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappyCode.NetCoreBoilerplate.Api
{
    public class ErrorResponse
    {
        public IEnumerable<string> Messages { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Exception { get; set; }
    }
}
