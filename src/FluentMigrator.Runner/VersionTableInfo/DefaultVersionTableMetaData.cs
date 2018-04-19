#region License

//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
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

using FluentMigrator.Expressions;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;

namespace FluentMigrator.Runner.VersionTableInfo
{
    public class DefaultVersionTableMetaData : IVersionTableMetaData, ISchemaExpression
    {
        [Obsolete("Use dependency inject")]
        public DefaultVersionTableMetaData()
            : this(string.Empty)
        {
        }

        [Obsolete("Use dependency inject")]
        public DefaultVersionTableMetaData(string schemaName)
        {
            SchemaName = schemaName ?? string.Empty;
        }

        public DefaultVersionTableMetaData(IConventionSet conventionSet, IRunnerContext runnerContext)
        {
#pragma warning disable 618
            ApplicationContext = runnerContext.ApplicationContext;
#pragma warning restore 618
            conventionSet.SchemaConvention?.Apply(this);
        }

        /// <summary>
        /// Provides access to <code>ApplicationContext</code> object.
        /// </summary>
        /// <remarks>
        /// ApplicationContext value is set by FluentMigrator immediately after instantiation of a class
        /// implementing <code>IVersionTableMetaData</code> and before any of properties of <code>IVersionTableMetaData</code>
        /// is called. Properties can use <code>ApplicationContext</code> value to implement context-depending logic.
        /// </remarks>
        [Obsolete("Use dependency injection to get the IRunnerContext")]
        public object ApplicationContext { get; set; }

        public virtual string SchemaName { get; set; }

        public virtual string TableName => "VersionInfo";

        public virtual string ColumnName => "Version";

        public virtual string UniqueIndexName => "UC_Version";

        public virtual string AppliedOnColumnName => "AppliedOn";

        public virtual string DescriptionColumnName => "Description";

        public virtual bool OwnsSchema => true;
    }
}
