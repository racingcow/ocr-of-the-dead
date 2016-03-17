using System.Collections.Generic;
using System.Linq;
using ocr_api.Models;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.OptionsModel;
using ocr_api.Helpers;

namespace ocr_api.Data
{
    public interface IWordRepository
    {
        Word[] Get(int count, int minConfPct, int maxConfPct, int minLength);
    }

    public class WordRepository : IWordRepository
    {
        private readonly Options.Settings m_settings;

        public WordRepository(IOptions<Options.Settings> dataOptionsAccessor)
        {
            m_settings = dataOptionsAccessor.Value;
        }

        public Word[] Get(int count, int minConfPct, int maxConfPct, int minLength)
        {
            var words = new List<Word>();

            using (var conn = new SqlConnection(m_settings.IndexConnString))
            {
                var rows = conn.Query(
                    "select top(@count) RepoId, BatchId, [Sequence], FieldId, Confidence, ExtractedValue, x, y, w, h from fotnindex.dbo.formsprocessingexceptions where confidence between @minConfPct and @maxConfPct and len(ExtractedValue) > @minLength",
                    new { count, minConfPct, maxConfPct, minLength });

                words.AddRange(rows.Select(row => new Word
                {
                    FileKey = DynamicHelper.PropsToKey(row, "RepoId", "BatchId", "Sequence", "FieldId"),
                    Confidence = row.Confidence,
                    Text = row.ExtractedValue,
                    X = row.x,
                    Y = row.y,
                    W = row.w,
                    H = row.h
                }));

                return words.ToArray();
            }
        }
    }
}
