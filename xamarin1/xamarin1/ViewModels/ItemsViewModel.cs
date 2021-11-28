﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using xamarin1.Models;
using xamarin1.Views;

namespace xamarin1.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> EditItem { get; }
        public Command DeleteCommand { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            EditItem = new Command<Item>(EditSelectedItem);
            AddItemCommand = new Command(OnAddItem);
            DeleteCommand = new Command(OnDeleteItem);
        }

        public void OnDeleteItem(object obj)
        {
            Items.Remove(obj as Item);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items) {
                    Items.Add(item);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set {
                SetProperty(ref _selectedItem, value);
                EditSelectedItem(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void EditSelectedItem(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}