using KeySeq.Common;
using KeySeq.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeySeq.ViewModels
{
    /// <summary>
    /// View model for MainWindowViewModel
    /// </summary>
    public class MainWindowViewModel : ViewModelBase<MainWindow>
    {
        #region Fields
        private ObservableCollection<Sequence> _sequencesFarm = new ObservableCollection<Sequence>();
        private ObservableCollection<Sequence> _sequencesBuff = new ObservableCollection<Sequence>();
        private bool _start = false;
        private string _startButtonContent = string.Empty;
        private ICommand _startCommand = null;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Get SequencesFarm
        /// </summary>
        public ObservableCollection<Sequence> SequencesFarm
        {
            get { return _sequencesFarm; }
        }

        /// <summary>
        /// Get SequencesBuff
        /// </summary>
        public ObservableCollection<Sequence> SequencesBuff
        {
            get { return _sequencesBuff; }
        }

        public ICommand StartCommand
        {
            get { return _startCommand; }
            set { _startCommand = value; }
        }

        public string StartButtonContent
        {
            get
            {
                return _startButtonContent;
            }
            set
            {
                _startButtonContent = value;
                RaisePropertyChanged(nameof(StartButtonContent));
            }
        }
        #endregion Properties

        /// <summary>
        /// Constructor with a view
        /// </summary>
        /// <param name="view"></param>
        public MainWindowViewModel(MainWindow view) : base(view)
        {
            StartCommand = new DelegateCommand(StartSequences);
            StartButtonContent = "Start";
            this.View.KeyDown += View_KeyDown;
            this.View.Closed += View_Closed;
        }

        private void View_Closed(object sender, EventArgs e)
        {
            this.View.SequenceControl1.VM.ThreadStart = false;
            this.View.SequenceControl2.VM.ThreadStart = false;
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            _start = true; //Set to true so that the StartSequences will reset
            StartSequences();
        }

        public void StartSequences()
        {
            _start = !_start;
            if(_start)
            {
                StartButtonContent = "Starting in 3";
                Thread backgroundThread = new Thread(new ThreadStart(HoldStartEndSequence));
                backgroundThread.Start();
            }
            else
            {
                StartEndSequences();
            }
        }

        public void HoldStartEndSequence()
        {
            Thread.Sleep(3000);
            StartEndSequences();
        }

        public void StartEndSequences()
        {
            this.View.SequenceControl1.VM.Start = _start;
            this.View.SequenceControl2.VM.Start = _start;
            if (_start)
            {
                StartButtonContent = "Stop";
            }
            else
            {
                StartButtonContent = "Start";
            }
        }
    }
}
