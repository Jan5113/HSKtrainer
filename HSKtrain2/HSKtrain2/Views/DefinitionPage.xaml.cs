using HSKtrain2.Models;
using HSKtrain2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HSKtrain2.Services;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HSKtrain2.Views {

    [DesignTimeVisible(false)]
    public partial class DefinitionPage : ContentPage {
        readonly DefinitionViewModel DefinitionViewModel;
        readonly TrainingViewModel Parent;
        ObservableCollection<CharDef> items = new ObservableCollection<CharDef>();
        public ObservableCollection<CharDef> Items { get { return items; } }
        public DefinitionPage(TrainingViewModel parent, CharDef[] list) {
            InitializeComponent();
            Parent = parent;
            DefinitionScrollList.ItemsSource = items;
            foreach (CharDef cd in list) {
                items.Add(cd);
            }

            BindingContext = DefinitionViewModel = new DefinitionViewModel(parent);
        }
        protected override bool OnBackButtonPressed() {
            Parent.SetTrainPage();
            return true;
        }
    }
}