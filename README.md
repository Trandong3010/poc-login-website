# Playwright .NET Test Framework - Hexagonal Architecture

A clean, maintainable Playwright testing framework for .NET following the Hexagonal Architecture (Ports & Adapters) pattern.

## ðŸ—ï¸ Architecture Overview

This project demonstrates how to build a test automation framework using **Hexagonal Architecture**, which provides:

- **Maintainability**: Business logic (test scenarios) is decoupled from infrastructure (Playwright)
- **Testability**: Easy to mock and test individual components
- **Flexibility**: Can swap Playwright for another tool (Selenium, Puppeteer) without changing test logic
- **Scalability**: Clear separation of concerns makes it easy to grow

### Architecture Layers

```
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚                     Tests Layer                         â”‚
â”‚   (Test Execution - NUnit, Fixtures, Test Cases)       â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
                           â”‚
                           â–¼
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚                  Application Layer                      â”‚
â”‚   (Test Orchestration, Page Objects, Use Cases)        â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
                           â”‚
                           â–¼
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚                    Core Layer (Ports)                   â”‚
â”‚   (Interfaces: IBrowserPort, IPagePort, IElementPort)   â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
                           â”‚
                           â–¼
â”Œâ”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”
â”‚              Infrastructure Layer (Adapters)            â”‚
â”‚   (Playwright Implementations, Configuration)           â”‚
â””â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”˜
```

## ðŸ“ Project Structure

```
PocLoginWebsite/
â”œâ”€â”€ src/
â”‚   â”œâ”€â”€ PocLoginWebsite.Core/              # Core Layer (Ports)
â”‚   â”‚   â”œâ”€â”€ Ports/
â”‚   â”‚   â”‚   â”œâ”€â”€ IBrowserPort.cs            # Browser lifecycle interface
â”‚   â”‚   â”‚   â”œâ”€â”€ IPagePort.cs               # Page interaction interface
â”‚   â”‚   â”‚   â”œâ”€â”€ IElementPort.cs            # Element interaction interface
â”‚   â”‚   â”‚   â””â”€â”€ IConfigurationPort.cs      # Configuration interface
â”‚   â”‚   â””â”€â”€ Models/
â”‚   â”‚       â””â”€â”€ TestResult.cs              # Domain models
â”‚   â”‚
â”‚   â”œâ”€â”€ PocLoginWebsite.Application/       # Application Layer
â”‚   â”‚   â”œâ”€â”€ PageObjects/
â”‚   â”‚   â”‚   â””â”€â”€ BasePageObject.cs          # Base page object class
â”‚   â”‚   â””â”€â”€ Services/
â”‚   â”‚       â””â”€â”€ TestOrchestrator.cs        # Test orchestration service
â”‚   â”‚
â”‚   â””â”€â”€ PocLoginWebsite.Infrastructure/    # Infrastructure Layer (Adapters)
â”‚       â”œâ”€â”€ Adapters/
â”‚       â”‚   â”œâ”€â”€ PlaywrightBrowserAdapter.cs # Playwright browser implementation
â”‚       â”‚   â”œâ”€â”€ PlaywrightPageAdapter.cs    # Playwright page implementation
â”‚       â”‚   â””â”€â”€ PlaywrightElementAdapter.cs # Playwright element implementation
â”‚       â”œâ”€â”€ Configuration/
â”‚       â”‚   â””â”€â”€ TestConfiguration.cs        # Configuration provider
â”‚       â””â”€â”€ PageObjects/
â”‚           â””â”€â”€ LoginPageObject.cs          # Example page object
â”‚
â””â”€â”€ tests/
    â””â”€â”€ PocLoginWebsite.Tests/             # Tests Layer
        â”œâ”€â”€ Fixtures/
        â”‚   â””â”€â”€ BaseTestFixture.cs         # Base test fixture
        â”œâ”€â”€ Examples/
        â”‚   â”œâ”€â”€ ExampleTests.cs            # Basic example tests
        â”‚   â””â”€â”€ LoginPageTests.cs          # Page object example tests
        â””â”€â”€ appsettings.json               # Test configuration
```

## ðŸš€ Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- PowerShell (for installing Playwright browsers)

### Installation

1. **Clone or navigate to the project directory**

2. **Restore dependencies**

   ```powershell
   dotnet restore PocLoginWebsite.sln
   ```

3. **Build the solution**

   ```powershell
   dotnet build PocLoginWebsite.sln
   ```

4. **Install Playwright browsers**

   ```powershell
   # Navigate to the output directory
   cd tests\PocLoginWebsite.Tests\bin\Debug\net8.0

   # Install browsers
   pwsh playwright.ps1 install
   ```

### Running Tests

```powershell
# Run all tests
dotnet test PocLoginWebsite.sln

# Run tests with detailed output
dotnet test PocLoginWebsite.sln --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~PocLoginWebsite.Tests.Examples.ExampleTests"
```

## ðŸ“– Usage Examples

### Creating a New Page Object

```csharp
using PocLoginWebsite.Application.PageObjects;
using PocLoginWebsite.Core.Ports;

public class HomePage : BasePageObject
{
    private const string SearchInputSelector = "#search";
    private const string SearchButtonSelector = "button[type='submit']";

    public HomePage(IPagePort page, IConfigurationPort configuration) : base(page)
    {
        _configuration = configuration;
    }

    public override async Task NavigateAsync(CancellationToken cancellationToken = default)
    {
        await Page.GotoAsync(_configuration.BaseUrl, cancellationToken);
    }

    public override async Task<bool> IsPageLoadedAsync(CancellationToken cancellationToken = default)
    {
        await Page.WaitForSelectorAsync(SearchInputSelector, cancellationToken: cancellationToken);
        return true;
    }

    public async Task SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var searchInput = await Page.GetElementAsync(SearchInputSelector, cancellationToken);
        await searchInput.FillAsync(query, cancellationToken);

        var searchButton = await Page.GetElementAsync(SearchButtonSelector, cancellationToken);
        await searchButton.ClickAsync(cancellationToken);
    }
}
```

### Writing a Test

```csharp
using NUnit.Framework;
using PocLoginWebsite.Tests.Fixtures;

[TestFixture]
public class HomePageTests : BaseTestFixture
{
    private IPagePort? _page;
    private HomePage? _homePage;

    [SetUp]
    public async Task SetUp()
    {
        _page = await CreatePageAsync();
        _homePage = new HomePage(_page, ConfigurationPort);
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_page != null)
        {
            await _page.CloseAsync();
        }
    }

    [Test]
    public async Task Search_ShouldDisplayResults()
    {
        // Arrange
        await _homePage!.NavigateAsync();

        // Act
        await _homePage.SearchAsync("test query");

        // Assert
        // Add your assertions here
    }
}
```

## ðŸŽ¯ Key Concepts

### Ports (Interfaces)

Ports define the contracts that the application needs. They are technology-agnostic:

- **IBrowserPort**: Browser lifecycle management
- **IPagePort**: Page-level operations
- **IElementPort**: Element interactions
- **IConfigurationPort**: Configuration access

### Adapters (Implementations)

Adapters implement the ports using specific technologies (Playwright in this case):

- **PlaywrightBrowserAdapter**: Implements `IBrowserPort`
- **PlaywrightPageAdapter**: Implements `IPagePort`
- **PlaywrightElementAdapter**: Implements `IElementPort`
- **TestConfiguration**: Implements `IConfigurationPort`

### Benefits

1. **Technology Independence**: Swap Playwright for another tool by creating new adapters
2. **Testable**: Mock ports to test your page objects and test logic
3. **Maintainable**: Clear separation makes code easier to understand and modify
4. **Reusable**: Page objects and business logic can be reused across different test frameworks

## âš™ï¸ Configuration

Edit `tests/PocLoginWebsite.Tests/appsettings.json` or modify `TestConfiguration.cs`:

```json
{
  "TestConfiguration": {
    "BaseUrl": "https://your-app.com",
    "DefaultTimeout": 30000,
    "Headless": true,
    "BrowserType": "chromium"
  }
}
```

Supported browser types: `chromium`, `firefox`, `webkit`

## ðŸ“ Best Practices

1. **Keep ports simple**: Interface methods should represent business operations
2. **One page object per page**: Follow the Page Object Model pattern
3. **Use meaningful selectors**: Prefer data-testid or semantic selectors
4. **Handle waits properly**: Use explicit waits via ports
5. **Take screenshots on failure**: Implement in teardown for debugging

## ðŸ”§ Extending the Framework

### Adding a New Port

1. Create interface in `Core/Ports/`
2. Implement in `Infrastructure/Adapters/`
3. Register in test fixtures or DI container

### Supporting Another Browser Automation Tool

1. Create new adapters implementing existing ports
2. Update test fixtures to use new adapters
3. No changes needed in page objects or tests!

## ðŸ“¦ Dependencies

- **Microsoft.Playwright** - Browser automation
- **Microsoft.Playwright.NUnit** - NUnit integration
- **NUnit** - Test framework

## ðŸ¤ Contributing

This is a template project. Feel free to modify and extend based on your needs!

## ðŸ“„ License

This project is provided as-is for educational and commercial use.
