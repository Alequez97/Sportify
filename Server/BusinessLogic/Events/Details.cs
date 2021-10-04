using DataServices;
using DomainEntities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Event>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Event>
        {
            private readonly SportifyDbContext _context;
            public Handler(SportifyDbContext context)
            {
                _context = context;
            }

            public async Task<Event> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Events.FindAsync(request.Id); 
            }
        }
    }
}