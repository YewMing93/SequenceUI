using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KeySeq.Common
{
    public abstract class ViewModelBase<TView> : BindableBase where TView : ContentControl, new()
    {
        /// <summary>
        /// Main View
        /// </summary>
        public TView View { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public ViewModelBase(TView view)
        {
            View = view;
            View.DataContext = this;
        }
    }
}
