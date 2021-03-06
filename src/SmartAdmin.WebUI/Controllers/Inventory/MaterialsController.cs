using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SideXC.WebUI.Data;
using SideXC.WebUI.Models.Inventory;

namespace SideXC.WebUI.Controllers.Inventory
{
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materials.Include(u => u.UnitMeassure).Include(i => i.MaterialType).Include(s => s.Supplier).ToListAsync());
        }

        public IActionResult Create()
        {
            var listUnitMeassures = _context.UnitMeassures.Where(c => c.Active == true).ToList();
            var listMaterialTypes = _context.MaterialTypes.Where(c => c.Active == true).ToList();
            var listSuppliers = _context.Suppliers.Where(c => c.Active == true).ToList();
            ViewBag.UnitMeassures = new SelectList(listUnitMeassures, "Id", "Description");
            ViewBag.MaterialTypes = new SelectList(listMaterialTypes, "Id", "Description");
            ViewBag.Suppliers = new SelectList(listSuppliers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StandardCost,MOQ,LeadTime,Active,Created,Modified")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StandardCost,MOQ,LeadTime,Active,Created,Modified")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            return View(material);
        }

        public string GetMaterialTypeId(string stdcost)
        {
            var dblStdCost = decimal.Parse(stdcost);
            var item = _context.MaterialTypes.FirstOrDefault(m =>  m.MinimunRange >= dblStdCost && m.MaximunRange <= dblStdCost);

            return JsonConvert.SerializeObject(item);
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
