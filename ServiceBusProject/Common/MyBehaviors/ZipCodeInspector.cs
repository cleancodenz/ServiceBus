using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBehaviors
{
    /**
     * Called before and after invocation to inspect and modify parameter values.
     *
     * As their names suggest, the runtime invokes BeforeCall before calling the target method 
     * on the service instance and AfterCall after having made the call. 
     * 
     * This gives you pre- and post-interception points for inspecting the parameters and return values, 
     * 
     * which are supplied to these methods as arrays of objects.
     *  
     **/

    public class ZipCodeInspector : IParameterInspector 
    {
        int zipCodeParamIndex; 
        string zipCodeFormat = @"\d{5}-\d{4}"; 
        public ZipCodeInspector() : 
            this(0) 
        { }
        
        public ZipCodeInspector(int zipCodeParamIndex)
        { 
            this.zipCodeParamIndex = zipCodeParamIndex; 
        }  // AfterCall is empty 
        
        public object BeforeCall(string operationName, object[] inputs) 
        {
            string zipCodeParam = inputs[this.zipCodeParamIndex] as string; 
            
            if (!Regex.IsMatch( zipCodeParam, this.zipCodeFormat, RegexOptions.None)) 
                throw new FaultException( "Invalid zip code format. Required format: #####-####"); 
            return null; 
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            
        }
    }
}
