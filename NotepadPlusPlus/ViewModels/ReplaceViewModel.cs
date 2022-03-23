using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotepadPlusPlus.Helper;

namespace NotepadPlusPlus.ViewModels
{
    class ReplaceViewModel : ViewModelPropertyChange
    {
        private string _valueToBeReplaced;

        public string ValueToBeReplacedOnce
        {
            get { return _valueToBeReplaced; }
            set
            {
                _valueToBeReplaced = value;
                RaisePropertyChanged(nameof(ValueToBeReplacedOnce));
            }
        }



        private string _newValue;

        public string NewValueOnce
        {
            get { return _newValue; }
            set
            {
                _newValue = value; 
                RaisePropertyChanged(nameof(NewValueOnce));
            }
        }
    }
}
