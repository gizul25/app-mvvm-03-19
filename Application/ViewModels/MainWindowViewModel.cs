using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace MyAvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _result = 0;

    [ObservableProperty]
    private int _firstOperand = 0;

    [NotifyCanExecuteChangedFor(nameof(DivideCommand))]
    [ObservableProperty]
    private int _secondOperand = 0;


    [RelayCommand]
    private void Add()
    {
        Result = FirstOperand + SecondOperand;
    }

    [RelayCommand]
    private void Substract()
    {
        Result = FirstOperand - SecondOperand;
    }

    [RelayCommand]
    private void Multiply()
    {
        Result = FirstOperand * SecondOperand;
    }

    [RelayCommand(CanExecute = nameof(CanDivide))]
    private void Divide()
    {
        Result = FirstOperand / SecondOperand;
    }

    private bool CanDivide()
    {
        return SecondOperand != 0;
    }
}