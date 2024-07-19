using MemeRepository.Db.Models;
using MemeSource.DAL.Interfaces;
using MemeSource.Interfaces;
using MemeSource.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeSource.Controllers;

public class SystemPropertyController : Controller
{
    private readonly ISystemPropertyRepository _repository;

    public SystemPropertyController(ISystemPropertyRepository repository)
    {
        _repository = repository;
    }

    // GET: SystemProperty
    public async Task<IActionResult> Index()
    {
        var properties = await _repository.GetAllAsync();
        return View(properties);
    }

    // GET: SystemProperty/Details/1
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var systemProperty = await _repository.GetByIdAsync(id.Value);
        if (systemProperty == null)
        {
            return NotFound();
        }

        return View(systemProperty);
    }

    // GET: SystemProperty/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SystemProperty/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,SP_Name,Parameter1,Parameter2,Parameter3,Parameter4,CreatedDate,UpdatedDate")] SystemProperty systemProperty)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(systemProperty);
            return RedirectToAction(nameof(Index));
        }
        return View(systemProperty);
    }

    // GET: SystemProperty/Edit/1
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var systemProperty = await _repository.GetByIdAsync(id.Value);
        if (systemProperty == null)
        {
            return NotFound();
        }
        return View(systemProperty);
    }

    // POST: SystemProperty/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("ID,SP_Name,Parameter1,Parameter2,Parameter3,Parameter4,CreatedDate,UpdatedDate")] SystemProperty systemProperty)
    {
        if (id != systemProperty.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _repository.UpdateAsync(systemProperty);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(systemProperty.ID))
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
        return View(systemProperty);
    }

    // GET: SystemProperty/Delete/1
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var systemProperty = await _repository.GetByIdAsync(id.Value);
        if (systemProperty == null)
        {
            return NotFound();
        }

        return View(systemProperty);
    }

    // POST: SystemProperty/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

}