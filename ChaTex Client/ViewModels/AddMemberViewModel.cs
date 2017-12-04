using IO.ChaTex.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Globalization;

namespace ChaTex_Client.ViewModels
{
    class AddMemberViewModel : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private bool isSelected;
        private bool isAdmin;

        public event PropertyChangedEventHandler PropertyChanged;

        public AddMemberViewModel(UserDTO user)
        {
            Id = user.Id;

            Name = user.FirstName + " ";
            if (user.MiddleInitial != null) Name += user.MiddleInitial + ". ";
            Name += user.LastName;

            IsSelected = false;
            IsAdmin = false;
        }

        public int Id
        {
            get => id;
            private set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => name;
            private set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsAdmin
        {
            get => isAdmin;
            private set
            {
                if (value != isAdmin)
                {
                    isAdmin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
