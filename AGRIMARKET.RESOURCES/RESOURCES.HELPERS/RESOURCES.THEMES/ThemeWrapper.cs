using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AGRIMARKET.RESOURCES.RESOURCES.HELPERS.RESOURCES.THEMES;

public class ThemeWrapper
{
    private readonly ILogger<ThemeWrapper> logger;
    public ThemeWrapper(ILogger<ThemeWrapper> log)
    {
        logger = log;
    }
    public bool IsDark { get; private set; }
    public event Action? OnChange;

    public void Toggle()
    {
        IsDark = !IsDark;
        Notify();

        logger.LogInformation("Theme toggled. IsDark: {IsDark}", IsDark);
    }

    public void Set(bool dark)
    {
        IsDark = dark;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();

}
