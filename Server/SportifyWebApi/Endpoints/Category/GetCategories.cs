using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SportifyWebApi.Endpoints.Category
{
    public class GetCategories : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<GetCategoriesResponse>
    {
        private readonly SportifyDbContext _context;

        public GetCategories(SportifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/categories")]
        public override async Task<ActionResult<GetCategoriesResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var res = await _context.Categories.Select(x => new GetCategoriesResponse()
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