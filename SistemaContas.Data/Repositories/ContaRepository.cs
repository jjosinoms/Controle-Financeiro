using Dapper;
using SistemaContas.Data.Configurations;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Data.Repositories
{
    public class ContaRepository : IRepository<Conta>
    {
        public void Add(Conta entity)
        {
            var query = @"
                INSERT INTO CONTA (ID,NOME, VALOR, DATA, OBSERVACOES, TIPO, IDUSUARIO, IDCATEGORIA)
                VALUES (@Id, @Nome, @Valor, @Data, @Observacoes, @Tipo, @IdUsuario, @IdCategoria)
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Update(Conta entity)
        {
            var query = @"
                UPDATE CONTA SET NOME = @Nome, VALOR = @Valor, DATA= @Data, OBSERVACOES = @Observacoes,
                TIPO = @Tipo, IDCATEGORIA = @IdCategoria
                WHERE ID = @Id
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public void Delete(Conta entity)
        {
            var query = @"
                DELETE FROM CONTA WHERE ID = @Id
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                connection.Execute(query, entity);
            }
        }
        public List<Conta> GetAll()
        {
            var query = @"
                SELECT * FROM CONTA ORDER BY NOME
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query<Conta>(query).ToList();
            }
        }

        public List<Conta> GetByUsuario(Guid idUsuario)
        {
            var query = @"
                SELECT * FROM CONTA WHERE IDUSUARIO = @idUsuario ORDER BY NOME    
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query<Conta>(query, new { idUsuario }).ToList();
            }
        }

        public Conta? Get(Guid id)
        {
            var query = @"
                SELECT * FROM CONTA
                WHERE ID = @Id
            ";
            using (var connection = new SqlConnection(SqlServerConfiguration.ConnectionString))
            {
                return connection.Query<Conta>(query, new { id }).FirstOrDefault();
            }
        }
    }
}
