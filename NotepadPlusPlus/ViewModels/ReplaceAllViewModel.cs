using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotepadPlusPlus.Helper;

namespace NotepadPlusPlus.ViewModels
{
    class ReplaceAllViewModel : ViewModelPropertyChange
    {
        private string _valueToBeReplaced;
        public string ValueToBeReplaced
        {
            get { return _valueToBeReplaced; }
            set
            {
                _valueToBeReplaced = value;
                RaisePropertyChanged(nameof(ValueToBeReplaced));
            }
        }


        private string _newValue;
        public string NewValue
        {
            get { return _newValue; }
            set
            {
                _newValue = value; 
                RaisePropertyChanged(nameof(NewValue));
            }
        }
    }
}
