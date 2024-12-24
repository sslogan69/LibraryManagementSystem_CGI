using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Services
{
    public class LibraryManager
    {
        private readonly LibraryContext _context;
        private readonly ILogger<LibraryManager> _logger;

        public LibraryManager(LibraryContext context, ILogger<LibraryManager> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task BorrowBookAsync(int userId, int bookId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                var book = await _context.Books.FindAsync(bookId);

                if (book == null || user == null || book.AvailableCopies <= 0)
                    throw new InvalidOperationException("Book is not available or user does not exist.");

                var borrowRecord = new BorrowRecord
                {
                    UserId = userId,
                    BookId = bookId,
                    BorrowDate = DateTime.Now
                };

                book.AvailableCopies--;
                _context.BorrowRecords.Add(borrowRecord);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Book borrowed successfully: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error borrowing book: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while borrowing book: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
        }

        public async Task ReturnBookAsync(int userId, int bookId)
        {
            try
            {
                var borrowRecord = await _context.BorrowRecords
                    .Where(br => br.UserId == userId && br.BookId == bookId && br.ReturnDate == null)
                    .FirstOrDefaultAsync() ?? throw new InvalidOperationException("No active borrow record found.");
                var book = await _context.Books.FindAsync(bookId) ?? throw new InvalidOperationException("Book not found.");
                borrowRecord.ReturnDate = DateTime.Now;
                book.AvailableCopies++;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Book returned successfully: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error returning book: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while returning book: UserId={UserId}, BookId={BookId}", userId, bookId);
            }
        }

        public async Task DisplayAvailableBooksAsync()
        {
            try
            {
                var availableBooks = await _context.Books
                    .Where(b => b.AvailableCopies > 0)
                    .Select(b => new { b.Title, b.Author, b.AvailableCopies })
                    .ToListAsync();

                foreach (var book in availableBooks)
                {
                    Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Copies Available: {book.AvailableCopies}");
                }

                _logger.LogInformation("Displayed available books successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while displaying available books.");
            }
        }
    }
}
