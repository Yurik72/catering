using System.Threading;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}