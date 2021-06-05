using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { Id = celestialObject.Id }, celestialObject);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool found = false;
            List<CelestialObject> theList = new List<CelestialObject>();
            foreach (CelestialObject co in _context.CelestialObjects.ToList())
            {
                if (co.Id == id || co.OrbitedObjectId == id)
                {
                    found = true;
                    theList.Add(co);
                }
            }
            if (!found)
            {
                return NotFound();
            }
            _context.CelestialObjects.RemoveRange(theList);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id,string name)
        {
            bool found = false;
            CelestialObject theCelestialObject = null;
            foreach (CelestialObject co in _context.CelestialObjects.ToList())
            {
                if (co.Id == id)
                {
                    found = true;
                    theCelestialObject = co;
                }
            }
            if (!found)
            {
                return NotFound();
            }
            theCelestialObject.Name = name;
            _context.CelestialObjects.Update(theCelestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            bool found = false;
            CelestialObject theCelestialObject = null;
            foreach (CelestialObject co in _context.CelestialObjects.ToList())
            {
                if (co.Id == id)
                {
                    found = true;
                    theCelestialObject = co;
                }
            }
            if (!found)
            {
                return NotFound();
            }
            theCelestialObject.Name = celestialObject.Name;
            theCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            theCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(theCelestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> allList = _context.CelestialObjects.ToList();
            foreach (CelestialObject co in allList)
            {
                co.Satellites = new List<CelestialObject>();
            }

            foreach (CelestialObject motherObject in allList)
            {
                foreach (CelestialObject satellite in allList)
                {
                    if (satellite.OrbitedObjectId == motherObject.Id)
                    {
                        motherObject.Satellites.Add(satellite);
                    }
                }
            }

            return Ok(allList);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> allList = _context.CelestialObjects.ToList();
            List<CelestialObject> returnList = new List<CelestialObject>();
            bool found = false;
            foreach (CelestialObject co in allList)
            {
                if (co.Name == name)
                {
                    found = true;
                    returnList.Add(co);
                }
            }
            if (!found)
            {
                return NotFound();
            }

            foreach(CelestialObject co in allList)
            {
                co.Satellites = new List<CelestialObject>();
            }

            foreach (CelestialObject motherObject in allList)
            {
                foreach (CelestialObject satellite in allList)
                {
                    if (satellite.OrbitedObjectId == motherObject.Id)
                    {
                        motherObject.Satellites.Add(satellite);
                    }
                }
            }

            return Ok(returnList);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            bool found=false;
            CelestialObject celestialObject = null;
            foreach (CelestialObject co in _context.CelestialObjects.ToList())
            {
                if(co.Id == id)
                {
                    found = true;
                    celestialObject = co;
                }
            }
            if (!found)
            {
                return NotFound();
            }

            foreach (CelestialObject co in _context.CelestialObjects.ToList())
            {
                co.Satellites = new List<CelestialObject>();
                if (co.OrbitedObjectId == celestialObject.Id)
                {
                    celestialObject.Satellites.Add(celestialObject);
                }
            }
            return Ok(celestialObject);
        }
    }
}
