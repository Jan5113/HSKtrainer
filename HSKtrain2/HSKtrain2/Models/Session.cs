using HSKtrain2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HSKtrain2.Models {
    public class Session {
        private VocDataStore MainVocs { get; set; }
        private int[] SessionVocIds;
        private bool[] VocCorr;
        private bool[] VocSeen;
        private int Index;
        private readonly bool Mode;
        private bool SessionWasReset = false;


        public Session(VocDataStore vocs, int num_vocs, bool mode, int set, int[] int_levels, bool starred) {
            MainVocs = vocs;
            Mode = mode;
            SessionVocIds = MainVocs.CreateSessionIdArray(num_vocs, mode, set, int_levels, starred);
            Index = 0;
            VocCorr = new bool[SessionVocIds.Length];
            VocSeen = new bool[SessionVocIds.Length];
        }

        public Voc GetNext() {
            bool looped = false;
            do {
                Index++;
                if (Index >= VocCorr.Length) {
                    if (looped) {
                        return null;
                    } // done
                    Index = 0;
                    looped = true;
                }
            } while (VocCorr[Index]);
            return MainVocs.GetVoc(SessionVocIds[Index]);
        }

        public Voc GetThis() {
            return MainVocs.GetVoc(SessionVocIds[Index]);
        }

        public void ResetSession() {
            Index = 0;
            SessionWasReset = true;
            VocCorr = new bool[SessionVocIds.Length];
            VocSeen = new bool[SessionVocIds.Length];
        }

        public void Response(bool correct) {
            if (correct && !VocSeen[Index]) {
                if (!SessionWasReset) MainVocs.IncreaseScore(SessionVocIds[Index], Mode);
                VocSeen[Index] = VocCorr[Index] = true;
            } else if (correct && VocSeen[Index] && !VocCorr[Index]) {
                VocSeen[Index] = VocCorr[Index] = true;
            } else {
                MainVocs.SetScore(SessionVocIds[Index], 1, Mode);
                VocSeen[Index] = true;
            }
        }

        public bool ToggleStarred() {
            return MainVocs.ToggleStarred(SessionVocIds[Index], Mode);
        }

        public bool GetMode() {
            return Mode;
        }

        public float GetProgress() {
            int c = 0;
            foreach (bool b in VocCorr) {
                if (b) c++;
            }
            return (float)c / (float)VocCorr.Length;
        }

    }
}
