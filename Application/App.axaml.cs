using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyAvaloniaApp.ViewModels;
using MyAvaloniaApp.Views;
using MyAvaloniaApp.Models;
using MyAvaloniaApp.Persistence;

namespace MyAvaloniaApp;

public partial class App : Application
{
    public static Database? Db;
    public static User? CurrentUser;
    static JSONPersistence? persistence;

    public override void Initialize()
    {
        InitDb();
        
        AvaloniaXamlLoader.Load(this);
    }

    public void InitDb()
    {
        persistence = new JSONPersistence("db.json");
        Db = persistence.Load<Database>();
    }

    public static void SaveDatabase()
    {
        Console.WriteLine("Saving database...");
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
        Console.WriteLine("Shutting down...");
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