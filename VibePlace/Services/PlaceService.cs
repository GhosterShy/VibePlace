using Microsoft.EntityFrameworkCore;
using VibePlace.Data;
using VibePlace.Data.Models;
using VibePlace.Migrations;

namespace VibePlace.Services
{
	public class PlaceService
	{
		private readonly AppIdentityDBContext _context;

		public PlaceService(AppIdentityDBContext context)
		{
			_context = context;
		}

		public async Task<Places?> GetPlaceByIdAsync(int placeId)
		{
			return await _context.places
				.Include(p => p.Images)  
				.Include(p => p.Reviews)
					.ThenInclude(r => r.User)
				.Include(p => p.Category)
				.Include(p => p.ServiceToPlaces)
				.AsSplitQuery()
				.FirstOrDefaultAsync(p => p.Id == placeId);
		}

		public async Task<List<Service?>> GetServiceByIdAsync(int placeId)
		{
			return await _context.services
				.Where(p => p.ServiceToPlaces.Any(s => s.PlaceId == placeId))
				.ToListAsync();
		}
	}	
}
