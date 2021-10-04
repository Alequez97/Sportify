using DataServices;
using DomainEntities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Events
{
    public class Create
    {
        public class Command : IRequest
        {
            public Event Event { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SportifyDbContext _dbContext;
            public Handler(SportifyDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _dbContext.Events.Add(request.Event);
                await _dbContext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}