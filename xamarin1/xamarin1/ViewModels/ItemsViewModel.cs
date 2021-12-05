using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using xamarin1.Views;

namespace xamarin1.ViewModels
{
    /// <summary>
    /// ItemsViewのViewモデル</br>
    /// ItemViewModel情報はDataStore経由MockDataStoreで管理している</br>
    /// ここのItemsはそのデータを引き出してView用データとして用意している。
    /// </summary>
    public class ItemsViewModel : BaseViewModel
    {
        /// <summary>
        /// 表示データの管理用
        /// </summary>
        ObservableCollection<ItemViewModel> items_;

        /// <summary>
        /// ドラッグ中のアイテム
        /// </summary>
        private ItemViewModel draggingItem_;

        /// <summary>
        /// 通過中のアイテム</br>
        /// 重複処理をしないために使用している
        /// </summary>
        private ItemViewModel dragOverItem_;

        public ObservableCollection<ItemViewModel> Items
        {
            get { return items_; }
            set { SetProperty(ref items_, value); }
        }

        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<ItemViewModel> EditItem { get; }
        public Command<ItemViewModel> DeleteCommand { get; }
        public Command<ItemViewModel> DragStart { get; }
        public Command DragEnd { get; }
        public Command<ItemViewModel> DragLeave { get; }
        public Command<ItemViewModel> DragOver { get; }
        public Command<ItemViewModel> Drop { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            items_ = new ObservableCollection<ItemViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            EditItem = new Command<ItemViewModel>(EditSelectedItem);
            AddItemCommand = new Command(OnAddItem);
            DeleteCommand = new Command<ItemViewModel>(OnDeleteItem);
            DragStart = new Command<ItemViewModel>(OnDragStart);
            DragEnd = new Command(OnDragEnd);
            DragLeave = new Command<ItemViewModel>(OnDragLeave);
            DragOver = new Command<ItemViewModel>(OnDragOver);
            Drop = new Command<ItemViewModel>(OnDrop);
        }

        /// <summary>
        /// データの削除
        /// </summary>
        /// <param name="obj">削除するオブジェクト</param>
        public async void OnDeleteItem(ItemViewModel obj)
        {
            await DataStore.DeleteItemAsync(obj.Data.Id);
            Items.Remove(obj);
        }

        /// <summary>
        /// オリジナルデータをロードする
        /// </summary>
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items) {
                    Items.Add(new ItemViewModel() { Data = item });
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
        }

        private void OnDragStart(ItemViewModel item)
        {
            Debug.WriteLine($"OnDragStart: {item?.Data.Text}");
            draggingItem_ = item;
            Items.ForEach(i => i.IsBeingDragged = item == i);
        }

        private void DragEndProcess()
        {
            if (draggingItem_ != null) {
                draggingItem_.IsBeingDragged = false;
            }
            draggingItem_ = null;
            dragOverItem_ = null;
        }

        private void OnDragEnd()
        {
            Debug.WriteLine($"OnDragEnd");
            DragEndProcess();
        }

        private void OnDragLeave(ItemViewModel item)
        {
            Debug.WriteLine($"OnDragLeave: {item?.Data.Text}");
            dragOverItem_ = null;
        }

        private void OnDrop(ItemViewModel item)
        {
            Debug.WriteLine($"OnDrop: {item?.Data.Text}");
            DragEndProcess();
            // ここで必要であればDataStore内のItemの順序をDrag&Dropで行った結果で反映させる
        }

        private void OnDragOver(ItemViewModel item)
        {
            Debug.WriteLine($"OnDragOver1: {item?.Data.Text}");
            // itemがdragOverItem_と一致していた場合は位置変更を行わない
            // 同じ動作をさせないため。
            if (item != null && item != draggingItem_ && item != dragOverItem_) {
                // 自分のアイテムより後ろにあるアイテムの場合は、一つ後に挿入。
                var overItemPos = Items.IndexOf(item);
                var myItemPos = Items.IndexOf(draggingItem_);
                if (overItemPos < myItemPos) {
                    // 自分のアイテムより前にあるアイテムの場合は、ひとつ前に挿入。
                    Items.Remove(draggingItem_);
                    Items.Insert(overItemPos, draggingItem_);
                }
                else if (overItemPos > myItemPos) {
                    // 自分のアイテムより後ろにあるアイテムの場合は、一つ後に挿入。
                    Items.Remove(draggingItem_);
                    Items.Insert(Items.IndexOf(item) + 1, draggingItem_);
                }
            }
            dragOverItem_ = item;
        }

        /// <summary>
        /// 追加ビューを呼び出す
        /// </summary>
        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        /// <summary>
        /// Editビューを呼び出す
        /// </summary>
        async void EditSelectedItem(ItemViewModel item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Data.Id}");
        }
    }
}