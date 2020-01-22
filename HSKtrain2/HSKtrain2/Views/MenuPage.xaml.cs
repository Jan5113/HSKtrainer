using HSKtrain2.Models;
using HSKtrain2.Services;
using HSKtrain2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HSKtrain2.Views {

	[DesignTimeVisible(false)]
	public partial class MenuPage : ContentPage {
		readonly MenuViewModel menuViewModel;

		public MenuPage(VocDataStore vocs, App app) {
			InitializeComponent();

			BindingContext = menuViewModel = new MenuViewModel(vocs, app);
		}

		public void OnToggleMode(object sender, ToggledEventArgs e) { menuViewModel.OnToggleMode(sender, e); }
		public void OnToggleStarred(object sender, ToggledEventArgs e) { menuViewModel.OnToggleStarred(sender, e); }
		public void OnL1Changed(object sender, ToggledEventArgs e) { menuViewModel.OnL1Changed(sender, e); }
		public void OnL2Changed(object sender, ToggledEventArgs e) { menuViewModel.OnL2Changed(sender, e); }
		public void OnL3Changed(object sender, ToggledEventArgs e) { menuViewModel.OnL3Changed(sender, e); }
		public void OnL4Changed(object sender, ToggledEventArgs e) { menuViewModel.OnL4Changed(sender, e); }

		public void Refresh() { menuViewModel.Refresh(); }
	}
}