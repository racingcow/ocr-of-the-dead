using System.Collections.Generic;
using System.Linq;
using ocr_api.Models;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.OptionsModel;
using ocr_api.Helpers;
using ocr_api.Options;

namespace ocr_api.Data
{
    public class WordRepository : IWordRepository
    {
        private readonly Options.Options m_options;

        public WordRepository(IOptions<Options.Options> dataOptionsAccessor)
        {
            m_options = dataOptionsAccessor.Value;
        }

        public Word[] Get(int count, int minConfPct, int maxConfPct, int minLength)
        {
            var words = new List<Word>();

            using (var conn = new SqlConnection(m_options.IndexConnString))
            {
                var rows = conn.Query(
                    "select top(@count) RepoId, BatchId, [Sequence], FieldId, Confidence, ExtractedValue, x, y, w, h from fotnindex.dbo.formsprocessingexceptions where confidence between @minConfPct and @maxConfPct and len(ExtractedValue) > @minLength",
                    new { count, minConfPct, maxConfPct, minLength });

                words.AddRange(rows.Select(row => new Word
                {
                    Key = DynamicHelper.PropsToKey(row, "RepoId", "BatchId", "Sequence", "FieldId"),
                    Confidence = row.Confidence,
                    Text = row.ExtractedValue
                }));

                return words.ToArray();
            }
        }
    }
}
