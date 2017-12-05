using IO.ChaTex.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChaTex_Client.ViewModels
{
    class MessageViewModel : ViewModelBase
    {
        private int id;
        private string authorName;
        private int authorId;
        private bool me;
        private bool firstInSequence;
        private string messageContent;
        private DateTime creationDate;
        private DateTime? deletionDate;
        private DateTime? editDate;

        public MessageViewModel(GetMessageDTO message)
        {
            Id = message.Id;

            AuthorName = message.Sender.FirstName + " ";
            if (message.Sender.MiddleInitial != null) AuthorName += message.Sender.MiddleInitial + ". ";
            AuthorName += message.Sender.LastName;

            AuthorId = message.Sender.Id;
            Me = message.Sender.Me;
            FirstInSequence = true;
            MessageContent = message.Content;
            CreationDate = message.CreationTime;
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

        public int AuthorId
        {
            get => authorId;
            private set
            {
                if (value != authorId)
                {
                    authorId = value;
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

        public bool FirstInSequence
        {
            get => firstInSequence;
            set
            {
                if (value != firstInSequence)
                {
                    firstInSequence = value;
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

        public DateTime CreationDate
        {
            get => creationDate;
            private set
            {
                if (value != creationDate)
                {
                    creationDate = value;
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
    }
}
