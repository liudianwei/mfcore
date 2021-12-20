using MediatR;

namespace MF.MediatR
{
    public interface ICommand<T> : IRequest<T>
    {
    }
}