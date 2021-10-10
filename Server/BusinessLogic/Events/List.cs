using DataServices;
using DomainEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Events
{
    public class List
    {
        public class Query : IRequest<List<Event>> { }

        public class Handler : IRequestHandler<Query, List<Event>>
        {
            private readonly SportifyDbContext _context;
            public Handler(SportifyDbContext context)
            {
                _context = context;
            }

            public async Task<List<Event>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Events.Include(v => v.Venue).ToListAsync();
            }
        }
    }
}