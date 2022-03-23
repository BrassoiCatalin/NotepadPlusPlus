using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotepadPlusPlus.Helper;

namespace NotepadPlusPlus.ViewModels
{
    class FindViewModel : ViewModelPropertyChange
    {
        private string _findValue;
        public string FindValue
        {
            get { return _findValue; }
            set
            {
                _findValue = value;
                RaisePropertyChanged(nameof(FindValue));
            }
        }
    }
}
