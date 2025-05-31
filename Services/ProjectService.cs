using SynBlazor.Models;

namespace SynBlazor.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task<Project> CreateProjectAsync(string title, string description);
    Task<Project> UpdateProjectAsync(Project project);
    Task<bool> DeleteProjectAsync(int id);
    Task<Project> UploadPdfAsync(int projectId, byte[] pdfData, string fileName);
    Task<Project> StartProcessingAsync(int projectId);
    Task<byte[]?> DownloadPptxAsync(int projectId);
}

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;
    private static readonly List<Project> _mockProjects = new();

    public ProjectService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // 初始化一些模拟数据用于开发
        if (!_mockProjects.Any())
        {
            _mockProjects.AddRange(new[]
            {
                    new Project
                    {
                        Id = 1,
                        Title = "深度学习在计算机视觉中的应用综述",
                        Description = "分析深度学习技术在图像识别、目标检测等领域的最新进展",
                        CreatedAt = DateTime.Now.AddDays(-5),
                        Status = ProjectStatus.Completed,
                        PdfFileName = "cv_review.pdf",
                        PptxFileName = "cv_review_slides.pptx",
                        Progress = new ProcessingProgress
                        {
                            PdfAnalysisProgress = 100,
                            ContentProcessingProgress = 100,
                            SlidevGenerationProgress = 100,
                            PptxConversionProgress = 100
                        }
                    },
                    new Project
                    {
                        Id = 2,
                        Title = "自然语言处理技术发展现状",
                        Description = "NLP领域大语言模型的发展历程和未来趋势",
                        CreatedAt = DateTime.Now.AddDays(-2),
                        Status = ProjectStatus.Analyzing,
                        PdfFileName = "nlp_review.pdf",
                        Progress = new ProcessingProgress
                        {
                            PdfAnalysisProgress = 75,
                            ContentProcessingProgress = 30,
                            SlidevGenerationProgress = 0,
                            PptxConversionProgress = 0
                        }
                    }
                });
        }
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        // TODO: 实现真实的API调用
        await Task.Delay(100); // 模拟网络延迟
        return _mockProjects.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        // TODO: 实现真实的API调用
        await Task.Delay(50);
        return _mockProjects.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Project> CreateProjectAsync(string title, string description)
    {
        // TODO: 实现真实的API调用
        await Task.Delay(100);

        var project = new Project
        {
            Id = _mockProjects.Count + 1,
            Title = title,
            Description = description,
            CreatedAt = DateTime.Now,
            Status = ProjectStatus.Created
        };

        _mockProjects.Add(project);
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        // TODO: 实现真实的API调用
        await Task.Delay(50);

        var existingProject = _mockProjects.FirstOrDefault(p => p.Id == project.Id);
        if (existingProject != null)
        {
            existingProject.Title = project.Title;
            existingProject.Description = project.Description;
            existingProject.UpdatedAt = DateTime.Now;
            existingProject.Status = project.Status;
            existingProject.Progress = project.Progress;
            // 更新其他属性...
        }

        return project;
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        // TODO: 实现真实的API调用
        await Task.Delay(50);

        var project = _mockProjects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            _mockProjects.Remove(project);
            return true;
        }
        return false;
    }

    public async Task<Project> UploadPdfAsync(int projectId, byte[] pdfData, string fileName)
    {
        // TODO: 实现真实的文件上传API调用
        await Task.Delay(200);

        var project = _mockProjects.FirstOrDefault(p => p.Id == projectId);
        if (project != null)
        {
            project.PdfFileName = fileName;
            project.PdfFilePath = $"/uploads/pdf/{fileName}";
            project.Status = ProjectStatus.PdfUploaded;
            project.UpdatedAt = DateTime.Now;
        }

        return project!;
    }

    public async Task<Project> StartProcessingAsync(int projectId)
    {
        // TODO: 实现真实的处理启动API调用
        await Task.Delay(100);

        var project = _mockProjects.FirstOrDefault(p => p.Id == projectId);
        if (project != null)
        {
            project.Status = ProjectStatus.Analyzing;
            project.UpdatedAt = DateTime.Now;
        }

        return project!;
    }

    public async Task<byte[]?> DownloadPptxAsync(int projectId)
    {
        // TODO: 实现真实的文件下载API调用
        await Task.Delay(100);

        var project = _mockProjects.FirstOrDefault(p => p.Id == projectId);
        if (project?.Status == ProjectStatus.Completed && !string.IsNullOrEmpty(project.PptxFilePath))
        {
            // 返回模拟的文件数据
            return new byte[1024]; // 模拟数据
        }

        return null;
    }
}
