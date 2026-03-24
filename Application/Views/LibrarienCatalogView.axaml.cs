using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MyAvaloniaApp.ViewModels;

namespace MyAvaloniaApp.Views;

public partial class LibrarienCatalogView : UserControl
{
    private bool PressedBefore = false;

    public LibrarienCatalogView()
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

        if (DataContext is LibrarienCatalogViewModel viewModel)
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

    private void BookChanged(object? sender, RoutedEventArgs e)
    {
        if (DataContext is LibrarienCatalogViewModel viewModel)
        {
            ICommand command = viewModel.SaveBookCommand;

            if (command.CanExecute(null))
            {
                command.Execute(null);
            }
        }
    }
}