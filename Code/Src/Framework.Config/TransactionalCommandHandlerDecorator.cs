using System;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using Framework.Core;

namespace Framework.Config
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
        public async Task Handle(T command)
        {
            try
            {
                await _unitOfWork.Begin();

                await _commandHandler.Handle(command);

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
