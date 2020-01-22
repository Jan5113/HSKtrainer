using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using HSKtrain2.Models;
using HSKtrain2.Views;
using HtmlAgilityPack;
using MediaManager;
using Xamarin.Forms;

namespace HSKtrain2.ViewModels {
    public class TrainingViewModel : BaseViewModel {
        App App;
        Session Session;
        bool SessionOver = false;

        string characterText1 = string.Empty;
        public string CharacterText1 {
            get { return characterText1; }
            set { SetProperty(ref characterText1, (value[0]).ToString()); }
        }

        string characterText2 = string.Empty;
        public string CharacterText2 {
            get { return characterText2; }
            set { 
                if (value.Length > 1) {
                    SetProperty(ref characterText2, (value[1]).ToString());
                } else {
                    SetProperty(ref characterText2, "");
                };}
        }

        string characterText3 = string.Empty;
        public string CharacterText3 {
            get { return characterText3; }
            set {
                if (value.Length > 2) {
                    SetProperty(ref characterText3, (value[2]).ToString());
                } else {
                    SetProperty(ref characterText3, "");
                };
            }
        }

        string characterText4 = string.Empty;
        public string CharacterText4 {
            get { return characterText4; }
            set {
                if (value.Length > 3) {
                    SetProperty(ref characterText4, (value[3]).ToString());
                } else {
                    SetProperty(ref characterText4, "");
                };
            }
        }


        string pinYinText = string.Empty;


        public string PinYinText {
            get { return pinYinText; }
            set { SetProperty(ref pinYinText, value); }
        }

        string translationText = string.Empty;
        public string TranslationText {
            get { return translationText; }
            set { SetProperty(ref translationText, value); }
        }

        string nextButtonText = "Next";
        public string NextButtonText {
            get { return nextButtonText; }
            set { SetProperty(ref nextButtonText, value); }
        }
        string starredText = "";
        public string StarredText {
            get { return starredText; }
            set { SetProperty(ref starredText, value); }
        }

        bool nextButtonVisible = true;
        public bool NextButtonVisible {
            get { return nextButtonVisible; }
            set { SetProperty(ref nextButtonVisible, value); AnswerButtonVisible = !value; }
        }

        bool answerButtonVisible = true;
        public bool AnswerButtonVisible {
            get { return answerButtonVisible; }
            set { SetProperty(ref answerButtonVisible, value); }
        }

        bool characterVisible = true;
        public bool CharacterVisible {
            get { return characterVisible; }
            set {
                characterVisible = value;
                if (value) {
                    CharacterText1 = CharacterText2 = CharacterText3 = CharacterText4 = Character;
                } else {
                    CharacterText1 = CharacterText2 = CharacterText3 = CharacterText4 = " ";
                }
            }
        }

        int characterSize = 120;
        public int CharacterSize {
            get { return characterSize; }
            set { SetProperty(ref characterSize, value); }
        }

        bool pinYinVisible = true;
        public bool PinYinVisible {
            get { return pinYinVisible; }
            set {
                pinYinVisible = value;
                if (value) PinYinText = PinYin;
                else PinYinText = " ";
            }
        }

        bool translationVisible = true;
        public bool TranslationVisible {
            get { return translationVisible; }
            set {
                translationVisible = value;
                if (value) TranslationText = Translation;
                else TranslationText = " ";
            }
        }

        float progress = 0;
        public float Progress {
            get { return progress; }
            set { SetProperty(ref progress, value); }
        }

        string Character = "";
        string PinYin = "";
        string Translation = "";


        public ICommand OnCorrectAnswer { get; }
        public ICommand OnIncorrectAnswer { get; }
        public ICommand OnBackToMenu { get; }
        public ICommand OnNext { get; }
        public ICommand OnToggleStarred { get; }
        public ICommand OnPlayPinYin { get; }
        public ICommand OnChar1 { get; }
        public ICommand OnChar2 { get; }
        public ICommand OnChar3 { get; }
        public ICommand OnChar4 { get; }

        public TrainingViewModel(App app) {
            App = app;
            
            OnCorrectAnswer = new Command(() => { HandleAction(2); });
            OnIncorrectAnswer = new Command(() => { HandleAction(3); });
            OnBackToMenu = new Command(() => {
                Debug.WriteLine("BackToMenu");
                app.SetMenuPage();
            });
            OnNext = new Command(() => {
                HandleAction(1);
            });
            OnToggleStarred = new Command(() => {
                if (Session.ToggleStarred()) StarredText = "★";
                else StarredText = "☆";
            });
            OnPlayPinYin = new Command(() => {
                if (PinYinVisible) {
                    PlaySound(Session.GetThis());
                }
            });
            OnChar1 = new Command(() => {
                Debug.WriteLine("OnChar1");
                if (CharacterVisible) GetDefinition((Character[0]).ToString());
            });
            OnChar2 = new Command(() => {
                Debug.WriteLine("OnChar2");
                if (CharacterVisible) GetDefinition((Character[1]).ToString());
            });
            OnChar3 = new Command(() => {
                Debug.WriteLine("OnChar3");
                if (CharacterVisible) GetDefinition((Character[2]).ToString());
            });
            OnChar4 = new Command(() => {
                Debug.WriteLine("OnChar4");
                if (CharacterVisible) GetDefinition((Character[3]).ToString());
            });
        }

        public void HandleAction(int action) { // 0 = new, 1 = next, 2 = correct, 3 = incorrect
            switch (action) {
                case 0:
                    SessionOver = false;
                    SetNextVoc();
                    if (Session.GetMode()) { // write
                        CharacterVisible = false;
                        PinYinVisible = false;
                        TranslationVisible = true;
                    } else { // read
                        CharacterVisible = true;
                        PinYinVisible = false;
                        TranslationVisible = false;
                    }
                    NextButtonVisible = true;
                    break;
                case 1:
                    if (!PinYinVisible) {
                        PinYinVisible = true;
                        NextButtonVisible = true;
                    } else {
                        if (Session.GetMode()) { // write
                            CharacterVisible = true;
                        } else { // read
                            TranslationVisible = true;
                        }
                        NextButtonVisible = false;
                    }
                    break;
                case 2:
                    if (SessionOver) { ResetSession(true); return; };
                    Session.Response(true);
                    SetNextVoc();
                    if (SessionOver) return;
                    if (Session.GetMode()) { // write
                        CharacterVisible = false;
                        PinYinVisible = false;
                        TranslationVisible = true;
                    } else { // read
                        CharacterVisible = true;
                        PinYinVisible = false;
                        TranslationVisible = false;
                    }
                    NextButtonVisible = true;
                    break;
                case 3:
                    if (SessionOver) { ResetSession(false); return; };
                    Session.Response(false);
                    SetNextVoc();
                    if (Session.GetMode()) { // write
                        CharacterVisible = false;
                        PinYinVisible = false;
                        TranslationVisible = true;
                    } else { // read
                        CharacterVisible = true;
                        PinYinVisible = false;
                        TranslationVisible = false;
                    }
                    NextButtonVisible = true;
                    break;
            }
        }

        public void ResetSession(bool reset) {
            App.SaveProgress();
            if (reset) {
                Session.ResetSession();
                HandleAction(0);
                Progress = 0;
            } else {
                Session = null;
                App.SetMenuPage();
            }
        }

        public void SetNextVoc() {
            Voc v = Session.GetNext();
            if (v == null) {
                Character = "✓";
                PinYin = "Congrats!";
                Translation = "You're done with this session! Repeat?";
                SessionOver = CharacterVisible = PinYinVisible = TranslationVisible = true;
                NextButtonVisible = false;
                StarredText = "";
                Progress = Session.GetProgress();
                return;
            }
            Character = v.Character;
            Translation = v.Translation;
            PinYin = v.PinYin;
            switch (Character.Length) {
                case 0:
                case 1:
                case 2:
                    CharacterSize = 120;
                    break;
                case 3:
                    CharacterSize = 100;
                    break;
                case 4:
                    CharacterSize = 80;
                    break;

            }
            if (v.IsStarred(Session.GetMode())) StarredText = "★";
            else StarredText = "☆";
            Progress = Session.GetProgress();
        }

        public async void PlaySound(Voc v) {
            string searchString = "https://chinese.yabla.com/chinese-english-pinyin-dictionary.php?define=" + v.Character;
            try {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(searchString);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//body/section/div/ul[@id='search_results']/li/div/span[@class='word']");
                if (nodes != null) {
                    foreach (HtmlNode node in nodes) {
                        HtmlNodeCollection wordParse = node.SelectNodes(".//a");
                        string word = "";
                        foreach (HtmlNode singleParse in wordParse) {
                            word += singleParse.InnerText;
                        }
                        if (word == v.Character) {
                            await CrossMediaManager.Current.Play(node.SelectSingleNode(".//i").Attributes["data-audio_url"].Value);
                            break;
                        }
                    }
                }
            } catch { }

        }

        public void GetDefinition(string c) {
            List<CharDef> list = new List<CharDef>();
            string searchString = "https://chinese.yabla.com/chinese-english-pinyin-dictionary.php?define=" + c;
            try {
                Debug.WriteLine(searchString);
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(searchString);
                string definition = "";
                string pinYin = "";
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//body/section/div/ul[@id='search_results']/li[not(@class='promo')]");

                if (nodes != null) {
                    foreach (HtmlNode node in nodes) {
                        HtmlNodeCollection wordParse = node.SelectNodes(".//div[contains(@class, 'term')]/span[@class='word']/a");
                        HtmlNode pinYinParse = node.SelectSingleNode(".//div[@class='definition']/span[@class='pinyin']");
                        HtmlNode meaningParse = node.SelectSingleNode(".//div[@class='definition']/div[@class='meaning']");
                        string word = "";
                        if (wordParse != null) {
                            foreach (HtmlNode singleParse in wordParse) {
                                word += singleParse.InnerText;
                            }
                        }
                        if (pinYinParse != null) pinYin = pinYinParse.InnerText;
                        if (meaningParse != null) definition = meaningParse.InnerText;
                        list.Add(new CharDef { Char = word, PinYin = pinYin, Definition = definition });
                    }
                }

                App.SetDefinitionPage(new DefinitionPage(this, list.ToArray()));
            } catch {

            }
        }


        public void SetTrainPage() {
            App.SetTrainPage();
        }

        public void SetSession(Session session) {
            Session = session;
            HandleAction(0);
        }

        public bool HasActiveSession() {
            return Session != null;
        }
    }
}
