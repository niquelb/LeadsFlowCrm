using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IFieldDAO
    {
        Task DeleteField(string Id);
        Task<Field?> GetField(string Id);
        Task<IEnumerable<Field>> GetFields();
        Task InsertField(Field field);
        Task UpdateField(Field field);
    }
}