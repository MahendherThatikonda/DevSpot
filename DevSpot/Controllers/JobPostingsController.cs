using DevSpot.Models;
using DevSpot.Repository;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
    public class JobPostingsController : Controller
    {

        private readonly IRepository<JobPosting> _repository;

        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(
            IRepository<JobPosting> repository,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllSync();
            return View(jobPostings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingvm)
        {
            if (ModelState.IsValid)
            {
                //   jobposting.UserId = _userManager.GetUserId(User);

                var jobPosting = new JobPosting
                {
                    Title = jobPostingvm.Title,
                    Description = jobPostingvm.Description,
                    Company = jobPostingvm.Company,
                    Location= jobPostingvm.Location,
                    UserId=_userManager.GetUserId(User)
                    //PostedDate=
                };
                await _repository.AddASync(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            // 
            return View(jobPostingvm);
        }
    }
}
