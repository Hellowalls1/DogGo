using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {

        //Allowing access to all of the different repositories because Owner Dtails view needs to know about all the things
        private readonly OwnerRepository _ownerRepo;
        private readonly DogRepository _dogRepo;
        private readonly WalkerRepository _walkerRepo;
        private readonly NeighborhoodRepository _neighborhoodRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnersController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
            _dogRepo = new DogRepository(config);
            _walkerRepo = new WalkerRepository(config);
            _neighborhoodRepo = new NeighborhoodRepository(config);
        }




        // GET: OwnersController
        //PUT THE VIEW IN EACH OF YOUR POST METHODS!!!
        public ActionResult Index()
        {

            List<Owner> owners = _ownerRepo.GetAllOwners();
            return View(owners);
        }

        // GET: OwnersController/Details/5
        // Takes whatever id is selected off of the Walkers listed in the application and passes its value here
        // Code looks for a walker with Id of 2 in database if it finds will return the walkers details (else 404 error)

        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }

        //TWO FUNCTIONS BELOW ARE ADD!!!
        // GET: OwnersController/Create
        //Create is a built in method
        //1st Method is What is Giving the User the Form (getting the user the form hehehe)
        //The get makes the view or Create.cshtml file that constructs the form (for Creating owners)
        // GET: Owners/Create

            public ActionResult Create()

            {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
            }

        // POST: OwnersController/Create
        // This method is actually sending(Posting) the form 
        //When the user hits the "Create" button, the browser is going to make a POST request to the url /owners/create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }




        // GET: OwnersController/Edit/5
        //the controller gets the owner id from the url route (owner/edit/2 whatever) 
        //we use that Id toi get the data from the database to fill out the inital state of the form
    
        public ActionResult Edit(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            //if owner doesn't exist return NotFound error
            if (owner == null)
            {
                return NotFound();
            }

            //
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = owner,
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Delete/5

        //the integer id parameter is coming from the route (owners/delete/5)
        //creating a view asking the uuser to confirm the deletion of an owner

        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            return View(owner);
        }

        // POST: OwnersController/Delete/5
        //when the user clicks the delete button on the view that populates above a Post request will be made in this method

        [HttpPost]
        [ValidateAntiForgeryToken]

        //the integer id parameter is coming from the route (owners/delete/5)
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }
    }
}
