using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class FileUploadMetadata
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public long FileSize { get; set; }
        [Required]
        public int ChunkCount { get; set; }
        [Required]
        public int ChunksUploaded { get; set; }
    }
}