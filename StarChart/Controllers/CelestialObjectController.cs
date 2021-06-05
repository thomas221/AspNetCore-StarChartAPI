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
