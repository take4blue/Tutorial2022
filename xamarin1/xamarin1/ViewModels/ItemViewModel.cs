using System;
using System.Collections.Generic;
using System.Text;

using xamarin1.Models;
using xamarin1.Views;

namespace xamarin1.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        private Item item_;
        bool isBeingDragged_ = false;

        public Item Data
        {
            get => item_;
            set { SetProperty(ref item_, value); }
        }

        public bool IsBeingDragged
        {
            get => isBeingDragged_;
            set { SetProperty(ref isBeingDragged_, value); }
        }
    }
}
