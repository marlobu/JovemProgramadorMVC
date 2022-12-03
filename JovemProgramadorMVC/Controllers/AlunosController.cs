using JovemProgramadorMVC.Data.Repositorio.Interface;
using JovemProgramadorMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JovemProgramadorMVC.Controllers
{
    public class AlunosController : Controller
    {
        private readonly IAlunoRepositorio _alunoRepositorio;
        private readonly IConfiguration _configuration;
        public AlunosController(IAlunoRepositorio alunoRepositorio,IConfiguration configuration)
        {
            _alunoRepositorio = alunoRepositorio;
            _configuration = configuration;

        }
        public IActionResult Index()
        {
            try
            {
                var alunos = _alunoRepositorio.BuscarAlunos();
                return View(alunos);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Erro na conexão com o banco de dados";
                return View();
            }
           
        }
        public IActionResult Adicionar()
        {
            return View();
        }
      

        public IActionResult Editar(int id)
        {
            var aluno =_alunoRepositorio.BuscarId(id);
            


            return View(aluno);
            
       
        }
        public IActionResult Excluir(AlunoModel aluno)
        {
            try
            {
                _alunoRepositorio.Excluir(aluno);
                TempData["MensagemSucesso"] = "Aluno excluído com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Erro ao excluir aluno!";
                return View(aluno);
            }
          
        }
        public IActionResult AlterarAluno(AlunoModel aluno)
        {
            try
            {
                _alunoRepositorio.EditarAluno(aluno);
                TempData["MensagemSucesso"] = "Aluno alterado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Erro ao alterar o aluno";
                return View();
            }
            

        }
        public IActionResult InserirAluno(AlunoModel alunos)
        {
            try
            {
                 _alunoRepositorio.InserirAluno(alunos);
            TempData["MensagemSucesso"] = "Aluno adicionado com sucesso!";
            return RedirectToAction("Index");

            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Erro ao inserir o aluno!";
                return View();
            }
           
        }
       public async Task<IActionResult> BuscarEndereco(string cep)

        {
            try
            {
                cep = cep.Replace("-", "");

                EnderecoModel enderecoModel = new();

                using var client = new HttpClient();

                var result = await client.GetAsync(_configuration.GetSection("ApiCep")["BaseUrl"] + cep + "/json");

                if (result.IsSuccessStatusCode)
                {
                    enderecoModel = JsonSerializer.Deserialize<EnderecoModel>(
                        await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { });
                }
                return View("Endereco", enderecoModel);
            }
            catch (Exception)
            {

                throw;
            }
         
           
        }

      
    }
}
