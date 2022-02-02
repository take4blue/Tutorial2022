using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Navigation1.ViewModels
{
    public class DetailViewModel : BindableBase
    {
        private string text_;
        public string Text
        {
            get { return text_; }
            set { SetProperty(ref text_, value); }
        }

        private readonly TaskCompletionSource<bool> task_ = new TaskCompletionSource<bool>();
        /// <summary>
        /// 詳細表示の結果</br>
        /// Trueは設定、Falseは取消
        /// </summary>
        public Task<bool> Result { get=> task_.Task; }

        public ICommand DetailSet { get; private set; }
        public ICommand DetailCancel { get; private set; }
        public ICommand Disappear { get; private set; }

        public DetailViewModel()
        {
            // 設定時のアクションと取消時のアクションを設定
            DetailSet = new Command(() =>
            {
                Shell.Current.Navigation.PopAsync();
                task_.SetResult(true);
            });
            DetailCancel = new Command(() =>
            {
                Shell.Current.Navigation.PopAsync();
                task_.SetResult(false);
            });

            // 画面が消えた時の動作
            // UWPはバックボタンに割り付けられているコマンドに飛ばないので
            // ページが消えた時、バックボタンが押されたとして処理する
            Disappear = new Command(() =>
            {
                if (!task_.Task.IsCompleted) {
                    task_.SetResult(false);
                }
            });
        }
    }
}
