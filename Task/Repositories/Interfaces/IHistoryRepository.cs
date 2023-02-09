namespace Tasks.Repositories.Interfaces;

public interface IHistoryRepository
{
    void Save(string userId, string oldValues, string newValues, string changed, DateTime dateTime, long id);
}