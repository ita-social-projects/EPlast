﻿using EPlast.BLL.Commands.Distinction;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class DeleteDistinctionHandler : IRequestHandler<DeleteDistinctionCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMediator _mediator;

        public DeleteDistinctionHandler(IRepositoryWrapper repositoryWrapper, IMediator mediator)
        {
            _repositoryWrapper = repositoryWrapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteDistinctionCommand request, CancellationToken cancellationToken)
        {
            var distinction = (await _repositoryWrapper.Distinction.GetFirstAsync(d => d.Id == request.Id));
            if (distinction == null)
                throw new ArgumentNullException($"Distinction with {request.Id} not found");
            _repositoryWrapper.Distinction.Delete(distinction);
            await _repositoryWrapper.SaveAsync();

            return Unit.Value;
        }
    }
}
