using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id) 
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            result.Satellites = _context.CelestialObjects.Where(w => w.OrbitedObjectId == id).ToList();
            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(w => w.Name == name);

            if (!result.Any())
            {
                return NotFound();
            }

            foreach (var r in result)
            {
                r.Satellites = _context.CelestialObjects.Where(w => w.OrbitedObjectId == r.OrbitedObjectId).ToList();
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();
            foreach (var r in result)
            {
                r.Satellites = _context.CelestialObjects.Where(w => w.OrbitedObjectId == r.OrbitedObjectId).ToList();
            }

            return Ok(result);
        }
    }
}
