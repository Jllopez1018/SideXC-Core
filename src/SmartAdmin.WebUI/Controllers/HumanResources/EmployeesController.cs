using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SideXC.WebUI.Classes;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Human_Resources;

namespace SideXC.WebUI.Controllers.HumanResources
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var listProfiles = _context.Profiles.Where(c=> c.Active == true).ToList();
            var listContactTypes = _context.ContactTypes.Where(c => c.Active == true).ToList();
            ViewBag.PasswordAutoGenerated = Common.GetRandomAlphanumericString(8);
            ViewBag.Profiles = new SelectList(listProfiles, "Id", "Description", 0);
            ViewBag.ContactTypes = new SelectList(listContactTypes, "Id", "Description", 0);
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName1,LastName2,PhotUrl,GrossSalary,NetSalary,DailySalary,IntegratedDailySalary,Active,Created,Modified")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Active = true;
                employee.Created = DateTime.Now;
                employee.CreatedBy = null;//Comms:Modificar a que sea variable
                employee.Modified = DateTime.Now;
                employee.ModifiedBy = null;//Comms:Modificar a que sea variable
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName1,LastName2,PhotUrl,GrossSalary,NetSalary,DailySalary,IntegratedDailySalary,Active,Created,Modified")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.Modified = DateTime.Now;
                    employee.ModifiedBy = null;//Comms:Modificar a que sea variable
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }       

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        public string BuscarCodigoPostal(string zipcode)
        {
                var colonias = _context.Neighborhoods.Where(m => m.ZipCode == zipcode).Include(a=> a.City).ThenInclude(t=> t.State).ThenInclude(s=> s.Country).ToList();
                var query = (
                                from c in colonias
                                select new
                                {
                                    idColonia = c.Id,
                                    Colonia = c.Description,
                                    Municipio = c.City?.Description,
                                    Estado = c.City?.State?.Description,
                                    Pais = c.City?.State?.Country?.Description
                                }
                    ).ToList();

                return JsonConvert.SerializeObject(query);
        }
    }
}
