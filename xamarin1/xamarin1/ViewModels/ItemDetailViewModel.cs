using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin1.Models;
using System.Collections.Generic;

namespace xamarin1.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private int position_;
        private string text;
        private string description;
        private List<int> dummy_ = new List<int>();
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

        public int Position
        {
            get => position_;
            set {
                if (value >= 0 && value < DataStore.CountItem()) {
                    SetProperty(ref position_, value);
                    LoadItemId(position_);
                }
            }
        }

        public int Count
        {
            get => DataStore.CountItem();
        }

        public List<int> Dummy
        {
            get => dummy_;
        }

        public ItemDetailViewModel()
        {
            LeftPage = new Command(DoLeftPage);
            RightPage = new Command(DoRightPage);
            for (int i = 0; i < DataStore.CountItem(); i++) {
                dummy_.Add(i);
            }
        }


        async void DoLeftPage()
        {
            ItemId = await DataStore.PrevItemAsync(ItemId);
        }

        async void DoRightPage()
        {
            ItemId = await DataStore.NextItemAsync(ItemId);
        }

        public async void LoadItemId(int no)
        {
            try {
                var item = await DataStore.GetItemAsync(no);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception) {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public async void LoadItemId(string itemId)
        {
            Position = await DataStore.FindLastIndex(itemId);
        }
    }
}
