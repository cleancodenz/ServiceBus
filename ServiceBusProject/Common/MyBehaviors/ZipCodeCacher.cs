using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace MyBehaviors
{
    /**
     * Called to invoke the operation.
     * 
     * This hook allows you to override the default process with a custom invoker object. 
     * 
     * In the ZIP code example, 
     * 
     * you could use the operation invoker to implement a simple output caching feature. 
     * 
     * For a given ZIP code, the result is always going to be the same, 
     * 
     * so if you cache the results, 
     * 
     * you'll never have to invoke the service instance more than once for a particular ZIP code value. 
     * 
     * This could greatly improve performance and response times in certain situations 
     * where the service logic is either costly or takes a long time to complete.
     * 
     * **/
    public class ZipCodeCacher : IOperationInvoker 
    { 
        IOperationInvoker innerOperationInvoker; 
        Dictionary<string, string> zipCodeCache = new Dictionary<string, string>(); 
        
        public ZipCodeCacher(IOperationInvoker innerOperationInvoker) 
        { 
            this.innerOperationInvoker = innerOperationInvoker; 
        } 
        
        public object Invoke(object instance, object[] inputs, out object[] outputs) 
        { 
            string zipcode = inputs[0] as string; 
            string value; 
            
            if (this.zipCodeCache.TryGetValue(zipcode, out value)) 
            { 
                outputs = new object[0]; 
                return value; 
            } 
            else 
            { 
                value = (string)this.innerOperationInvoker.Invoke( instance, inputs, out outputs); 
                
                zipCodeCache[zipcode] = value; return value; 
            } 
        }  
        // remaining methods elided 
        // they simply delegate to innerOperationInvoker 

        public object[] AllocateInputs()
        {
            return this.innerOperationInvoker.AllocateInputs();
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            
            return this.innerOperationInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            object operationResult = this.innerOperationInvoker.InvokeEnd(instance, out outputs, result);
         
            return operationResult;
        }

        public bool IsSynchronous
        {
            get { return this.innerOperationInvoker.IsSynchronous; }
        }
    }
}
