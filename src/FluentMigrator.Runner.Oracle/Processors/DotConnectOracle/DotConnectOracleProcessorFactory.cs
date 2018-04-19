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

using System;

using FluentMigrator.Runner.Generators.Oracle;

namespace FluentMigrator.Runner.Processors.DotConnectOracle
{
    public class DotConnectOracleProcessorFactory : MigrationProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        [Obsolete]
        public DotConnectOracleProcessorFactory()
            : this(serviceProvider: null)
        {
        }

        public DotConnectOracleProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IMigrationProcessor Create(string connectionString, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new DotConnectOracleDbFactory(_serviceProvider);
            var connection = factory.CreateConnection(connectionString);
            return new DotConnectOracleProcessor(connection, new OracleGenerator(), announcer, options, factory);
        }
    }
}
