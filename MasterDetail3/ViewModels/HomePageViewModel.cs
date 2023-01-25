using CommunityToolkit.Mvvm.Input;
using MasterDetail3.Models;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace MasterDetail3.ViewModels
{
    public partial class HomePageViewModel : MasterDetailViewModel<Character>
    {
        public HomePageViewModel()
        {
            Character.GettingStarted
                .OrderBy(c => c.Name)
                .ToList()
                .ForEach(c => Items.Add(c))
                ;

            Items.CollectionChanged
                += Items_CollectionChanged;
        }

        public ICommand DuplicateCommand
            => new RelayCommand<string>(DuplicateCommand_Executed);

        public ICommand DeleteCommand
            => new RelayCommand<string>(DeleteCommand_Executed);

        public override bool ApplyFilter(Character item, string filter)
        {
            return item.ApplyFilter(filter);
        }

        public override Character UpdateItem(Character item, Character original)
        {
            return original.UpdateFrom(item);
        }

        private void DuplicateCommand_Executed(string obj)
        {
            var toBeDuplicated = Items
                .FirstOrDefault(c => c.Name == obj)
                ;

            var clone = toBeDuplicated.Clone();
            AddItem(clone);

            if (Items.Contains(clone))
            {
                Current = clone;
            }
        }

        private void DeleteCommand_Executed(string obj)
        {
            if (obj is null)
            {
                return;
            }

            var toBeDeleted = Items
                .FirstOrDefault(c => c.Name == obj)
                ;

            DeleteItem(toBeDeleted);
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"Collection changed: {e.Action}");
        }
    }
}
