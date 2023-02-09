

using Tasks.Data;
using Tasks.Entities;
using Tasks.Repositories.Interfaces;

namespace Tasks.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly AppDbContext _context;

    public HistoryRepository(AppDbContext context)
    {
        _context = context ;
    }
    public void Save(string userId, string oldValues, string newValues, string changed, DateTime dateTime, long id)
    {
        History history = new History();
        history.UserId = userId;
        history.OldValues = oldValues;
        history.NewValues = newValues;
        history.Changed = changed;
        history.DateTime = dateTime;
        history.ProductId = id;

        _context.Histories!.Add(history);
        _context.SaveChanges();

    }
}