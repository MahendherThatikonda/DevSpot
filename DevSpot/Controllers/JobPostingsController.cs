using DevSpot.Models;
using DevSpot.Repository;
using DevSpot.ViewModels;
using DevSpot.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllSync();
            return View(jobPostings);
        }

        [Authorize(Roles ="Admin, Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
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

        [HttpDelete]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetbyIdAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (User.IsInRole(Roles.Admin) == false && jobPosting.UserId!=userId)
            {
                return Forbid();
            }

            await _repository.DeleteASync(id);

            return Ok();

        }
    }
}
