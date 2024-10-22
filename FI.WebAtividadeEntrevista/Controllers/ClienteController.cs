using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing;
using FI.WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;

                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.CPF = RemoverCaracteresEspeciais(model.CPF);

                if (!CpfValido(model.CPF))
                {
                    var erros = new List<string>() { "CPF inválido" };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(string.Join(Environment.NewLine, erros));
                }

                if (bo.VerificarExistencia(model.CPF))
                {
                    var erros = new List<string>() { "O CPF informado já consta no banco de dados" };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(string.Join(Environment.NewLine, erros));
                }

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                var boBeneficiario = new BoBeneficiario();

                var errosBeneficiarios = new List<string>();

                foreach (var beneficiario in model.Beneficiarios)
                {
                    beneficiario.CPF = RemoverCaracteresEspeciais(beneficiario.CPF);

                    if (!CpfValido(beneficiario.CPF))
                    {
                        errosBeneficiarios.Add(string.Format("CPF do beneficiario {0} e invalido", beneficiario.Nome));
                        continue;
                    }

                    if (boBeneficiario.VerificarExistencia(beneficiario.CPF))
                    {
                        errosBeneficiarios.Add(string.Format("O CPF informado para o beneficiario {0} ja consta no banco de dados", beneficiario.Nome));
                        continue;
                    }
                }

                if (errosBeneficiarios.Count > 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(string.Join(Environment.NewLine, errosBeneficiarios));
                }

                foreach (var beneficiario in model.Beneficiarios)
                {
                    boBeneficiario.Incluir(new Beneficiario()
                    {
                        Nome = beneficiario.Nome,
                        CPF = beneficiario.CPF
                    }, model.Id);
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                model.CPF = RemoverCaracteresEspeciais(model.CPF);

                if (!CpfValido(model.CPF))
                {
                    var erros = new List<string>() { "CPF inválido" };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(string.Join(Environment.NewLine, erros));
                }

                if (bo.VerificarExistenciaParaUmIdDiferente(model.CPF, model.Id))
                {
                    var erros = new List<string>() { "O CPF informado já consta no banco de dados" };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(string.Join(Environment.NewLine, erros));
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            BoBeneficiario boBeneficiario = new BoBeneficiario();
            var beneficiarios = boBeneficiario.ListarPeloIdCliente(cliente.Id);

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = beneficiarios.Select(beneficiario => CarregarBeneficiarioModel(beneficiario)).ToList()
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private bool CpfValido(string cpf)
        {
            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se todos os dígitos são iguais, como "111.111.111-11", que é inválido
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula os primeiros 9 dígitos do CPF
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            // Primeiro dígito verificador
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            // Segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            // Verifica se os dois dígitos verificadores estão corretos
            return cpf.EndsWith(digito);
        }

        public static string RemoverCaracteresEspeciais(string cpf) => Regex.Replace(cpf, @"[^\d]", "");

        private BeneficiarioModel CarregarBeneficiarioModel(Beneficiario beneficiario) => 
            new BeneficiarioModel()
            {
                CPF = beneficiario.CPF,
                Nome = beneficiario.Nome,
                Id = beneficiario.Id,
            };
    }
}