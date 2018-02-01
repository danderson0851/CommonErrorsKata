using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonErrors.Shared;
using CommonErrorsKata.Properties;

namespace CommonErrorsKata
{
    public partial class CommonErrorsForm : Form
    {
        private readonly string[] _files;
        private readonly SynchronizationContext _synchronizationContext;
        private int _i = 100;
        private string _visibleImagePath;
        private readonly string[] _possibleAnswers;
        private readonly int _maxAnswers = 15;

        public AnswerQueue<TrueFalseAnswer> AnswerQueue { get; }

        public CommonErrorsForm()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
            _files = Directory.GetFiles(Environment.CurrentDirectory +  @"..\..\..\ErrorPics");
            _possibleAnswers = _files.Select(f => Path.GetFileName(f)?.Replace(".png", "")).ToArray();
            lstAnswers.DataSource = _possibleAnswers;
            AnswerQueue = new AnswerQueue<TrueFalseAnswer>(15);
            Next();
            lstAnswers.Click += LstAnswers_Click;
            StartTimer();
        }
        private async void StartTimer()
        {
            await Task.Run(() =>
            {
                for (_i = 100; _i > 0; _i--)
                {
                    UpdateProgress(_i);
                    Thread.Sleep(50);
                }
                Message("Need to be quicker on your feet next time!  Try again...");
            });
        }

        private void LstAnswers_Click(object sender, EventArgs e)
        {
            _i = 100;
            var selected = _possibleAnswers[lstAnswers.SelectedIndex];
            if (selected != null && selected == _visibleImagePath)
            {
                AnswerQueue.Enqueue(new TrueFalseAnswer(true));
            }
            else
            {
                AnswerQueue.Enqueue(new TrueFalseAnswer(false));
            }



            Next();
        }
        private void Next()
        {
            if (AnswerQueue.Count == _maxAnswers && AnswerQueue.Grade >= 98)
            {
                MessageBox.Show(Resources.CommonErrorsForm_Next_Congratulations_you_ve_defeated_me_);
                Application.Exit();
                return;
            }
            label1.Text = AnswerQueue.Grade + Resources.CommonErrorsForm_Next__;
            var file = _files.GetRandom();
            _visibleImagePath = Path.GetFileName(file)?.Replace(".png","");
            pbImage.ImageLocation = file;
        }

        public void UpdateProgress(int value)
        {
            _synchronizationContext.Post(x => {
                progress.Value = value;
            }, value);
        }
        public void Message(string value)
        {
            _synchronizationContext.Post(x => {
                MessageBox.Show(value);
            }, value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
