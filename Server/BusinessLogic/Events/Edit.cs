using AutoMapper;
using DataServices;
using DomainEntities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Event Event { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SportifyDbContext _dbContext;
            private readonly IMapper _mapper;
            public Handler(SportifyDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _dbContext = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var @event = await _dbContext.Events.FindAsync(request.Event.Id);
                _mapper.Map(request.Event, @event);
                await _dbContext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}