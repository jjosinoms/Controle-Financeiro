using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Data.Configurations
{
    public class SqlServerConfiguration
    {
        //    public static string ConnectionString
        //      => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BD_SistemaContas;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static string ConnectionString
            => @"Data Source=SQL8004.site4now.net;Initial Catalog=db_a94bdf_bdsistemacontas;User Id=db_a94bdf_bdsistemacontas_admin;Password=j05JMS07";

    }
}


