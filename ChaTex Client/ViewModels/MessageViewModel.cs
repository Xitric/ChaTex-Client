using IO.ChaTex.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChaTex_Client.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private int id;
        private string authorName;
        private bool me;
        private string messageContent;
        private DateTime? deletionDate;
        private DateTime? editDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public MessageViewModel(GetMessageDTO message)
        {
            Id = message.Id;

            AuthorName = message.Sender.FirstName + " ";
            if (message.Sender.MiddleInitial != null) AuthorName += message.Sender.MiddleInitial + ". ";
            AuthorName += message.Sender.LastName;

            Me = message.Sender.Me;
            MessageContent = message.Content;
            DeletionDate = message.DeletionDate;
            EditDate = message.LastEdited;
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

        public string AuthorName
        {
            get => authorName;
            private set
            {
                if (value != authorName)
                {
                    authorName = value;
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

        public string MessageContent
        {
            get
            {
                return Deleted ? "Deleted" : messageContent;
            }
            private set
            {
                if (value != messageContent)
                {
                    messageContent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? DeletionDate
        {
            get => deletionDate;
            private set
            {
                if (value != deletionDate)
                {
                    deletionDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateTime? EditDate
        {
            get => editDate;
            private set
            {
                if (value != editDate)
                {
                    editDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Deleted
        {
            get => deletionDate != null;
        }

        public bool Edited
        {
            get => editDate != null;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
