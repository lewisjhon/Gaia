using Gaia.Model;
using Gaia.View;
using Gaia.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gaia.ViewModel
{
    public enum CommandTypes
    {
        DeleteLeft,
        DeleteRight,
        DeleteAll,
        SaveAll
    }

    public class SelectWindowViewModel : ViewModelBase
    {
        #region Property
        private Picture _pic1 = null;
        public Picture Pic1
        {
            get
            {
                return _pic1 ?? new Picture();
            }
            set
            {
                _pic1 = value;
                OnPropertyChanged("Pic1");
            }
        }

        private Picture _pic2 = null;
        public Picture Pic2
        {
            get
            {
                return _pic2 ?? new Picture();
            }
            set
            {
                _pic2 = value;
                OnPropertyChanged("Pic2");
            }
        }

        private int _similarity = 0;
        public int Similarity
        {
            get
            {
                return _similarity;
            }
            set
            {
                _similarity = value;
                OnPropertyChanged("Similarity");
            }
        }


        #endregion

        #region RelayCommand
        private RelayCommand<Window> _deleteLeft;                       

        public RelayCommand<Window> deleteLeft
        {
            get
            {
                return _deleteLeft ?? (_deleteLeft = new RelayCommand<Window>(param => this.Execute(param, CommandTypes.DeleteLeft)));
            }
        }

        private RelayCommand<Window> _deleteRight;
        public RelayCommand<Window> deleteRight
        {
            get
            {
                return _deleteRight ?? (_deleteRight = new RelayCommand<Window>(param => this.Execute(param, CommandTypes.DeleteRight)));
            }
        }

        private RelayCommand<Window> _deleteAll;
        public RelayCommand<Window> deleteAll
        {
            get
            {
                return _deleteAll ?? (_deleteAll = new RelayCommand<Window>(param => this.Execute(param, CommandTypes.DeleteAll)));
            }
        }

        private RelayCommand<Window> _saveAll;
        public RelayCommand<Window> SaveAll
        {
            get
            {
                return _saveAll ?? (_saveAll = new RelayCommand<Window>(param => this.Execute(param, CommandTypes.SaveAll)));
            }
        }

        #endregion


        private void Execute(Window sender, CommandTypes type)
        {            
            switch (type)
            {
                case CommandTypes.DeleteAll:
                    Pic1.Delete = true;
                    Pic2.Delete = true;
                    break;
                case CommandTypes.DeleteLeft:
                    Pic1.Delete = true;
                    break;
                case CommandTypes.DeleteRight:
                    Pic2.Delete = true;
                    Pic2 = null;
                    break;
                case CommandTypes.SaveAll:
                    break;
            }

            sender.Close();
            
        }
    }
}
