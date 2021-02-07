using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dictionary.Data;
using Dictionary.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dictionary.Controllers
{
    public class DefinitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DefinitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Definitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Definition.ToListAsync());
        }

        // GET: Definitions/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // PoST: Definitions/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Definition.Where( j => j.Word.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Definitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var definition = await _context.Definition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (definition == null)
            {
                return NotFound();
            }

            return View(definition);
        }

        // GET: Definitions/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Definitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Word,WordDefinition")] Definition definition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(definition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(definition);
        }

        // GET: Definitions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var definition = await _context.Definition.FindAsync(id);
            if (definition == null)
            {
                return NotFound();
            }
            return View(definition);
        }

        // POST: Definitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Word,WordDefinition")] Definition definition)
        {
            if (id != definition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(definition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefinitionExists(definition.Id))
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
            return View(definition);
        }

        // GET: Definitions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var definition = await _context.Definition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (definition == null)
            {
                return NotFound();
            }

            return View(definition);
        }

        // POST: Definitions/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var definition = await _context.Definition.FindAsync(id);
            _context.Definition.Remove(definition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefinitionExists(int id)
        {
            return _context.Definition.Any(e => e.Id == id);
        }
    }
}
