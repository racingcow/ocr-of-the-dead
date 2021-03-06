﻿using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Microsoft.Extensions.OptionsModel;
using Dapper;
using ocr_api.Models;
using ocr_api.Helpers;

namespace ocr_api.Data
{
    public interface IWordRepository
    {
        Word[] Get(int count, int minConfPct, int maxConfPct, int minLength);
        void AddCorrection(Word word);
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

        public void AddCorrection(Word word)
        {
            dynamic keyObj = word.FileKey.KeyToProps();
            using (var conn = new SqlConnection(m_settings.IndexConnString))
            {
                conn.Execute("insert into fotnindex.dbo.formsprocessingsuggestions values (@RepoId, @BatchId, @Sequence, @FieldId, @SuggestedValue)",
                    new { keyObj.RepoId, keyObj.BatchId, keyObj.Sequence, keyObj.FieldId, SuggestedValue = word.Text });
            }
        }
    }
}
