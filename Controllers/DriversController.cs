using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;
using TransportApi.Models; // Make sure to use the correct namespace

[ApiController]
[Route("api/drivers")]
public class DriversController : ControllerBase
{
    private readonly IMongoCollection<Driver> _driversCollection;

    public DriversController(IMongoDbService mongoDbService)
    {
        _driversCollection = mongoDbService.GetCollection<Driver>("drivers");
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] Driver newDriver)
    {
        int x = 10;
        var y = 20;
        string name = "John";
        Console.WriteLine(JsonSerializer.Serialize(newDriver));
        if (newDriver == null)
        {
            return BadRequest("Invalid driver data.");
        }

        // The InsertOneAsync method will automatically generate a new _id for the newDriver object
        await _driversCollection.InsertOneAsync(newDriver);

        // Return a 201 Created status with the newly created driver object
        return CreatedAtAction(
            nameof(GetDriverById),
            new { id = newDriver.Id },
            newDriver
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> GetDriverById(string id)
    {
        var driver = await _driversCollection.Find(d => d.Id == id).FirstOrDefaultAsync();

        if (driver == null)
        {
            return NotFound();
        }

        return Ok(driver);
    }
}