using System;
using System.Threading.Tasks;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces;

public interface IFireForgetService<TInterface>
{
    void Execute(Func<TInterface, Task> serviceToInvoke);
}
