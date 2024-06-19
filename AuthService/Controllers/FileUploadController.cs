using AuthService.Helpers;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private const int ChunkSize = 1024 * 1024 * 2; // 1 МБ
        private readonly AppDBContext _dbContext;
        private static readonly object _fileLock = new object(); // Lock object
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FileUploadController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("start")]
        public IActionResult StartUpload([FromForm] string fileName, [FromForm] long fileSize)
        {
            var uploadMetadata = new FileUploadMetadata
            {
                FileName = fileName,
                FileSize = fileSize,
                ChunkCount = (int)Math.Ceiling((double)fileSize / ChunkSize),
                ChunksUploaded = 0,
            };

            _dbContext.FileUploads.Add(uploadMetadata);
            _dbContext.SaveChanges();

            // Log the start of the file upload
            var eventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, "File upload started");
            eventInfo.Properties["fileName"] = fileName;
            eventInfo.Properties["fileSize"] = fileSize.ToString();
            eventInfo.Properties["uploadId"] = uploadMetadata.Id.ToString();
            _logger.Log(eventInfo);

            return Ok(new { Id = uploadMetadata.Id });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadChunk([FromForm] UploadChunkModel model)
        {
            FileUploadMetadata uploadMetadata;
            var filePath = string.Empty;
            bool isUploadCompleted = false;

            // Lock to prevent multiple accesses to the same metadata and file
            lock (_fileLock)
            {
                uploadMetadata = _dbContext.FileUploads.Find(model.Id);
                if (uploadMetadata == null)
                {
                    _logger.Warn($"Upload metadata not found for ID: {model.Id}");
                    return NotFound();
                }

                filePath = Path.Combine("uploads", uploadMetadata.FileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure directory exists

                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, useAsync: true))
                {
                    fileStream.Position = (long)model.ChunkIndex * ChunkSize;
                    using (var stream = model.Chunk.OpenReadStream())
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                uploadMetadata.ChunksUploaded++;
                _dbContext.FileUploads.Update(uploadMetadata);

                // Check if all chunks are uploaded
                if (uploadMetadata.ChunksUploaded == uploadMetadata.ChunkCount)
                {
                    isUploadCompleted = true;
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("end")]
        public async Task<IActionResult> End()
        {

            var eventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, "File upload completed");
            _logger.Log(eventInfo);

            return Ok();
        }




    }

    public class UploadChunkModel
    {
        public int Id { get; set; }
        public int ChunkIndex { get; set; }
        public IFormFile Chunk { get; set; }
    }
}
