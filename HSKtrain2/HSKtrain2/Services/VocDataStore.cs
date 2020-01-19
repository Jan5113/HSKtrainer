using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HSKtrain2.Models;

namespace HSKtrain2.Services {
	public class VocDataStore {
		private readonly Voc[] Vocs;
		private readonly string FilePath;
		private Boolean Changed;
		private readonly Random rng = new Random();

		public VocDataStore(string filePath_in) {
			FilePath = filePath_in;
			string wholeFile = System.IO.File.ReadAllText(FilePath);
			string[] lines = wholeFile.Split('\n');
			Vocs = new Voc[lines.Length];
			for (int i = 0; i < lines.Length; i++) {
				Vocs[i] = new Voc(lines[i]);
			}
			Changed = false;
		}
		public void ExportVocDataStore() {
			if (!Changed) return;
			string[] lines = new string[Vocs.Length];
			for (int i = 0; i < Vocs.Length; i++) {
				lines[i] = Vocs[i].Export();
			}
			string wholeFile = string.Join("\n", lines);
			System.IO.File.WriteAllText(FilePath, wholeFile);
		}

		public Voc GetVoc(int index) {
			return Vocs[index].Clone();
		}

		public void SetScore(int index, int score, bool mode) {
			if (mode) {
				Vocs[index].WritingScore = score;
			} else {
				Vocs[index].Score = score;
			}
			Changed = true;
		}

		public void IncreaseScore(int index, bool mode) {
			if (mode) {
				if (Vocs[index].WritingScore < 5) {
					Vocs[index].WritingScore += 1;
					if (Vocs[index].WritingScore == 1) Vocs[index].WritingScore += 1;
				}
			} else {
				if (Vocs[index].Score < 5) Vocs[index].Score += 1;
				if (Vocs[index].Score == 1) Vocs[index].Score += 1;
			}
			Changed = true;
		}

		public bool ToggleStarred(int index, bool mode) {
			if (mode) {
				if (Vocs[index].WritingStarred == 0) {
					Vocs[index].WritingStarred = 1;
					return true;
				} else {
					Vocs[index].WritingStarred = 0;
					return false;
				}
			} else {
				if (Vocs[index].Starred == 0) {
					Vocs[index].Starred = 1;
					return true;
				} else {
					Vocs[index].Starred = 0;
					return false;
				}
			}
		}

		public int GetSize() {
			return Vocs.Length;
		}

		public int[] CreateSessionIdArray(int size, bool mode, int set, int[] levels, bool starred) {
			Debug.WriteLine("creating set with levels " + string.Join(", ", levels));
			List<int> tempList = new List<int>();
			foreach (Voc v in Vocs) {
				if (v.GetScore(mode) == set && levels.Contains(v.HSKLevel)) {
					if ((starred && v.IsStarred(mode)) || !starred) tempList.Add(v.Id);
				}
			}
			Shuffle(tempList);
			return tempList.Take(size).ToArray();
		}
		private void Shuffle(List<int> list) {
			int n = list.Count;
			while (n > 1) {
				n--;
				int k = rng.Next(n + 1);
				int value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public int[] GetSetInfo(bool mode, bool starred, int[] levels) {
			int[] info = { 0, 0, 0, 0, 0, 0 };
			foreach (Voc v in Vocs) {
				if (levels.Contains(v.HSKLevel) && ((starred && v.IsStarred(mode)) || !starred)) {
					info[v.GetScore(mode)] += 1;
				}
			}
			return info;
		}
	}
}