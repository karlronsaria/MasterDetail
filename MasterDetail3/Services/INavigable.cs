using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace MasterDetail3.Services
{
    public interface INavigable
    {
        NavigationViewItem GetCurrentNavigationViewItem();
        List<NavigationViewItem> GetNavigationViewItems();
        List<NavigationViewItem> GetNavigationViewItems(Type type);
        List<NavigationViewItem> GetNavigationViewItems(Type type, string title);
        void SetCurrentNavigationViewItem(NavigationViewItem item);
        void SetCurrentPage(Type type);
    }
}
