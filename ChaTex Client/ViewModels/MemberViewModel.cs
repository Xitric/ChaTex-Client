using IO.ChaTex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChaTex_Client.ViewModels
{
    class MemberViewModel : ViewModelBase
    {
        private int id;
        private string name;
        private string email;
        private bool me;
        private bool isAdmin;
        private IList<RoleDTO> roles;

        public MemberViewModel(UserDTO user, bool isAdmin)
        {
            Id = user.Id;

            Name = user.FirstName + " ";
            if (user.MiddleInitial != null) Name += user.MiddleInitial + ". ";
            Name += user.LastName;

            Email = user.Email;
            Me = user.Me;
            IsAdmin = isAdmin;
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

        public string Email
        {
            get => email;
            private set
            {
                if (value != email)
                {
                    email = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Me
        {
            get => me;
            private set
            {
                if (value != me)
                {
                    me = value;
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

        public IList<RoleDTO> Roles
        {
            get => roles;
            set
            {
                if (value != roles)
                {
                    roles = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
