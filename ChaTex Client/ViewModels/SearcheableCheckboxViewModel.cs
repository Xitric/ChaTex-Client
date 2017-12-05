using IO.ChaTex.Models;

namespace ChaTex_Client.ViewModels
{
    public class SearcheableCheckboxViewModel : ViewModelBase
    {
        private int innerValue;
        private string content;
        private bool isSelected;

        public SearcheableCheckboxViewModel(int value, string content)
        {
            Value = value;
            Content = content;
            IsSelected = false;
        }

        public int Value
        {
            get => innerValue;
            private set
            {
                if (value != innerValue)
                {
                    innerValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Content
        {
            get => content;
            protected set
            {
                if (value != content)
                {
                    content = value;
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
    }

    class AddRoleViewModel : SearcheableCheckboxViewModel
    {
        public AddRoleViewModel(RoleDTO role) : base(role.Id, role.Name) { }
    }

    class AddMemberViewModel : SearcheableCheckboxViewModel
    {
        private bool isAdmin;

        public AddMemberViewModel(UserDTO user) : base(user.Id, null)
        {
            Content = user.FirstName + " ";
            if (user.MiddleInitial != null) Content += user.MiddleInitial + ". ";
            Content += user.LastName;

            IsAdmin = false;
        }

        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                if (value != isAdmin)
                {
                    isAdmin = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
