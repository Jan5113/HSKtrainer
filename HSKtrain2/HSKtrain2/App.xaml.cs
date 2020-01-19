using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HSKtrain2.Services;
using HSKtrain2.Views;
using HSKtrain2.Models;

namespace HSKtrain2 {
    public partial class App : Application {
        private readonly VocDataStore MainVocs;
        private MenuPage MenuPage;
        private TrainPage TrainPage;
        public App(string vocPath) {
            InitializeComponent();
            //DependencyService.Register<VocDataStore>();
            MainVocs = new VocDataStore(vocPath);
            MainPage = MenuPage = new MenuPage(MainVocs, this);
            TrainPage = new TrainPage(this);
        }

        public void SetTrainPage() {
            MainPage = TrainPage;
        }
        public void SetMenuPage() {
            MainPage = MenuPage;
            MenuPage.Refresh();
        }

        public void SetSession(Session session) {
            TrainPage.SetSession(session);
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
            SaveProgress();
        }

        protected override void OnResume() {
        }

        public void SaveProgress() {
            MainVocs.ExportVocDataStore();
        }
    }
}
