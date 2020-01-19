using System;
using System.Diagnostics;

namespace HSKtrain2.Models {
	public class Voc {
		public int Id { get; set; }
		public string Character { get; set; }
		public string CharacterOld { get; set; }
		public string PinYinAscii { get; set; }
		public string PinYin { get; set; }
		public string Translation { get; set; }
		public int HSKLevel { get; set; }
		public int Score { get; set; }
		public int Starred { get; set; }
		public int WritingScore { get; set; }
		public int WritingStarred { get; set; }
		public Voc(string import) {
			char delimiterChar = '\t';
			string[] fields = import.Split(delimiterChar);	

			this.Id = int.Parse(fields[0]);
			this.Character = fields[1];
			this.CharacterOld = fields[2];
			this.PinYinAscii = fields[3];
			this.PinYin = fields[4];
			this.Translation = fields[5];
			this.HSKLevel = int.Parse(fields[6]);
			this.Score = int.Parse(fields[7]);
			this.Starred = int.Parse(fields[8]);
			this.WritingScore = int.Parse(fields[9]);
			this.WritingStarred = int.Parse(fields[10]);

		}

		public int GetScore(bool mode) {
			if (mode) {
				return WritingScore;
			} else {
				return Score;
			}
		}

		public bool IsStarred(bool mode) {
			if (mode) {
				return WritingStarred == 1;
			} else {
				return Starred == 1;
			}
		}

		public string Export() {
			string[] strOut = { Id.ToString(), Character, CharacterOld, PinYinAscii, PinYin, Translation,
				HSKLevel.ToString(), Score.ToString(), Starred.ToString(), WritingScore.ToString(), WritingStarred.ToString() };
			return string.Join("\t", strOut);
		}

		public Voc Clone() {
			return new Voc(this.Export());
		}
	}
}