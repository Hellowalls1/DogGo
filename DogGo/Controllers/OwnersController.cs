using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly OwnerRepository _ownerRepo;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnersController(IConfiguration config)
        {
            _ownerRepo = new OwnerRepository(config);
        }

        //PUT THE VIEW IN EACH OF YOUR POST METHODS!!!

        // GET: OwnersController
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

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        //TWO FUNCTIONS BELOW ARE ADD!!!
        // GET: OwnersController/Create
        //Create is a built in method
        //1st Method is What is Giving the User the Form (getting the user the form hehehe)
        //The get makes the view or Create.cshtml file that constructs the form (for Creating owners)
        public ActionResult Create()
        {
            return View();
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

            //else return form with owner info
            return View(owner);
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
