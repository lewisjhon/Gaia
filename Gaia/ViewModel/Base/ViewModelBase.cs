using Gaia.Helper;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.ViewModel.Base
{
    public class ViewModelBase : BrowseForFolderDialog, INotifyPropertyChanged
    {

        protected UITaskHelper _uiTaskHelper;
        protected TaskScheduler _uiScheduler;
        
        public ViewModelBase()
        {
            _uiTaskHelper = new UITaskHelper();
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }

        protected string ShowOpenFileDialog(string filterText)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = filterText;

            string filePath = string.Empty;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }

            return filePath;
        }

        protected string ShowOpenFolderDialog(string defaultFolder)
        {
            base.Title = "폴더 선택";
            base.InitialExpandedFolder = defaultFolder;
            base.OKButtonText = "OK";

            if (base.ShowDialog() == true)
                return base.SelectedFolder;
            else
                return string.Empty;
        }

        public static async void ShowMessageOkOnly(string title, string message, string okText = "확인")
        {
            var dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = okText,
                AnimateShow = true,
                AnimateHide = false
            };

            await ((MetroWindow)App.Current.MainWindow).ShowMessageAsync(
                title
                , message
                , MessageDialogStyle.Affirmative
                , dialogSettings); ;
        }

        #endregion Methods
    }
}
