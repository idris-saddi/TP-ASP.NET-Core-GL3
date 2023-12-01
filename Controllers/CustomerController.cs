using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1.Models;

namespace TP1.Controllers;

public class CustomerController : Controller
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var customers = _context
            .Customers
            .Include(x => x.MembershipType)
            .ToList();

        return View(customers);
    }

    public IActionResult Create()
    {
        var customer = new Customer();
        ViewBag.MembershipTypeId = MembershipTypeSelectListItems(customer);
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            ViewBag.MembershipTypeId = MembershipTypeSelectListItems(customer);
            return View(customer);
        }

        _context.Customers.Add(customer);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(Guid id)
    {
        try
        {
            var customer = _context
                .Customers
                .Include(x => x.MembershipType)
                .Where(x => x.Id == id)
                .Single();

            return View(customer);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    public IActionResult Edit(Guid id)
    {
        var customer = _context.Customers.Find(id);

        if (customer == null)
        {
            return NotFound();
        }

        ViewBag.MembershipTypeId = MembershipTypeSelectListItems(customer);
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            ViewBag.MembershipTypeId = MembershipTypeSelectListItems(customer);
            return View(customer);
        }

        _context.Customers.Update(customer);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        try
        {
            var customer = _context
                .Customers
                .Include(x => x.MembershipType)
                .Where(x => x.Id == id)
                .Single();

            return View(customer);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private IEnumerable<SelectListItem> MembershipTypeSelectListItems(Customer customer)
    {
        var selectedMembershipTypeId = customer.MembershipTypeId;

        return _context
            .MembershipTypes
            .Select(membershipType => new SelectListItem()
            {
                Text = membershipType.Name,
                Value = membershipType.Id.ToString(),
                Selected = (selectedMembershipTypeId != null) && (membershipType.Id == selectedMembershipTypeId),
            })
            .ToList()
            .Prepend(new()
            {
                Text = "Please select a membershipType",
                Value = "",
                Selected = selectedMembershipTypeId == null,
            });
    }
}

