using TransportApi.Models; // Dòng này cần được thêm vào
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IMongoCollection<Vehicle> _vehiclesCollection;

    public VehiclesController(IMongoDbService mongoDbService)
    {
        // Lấy collection "Vehicles" từ database
        _vehiclesCollection = mongoDbService.GetCollection<Vehicle>("Vehicles");
    }

    [HttpGet]
    public async Task<List<Vehicle>> Get()
    {
        // Lấy tất cả tài liệu trong collection
        return await _vehiclesCollection.Find(_ => true).ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Vehicle newVehicle)
    {
        if (newVehicle == null)
        {
            return BadRequest("Invalid vehicle data.");
        }

        await _vehiclesCollection.InsertOneAsync(newVehicle);

        // Trả về trạng thái 201 Created và thông tin của đối tượng vừa tạo
        return CreatedAtAction(nameof(Get), new { id = newVehicle.Id }, newVehicle);
    }
}