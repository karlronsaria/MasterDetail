// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MasterDetail3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            Title = "XAML Brewer WinUI 3 Master-Detail Sample";
            this.InitializeComponent();
        }
    }

    public sealed partial class MainWindow : INavigate, MasterDetail3.Services.INavigable
    {
        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            SetCurrentNavigationViewItem(
                GetNavigationViewItems(typeof(Views.HomePage))
                    .FirstOrDefault()
            );
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            SetCurrentNavigationViewItem(
                args.SelectedItemContainer
                as NavigationViewItem
            );
        }

        public List<NavigationViewItem>
        GetNavigationViewItems()
        {
            List<NavigationViewItem>
            result = new();
            var items = NavigationView
                .MenuItems
                .Select(i => (NavigationViewItem)i)
                .ToList();
            items.AddRange(
                NavigationView
                    .FooterMenuItems
                    .Select(i => (NavigationViewItem)i)
            );
            result.AddRange(items);

            foreach (NavigationViewItem mainItem in items)
            {
                result.AddRange(
                    mainItem
                        .MenuItems
                        .Select(i => (NavigationViewItem)i)
                );
            }

            return result;
        }

        public List<NavigationViewItem>
        GetNavigationViewItems(Type type)
        {
            return GetNavigationViewItems()
                .Where(i => i.Tag.ToString() == type.Name)
                .ToList();
        }

        public
        List<NavigationViewItem>
        GetNavigationViewItems(Type type, string title)
        {
            return GetNavigationViewItems(type)
                .Where(i => i.Content.ToString() == title)
                .ToList();
        }

        public void
        SetCurrentNavigationViewItem(NavigationViewItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.Tag == null)
            {
                return;
            }

            ContentFrame.Navigate(Type.GetType(item.Tag.ToString()));
            NavigationView.Header = item.Content;
            NavigationView.SelectedItem = item;
        }

        public NavigationViewItem
        GetCurrentNavigationViewItem()
        {
            return NavigationView.SelectedItem
                as NavigationViewItem;
        }

        public bool Navigate(Type sourcePageType)
        {
            return ContentFrame.Navigate(sourcePageType);
        }

        public void SetCurrentPage(Type sourcePageType)
        {
            _ = Navigate(sourcePageType);
        }
    }
}
