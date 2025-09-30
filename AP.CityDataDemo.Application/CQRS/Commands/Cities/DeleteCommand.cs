using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

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
            await _unitOfWork.Commit();
            return true;
        }
    }

    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .MustAsync(NotLastCity).WithMessage("Cannot delete the last city in the database.");
        }

        private async Task<bool> NotLastCity(int cityId, CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.CitiesRepository.GetCountAsync();
            return count > 1;
        }
    }
}
