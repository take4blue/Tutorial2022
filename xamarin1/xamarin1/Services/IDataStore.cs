using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xamarin1.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<int> FindLastIndex(string id);
        Task<string> NextItemAsync(string id);
        Task<string> PrevItemAsync(string id);
        Task<T> GetItemAsync(int no);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        int CountItem();
    }
}
