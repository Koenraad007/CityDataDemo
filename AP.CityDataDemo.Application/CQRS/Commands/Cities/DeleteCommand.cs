using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using AP.CityDataDemo.Application.Exceptions;

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
        private readonly IEmailService _emailService;

        public DeleteCommandHandler(IUnitOfWork uow, IEmailService emailService)
        {
            _unitOfWork = uow;
            _emailService = emailService;
        }

        public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CitiesRepository.DeleteByIdAsync(request.Id, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            var city = await _unitOfWork.CitiesRepository.GetByIdAsync(request.Id, cancellationToken);
            if (city != null)
            {
                throw new TransactionFailedException($"City with id {request.Id} could not be deleted.");
            }

            try
            {
                await _emailService.SendEmailAsync(
                    "koenvanaken1999@gmail.com",
                    "City Deleted",
                    $"City with id {request.Id} was deleted."
                    );
            }
            catch (Exception ex)
            {
                // Log or handle the email sending failure as needed
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

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
            var count = await _unitOfWork.CitiesRepository.GetCountAsync(cancellationToken);
            return count > 1;
        }
    }
}
