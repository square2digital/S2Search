using System;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFireForgetService<TInterface>
    {
        void Execute(Func<TInterface, Task> serviceToInvoke);
    }
}
