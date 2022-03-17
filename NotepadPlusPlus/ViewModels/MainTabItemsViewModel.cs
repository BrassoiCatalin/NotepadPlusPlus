using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// Will hold the tab items
        /// </summary>
        public ObservableCollection<TabItem> Items { get; set; }

        private TabItem _selectedItem;

        private Dictionary<TabItem, string> _itemsPath = new Dictionary<TabItem, string>();

        public TabItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
        }
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
        }


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

        public ICommand AddNewItemCommand { get; }
        public ICommand AboutCommand { get; }

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

        public ICommand OpenFileCommand { get; }
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

        public ICommand SaveAsCommand { get; }

        private void SaveAsFile(object commandParameter)
        {
            if (SelectedItem == null)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, ((TextBoxCustom)_selectedItem.Content).Text);
                _itemsPath[_selectedItem] = saveFileDialog.FileName;
                _selectedItem.Header = saveFileDialog.SafeFileName;
            }

            ((TextBoxCustom)SelectedItem.Content).TextWasSaved = true;
        }

        public ICommand SaveCommand { get; }
        private void SaveFile(object commandParameter)
        {
            if (SelectedItem == null)
                return;

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

        public ICommand CloseFileCommand { get; }

        private void CloseFile(object commandParameter)
        {
            if (SelectedItem.Header.ToString().EndsWith("*"))
            {
                MessageBoxResult result = MessageBox.Show("Do you first want to save?", "Remove", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(null);
                }
            }

            _itemsPath.Remove(SelectedItem);
            Items.Remove(SelectedItem);
        }
    }
}
