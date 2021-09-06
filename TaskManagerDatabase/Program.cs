using System;
using System.Data.SqlClient;

namespace TaskManagerDatabase
{
    class Program
    {
        private static SqlConnection extConnection;
        internal static SqlConnection ExtConnection
        {
            get
            {
                return extConnection = new SqlConnection("Data Source=task-manager.ciwlibbncgvd.us-east-2.rds.amazonaws.com,1433;Database=TaskManager;Integrated Security=false;User ID=sealater;Password=cjcU6srsAqit3oYnHH4R;");
            }
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
        }
    }
}
