using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class DonationRepository
    {
        private ApplicationDbContext _context;
        public DonationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Donation donation) =>
        await _context.Donations.AddAsync(donation);

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


    }
}
