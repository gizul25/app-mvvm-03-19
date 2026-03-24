using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyAvaloniaApp.ViewModels;
using MyAvaloniaApp.Views;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Persistence;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using MyAvaloniaApp.Domain;

namespace MyAvaloniaApp;

public partial class App : Application
{
    public static event EventHandler<ChangePageArgs>? ChangePage;

    public static Database Db { get; set; } = new();
    public static User? CurrentUser;

    static JSONPersistence? persistence;

    public override void Initialize()
    {
        InitDb();
        
        AvaloniaXamlLoader.Load(this);
    }

    public static void ChangePageTo(ObservableObject Page)
    {
        ChangePage?.Invoke(null, new(Page));
    }
    

    public void InitDb()
    {
        persistence = new JSONPersistence("db.json");
        Db = persistence.Load<Database>() ?? Generator.CreateFreshDatabase();
    }

    public static void SaveDatabase()
    {
        persistence!.Save(Db);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            desktop.ShutdownRequested += OnShutdownRequested;
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        SaveDatabase();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}

public class ChangePageArgs(ObservableObject Page)
{
    public ObservableObject Page = Page;
}