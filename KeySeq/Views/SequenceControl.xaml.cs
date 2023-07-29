using KeySeq.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace KeySeq.Views
{
    /// <summary>
    /// Interaction logic for SequenceControl.xaml
    /// </summary>
    public partial class SequenceControl : UserControl
    {
        #region Fields

        SequenceControlViewModel _vm = null;
        #endregion Fields


        #region Properties

        public SequenceControlViewModel VM
        {
            get { return _vm; }
        }

        #endregion Properties
        public SequenceControl()
        {
            InitializeComponent();
            _vm = new SequenceControlViewModel(this);
        }
    }
}
