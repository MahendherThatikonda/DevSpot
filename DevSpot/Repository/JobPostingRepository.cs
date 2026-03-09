using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repository
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddASync(JobPosting entity)
        {
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteASync(int id)
        {
            var jobPosting  = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }

            _context.JobPostings.Remove(jobPosting);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<JobPosting>> GetAllSync()
        {
            return await _context.JobPostings.ToListAsync();
         
        }

        public async Task<JobPosting> GetbyIdAsync(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }

            return jobPosting;
        }

        public async Task UpdateASync(JobPosting entity)
        {
            _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
         
        }
    }
}
