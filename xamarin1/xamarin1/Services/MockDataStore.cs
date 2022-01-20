using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xamarin1.Models;

namespace xamarin1.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<string> NextItemAsync(string id)
        {
            var no = items.FindLastIndex(s => s.Id == id);
            if (no == -1) {
                no = 0;
            }
            else if (no < items.Count - 1) {
                no++;
            }
            return await Task.FromResult(items[no].Id);
        }

        public async Task<string> PrevItemAsync(string id)
        {
            var no = items.FindLastIndex(s => s.Id == id);
            if (no == -1) {
                no = 0;
            }
            else if (no != 0) {
                no--;
            }
            return await Task.FromResult(items[no].Id);
        }

        public async Task<int> CountItemAsync()
        {
            return await Task.FromResult(items.Count());
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}