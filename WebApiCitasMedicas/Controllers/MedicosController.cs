﻿using Microsoft.AspNetCore.Mvc;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    public class MedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public MedicosController(ApplicationDbContext context)
        { 
            this.dbContext = context;
        }
        [HttpGet]
        public ActionResult<List<Medico>> Get() {
            return new List<Medico>(){
             new Medico() { Id = 1, Nombre_med = "Saul" },
             new Medico() { Id = 2, Nombre_med = "Luis" },
            };
        }
    }
}
