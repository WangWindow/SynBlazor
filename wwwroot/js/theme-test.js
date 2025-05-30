// Theme Testing Utilities
console.log('主题测试工具已加载');

// 测试主题切换功能
window.testThemeSwitch = function () {
    console.log('开始主题切换测试...');

    // 测试所有主题
    const themes = ['light', 'dark', 'tiger'];
    let currentIndex = 0;

    const switchToNextTheme = () => {
        const theme = themes[currentIndex];
        console.log(`切换到主题: ${theme}`);

        // 移除所有主题类
        document.documentElement.classList.remove('dark', 'tiger');

        // 应用新主题（除了light主题）
        if (theme !== 'light') {
            document.documentElement.classList.add(theme);
        }

        // 保存到localStorage
        localStorage.setItem('theme', theme);

        currentIndex = (currentIndex + 1) % themes.length;

        // 继续测试下一个主题
        if (currentIndex !== 0) {
            setTimeout(switchToNextTheme, 2000);
        } else {
            console.log('主题切换测试完成');
        }
    };

    switchToNextTheme();
};

// 验证主题变量
window.validateThemeVariables = function () {
    console.log('验证主题变量...');

    const requiredVariables = [
        '--color-bg-base',
        '--color-bg-surface',
        '--color-text-main',
        '--color-text-secondary',
        '--color-border-base',
        '--color-accent',
        '--color-error',
        '--color-error-bg'
    ];

    const computedStyle = getComputedStyle(document.documentElement);

    requiredVariables.forEach(variable => {
        const value = computedStyle.getPropertyValue(variable).trim();
        if (value) {
            console.log(`✓ ${variable}: ${value}`);
        } else {
            console.error(`✗ ${variable}: 未找到`);
        }
    });
};

// 检查主题类应用
window.checkThemeClasses = function () {
    console.log('检查主题类应用...');

    const themeElements = document.querySelectorAll('[class*="theme-"]');
    console.log(`找到 ${themeElements.length} 个使用主题类的元素`);

    // 检查一些关键元素
    const keySelectors = [
        '.bg-theme-base',
        '.text-theme-primary',
        '.text-theme-secondary',
        '.bg-theme-surface'
    ];

    keySelectors.forEach(selector => {
        const elements = document.querySelectorAll(selector);
        console.log(`${selector}: ${elements.length} 个元素`);
    });
};

// 在控制台提供快捷命令
console.log('可用的测试命令:');
console.log('- testThemeSwitch(): 自动测试所有主题切换');
console.log('- validateThemeVariables(): 验证CSS变量');
console.log('- checkThemeClasses(): 检查主题类应用');
