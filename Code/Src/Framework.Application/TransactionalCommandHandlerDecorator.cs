using System;
using System.Threading;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using Framework.Core;

namespace Framework.Application
{
    public class TransactionalCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _commandHandler;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalCommandHandlerDecorator(ICommandHandler<T> commandHandler, IUnitOfWork unitOfWork)
        {
            _commandHandler = commandHandler;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Begin();

                await _commandHandler.Handle(command, cancellationToken);

                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                try
                {
                    await _unitOfWork.RollBack();
                }
                catch (Exception) { }

                throw;
            }
        }
    }
}
