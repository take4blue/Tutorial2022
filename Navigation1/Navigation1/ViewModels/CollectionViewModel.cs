using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Navigation1.ViewModels
{
    public class HistoryItem : BindableBase
    {
        string hisotyrTextTitle_;

        public string HistoryTextTitle {
            get { return hisotyrTextTitle_; }
            set { SetProperty(ref hisotyrTextTitle_, value); }
        }
        public Int32 No { get; set; }
    }

    public class CollectionViewModel : BindableBase
    {
        static int MaxDisplayData = 100;
        static int DeltaItemsNum = 10;
        static int InitialPageNum = 40;

        ObservableCollection<HistoryItem> items_ = new ObservableCollection<HistoryItem>();
        HistoryItem select_;

        public ObservableCollection<HistoryItem> Items
        {
            get { return items_; }
            set { SetProperty(ref items_, value); }
        }

        public HistoryItem Select
        {
            get { return select_; }
            set { SetProperty(ref select_, value); }
        }

        /// <summary>
        /// 履歴の詳細表示
        /// </summary>
        public ICommand DetailHistory { get; private set; }

        /// <summary>
        /// もっとデータを!!
        /// </summary>
        public ICommand LoadMoreData { get; private set; }

        public CollectionViewModel()
        {
            DetailHistory = new Command(DoDetailHistory);
            LoadMoreData = new Command(DoLoadMoreData);

            // 表示する履歴情報を作成
            while (items_.Count < MaxDisplayData && items_.Count < InitialPageNum) {
                DoLoadMoreData();
            }
        }

        async void DoDetailHistory()
        {
            if (select_ != null) {
                var detail = new DetailViewModel()
                {
                    Text = select_.HistoryTextTitle
                };
                var detailPage = new Views.DetailPage()
                {
                    BindingContext = detail,
                };

                await Shell.Current.Navigation.PushAsync(detailPage);
                if (await detail.Result) {
                    select_.HistoryTextTitle = detail.Text;
                }
                Select = null;
            }
        }

        void DoLoadMoreData()
        {
            var lowNo = items_.Count;
            var addCount = Math.Min(MaxDisplayData - lowNo, DeltaItemsNum);
            for (int i = 0; i < addCount; i++) {
                Items.Add(new HistoryItem()
                {
                    HistoryTextTitle = "History " + (i + lowNo),
                    No = i + lowNo,
                });
            }
            Debug.WriteLine("Count: " + lowNo + addCount);
        }
    }
}
