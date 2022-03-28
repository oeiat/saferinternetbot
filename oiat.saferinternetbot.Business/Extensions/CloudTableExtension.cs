using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Extensions
{
    public static class CloudTableExtension
    {
        public static async Task<IList<T>> ExecuteQueryAsync<T>(this CloudTable table, TableQuery<T> query, CancellationToken ct = default(CancellationToken), Action<IList<T>> onProgress = null) where T : ITableEntity, new()
        {
            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<T> seg = await table.ExecuteQuerySegmentedAsync<T>(query, token);
                token = seg.ContinuationToken;
                items.AddRange(seg);
                onProgress?.Invoke(items);

            } while (token != null && !ct.IsCancellationRequested);

            return items;
        }

        public static async Task<IList<DynamicTableEntity>> ExecuteQueryAsync(this CloudTable table, TableQuery query, CancellationToken ct = default(CancellationToken), Action<IList<DynamicTableEntity>> onProgress = null)
        {

            var items = new List<DynamicTableEntity>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<DynamicTableEntity> seg = await table.ExecuteQuerySegmentedAsync(query, token);
                token = seg.ContinuationToken;
                items.AddRange(seg);
                onProgress?.Invoke(items);

            } while (token != null && !ct.IsCancellationRequested);

            return items;
        }
    }
}
