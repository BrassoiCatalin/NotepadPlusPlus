using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using NotepadPlusPlus.Annotations;

namespace NotepadPlusPlus.Models
{
    public class TextBoxCustom : TextBox
    {
        public bool TextWasSaved { get; set; } = true;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (this.Parent == null)
                return;

            if (TextWasSaved)
            {
                ((TabItem)this.Parent).Header = ((TabItem)this.Parent).Header + "*";
                TextWasSaved = false;
            }

            base.OnTextChanged(e);
        }
    }
}