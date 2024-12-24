using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Threads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

var serviceProvider = new ServiceCollection()
                .AddDbContext<LibraryContext>(options =>
                    options.UseSqlServer("Server=localhost;Database=LibraryDB;Integrated Security=True;TrustServerCertificate=True;"))
                .AddScoped<LibraryManager>()
                .AddScoped<MultiThreadingLibrary>()
                .AddLogging(configure => configure.AddConsole()) 
                .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

try
{
    var libraryManager = serviceProvider.GetRequiredService<LibraryManager>();
    var multiThreadingLibrary = serviceProvider.GetRequiredService<MultiThreadingLibrary>();

    // Display available books
    await libraryManager.DisplayAvailableBooksAsync();

    // Borrow a book
    await libraryManager.BorrowBookAsync(userId: 1, bookId: 1);

    // Return a book
    await libraryManager.ReturnBookAsync(userId: 1, bookId: 1);

    // Concurrent borrowing and returning using multi-threading
    await multiThreadingLibrary.BorrowBooksConcurrentlyAsync(2, 2);
    await multiThreadingLibrary.ReturnBooksConcurrentlyAsync(2, 2);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during the library operations.");
}
finally
{
    if (serviceProvider is IDisposable disposable)
    {
        disposable.Dispose();
    }
}
