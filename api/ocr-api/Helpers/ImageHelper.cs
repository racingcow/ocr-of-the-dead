using System;
using ocr_api.Models;

namespace ocr_api.Helpers
{
    public static class ImageHelper
    {
        private const string CACHE_FOLDER_NAME = "CACHE";

        public static string GetFilePath(StorageServer ss, dynamic key, dynamic imageInfo)
        {
            int repoId = Convert.ToInt32(key.RepoId);
            string batchId = key.BatchId;
            int imageId = Convert.ToInt32(imageInfo.ImageId);
            string objType = imageInfo.ObjectType;
            var ssName = ss.StorageServerName;

            var repoPath = SplitTwos(repoId.ToString().PadLeft(6, '0'));
            var batchPath = SplitTwos(batchId);
            var cacheObjFileName = $"{repoId}.{batchId}.{imageId}.{objType}";
            
            return $@"{ssName}\{CACHE_FOLDER_NAME}\{repoPath}\{batchPath}\{cacheObjFileName}";
        }

        private static string SplitTwos(object obj)
        {
            var splitee = obj.ToString();
            return $@"{splitee.Substring(0, 2)}\{splitee.Substring(2, 2)}\{splitee.Substring(4, 2)}";
        }
    }
}
