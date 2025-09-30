
using AP.CityDataDemo.Application.Interfaces;
using MediatR;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities
{
    public class DeleteCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CitiesRepository.DeleteByIdAsync(request.Id);
            return true;
        }
    }
}
