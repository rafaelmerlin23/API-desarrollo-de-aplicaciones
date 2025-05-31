using foroLIS_backend.Models;
using foroLIS_backend.Repository;

namespace foroLIS_backend.Services
{
    public class DonationService
    {
        private DonationRepository _repository;
        public DonationService(DonationRepository repository) { 
            _repository = repository;
        }


        public async Task<Donation> Add(Donation donation)
        {
            await _repository.Add(donation);
            await _repository.Save();
            return donation;
        }

    }
}
