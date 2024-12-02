using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.CORE.Models;
using EmployeeManagement.PL.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.PL.Areas.Management.Controllers;
[Area("Management")]
public class OrderController : Controller
{
    private readonly IGenericCRUDService _service;
    IWebHostEnvironment _webHostEnvironment;
    
    public OrderController(IGenericCRUDService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Order> order = await _service.GetAllAsync<Order>();
        List<Order> activeOrders = order.Where(s => !s.IsDeleted).ToList();
        foreach (Order appointment in activeOrders)
        {
            appointment.Service = await _service.GetByIdAsync<Service>(appointment.ServiceId);
            appointment.Master = await _service.GetByIdAsync<Master>(appointment.MasterId);
        }
        
        return View(activeOrders);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Order> order = await _service.GetAllAsync<Order>();
        List<Order> deactiveOrders = order.Where(s => s.IsDeleted).ToList();
        foreach (Order appointment in deactiveOrders)
        {
            appointment.Service = await _service.GetByIdAsync<Service>(appointment.ServiceId);
            appointment.Master = await _service.GetByIdAsync<Master>(appointment.MasterId);
        }
        return View(deactiveOrders); 
    }

    public async Task<IActionResult> Details(int id)
    { 
        Order appointment = await _service.GetByIdAsync<Order>(id);
        appointment.Service = await _service.GetByIdAsync<Service>(appointment.ServiceId);
        appointment.Master = await _service.GetByIdAsync<Master>(appointment.MasterId);
       return View(appointment);
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        var model = new OrderVM
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }),
            Masters = Task.Run(() => _service.GetAllAsync<Master>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            })
        };
        
        return View(model);
    }
    
    [HttpPost]

    public async Task<IActionResult> Create(OrderVM orderVM)
    {
        var model = new OrderVM
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }),
            Masters = Task.Run(() => _service.GetAllAsync<Master>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            })
        };
        
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid slider item");
            return View(model);
        }
        
        Order order = new Order()
        {
           ClientName = orderVM.Order.ClientName,
           ClientSurName = orderVM.Order.ClientSurName,
           ClientPhoneNumber = orderVM.Order.ClientPhoneNumber,
           ClientEmail = orderVM.Order.ClientEmail,
           Problem = orderVM.Order.Problem,
           IsActive = orderVM.Order.IsActive,
           ServiceId = orderVM.Order.ServiceId,
           MasterId = orderVM.Order.MasterId
        };
        
        await _service.CreateAsync(order);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Order thisOrder = await _service.GetByIdAsync<Order>(id);
        var model = new OrderVM()
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }),
            Masters = Task.Run(() => _service.GetAllAsync<Master>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }),
            Order = thisOrder
        };
        return View(model);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, OrderVM orderVM)
    {
        if (!ModelState.IsValid)
        {
            Order thisOrder = await _service.GetByIdAsync<Order>(id);
            var model = new OrderVM()
            {
                Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }),
                Masters = Task.Run(() => _service.GetAllAsync<Master>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName + " " + x.LastName
                }),
                Order = thisOrder
            }; 
            return View(model);
        }

        var existingOrder = await _service.GetByIdAsync<Order>(id);
        if (existingOrder == null)
        {
            return NotFound();
        }
    
        existingOrder.ClientName = orderVM.Order.ClientName;
        existingOrder.ClientSurName = orderVM.Order.ClientSurName;
        existingOrder.ClientPhoneNumber = orderVM.Order.ClientPhoneNumber;
        existingOrder.ClientEmail = orderVM.Order.ClientEmail;
        existingOrder.Problem = orderVM.Order.Problem;
        existingOrder.IsActive = orderVM.Order.IsActive;
        existingOrder.ServiceId = orderVM.Order.ServiceId;
        existingOrder.MasterId = orderVM.Order.MasterId;
        

        await _service.UpdateAsync(existingOrder);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Order>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Order>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Order>(id);
        return RedirectToAction(nameof(Trash));
    }
}