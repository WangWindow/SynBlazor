using Microsoft.JSInterop;

namespace SynBlazor.Services
{
    public enum ThemeMode
    {
        Light,
        Dark,
        System
    }

    public interface IThemeService
    {
        ThemeMode CurrentTheme { get; }
        event Action<ThemeMode>? ThemeChanged;
        Task SetThemeAsync(ThemeMode theme);
        Task InitializeAsync();
    }
    public class ThemeService : IThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private ThemeMode _currentTheme = ThemeMode.System; // 默认主题为系统主题

        public ThemeMode CurrentTheme => _currentTheme;

        public event Action<ThemeMode>? ThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // 从localStorage读取保存的主题设置
                var savedTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
                if (!string.IsNullOrEmpty(savedTheme) && Enum.TryParse<ThemeMode>(savedTheme, out var theme))
                {
                    _currentTheme = theme;
                }
                else
                {
                    _currentTheme = ThemeMode.Light; // 默认为亮色主题
                }

                await ApplyThemeAsync(_currentTheme);
            }
            catch
            {
                // 如果出错，使用默认主题
                _currentTheme = ThemeMode.Light; // 默认为亮色主题
                await ApplyThemeAsync(_currentTheme);
            }
        }

        public async Task SetThemeAsync(ThemeMode theme)
        {
            _currentTheme = theme;

            // 保存到localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme.ToString());

            // 应用主题
            await ApplyThemeAsync(theme);

            // 通知主题变更
            ThemeChanged?.Invoke(theme);
        }
        private async Task ApplyThemeAsync(ThemeMode theme)
        {
            try
            {
                var themeClass = theme switch
                {
                    ThemeMode.Light => "light",
                    ThemeMode.Dark => "dark",
                    ThemeMode.System => await GetSystemThemeAsync(),
                    _ => "light" // 默认值
                };

                // 设置html元素的class以应用Tailwind CSS主题
                await _jsRuntime.InvokeVoidAsync("eval",
                    $"document.documentElement.className = document.documentElement.className.replace(/\\b(light|dark)\\b/g, '').trim(); document.documentElement.classList.add('{themeClass}')");

                // 同时设置body的class用于CSS变量
                await _jsRuntime.InvokeVoidAsync("eval",
                    $"document.body.className = document.body.className.replace(/\\b(light|dark)\\b/g, '').trim(); document.body.classList.add('{themeClass}')");
            }
            catch
            {
                // 静默处理JS错误
            }
        }

        private async Task<string> GetSystemThemeAsync()
        {
            try
            {
                var prefersDark = await _jsRuntime.InvokeAsync<bool>("eval",
                    "window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches");
                return prefersDark ? "dark" : "light";
            }
            catch
            {
                return "light";
            }
        }
    }
}
