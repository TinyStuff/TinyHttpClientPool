using System;
using System.Collections.Generic;
using Sample.ViewModels;
using Xamarin.Forms;

namespace Sample.Views
{
    public partial class TodoView : ContentPage
    {
        public TodoView()
        {
            InitializeComponent();
            BindingContext = new TodoViewModel();
        }
    }
}
