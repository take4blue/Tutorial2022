using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin1.Models;

namespace xamarin1.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        public string Id { get; set; }

        public Command LeftPage { get; private set; }
        public Command RightPage { get; private set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get {
                return itemId;
            }
            set {
                itemId = value;
                LoadItemId(value);
            }
        }

        public ItemDetailViewModel()
        {
            LeftPage = new Command(DoLeftPage);
            RightPage = new Command(DoRightPage);
        }


        async void DoLeftPage()
        {
            ItemId = await DataStore.PrevItemAsync(ItemId);
        }

        async void DoRightPage()
        {
            ItemId = await DataStore.NextItemAsync(ItemId);
        }

        public async void LoadItemId(string itemId)
        {
            try {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception) {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
