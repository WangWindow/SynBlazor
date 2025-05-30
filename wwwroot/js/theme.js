// Theme management JavaScript functions
window.applyTheme = function (themeName) {
    const html = document.documentElement;
    const body = document.body;

    // Remove all existing theme classes
    html.classList.remove('light', 'dark', 'tiger');
    body.classList.remove('light', 'dark', 'tiger');

    // Add the new theme class (light is default, no class needed)
    if (themeName !== 'light') {
        html.classList.add(themeName);
        body.classList.add(themeName);
    }

    // Store theme preference
    localStorage.setItem('theme', themeName);

    console.log(`Theme applied: ${themeName}`);
};

window.getSystemTheme = function () {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return 'dark';
    }
    return 'light';
};

// Initialize theme on page load
document.addEventListener('DOMContentLoaded', function () {
    // Try to get saved theme, otherwise use system theme
    const savedTheme = localStorage.getItem('theme');

    if (savedTheme && savedTheme !== 'system') {
        applyTheme(savedTheme);
    } else {
        // Use system theme if no saved theme or if system mode is selected
        const systemTheme = getSystemTheme();
        applyTheme(systemTheme);
    }

    // Listen for system theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function (e) {
        const currentTheme = localStorage.getItem('theme');
        if (!currentTheme || currentTheme === 'system') {
            applyTheme(e.matches ? 'dark' : 'light');
        }
    });
});
