using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace proiect_ProgramareAvansataPePlatforma.NET.Models
{
    public class CustomDbConfiguration : DbConfiguration


    {
        public CustomDbConfiguration()
        {


            SetHistoryContext(Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.GetType().Namespace, (connection, defaultSchema) => new CustomHistoryContext(connection, defaultSchema));


        }

    }
}