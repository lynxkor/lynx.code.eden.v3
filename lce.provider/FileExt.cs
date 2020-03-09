/* action： file name：lce.provider.FileExt.cs
 * author：lynx lynx.kor@163.com @ 2019/6/5 23:12
 * copyright (c) 2019 lynxce.com
 * desc：
 * > add description for FileExt
 * revision：
 *
 */

namespace lce.provider
{
    /// <summary>
    /// 文件扩展类
    /// </summary>
    public static class FileExt
    {
        /// <summary>
        /// Get the file size.
        /// </summary>
        /// <returns>The size.</returns>
        /// <param name="contentLength">Content length.</param>
        public static string GetSize(long contentLength = 0)
        {
            var size = string.Empty;

            if (contentLength / 1024 < 1024)
            {
                size = contentLength / 1024.00 + " KB";
            }
            else if (contentLength / 1024 / 1024 < 1024)
            {
                size = (contentLength / 1024.00 / 1024.00).ToString("f2") + " M";
            }
            else if (contentLength / 1024 / 1024 / 1024 < 1024)
            {
                size = (contentLength / 1024.00 / 1024.00 / 1024.00).ToString("f2") + " G";
            }
            return size;
        }

        /// <summary>
        /// Get the type of the MIME.
        /// </summary>
        /// <returns>The MIME type.</returns>
        /// <param name="fileName">File name.</param>
        public static string GetMimeType(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName.ToLower());
            string mimeType;
            switch (ext)
            {
                case ".doc":
                case ".docx":
                case ".dot":
                    mimeType = "application/msword";
                    break;

                case ".xls":
                case ".xlsx":
                case ".xla":
                case ".xlc":
                case ".xlm":
                case ".xlt":
                case ".xlw":
                    mimeType = "application/vnd.ms-excel";
                    break;

                case ".pdf":
                    mimeType = "application/pdf";
                    break;

                case ".gif":
                    mimeType = "image/gif";
                    break;

                case ".bmp":
                    mimeType = "image/bmp";
                    break;

                case ".ico":
                    mimeType = "image/x-icon";
                    break;

                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    mimeType = "image/jpeg";
                    break;

                case ".png":
                    mimeType = "image/png";
                    break;

                case ".svg":
                    mimeType = "image/svg+xml";
                    break;

                case ".avi":
                    mimeType = "video/x-msvideo";
                    break;

                case ".wav":
                    mimeType = "audio/x-wav";
                    break;

                case ".txt":
                    mimeType = "text/plain";
                    break;

                default:
                    mimeType = string.Empty;
                    break;
            }
            return mimeType;
        }
    }
}