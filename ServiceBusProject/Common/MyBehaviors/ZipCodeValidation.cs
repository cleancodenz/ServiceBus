using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace MyBehaviors
{
    public class ZipCodeValidation : Attribute, IOperationBehavior {

        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, 
            ClientOperation clientOperation) 
        {
            ZipCodeInspector zipCodeInspector = new ZipCodeInspector(); 
            
            clientOperation.ParameterInspectors.Add(zipCodeInspector); 
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation)
        { 
            ZipCodeInspector zipCodeInspector = new ZipCodeInspector(); 
            
            dispatchOperation.ParameterInspectors.Add(zipCodeInspector); 
        
        }  // remaining methods empty 

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }
    } 
    
    public class ZipCodeCaching : Attribute, IOperationBehavior 
    {
        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, 
            DispatchOperation dispatchOperation) 
        { 
            dispatchOperation.Invoker = new ZipCodeCacher(dispatchOperation.Invoker);
        }  // remaining methods empty 

        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }
    }
}
