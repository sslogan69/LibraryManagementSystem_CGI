using LibraryManagementSystem.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Threads
{
    public class MultiThreadingLibrary
    {
        private readonly LibraryManager _libraryManager;
        private readonly ILogger<MultiThreadingLibrary> _logger;

        // Constructor accepting LibraryManager and ILogger via Dependency Injection
        public MultiThreadingLibrary(LibraryManager libraryManager, ILogger<MultiThreadingLibrary> logger)
        {
            _libraryManager = libraryManager ?? throw new ArgumentNullException(nameof(libraryManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task BorrowBooksConcurrentlyAsync(int userId, int bookId)
        {
            try
            {
                _logger.LogInformation("Starting to borrow book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
                await Task.Run(async () => await _libraryManager.BorrowBookAsync(userId, bookId));
                _logger.LogInformation("Finished borrowing book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while borrowing book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
        }

        public async Task ReturnBooksConcurrentlyAsync(int userId, int bookId)
        {
            try
            {
                _logger.LogInformation("Starting to return book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
                await Task.Run(async () => await _libraryManager.ReturnBookAsync(userId, bookId));
                _logger.LogInformation("Finished returning book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while returning book concurrently: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
        }

        public async Task DisplayAvailableBooksAsync()
        {
            try
            {
                _logger.LogInformation("Starting to display available books.");
                await _libraryManager.DisplayAvailableBooksAsync();
                _logger.LogInformation("Finished displaying available books.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while displaying available books.");
            }
        }
    }
}
