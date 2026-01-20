using PocLoginWebsite.Core.Common;

namespace PocLoginWebsite.Application.Services;

/// <summary>
/// Service for managing browser state persistence.
/// Allows saving and loading browser state (cookies, localStorage, etc.) to skip login.
/// </summary>
public class BrowserStateService
{
    private readonly string _statePath;

    public BrowserStateService(string statePath = "state.json")
    {
        Guard.Against.NullOrWhiteSpace(statePath, nameof(statePath));
        _statePath = statePath;
    }

    /// <summary>
    /// Checks if a saved state file exists.
    /// </summary>
    public bool StateExists()
    {
        return File.Exists(_statePath);
    }

    /// <summary>
    /// Gets the path to the state file.
    /// </summary>
    public string GetStatePath() => _statePath;

    /// <summary>
    /// Deletes the saved state file.
    /// </summary>
    public void ClearState()
    {
        if (File.Exists(_statePath))
        {
            File.Delete(_statePath);
        }
    }

    /// <summary>
    /// Gets information about the saved state.
    /// </summary>
    public (bool Exists, DateTime? LastModified, long? SizeBytes) GetStateInfo()
    {
        if (!File.Exists(_statePath))
        {
            return (false, null, null);
        }

        var fileInfo = new FileInfo(_statePath);
        return (true, fileInfo.LastWriteTime, fileInfo.Length);
    }
}
