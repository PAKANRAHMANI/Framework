using System;
using System.Threading;
using System.Threading.Tasks;
using Framework.Application.Contracts;
using Framework.Core;

namespace Framework.Config
{
    public class TransactionalRequestHandlerDecorator<TRequest, Tresponse> : IRequestHandler<TRequest,Tresponse> where TRequest : IRequest
    {
        private readonly IRequestHandler<TRequest, Tresponse> _requestHandler;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalRequestHandlerDecorator(IRequestHandler<TRequest, Tresponse> requestHandler, IUnitOfWork unitOfWork)
        {
            _requestHandler = requestHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<Tresponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Begin();

                var response = await _requestHandler.Handle(request,cancellationToken);

                await _unitOfWork.Commit();

                return response;
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
