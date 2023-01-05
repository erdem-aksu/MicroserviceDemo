using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace MicroserviceDemo.Shared.Hosting.Microservices.DistributedLocking;

[Dependency(ReplaceServices = true)]
public class MedallionAbpDistributedLock : IAbpDistributedLock, ITransientDependency
{
    protected IDistributedLockProvider DistributedLockProvider { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IDistributedLockKeyNormalizer DistributedLockKeyNormalizer { get; }

    public MedallionAbpDistributedLock(
        IDistributedLockProvider distributedLockProvider,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedLockKeyNormalizer distributedLockKeyNormalizer)
    {
        DistributedLockProvider = distributedLockProvider;
        CancellationTokenProvider = cancellationTokenProvider;
        DistributedLockKeyNormalizer = distributedLockKeyNormalizer;
    }

    public async Task<IAbpDistributedLockHandle> TryAcquireAsync(
        string name,
        TimeSpan timeout = default,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        var str = DistributedLockKeyNormalizer.NormalizeKey(name);
        CancellationTokenProvider.FallbackToProvider(cancellationToken);
        var handle = await DistributedLockProvider.TryAcquireLockAsync(str, timeout, CancellationTokenProvider.FallbackToProvider(cancellationToken)).ConfigureAwait(false);
        return handle != null ? new MedallionAbpDistributedLockHandle(handle) : (IAbpDistributedLockHandle) null;
    }
}