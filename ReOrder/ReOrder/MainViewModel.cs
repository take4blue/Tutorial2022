using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ReOrder
{
    /// <summary>
    /// 表示アイテムを格納するクラス
    /// </summary>
    public class HistoryItem : BindableBase, ManualOrderItems<HistoryItem>.IHasDragMarker
    {
        string hisotyrTextTitle_;

        public string HistoryTextTitle
        {
            get { return hisotyrTextTitle_; }
            set { SetProperty(ref hisotyrTextTitle_, value); }
        }

        bool isBeingDragged_ = false;

        public bool IsBeingDragged
        {
            get => isBeingDragged_;
            set { SetProperty(ref isBeingDragged_, value); }
        }
    }

    public class MainViewModel : BindableBase
    {
        static int MaxDisplayData = 100;

        public ManualOrderItems<HistoryItem> OrderControl { get; private set; }

        ObservableCollection<HistoryItem> items_ = new ObservableCollection<HistoryItem>();

        public ObservableCollection<HistoryItem> Items
        {
            get { return items_; }
            set { SetProperty(ref items_, value); }
        }

        public MainViewModel()
        {
            // 手動順序制御オブジェクトの初期化(ItemsSourceの要素を渡して作成)
            OrderControl = new ManualOrderItems<HistoryItem>(items_);

            // 表示データの作成
            for (int i = 0; i < MaxDisplayData; i++) {
                Items.Add(new HistoryItem()
                {
                    HistoryTextTitle = "History " + i,
                });
            }
        }
    }
}
