using System.Globalization;
using System.Threading.Tasks;
using Medallion.Threading;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;

namespace MicroserviceDemo.Shared.Hosting.Microservices.DistributedLocking;

public class MedallionAbpDistributedLockHandle : IAbpDistributedLockHandle
{
    public IDistributedSynchronizationHandle Handle { get; }

    public MedallionAbpDistributedLockHandle(IDistributedSynchronizationHandle handle) => Handle = handle;

    public async ValueTask DisposeAsync()
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            await Handle.DisposeAsync();
        }
    }
}