using System;
using Sample.Views;
using Xamarin.Forms;

namespace Sample
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new TodoView());
            Children.Add(new PostView());

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
