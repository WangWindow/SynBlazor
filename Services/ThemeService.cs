using Microsoft.JSInterop;

namespace SynBlazor.Services;

public enum ThemeMode
{
    Light,
    Dark,
    Tiger,
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
    private ThemeMode _currentTheme = ThemeMode.Light;

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
                _currentTheme = ThemeMode.Light;
            }

            await ApplyThemeAsync(_currentTheme);
        }
        catch
        {
            // 如果出错，使用默认主题
            _currentTheme = ThemeMode.Light;
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
                ThemeMode.Tiger => "tiger",
                ThemeMode.System => await GetSystemThemeAsync(),
                _ => "light"
            };

            // 更安全的方式设置主题类
            await _jsRuntime.InvokeVoidAsync("applyTheme", themeClass);
        }
        catch
        {
            // 如果自定义函数不可用，回退到直接DOM操作
            var themeClass = theme switch
            {
                ThemeMode.Light => "light",
                ThemeMode.Dark => "dark",
                ThemeMode.Tiger => "tiger",
                ThemeMode.System => await GetSystemThemeAsync(),
                _ => "light"
            };

            try
            {
                // 移除所有主题类并应用新主题
                await _jsRuntime.InvokeVoidAsync("eval",
                    $@"
                        const html = document.documentElement;
                        const body = document.body;

                        // 移除现有主题类
                        html.classList.remove('light', 'dark', 'tiger');
                        body.classList.remove('light', 'dark', 'tiger');

                        // 添加新主题类（如果不是light则添加，因为light是默认主题）
                        if ('{themeClass}' !== 'light') {{
                            html.classList.add('{themeClass}');
                            body.classList.add('{themeClass}');
                        }}
                        ");
            }
            catch
            {
                // 静默处理JS错误
            }
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
