using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace FI.AtividadeEntrevista.DAL.Beneficiarios
{
    internal class DaoBeneficiario : AcessoDados
    {
        internal long Incluir(Beneficiario beneficiario, long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID_CLIENTE", idCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);

            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal List<Beneficiario> ListarPeloIdCliente(long id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", id));

            DataSet ds = base.Consultar("FI_SP_ListarPeloIdCliente", parametros);

            if (ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return new List<Beneficiario>();

            List<Beneficiario> lista = new List<Beneficiario>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DML.Beneficiario ben = new DML.Beneficiario();
                ben.Id = row.Field<long>("Id");
                ben.CPF = row.Field<string>("CPF");
                ben.Nome = row.Field<string>("NOME");
                lista.Add(ben);
            }

            return lista;
        }

        internal bool VerificarExistencia(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }
    }
}
