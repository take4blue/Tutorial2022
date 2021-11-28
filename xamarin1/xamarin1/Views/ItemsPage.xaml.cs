using System;
using Xamarin.Forms;
using xamarin1.Models;
using xamarin1.ViewModels;

namespace xamarin1.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel_;

        /// <summary>
        /// 現在スワイプ中のオブジェクト
        /// </summary>
        SwipeView swipedView_;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel_ = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel_.OnAppearing();
        }

        // SwipeView_SwipeStartedとSwipeView_SwipeEndedはSwipeの単一スワイプを実現するための仕組み
        // https://stackoverflow.com/questions/62874278/how-can-i-make-xamarin-forms-close-open-swiped-views
        // 上の回答を利用しているが、swipedView_への登録はオープンされた時のみ行っている。
        // Listに入れ込んでいるのはちょっと不明だが、安全策なのだろうか。
        private bool CloseSwipe()
        {
            if (swipedView_ != null) {
                swipedView_.Close();
                swipedView_ = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// スワイプ完了時の処理</br>
        /// オープン中のものがクローズしたら変数クリア</br>
        /// オープン時はすでにオープン中のものをクローズして、オープン中のアイテムとして自分を登録しておく
        /// </summary>
        private void SwipeView_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            if (swipedView_ == sender && !e.IsOpen) {
                swipedView_ = null;
            }
            else {
                CloseSwipe();
                if (e.IsOpen) {
                    swipedView_ = sender as SwipeView;
                }
            }
        }

        /// <summary>
        /// スワイプ動作を開始時の処理</br>
        /// 他にスワイプがオープン中のものがあればクローズする。
        /// </summary>
        private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            CloseSwipe();
        }

        /// <summary>
        /// スワイプコマンド実行時の処理</br>
        /// 現在スワイプ中のオブジェクトのクリア(メンテナンス)をするために用意したもの。
        /// </summary>
        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            CloseSwipe();
        }

        /// <summary>
        /// タップ時の動作</br>
        /// オープン中のスワイプがなければタップ時のコマンドを実行する。</br>
        /// オープン中のがあればタップ中の動作はせず、スワイプをクローズする。
        /// </summary>
        /// <param name="sender">バインド済みのオブジェクトを取得するためのものとして認識している</param>
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!CloseSwipe()) {
                var target = sender as BindableObject;
                viewModel_.EditItem.Execute(target.BindingContext as Item);
            }
        }

    }
}