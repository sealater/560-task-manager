using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using TaskManagerDatabase;

namespace TaskManagerDatabaseTests
{
    [TestClass]
    public class SqlGroupRepositoryTest
    {
        const string connectionString = @"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;";

        private static string GetTestString() => Guid.NewGuid().ToString("N");

        private SqlGroupRepository repo;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeTest()
        {
            repo = new SqlGroupRepository(connectionString);

            transaction = new TransactionScope();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void FetchGroupShouldWork()
        {
            var fetch = repo.GetGroupsByUserId(3);
        }
    }
}
