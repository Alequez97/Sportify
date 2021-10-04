using DataServices;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SportifyDbContext _context;
            public Handler(SportifyDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Events.FindAsync(request.Id);

                _context.Remove(activity);

                await _context.SaveChangesAsync(); 
                
                return Unit.Value;
            }
        }
    }
}