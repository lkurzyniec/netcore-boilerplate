using System;
using System.Data;
using DbUp.Engine;

namespace HappyCode.NetCoreBoilerplate.Db.Scripts.Code
{
    public class S002_ModifySomeRows : IScript
    {
        public string ProvideScript(Func<IDbCommand> dbCommandFactory)
        {
            // just an example
            using var cmd = dbCommandFactory();

            cmd.CommandText = "SELECT COUNT(*) FROM Cars";
            int count = (int)cmd.ExecuteScalar();

            return @$"UPDATE Cars SET Plate = Plate + '+{count}' WHERE Id = 2";
        }
    }
}
