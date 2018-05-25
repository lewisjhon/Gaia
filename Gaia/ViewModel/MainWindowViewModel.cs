using Gaia.Controls;
using Gaia.Helper;
using Gaia.Model;
using Gaia.View;
using Gaia.ViewModel.Base;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Gaia.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Property
        private bool _IsShowLoading = false;
        public bool IsShowLoading
        {
            get
            {
                return _IsShowLoading;
            }
            set
            {
                _IsShowLoading = value;
                OnPropertyChanged("IsShowLoading");
            }
        }

        private int _progressValue = 0;
        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        private string _path = string.Empty;
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }
        private string _watermarkText = string.Empty;
        public string WatermarkText
        {
            get
            {
                return _watermarkText;
            }
            set
            {
                _watermarkText = value;
                OnPropertyChanged("WatermarkText");
            }
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        private ObservableCollection<Picture> _imageList;
        public ObservableCollection<Picture> ImageList
        {
            get
            {
                return _imageList ?? new ObservableCollection<Picture>();
            }
            set
            {
                _imageList = value;
                OnPropertyChanged("ImageList");
            }
        }
        #endregion

        #region RelayCommand
        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(param => this.ExecuteSearch()));
            }
        }

        private RelayCommand _checkCommand;
        public RelayCommand CheckCommand
        {
            get
            {
                return _checkCommand ?? (_checkCommand = new RelayCommand(param => this.ExecuteCheck()));
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand(param => this.ExecuteDelete()));
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {            
            WatermarkText = "Select Folder...";
        }

        private void ExecuteSearch()
        {
            var defaultFolder = string.IsNullOrEmpty(Path) ? @"C:\" : Path;

            Path = base.ShowOpenFolderDialog(defaultFolder);

            if (string.IsNullOrEmpty(Path))
                WatermarkText = "Select Folder...";
            else
                WatermarkText = string.Empty;

            CreateList();
        }

        private void ExecuteCheck()
        {
            if (ImageList.Count > 1)
            {
                double totalCompareCount = (ImageList.Count * (ImageList.Count - 1)) / 2;
                double completeCount = 0.0;

                Enabled = false;

                foreach (var source in ImageList)
                {
                    foreach (var target in ImageList.Where(x => x.No != source.No && x.Checked == false))
                    {
                        var comparer = new ImageComparer();

                        comparer.Source = source;
                        source.Checked = true;

                        comparer.Target = target;
                        var similarity = comparer.CalculateSimilarity();

                        if (similarity > 0.9)
                        {
                            var window = new SelectWindow();
                            var vm = window.DataContext as SelectWindowViewModel;

                            if (vm.IsNotNull())
                            {
                                vm.Pic1 = source;
                                vm.Pic2 = target;
                                vm.Similarity = (int)((System.Math.Round(similarity, 2)) * 100);
                                window.ShowDialog();
                                window.DataContext = null;
                            }
                        }

                        comparer.Dispose();

                        _uiTaskHelper.ChangeByDispatcher(() =>
                        {
                            ProgressValue = (int)((System.Math.Round(++completeCount / totalCompareCount, 2)) * 100);
                        });
                    }
                }

                ShowMessageOkOnly("결과메시지", "중복 이미지 검색을 마쳤습니다.");
                Enabled = true;
            }
        }

        private void ExecuteDelete()
        {
            if (MessageBox.Show("선택 이미지를 삭제 하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var item in ImageList.Where(x => x.Delete))
                {
                    File.Delete(item.Path);
                }
                ShowMessageOkOnly("결과메시지", "모두 삭제하였습니다.");
            }       
        }

        private void CreateList()
        {
            if (string.IsNullOrEmpty(Path))
                return;

            Enabled = false;
            IsShowLoading = true;

            Task.Factory.StartNew(() => {
                ObservableCollection<Picture> curList = new ObservableCollection<Picture>();
                DirectoryInfo sourceDir = new DirectoryInfo(Path);
                int index = 0;

                var imageExtention = new[] { ".JPG", ".PNG", ".BMP" }; //...

                foreach (var item in sourceDir.GetFiles("*", SearchOption.AllDirectories).Where(file => imageExtention.Contains(file.Extension.ToUpper())))
                {
                    if (item.IsValidImage())
                    {
                        using (var image = Image.FromFile(item.FullName))
                        {
                            curList.Add(new Picture
                            {
                                No = ++index,
                                CreateDate = item.CreationTime,
                                DiskVolumn = item.Length,
                                Extention = item.Extension,
                                Path = item.FullName,
                                Size = string.Format("{0}x{1}", image.Width, image.Height),
                                Checked = false,
                                Delete = false
                            });
                        }
                    }
                }

                return curList;

            }).ContinueWith((x) => {
                ImageList = x.Result;
                Enabled = true;
                IsShowLoading = false;
            }, _uiScheduler);

        }
    }
}
