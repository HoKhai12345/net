using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TransportApi.Models; // Dòng này cần được thêm vào

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly IMongoCollection<Trip> _tripsCollection;

    public TripsController(IMongoDbService mongoDbService)
    {
        _tripsCollection = mongoDbService.GetCollection<Trip>("trips");
    }

    [HttpPut("{tripId}")]
    public async Task<IActionResult> UpdateTripStatus(string tripId, [FromBody] string newStatus)
    {
        // 1. Tìm chuyến xe theo ID
        var filter = Builders<Trip>.Filter.Eq(t => t.Id, tripId);
        var update = Builders<Trip>.Update.Set(t => t.Status, newStatus);

        // 2. Thực hiện cập nhật trong MongoDB
        var result = await _tripsCollection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
        {
            return NotFound(); // Trả về 404 nếu không tìm thấy
        }

        return NoContent(); // Cập nhật thành công
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Trip>> GetTripById(string id)
    {
        var trip = await _tripsCollection.Find(t => t.Id == id).FirstOrDefaultAsync();

        if (trip == null)
        {
            return NotFound(); // Trả về 404 nếu không tìm thấy
        }

        return Ok(trip);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] Trip newTrip)
    {
        if (newTrip == null) {
            return BadRequest("Invalid trips data.");
        }

        await _tripsCollection.InsertOneAsync(newTrip);

        return CreatedAtAction(nameof(GetTripById), new { id = newTrip.Id }, newTrip);

    }
}