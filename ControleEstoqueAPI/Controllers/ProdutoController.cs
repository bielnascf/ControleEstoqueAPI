using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueAPI.Data;
using ControleEstoqueAPI.Models;

namespace ControleEstoqueAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly DataContext _context;

        public ProdutoController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Listar()
        {
          if (_context.Produtos == null)
          {
              return NotFound();
          }
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/Produto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> ListarPorId(int id)
        {
          if (_context.Produtos == null)
          {
              return NotFound();
          }
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // PUT: api/Produto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Alterar(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Produto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produto>> Cadastrar([FromServices] DataContext context, [FromBody] Produto body)
        {
          if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = new Produto()
            {
                NomeProduto = body.NomeProduto,
                DataValidade = body.DataValidade,
                Quantidade = body.Quantidade,
                Lote = body.Lote,
                TipoProduto = body.TipoProduto,
            };

            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            return body;
        }

        // DELETE: api/Produto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
