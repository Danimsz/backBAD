using BadServer.DataBase;
using BadServer.DataBase.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace BadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private MyDbContext _dbContext;

        public UsuarioController(MyDbContext dbcontext)
        {
            _dbContext = dbcontext;

        }


        // devuelve todos los Clientes 

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            return _dbContext.Cliente;

        }

        // POST: api/Usuario/Signup
        [HttpPost("Signup")]
        public void UserRegister([FromBody] Cliente cliente)
        {




       

        }
        // [HttpPost("Register")]


    }



}
    
  