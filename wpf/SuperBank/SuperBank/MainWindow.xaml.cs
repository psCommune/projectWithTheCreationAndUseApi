using SuperBank.DataTransfer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SuperBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QuestionsLoader loader;
        public ObservableCollection<QuestionDTO> Questions { get; set; }
        private SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public MainWindow()
        {
            loader = new QuestionsLoader(Links.Questions);
            Questions = new ObservableCollection<QuestionDTO>();

            timer.Tick += Timer_Tick;

            InitializeComponent();
            DataContext = this;
        }
        private QuestionDTO? getSameQuestionOrDefault(QuestionDTO question)
        {
            return Questions.FirstOrDefault(q => q.QuestionCode == question.QuestionCode);
        }

        private void updateCollection(List<QuestionDTO>? questions)
        {
            if (questions == null)
            {
                return;
            }

            foreach (var question in questions)
            {
                var same = getSameQuestionOrDefault(question);
                if (same != null && question.Window != same.Window)
                {
                    question.IsUpdated = true;
                }
                if (same == null && question.Window != null)
                {
                    question.IsUpdated = true;
                }
            }

            Questions.Clear();
            foreach (var question in questions)
            {
                Questions.Add(question);
            }
        }
        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10),
            IsEnabled = true
        };
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            var list = loader.Load();
            updateCollection(list);
            speech();
        }
        private void speech()
        {
            foreach (QuestionDTO question in Questions.Where(q => q.IsUpdated))
            {
                synthesizer.SpeakAsync($"Клиент {question.QuestionCode} окно {question.Window}");
            }
        }
    }
}
