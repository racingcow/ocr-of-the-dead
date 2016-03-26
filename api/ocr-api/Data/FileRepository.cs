using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.OptionsModel;
using ocr_api.Models;
using Dapper;
using Microsoft.AspNet.Mvc;
using ocr_api.Helpers;
using RasterImageService;

namespace ocr_api.Data
{
    public interface IFileRepository
    {
        byte[] GetFile(string key, Rect wordBounds);
    }

    public class FileRepository : IFileRepository
    {
        private readonly Options.Settings m_settings;

        public FileRepository(IOptions<Options.Settings> dataOptionsAccessor)
        {
            m_settings = dataOptionsAccessor.Value;
        }

        public byte[] GetFile([FromQuery] string key, Rect wordBounds)
        {
            dynamic keyObj = key.KeyToProps();
            dynamic imageInfo = GetImageInfo(keyObj);
            StorageServer ss = GetStorageServer(imageInfo.StorageServerId);
            var path = ImageHelper.GetFilePath(ss, keyObj, imageInfo);
            var hint = new RenderingHint { OptimizeContentSize = true, PreferBitonalContent = true };

            var fileInfo = new ImageFileInfo
            {
                HasAnnotations = false,
                IsReindexSupported = false,
                IsRotationSupported = false,
                Rotation = 0,
                IsResequenceSupported = false
            };

            var param = new BatchImageParam
            {
                FileExtension = imageInfo.ObjectType,
                FilePath = path,
                PageFileInfo = fileInfo,
                PreferedFormat = "PNG",
                RenderingHint = hint
            };

            var rect = new BoundingRectangle
            {
                Left = wordBounds.X,
                Top = wordBounds.Y,
                Width = wordBounds.W,
                Height = wordBounds.H
            };

            var svc = new RasterImageServiceClient(RasterImageServiceClient.EndpointConfiguration.RasterServiceNetHttp);
            var result = svc.GetBatchImageSnippetAsync(param, rect).Result;
            return result;
        }

        private StorageServer GetStorageServer(int storageServerId)
        {
            using (var conn = new SqlConnection(m_settings.ConfigConnString))
            {
                return conn.Query<StorageServer>(
                    "select StorageServerId, StorageServerName, Status from fotnconfig.dbo.storageservers where storageserverid = @storageServerId",
                    new { storageServerId }).Single();
            }
        }

        private dynamic GetImageInfo(dynamic key)
        {
            using (var conn = new SqlConnection(m_settings.IndexConnString))
            {
                return conn.Query(
                    $"select ObjectType, StorageServerId, ImageId from fotnindex.dbo.i{key.RepoId} where BatchId = @BatchId and Sequence = @Sequence",
                    new { key.BatchId, key.Sequence }).Single();
            }
        }
    }
}
