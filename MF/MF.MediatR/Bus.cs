using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace MF.MediatR
{
    public class Bus : IBus
    {
        private readonly IMediator _mediator;

        public Bus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TPubResponse> SendAsync<TPubResponse>(IRequest<TPubResponse> request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(request, cancellationToken);
        }

        public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return _mediator.Publish(notification, cancellationToken);
        }
    }
}