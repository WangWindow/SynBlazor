using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace SynBlazor.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Created;

        public string PdfFileName { get; set; } = string.Empty;

        public string PdfFilePath { get; set; } = string.Empty;

        public string? SlidevMarkdown { get; set; }

        public string? PptxFileName { get; set; }

        public string? PptxFilePath { get; set; }

        public string? ErrorMessage { get; set; }

        public ProcessingProgress Progress { get; set; } = new();
    }

    public class ProcessingProgress
    {
        public int PdfAnalysisProgress { get; set; } = 0;
        public int ContentProcessingProgress { get; set; } = 0;
        public int SlidevGenerationProgress { get; set; } = 0;
        public int PptxConversionProgress { get; set; } = 0;

        public int TotalProgress => (PdfAnalysisProgress + ContentProcessingProgress +
                                   SlidevGenerationProgress + PptxConversionProgress) / 4;
    }

    public enum ProjectStatus
    {
        Created,
        PdfUploaded,
        Analyzing,
        ProcessingContent,
        GeneratingSlidev,
        ConvertingToPptx,
        Completed,
        Failed
    }

    public class UploadRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty; [Required]
        public IBrowserFile PdfFile { get; set; } = null!;
    }

    public class MenuButtonItem
    {
        public string Text { get; set; } = string.Empty;
        public Action? OnClick { get; set; }
        public string IconName { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;
    }

    public class CreateProjectData
    {
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
    }
}
