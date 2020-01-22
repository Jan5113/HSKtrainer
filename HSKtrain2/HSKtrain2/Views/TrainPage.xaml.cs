using HSKtrain2.Models;
using HSKtrain2.Services;
using HSKtrain2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace HSKtrain2.Views {
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class TrainPage : ContentPage {
        readonly TrainingViewModel TrainViewModel;
        readonly App App;


        public TrainPage(App app) {
            InitializeComponent();
            App = app;
            BindingContext = TrainViewModel = new TrainingViewModel(app);
        }

        public void SetSession(Session session) {
            TrainViewModel.SetSession(session);
        }
        
        public bool HasActiveSession() {
            return TrainViewModel.HasActiveSession();
        }
        protected override bool OnBackButtonPressed() {
            App.SetMenuPage();
            return true;
        }
    }

}