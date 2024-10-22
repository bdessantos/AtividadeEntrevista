using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public long Incluir(Beneficiario beneficiario, long idCliente)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.Incluir(beneficiario, idCliente);
        }

        public List<Beneficiario> ListarPeloIdCliente(long id)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.ListarPeloIdCliente(id);
        }

        public bool VerificarExistencia(string CPF)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            return ben.VerificarExistencia(CPF);
        }
    }
}
