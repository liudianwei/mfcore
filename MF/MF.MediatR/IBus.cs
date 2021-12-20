using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MF.MediatR
{
    public interface IBus
    {
        Task<TPubResponse> SendAsync<TPubResponse>(IRequest<TPubResponse> request, CancellationToken cancellationToken = default);

        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
    }
}