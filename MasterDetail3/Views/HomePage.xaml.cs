// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;
using MasterDetail3.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MasterDetail3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void
        OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            base.OnNavigatedTo(e);
        }

        protected override void
        OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            base.OnNavigatedTo(e);
        }

        public ICommand NewCommand => new AsyncRelayCommand(OpenNewDialog);
        public ICommand EditCommand => new AsyncRelayCommand(OpenEditDialog);
        public ICommand UpdateCommand => new RelayCommand(Update);
        public ICommand InsertCommand => new RelayCommand(Insert);

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Current" && ViewModel.HasCurrent)
            {
                CharacterListView.ScrollIntoView(ViewModel.Current);
            }
        }

        private void ListViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType is
                Microsoft.UI.Input.PointerDeviceType.Mouse or
                Microsoft.UI.Input.PointerDeviceType.Pen)
            {
                VisualStateManager
                    .GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        private void ListViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager
                .GoToState(sender as Control, "HoverButtonsHidden", true);
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Filter = args.QueryText;
        }

        private async Task OpenNewDialog()
        {
            EditDialog.Title = "New Character";
            EditDialog.PrimaryButtonText = "Insert";
            EditDialog.PrimaryButtonCommand = InsertCommand;
            EditDialog.DataContext = new Character();
            await EditDialog.ShowAsync();
        }

        private async Task OpenEditDialog()
        {
            EditDialog.Title = "Edit Character";
            EditDialog.PrimaryButtonText = "Update";
            EditDialog.PrimaryButtonCommand = UpdateCommand;
            var clone = ViewModel.Current.Clone();
            clone.Name = ViewModel.Current.Name;
            EditDialog.DataContext = clone;
            await EditDialog.ShowAsync();
        }

        private void Update()
        {
            ViewModel.UpdateItem(
                EditDialog.DataContext as Character,
                ViewModel.Current
            );
        }

        private void Insert()
        {
            var character = ViewModel
                .AddItem(EditDialog.DataContext as Character);

            if (ViewModel.Items.Contains(character))
            {
                ViewModel.Current = character;
            }
        }
    }
}
