using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Respawn.Graph;

namespace Respawn
{
    public interface IDbAdapter
    {
        string BuildTableCommandText(RespawnerOptions options);
        string BuildTemporalTableCommandText(RespawnerOptions options);
        string BuildRelationshipCommandText(RespawnerOptions options);
        string BuildDeleteCommandText(GraphBuilder builder, RespawnerOptions options);
        string BuildReseedSql(IEnumerable<Table> tablesToDelete);
        string BuildTurnOffSystemVersioningCommandText(IEnumerable<TemporalTable> tablesToTurnOffSystemVersioning);
        string BuildTurnOnSystemVersioningCommandText(IEnumerable<TemporalTable> tablesToTurnOnSystemVersioning);
#if NET48
        Task<bool> CheckSupportsTemporalTables(DbConnection connection);
#else
        Task<bool> CheckSupportsTemporalTables(DbConnection connection)
        {
            return Task.FromResult(false);
        }
#endif
        bool RequiresStatementsToBeExecutedIndividually();
    }
}
