using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReOrder
{
    /// <summary>
    /// 手動で順序を入れ替えるための制御クラス。</br>
    /// Drag&DropでItemsViewのItemsSourceの位置を入れ替える部分のみを処理するために用意したクラス。
    /// </summary>
    /// <typeparam name="T">ItemsSourceに定義しているクラス</typeparam>
    public class ManualOrderItems<T> where T : ManualOrderItems<T>.IHasDragMarker
    {
        /// <summary>
        /// ドラッグ中のマークを保持するというインターフェース</br>
        /// ItemsSource用のクラスにこのインターフェースを用意しておく
        /// </summary>
        public interface IHasDragMarker
        {
            bool IsBeingDragged { get; set; }
        }

        public ICommand DragStart { get; }
        public ICommand DragEnd { get; }
        public ICommand DragLeave { get; }
        public ICommand DragOver { get; }

        /// <summary>
        /// ドラッグ中のアイテム
        /// </summary>
        private int draggingItem_ = -1;

        /// <summary>
        /// 通過中のアイテム</br>
        /// 重複処理をしないために使用している
        /// </summary>
        private int dragOverItem_ = -1;

        private ObservableCollection<T> items_;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">ItemsSourceに割り当てているデータ</param>
        public ManualOrderItems(ObservableCollection<T> items)
        {
            items_ = items;

            DragEnd = new Command(() => {
                DragEndProcess();
            });
            DragLeave = new Command(() => {
                dragOverItem_ = -1;
            });

            DragStart = new Command<T>(OnDragStart);
            DragOver = new Command<T>(OnDragOver);
        }

        private void OnDragStart(T item)
        {
            draggingItem_ = items_.IndexOf(item);
            if (draggingItem_ != -1) {
                items_.ForEach(i => i.IsBeingDragged = false);
                item.IsBeingDragged = true;
            }
        }

        private void DragEndProcess()
        {
            if (draggingItem_ > -1) {
                items_[draggingItem_].IsBeingDragged = false;
            }
            draggingItem_ = -1;
            dragOverItem_ = -1;
        }

        private void OnDragOver(T item)
        {
            if (draggingItem_ > -1) {
                // T自身をドラッグ中で処理を行う
                var overItemPos = items_.IndexOf(item);
                if (overItemPos > -1 && overItemPos != draggingItem_ && overItemPos != dragOverItem_) {
                    // 移動処理
                    // ・ドラッグ中のアイテムを配列から削除する
                    // ・マウスがあった場所の前にドラッグ中のアイテムを挿入する
                    // 　ただし挿入位置が最後だった場合最後に追加する
                    // 上から下へドラッグした場合、自分自身が削除されるので、マウスがある位置のアイテム(1)は
                    // 一つ上に移動する。そこで(1)に挿入することでドラッグしたオブジェクトが(1)の下に来ることになる。
                    // 下から上にドラッグした場合は、マウスのあるオブジェクト(2)の前に挿入するので、ドラッグ
                    // したオブジェクトは(2)のひとつ前に配置されることになる。
                    var work = items_[draggingItem_];
                    items_.Remove(work);
                    if (overItemPos < items_.Count) {
                        items_.Insert(overItemPos, work);
                    }
                    else {
                        items_.Add(work);
                    }
                    draggingItem_ = items_.IndexOf(work);   // ドラッグ中のアイテムの位置更新
                }
                dragOverItem_ = overItemPos;
            }
        }
    }
}
