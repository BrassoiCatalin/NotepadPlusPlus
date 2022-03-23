using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using NotepadPlusPlus.Helper;
using NotepadPlusPlus.Models;
using NotepadPlusPlus.Views;

namespace NotepadPlusPlus.ViewModels
{
    public class MainTabItemsViewModel : ViewModelPropertyChange
    {
        #region Members

        public ObservableCollection<TabItem> Items { get; set; }

        private Dictionary<TabItem, string> _itemsPath = new Dictionary<TabItem, string>();

        private TabItem _selectedItem;
        public TabItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
        }

        #endregion

        #region Constructor

        public MainTabItemsViewModel()
        {
            Items = new ObservableCollection<TabItem>();
            AddNewTabItem("", "File " + (Items.Count + 1), "");

            AddNewItemCommand = new RelayCommand(MenuItemClicked);
            AboutCommand = new RelayCommand(AboutClicked);
            OpenFileCommand = new RelayCommand(OpenFile);
            SaveAsCommand = new RelayCommand(SaveAsFile);
            SaveCommand = new RelayCommand(SaveFile);
            CloseFileCommand = new RelayCommand(CloseFile);
            FindCommand = new RelayCommand(Find);
            ReplaceAllCommand = new RelayCommand(ReplaceAll);
            ExitProgramCommand = new RelayCommand(ExitProgram);
            ReplaceCommand = new RelayCommand(Replace);
        }

        #endregion

        #region Commands
        public ICommand AddNewItemCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand FindCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseFileCommand { get; }
        public ICommand ReplaceAllCommand { get; }
        public ICommand ExitProgramCommand { get;  }
        public ICommand ReplaceCommand { get; }

        #endregion

        public void AddNewTabItem(string text, string header, string path)
        {
            TabItem tabItem = new TabItem();
            TextBoxCustom textBox = new TextBoxCustom
            {
                Text = text,
            };

            tabItem.Header = header;
            tabItem.Content = textBox;

            Items.Add(tabItem);
            _itemsPath.Add(tabItem, path);
            SelectedItem = tabItem;
        }


        private string _differentString = "";
        private int _currentPosition = -1;
        private void Find(object obj)
        {
            var findWindow = new FindWindow();
            findWindow.ShowDialog();

            var textToFind = ((FindViewModel)findWindow.DataContext).FindValue;
            var currentText = ((TextBox)SelectedItem.Content).Text;

            if (textToFind == null)
                return;

            if (textToFind != _differentString)
            {
                _differentString = textToFind;
                _currentPosition = currentText.IndexOf(textToFind, 0, StringComparison.Ordinal);
            }
            else
            {
                _currentPosition = currentText.IndexOf(textToFind, _currentPosition + 1, StringComparison.Ordinal);
            }

            if (_currentPosition == -1)
            {
                ((TextBox)SelectedItem.Content).Select(0, 0);
                MessageBox.Show("Item Not Found");
            }
            else
            {
                ((TextBox)SelectedItem.Content).Select(_currentPosition, textToFind.Length);
            }
        }

        private void Replace(object obj)
        {
            var replaceWindow = new ReplaceWindow();
            replaceWindow.ShowDialog();

            var oldValue = ((ReplaceViewModel)replaceWindow.DataContext).ValueToBeReplacedOnce;
            var newValue = ((ReplaceViewModel)replaceWindow.DataContext).NewValueOnce;

            if (oldValue == null || newValue == null)
                return;

            if(((TextBox)SelectedItem.Content).Text.Contains(oldValue))
            {
                ((TextBox)SelectedItem.Content).Text =
                    ((TextBox)SelectedItem.Content).Text.Remove(((TextBox)SelectedItem.Content).Text.IndexOf(oldValue)) 
                    + newValue
                    + ((TextBox)SelectedItem.Content).Text.Substring
                        (((TextBox)SelectedItem.Content).Text.IndexOf(oldValue) + oldValue.Length);
            }
        }

        private void ReplaceAll(object obj)
        {
            var replaceAllWindow = new ReplaceAllWindow();
            replaceAllWindow.ShowDialog();

            var oldValue = ((ReplaceAllViewModel)replaceAllWindow.DataContext).ValueToBeReplaced;
            var newValue = ((ReplaceAllViewModel)replaceAllWindow.DataContext).NewValue;

            if (oldValue == null || newValue == null)
                return;

            ((TextBox)SelectedItem.Content).Text = ((TextBox)SelectedItem.Content).Text.Replace(oldValue, newValue);
        }


        private int _index = 1;
        private void MenuItemClicked(object obj)
        {
            _index++;
            AddNewTabItem("", "File " + _index, "");
        }


        private void AboutClicked(object obj)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }


        private void OpenFile(object commandParameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                var content = File.ReadAllText(openFileDialog.FileName);

                var fileName = openFileDialog.SafeFileName;
                AddNewTabItem(content, fileName, openFileDialog.FileName);

            }

            ((TextBoxCustom)SelectedItem.Content).TextWasSaved = true;
        }


        private void SaveAsFile(object commandParameter)
        {
            if (SelectedItem == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, ((TextBoxCustom)_selectedItem.Content).Text);
                _itemsPath[_selectedItem] = saveFileDialog.FileName;
                _selectedItem.Header = saveFileDialog.SafeFileName;
            }

            ((TextBoxCustom)SelectedItem.Content).TextWasSaved = true;
        }


        private void SaveFile(object commandParameter)
        {
            if (SelectedItem == null)
            {
                return;
            }

            if (_itemsPath[_selectedItem] == "")
            {
                SaveAsFile(null);
                return;
            }

            File.WriteAllText(_itemsPath[_selectedItem], ((TextBox)_selectedItem.Content).Text);
            if (SelectedItem.Header.ToString().EndsWith("*"))
                SelectedItem.Header = SelectedItem.Header.ToString().Remove(SelectedItem.Header.ToString().Length - 1);
            ((TextBoxCustom)SelectedItem.Content).TextWasSaved = true;
        }


        private void CloseFile(object commandParameter)
        {
            if (SelectedItem.Header.ToString().EndsWith("*"))
            {
                var result = MessageBox.Show("Do you first want to save?", "Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(null);
                }
            }
            _itemsPath.Remove(SelectedItem);
            Items.Remove(SelectedItem);
        }


        private void ExitProgram(object commandParameter)
        {
            Environment.Exit(0);
        }
    }
}
