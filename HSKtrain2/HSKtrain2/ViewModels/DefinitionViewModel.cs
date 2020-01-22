using HSKtrain2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HSKtrain2.ViewModels {
    
    public class DefinitionViewModel : BaseViewModel {
        TrainingViewModel Parent;

        public ICommand OnBackToTraining { get; }
        public DefinitionViewModel(TrainingViewModel parent) {
            Parent = parent;
            OnBackToTraining = new Command(() => {
                Parent.SetTrainPage();
            });
            
        }
    }
}