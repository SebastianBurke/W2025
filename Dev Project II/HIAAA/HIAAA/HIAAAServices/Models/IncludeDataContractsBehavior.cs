using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Description;
using HIAAAServices.Controllers;

public class IncludeDataContractsBehavior : IServiceBehavior
{
    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    {
        Console.WriteLine("IncludeDataContractsBehavior: AddBindingParameters called.");
    }

    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        Console.WriteLine("IncludeDataContractsBehavior: ApplyDispatchBehavior called.");

        foreach (var endpoint in serviceDescription.Endpoints)
        {
            Console.WriteLine($"Endpoint: {endpoint.Address.Uri}");

            foreach (var operation in endpoint.Contract.Operations)
            {
                Console.WriteLine($"Operation: {operation.Name}");

                // Add known types
                operation.KnownTypes.Add(typeof(UserBLL));
                operation.KnownTypes.Add(typeof(RoleBLL));
                
                Console.WriteLine("DataContractSerializerOperationBehavior added.");
            }
        }
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        Console.WriteLine("IncludeDataContractsBehavior: Validate called.");
    }
}