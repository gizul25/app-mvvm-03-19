using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyAvaloniaApp.ViewModels;

namespace MyAvaloniaApp.Views;

public partial class CatalogView : UserControl
{
    private bool PressedBefore = false;

    public CatalogView()
    {
        InitializeComponent();
    }

    private void SelectionChanged(object? sender, RoutedEventArgs e)
    {
        if(PressedBefore) 
        {
            PressedBefore = false;
            return;
        }

        if (DataContext is CatalogViewModel viewModel)
        {
            ICommand command = viewModel.ShowDetailsCommand;

            if (command.CanExecute(BookList.SelectedIndex))
            {
                command.Execute(BookList.SelectedIndex);
            }
        }
        
        PressedBefore = true;
        BookList.SelectedItem = null;
    }
}