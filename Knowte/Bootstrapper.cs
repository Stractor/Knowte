﻿using Digimezzo.Utilities.Log;
using Digimezzo.Utilities.Settings;
using Knowte.Core.Extensions;
using Knowte.Data;
using Knowte.Data.Contracts;
using Knowte.Services.Constracts.Dialog;
using Knowte.Services.Constracts.I18n;
using Knowte.Services.Dialog;
using Knowte.Services.I18n;
using Knowte.Views;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace Knowte
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.RegisterFactories();
            this.RegisterRepositories();
            this.RegisterServices();
            this.InitializeServices();
            this.RegisterViews();
            this.RegisterViewModels();

            ViewModelLocationProvider.SetDefaultViewModelFactory((type) => { return Container.Resolve(type); });
        }

        private void RegisterFactories()
        {
            Container.RegisterSingletonType<ISQLiteConnectionFactory, SQLiteConnectionFactory>();
        }

        protected void RegisterRepositories()
        {
            //Container.RegisterSingletonType<IBankRepository, BankRepository>();
        }

        private void RegisterServices()
        {
            Container.RegisterSingletonType<IDialogService, DialogService>();
            Container.RegisterSingletonType<II18nService, I18nService>();
            //Container.RegisterSingletonType<IJumpListService, JumpListService>();
        }

        private void InitializeServices()
        {
            // Making sure resources are set before we need them
            Container.Resolve<II18nService>().ApplyLanguageAsync(SettingsClient.Get<string>("Configuration", "Language")); // Set default language
            //Container.Resolve<IJumpListService>().RefreshJumpListAsync(); // Create the jump list
        }

        protected void RegisterViews()
        {
            Container.RegisterType<object, Shell>(typeof(Shell).FullName);
        }

        protected void RegisterViewModels()
        {
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;

            LogClient.Info("Showing Main screen");
            Application.Current.MainWindow.Show();
        }
    }
}
