using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyWebApi.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace SportifyWebApi.Endpoints.Category
{
    public class GetEventsCategories : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetCategoriesResponse>
    {
        private readonly SportifyDbContext _context;

        public GetEventsCategories(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("/api/events/categories")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Events })]
        public override async Task<ActionResult<GetCategoriesResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var res = await _context.EventCategories.Select(x => new GetCategoriesResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return res != null ? Ok(res) : NotFound();
        }
    }

    public class GetCategoriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
