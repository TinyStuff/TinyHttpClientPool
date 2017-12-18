using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Sample.Models;
using Sample.Services;
using TinyHttpClientPoolLib;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface] // Using PropertyChanged.Fody to auto generate INotifyPropertyChanged implementation
    public class TodoViewModel
    {
        public IEnumerable<TodoItem> Items { get; set; }

        public int PoolSize { get; set; } 
        public int Available { get; set; }

        public TodoViewModel()
        {
            TinyHttpClientPool.Current.PoolChanged += (sender, e) => UpdateStats();
        }

        public ICommand GetItemsSlow
        {
            get 
            {
                return new Command(async () =>
                {
                    var service = DependencyService.Get<TodoService>();
                    Items = await service.GetTodoItems(slow: true);
                });
            }
        }

        public ICommand Flush
        {
            get
            {
                return new Command(() =>
                {
                    TinyHttpClientPool.Current.Flush();
                });
            }
        }

        private void UpdateStats()
        {
            PoolSize = TinyHttpClientPool.Current.TotalPoolSize;
            Available = TinyHttpClientPool.Current.AvailableCount;
        }
    }
}
