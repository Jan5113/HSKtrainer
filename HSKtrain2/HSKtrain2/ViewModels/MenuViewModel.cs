using HSKtrain2.Models;
using HSKtrain2.Services;
using MediaManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace HSKtrain2.ViewModels {
	public class MenuViewModel : BaseViewModel {

		readonly VocDataStore Vocs;
		readonly App App;
		bool[] CheckedLevels = { true, true, true, true };
		bool Mode = false;
		bool Starred = false;

		public ICommand Train { get; }
		public ICommand StepperLess { get; }
		public ICommand StepperMore { get; }
		public ICommand StepperHalf { get; }
		public ICommand StepperAll { get; }
		public ICommand OnSet0 { get; }
		public ICommand OnSet1 { get; }
		public ICommand OnSet2 { get; }
		public ICommand OnSet3 { get; }
		public ICommand OnSet4 { get; }
		public ICommand OnSet5 { get; }

		int selectedSet = 0;
		public int SelectedSet {
			get { return selectedSet; }
			set { SetProperty(ref selectedSet, value); }
		}

		string starredText = string.Empty;
		public string StarredText {
			get { return starredText; }
			set { SetProperty(ref starredText, value); }
		}

		string modeText = string.Empty;
		public string ModeText {
			get { return modeText; }
			set { SetProperty(ref modeText, value); }
		}

		float progress0 = 0;
		public float Progress0 {
			get { return progress0; }
			set { SetProperty(ref progress0, value); }
		}

		float progress1 = 0;
		public float Progress1 {
			get { return progress1; }
			set { SetProperty(ref progress1, value); }
		}

		float progress2 = 0;
		public float Progress2 {
			get { return progress2; }
			set { SetProperty(ref progress2, value); }
		}

		float progress3 = 0;
		public float Progress3 {
			get { return progress3; }
			set { SetProperty(ref progress3, value); }
		}

		float progress4 = 0;
		public float Progress4 {
			get { return progress4; }
			set { SetProperty(ref progress4, value); }
		}

		float progress5 = 0;
		public float Progress5 {
			get { return progress5; }
			set { SetProperty(ref progress5, value); }
		}

		int numVocs = 0;
		public int NumVocs {
			get { return numVocs; }
			set { 
				SetProperty(ref numVocs, value);
				if (numVocs <= 0) TrainEnabled = false;
				else TrainEnabled = true;
			}
		}

		bool trainEnabled = true;
		public bool TrainEnabled {
			get { return trainEnabled; }
			set { SetProperty(ref trainEnabled, value); }
		}
		public MenuViewModel(VocDataStore vocs_in, App app) {
			App = app;
			Debug.WriteLine("INIT MenuViewModel");
			Vocs = vocs_in;
			Starred = Mode = false;
			StarredText = "☆";
			ModeText = "👀";
			Refresh();

			Train = new Command(() => {
				Debug.WriteLine("start session");
				Session session = new Session(Vocs, NumVocs, Mode, SelectedSet, ToIntLevels(), Starred);
				app.SetSession(session);
				app.SetTrainPage();

			});
			StepperLess = new Command(() => {
				Debug.WriteLine("less");
				if (NumVocs > 1) {
					NumVocs -= 1;
				}
			});

			StepperMore = new Command(() => {
				Debug.WriteLine("more");
				NumVocs += 1;
			});

			StepperHalf = new Command(() => {
				Debug.WriteLine("half");
				NumVocs = (int)Math.Ceiling((double)NumVocs / 2d);
			});

			StepperAll = new Command(() => {
				Debug.WriteLine("all");
				NumVocs = Vocs.GetSetInfo(Mode, Starred, ToIntLevels())[SelectedSet];
			});

			OnSet0 = new Command(() => {
				SelectedSet = 0;
				Refresh();
			});
			OnSet1 = new Command(() => {
				SelectedSet = 1;
				Refresh();
			});
			OnSet2 = new Command(() => {
				SelectedSet = 2;
				Refresh();
			});
			OnSet3 = new Command(() => {
				SelectedSet = 3;
				Refresh();
			});
			OnSet4 = new Command(() => {
				SelectedSet = 4;
				Refresh();
			});
			OnSet5 = new Command(() => {
				SelectedSet = 5;
				Refresh();
			});
		}

		public void Refresh() {
			int[] info = Vocs.GetSetInfo(Mode, Starred, ToIntLevels());
			int total = Vocs.GetSize();
			Progress0 = (float) info[0] / (float) total;
			Progress1 = (float) info[1] / (float) total;
			Progress2 = (float)info[2] / (float)total;
			Progress3 = (float)info[3] / (float)total;
			Progress4 = (float)info[4] / (float)total;
			Progress5 = (float)info[5] / (float)total;
			if (SelectedSet == 0 && info[0] > 15) {
				NumVocs = 15;
			} else {
				NumVocs = info[SelectedSet];
			}

		}

		public void OnToggleStarred(object sender, ToggledEventArgs e) {
			Starred = e.Value;
			if (Starred) {
				StarredText = "★";
			} else {
				StarredText = "☆";
			}
			Refresh();
		}
		public void OnToggleMode(object sender, ToggledEventArgs e) {
			Mode = e.Value;
			if (Mode) {
				ModeText = "✍";
			} else {
				ModeText = "👀";
			}
			Refresh();
		}     
		public void OnL1Changed(object sender, ToggledEventArgs e) {
			Debug.WriteLine("L1");
			CheckedLevels[0] = e.Value;
			Refresh();
		}             
		public void OnL2Changed(object sender, ToggledEventArgs e) {
			Debug.WriteLine("L2");
			CheckedLevels[1] = e.Value;
			Refresh();
		}             
		public void OnL3Changed(object sender, ToggledEventArgs e) {
			Debug.WriteLine("L3");
			CheckedLevels[2] = e.Value;
			Refresh();
		}
		public void OnL4Changed(object sender, ToggledEventArgs e) {
			Debug.WriteLine("L4");
			CheckedLevels[3] = e.Value;
			Refresh();
		}

		private int[] ToIntLevels() {
			List<int> int_levels = new List<int>();
            for (int i = 0; i< 4; i++) {
                if (CheckedLevels[i]) {
                    int_levels.Add(i + 1);
                }
			}
			return int_levels.ToArray();
		}

	}               
}                   
					
					