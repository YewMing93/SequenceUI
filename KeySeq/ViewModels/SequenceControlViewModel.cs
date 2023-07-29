using KeySeq.Common;
using KeySeq.Models;
using KeySeq.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsInput;

namespace KeySeq.ViewModels
{
    public class SequenceControlViewModel : ViewModelBase<SequenceControl>
    {
        #region Fields
        private ObservableCollection<Sequence> _sequences = new ObservableCollection<Sequence>();
        private InputSimulator inputSim = new InputSimulator();
        private ICommand _insertCommand = null;
        private bool enableInset = false;
        private static int _sequenceCreated = 1;
        string _filename = String.Empty;
        #endregion

        #region Properties

        public bool ThreadStart
        {
            get; set;
        }

        public bool Start
        {
            get; set;
        }

        public bool EnableInsert
        {
            get
            {
                return enableInset;
            }
            set
            {
                enableInset = value;
                RaisePropertyChanged(nameof(EnableInsert));
            }
        }
        public bool SequenceAddEnable
        {
            get; set;
        }

        public string SequenceAddKey
        {
            get; set;
        }

        public string SequenceAddDuration
        {
            get; set;
        }

        public string SequenceAddDir
        {
            get; set;
        }

        public ICommand InsertCommand
        {
            get { return _insertCommand; }
            set { _insertCommand = value; }
        }
        
        public ObservableCollection<Sequence> Sequences
        {
            get { return _sequences; }
        }

        public string SequenceName
        {
            get; set;
        }
        #endregion Properties

        public SequenceControlViewModel(SequenceControl view) : base(view)
        {
            ThreadStart = true;
            // Create a thread and call a background method
            Thread backgroundThread = new Thread(new ThreadStart(ThreadRunSequence));
            // Start thread
            backgroundThread.Start();
            SequenceName = "Sequence " + _sequenceCreated++;
            InsertCommand = new DelegateCommand(InsertSequence);
            _filename = SequenceName + ".txt";
            LoadSequence();
            SetDeaultInsertVal();
        }

        public void ThreadRunSequence()
        {
            while(ThreadStart)
            {
                EnableInsert = !Start;
                if (Start)
                {
                    for(int i = 0; i < _sequences.Count && Start; ++i)
                    {
                        //Direction
                        if (_sequences[i].DirectionToFace == Direction.Left)
                        {
                            //System.Windows.Forms.SendKeys.Send("{LEFT}");
                            inputSim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
                        }
                        else
                        {
                            //System.Windows.Forms.SendKeys.Send("{RIGHT}");
                            inputSim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);
                        }
                        Thread.Sleep(50);
                        inputSim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_A + (char.ToLower(_sequences[i].KeyToPress) - 'a'));
                        inputSim.Keyboard.Sleep(_sequences[i].DurationKeyToPress);
                        inputSim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_A + (char.ToLower(_sequences[i].KeyToPress) - 'a'));
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void InsertSequence()
        {
            _sequences.Add(new Sequence()
            {
                DirectionToFace = IsStringValid(SequenceAddDir) ? (Direction)int.Parse(SequenceAddDir) : Direction.Right,
                Enabled = SequenceAddEnable,
                DurationKeyToPress = IsStringValid(SequenceAddDuration) ? int.Parse(SequenceAddDuration) : 100,
                KeyToPress = IsStringValid(SequenceAddDuration) ? SequenceAddKey[0] : ' '
            });
            SaveSequence();
            SetDeaultInsertVal();
        }

        private bool IsStringValid(string check)
        {
            return !(string.IsNullOrEmpty(check) || string.IsNullOrWhiteSpace(check));
        }

        private void EnableDisableInsert()
        {
            this.View.CheckBoxEnable.IsEnabled = !Start;
            this.View.TextBoxAddKey.IsEnabled = !Start;
            this.View.TextBoxAddDir.IsEnabled = !Start;
            this.View.TextBoxAddDuration.IsEnabled = !Start;
            this.View.ButtonInsert.IsEnabled = !Start;
        }

        private void SaveSequence()
        {
            using (var fs = new StreamWriter(_filename, false))
            {
                fs.WriteLine(SequenceName);
                foreach (var s in _sequences)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(nameof(s.Enabled));
                    sb.Append(":");
                    sb.Append(s.Enabled);

                    sb.Append(",");

                    sb.Append(nameof(s.DirectionToFace));
                    sb.Append(":");
                    sb.Append(s.DirectionToFace == Direction.Left? "1" : "2");

                    sb.Append(",");

                    sb.Append(nameof(s.DurationKeyToPress));
                    sb.Append(":");
                    sb.Append(s.DurationKeyToPress);

                    sb.Append(",");

                    sb.Append(nameof(s.KeyToPress));
                    sb.Append(":");
                    sb.Append(s.KeyToPress);

                    fs.WriteLine(sb);
                }
            }
        }

        private void LoadSequence()
        {
            if(!File.Exists(_filename))
            {
                return;
            }
            IEnumerable<string> lines = File.ReadLines(_filename);
            foreach (var line in lines)
            {
                Sequence newSequence = new Sequence();
                List<string> sequence = line.Split(',').ToList();

                if(sequence.Count() == 4)
                {
                    newSequence.Enabled             = bool.Parse(sequence[0].Split(':').ToList()[1]);
                    newSequence.DirectionToFace     = (Direction)int.Parse(sequence[1].Split(':').ToList()[1]);
                    newSequence.DurationKeyToPress  = int.Parse(sequence[2].Split(':').ToList()[1]);
                    newSequence.KeyToPress          = char.Parse(sequence[3].Split(':').ToList()[1]);
                    _sequences.Add(newSequence);
                }
            }
        }

        private void SetDeaultInsertVal()
        {
            SequenceAddDir = "2";
            SequenceAddEnable = true;
            SequenceAddDuration = "200";
            SequenceAddKey = "a";
        }
    }
}
