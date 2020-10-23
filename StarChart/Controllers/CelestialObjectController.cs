using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject) 
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject) 
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            result.Name = celestialObject.Name;
            result.OrbitalPeriod = celestialObject.OrbitalPeriod;
            result.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name) 
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            result.Name = name;

            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var result = _context.CelestialObjects.Where(w => w.Id == id);

            if (!result.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(result);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
