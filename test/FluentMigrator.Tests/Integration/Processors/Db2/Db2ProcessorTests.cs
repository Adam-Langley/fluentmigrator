#region License
//
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System.Diagnostics;

using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.DB2;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.DB2;
using FluentMigrator.Tests.Helpers;

using NUnit.Framework;
using NUnit.Should;

namespace FluentMigrator.Tests.Integration.Processors.Db2
{
    [TestFixture]
    [Category("Integration")]
    [Category("Db2")]
    public class Db2ProcessorTests
    {
        static Db2ProcessorTests()
        {
            try { EnsureReference(); } catch { /* ignore */ }
        }

        #region Properties

        public System.Data.IDbConnection Connection
        {
            get; set;
        }

        public Db2DbFactory Factory
        {
            get; set;
        }

        public Db2Processor Processor
        {
            get; set;
        }

        public Db2Quoter Quoter
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        [SetUp]
        public void SetUp()
        {
            if (!IntegrationTestOptions.Db2.IsEnabled)
                Assert.Ignore();
            Factory = new Db2DbFactory(serviceProvider: null);
            Connection = Factory.CreateConnection(IntegrationTestOptions.Db2.ConnectionString);
            Quoter = new Db2Quoter();
            Processor = new Db2Processor(Connection, new Db2Generator(Quoter), new TextWriterAnnouncer(TestContext.Out), new ProcessorOptions(), Factory);
            Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            Processor?.Dispose();
        }

        [Test]
        public void CallingColumnExistsReturnsFalseIfColumnExistsInDifferentSchema()
        {
            using (var table = new Db2TestTable(Processor, "TstSchma", "ID INT"))
            {
                Processor.ColumnExists("DNE", table.Name, "ID").ShouldBeFalse();
            }
        }

        [Test]
        public void CallingConstraintExistsReturnsFalseIfConstraintExistsInDifferentSchema()
        {
            using (var table = new Db2TestTable(Processor, "TstSchma", "ID INT NOT NULL"))
            {
                table.WithUniqueConstraintOn("ID", "c1");
                Processor.ConstraintExists("DNE", table.Name, "c1").ShouldBeFalse();
            }
        }

        [Test]
        public void CallingTableExistsReturnsFalseIfTableExistsInDifferentSchema()
        {
            using (var table = new Db2TestTable(Processor, "TstSchma", "ID INT"))
            {
                Processor.TableExists("DNE", table.Name).ShouldBeFalse();
            }
        }

        private static void EnsureReference()
        {
            // This is here to avoid the removal of the referenced assembly
            Debug.WriteLine(typeof(IBM.Data.DB2.DB2Factory));
        }

        #endregion Methods
    }
}
