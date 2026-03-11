using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpots.Tests
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb")
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddSync_ShouldAddJobPosting()
        {
            //Dbcontext
            var db = CreateDbContext();
            //JobPostingRepository
            var repository = new JobPostingRepository(db);
            //JobPosting
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "TestCompany",
                Location = "Testlocation",
                UserId = "1234"
            };

           await repository.AddASync(jobPosting);

           var result =db.JobPostings.SingleOrDefault(x => x.Title=="Test Title");

            Assert.NotNull(result);
            Assert.Equal("Test Title",result.Title);
        }

        [Fact]
        public async Task GetbyIdAsync_ShouldReturnJobPosting()
        {
            var db = CreateDbContext();
     
            var repository = new JobPostingRepository(db);
     
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "TestCompany",
                Location = "Testlocation",
                UserId = "1234"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();



            var result = await repository.GetbyIdAsync(jobPosting.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public async Task GetbyIdAsync_ShouldThrowkeyNotFoundException()
        {
            var db =CreateDbContext();

            var repository = new JobPostingRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repository.GetbyIdAsync(654)  
              );
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllJobPostings()
        {
            var db = CreateDbContext();

            var repository = new JobPostingRepository(db);

            var jobPosting1 = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "TestCompany",
                Location = "Testlocation",
                UserId = "1234"
            };

            var jobPosting2 = new JobPosting
            {
                Title = "Test Title 2",
                Description = "Test Description 2",
                PostedDate = DateTime.Now,
                Company = "TestCompany 2",
                Location = "Testlocation 2",
                UserId = "12345"
            };

            await db.JobPostings.AddRangeAsync(jobPosting1,jobPosting2);
            await db.SaveChangesAsync();

            var result = await repository.GetAllSync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobPosting()
        {
            var db =CreateDbContext();
            var repository = new JobPostingRepository(db);

            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "TestCompany",
                Location = "Testlocation",
                UserId = "1234"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            jobPosting.Description = "Updated Description";

            await repository.UpdateASync(jobPosting);

            var result = db.JobPostings.Find(jobPosting.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Description", result.Description);

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);

            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "TestCompany",
                Location = "Testlocation",
                UserId = "1234"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            await repository.DeleteASync(jobPosting.Id);

            var result = db.JobPostings.Find(jobPosting.Id);

            Assert.Null(result);

        }

    }
}
