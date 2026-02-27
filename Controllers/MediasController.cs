using DAL;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class MediasController : Controller
{
    // !!! Code de base venant de l'exemple ContactsJSON - Upgraded fait par Nicolas Chourot //https://github.com/Nicolas-Chourot/ContactsJSON-Upgraded/tree/master
    //Code modifié par Alexandre Rochefort Février 2026
    private void InitSessionVariables() //Modifié
    {
        // Session is a dictionary that hold keys values specific to a session
        // Each user of this web application have their own Session
        // A Session has a default time out of 20 minutes, after time out it is cleared

        if (Session["CurrentMediaId"] == null) Session["CurrentMediaId"] = 0;
        if (Session["CurrentMediaTitle"] == null) Session["CurrentMediaTitle"] = "";
        if (Session["Categories"] == null) Session["Categories"] = DB.MediasCategories();
        if (Session["SortByTitle"] == null) Session["SortByTitle"] = true;
        if (Session["Search"] == null) Session["Search"] = false;
        if (Session["SearchString"] == null) Session["SearchString"] = "";
        if (Session["SelectedCategory"] == null) Session["SelectedCategory"] = "";
    }
    private void ResetCurrentMediaInfo() //Modifié
    {
        Session["CurrentMediaId"] = 0;
        Session["CurrentMediaTitle"] = "";
    }
    public ActionResult GetMediasCategoriesList(bool forceRefresh = false) //Modifié
    {
        InitSessionVariables();

        bool search = (bool)Session["Search"];

        if (search)
        {
            return PartialView();
        }
        return null;
    }
    public ActionResult GetMedias(bool forceRefresh = false) //Modifié
    {
        IEnumerable<Media> result = null;
        if (forceRefresh || DB.Medias.HasChanged)
        {
            // forceRefresh is true when a related view is produce
            // DB.Contacts.HasChanged is true when a change has been applied on any contact

            InitSessionVariables();
            bool search = (bool)Session["Search"];
            string searchString = (string)Session["SearchString"];

            if (search)
            {
                result = DB.Medias.ToList().Where(c => c.Title.ToLower().Contains(searchString)).OrderBy(c => c.Title);
                string SelectedCategory = (string)Session["SelectedCategory"];
                if (SelectedCategory != "")
                    result = result.Where(c => c.Category == SelectedCategory);
            }
            else
                result = DB.Medias.ToList();
            if ((bool)Session["SortByTitle"])
                result = result.OrderBy(c => c.Title);
            else
                result = result.OrderBy(c => c.PublishDate);
            return PartialView(result);
        }
        return null;
    }
    public ActionResult List() //Modifié
    {
        ResetCurrentMediaInfo();
        return View();
    }

    public ActionResult Details(int id)
    {
        Session["CurrentMediaId"] = id;
        Media Media = DB.Medias.Get(id);
        if (Media != null)
        {
            Session["CurrentMediaTitle"] = Media.Title;
            return View(Media);
        }
        return RedirectToAction("List");
    } //Modifié
    public ActionResult About()
    {
        return View();
    }
    public ActionResult Create() //Modifié
    {
        return View(new Media());
    }

    [HttpPost]
    /* Install anti forgery token verification attribute.
     * the goal is to prevent submission of data from a page 
     * that has not been produced by this application*/
    [ValidateAntiForgeryToken()]
    public ActionResult Create(Media Media) //Modifié
    {
        DB.Medias.Add(Media);
        return RedirectToAction("List");
    }
    public ActionResult Edit() //Modifé
    {
        // Note that id is not provided has a parameter.
        // It use the Session["CurrentContactId"] set within
        // Details(int id) action
        // This way we prevent from malicious requests that could
        // modify or delete programatically the all the contacts

        int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;

        // Make sure that the contact of id really exist
        if (id != 0)
        {
            Media Media = DB.Medias.Get(id);
            if (Media != null)
                return View(Media);
        }
        return RedirectToAction("Details/" + id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken()]
    public ActionResult Edit(Media Media) //Modifié
    {
        // Has explained earlier, id of contact is stored server side an not provided in form data
        // passed in the method in order to prever from malicious requests

        int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;

        // Make sure that the Media of id really exist
        Media storedMedia = DB.Medias.Get(id);
        if (storedMedia != null)
        {
            Media.Id = id; // patch the Id
            DB.Medias.Update(Media);
        }
        return RedirectToAction("Details/" + id);
    }
    public ActionResult Delete() //Modifié
    {
        int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;
        if (id != 0)
        {
            DB.Medias.Delete(id);
        }
        return RedirectToAction("List");
    }

    // This action is meant to be called by an AJAX request
    // Return true if there is a name conflict
    // Look into validation.js for more details
    // and also into Views/Contacts/ContactForm.cshtml
    public JsonResult CheckConflict(string YoutubeId) //Modifié
    {
        int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;
        // Response json value true if name is used in other contacts than the current contact
        return Json(DB.Medias.ToList().Where(c => c.YoutubeId == YoutubeId && c.Id != id).Any(),
                    JsonRequestBehavior.AllowGet /* must have for CORS verification by client browser */);
    }
    public ActionResult ToggleSearch()
    {
        if (Session["Search"] == null) Session["Search"] = false;
        Session["Search"] = !(bool)Session["Search"];
        return RedirectToAction("List");
    }
    public ActionResult SortByTitle()
    {
        Session["SortByTitle"] = true;
        return RedirectToAction("List");
    }
    public ActionResult SortByDate()
    {
        Session["SortByTitle"] = false;
        return RedirectToAction("List");
    }
    public ActionResult SetSearchString(string value)
    {
        Session["SearchString"] = value.ToLower();
        return RedirectToAction("List");
    }

    public ActionResult SetSearchCategory(string value)
    {
        Session["SelectedCategory"] = value;
        return RedirectToAction("List");
    }
}
