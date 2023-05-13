﻿using System.Collections.Generic;
using Autofac;
using Framework.Application.Contracts;
using MassTransit.Initializers.Variables;

namespace Framework.Config.Autofac
{
    public class AutofacCommandHandlerResolver : ICommandHandlerResolver
    {
        private readonly IComponentContext _context;

        public AutofacCommandHandlerResolver(IComponentContext context)
        {
            _context = context;
        }
        public IEnumerable<ICommandHandler<T>> ResolveHandlers<T>(T command) where T : ICommand
        {
           return _context.Resolve<IEnumerable<ICommandHandler<T>>>();
        }
    }
}
