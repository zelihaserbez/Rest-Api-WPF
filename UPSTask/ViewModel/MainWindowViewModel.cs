using Microsoft.AspNetCore.Mvc;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using UPSTask.TaskObjects;

namespace UPSTask
{
    class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Properties

        private List<User> _users;

        public List<User> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private bool _isLoadData;

        public bool IsLoadData
        {
            get { return _isLoadData; }
            set { SetProperty(ref _isLoadData, value); }
        }

        private string _responseMessage = "";

        public string ResponseMessage
        {
            get { return _responseMessage; }
            set { SetProperty(ref _responseMessage, value); }
        }

        #region [Create User Properties]

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private string _email;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set { SetProperty(ref _gender, value); }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        #endregion
        private bool _isShowForm;

        public bool IsShowForm
        {
            get { return _isShowForm; }
            set { SetProperty(ref _isShowForm, value); }
        }

        private string _showPostMessage = "Fill the form to register an user!";

        public string ShowPostMessage
        {
            get { return _showPostMessage; }
            set { SetProperty(ref _showPostMessage, value); }
        }

        private int _pageNumber = 1;

        public int PageNumber
        {
            get { return _pageNumber; }
            set { SetProperty(ref _pageNumber, value); }
        }

        private int id = 0;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        #endregion

        #region ICommands
        public DelegateCommand GetButtonClicked { get; set; }
        public DelegateCommand ShowRegistrationForm { get; set; }
        public DelegateCommand PostButtonClick { get; set; }
        public DelegateCommand<User> PutButtonClicked { get; set; }
        public DelegateCommand<User> DeleteButtonClicked { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initalize perperies & delegate commands
        /// </summary>
        public MainWindowViewModel()
        {
            GetButtonClicked = new DelegateCommand(GetUserDetails);
            PutButtonClicked = new DelegateCommand<User>(UpdateUserDetails);
            DeleteButtonClicked = new DelegateCommand<User>(DeleteUserDetails);
            PostButtonClick = new DelegateCommand(CreateNewUser);
            ShowRegistrationForm = new DelegateCommand(RegisterUser);
        }
        #endregion

        #region CRUD
        /// <summary>
        /// Make visible Regiter user form
        /// </summary>
        private void RegisterUser()
        {
            IsShowForm = true;
        }

        /// <summary>
        /// Fetches user details
        /// </summary>
        /// 
        private async void GetUserDetails()
        {
            Filters filters = new Filters
            {
                pageNumber = PageNumber,
                id = Id,
                name = Name
            };

            WebAPI api = new WebAPI();
            var okResult = await api.GetUser(filters);
            var value = (okResult as OkObjectResult)?.Value as RestResponse;
            Users = value.Data;
            IsLoadData = true;
        }

        /// <summary>
        /// Adds new user
        /// </summary>
        private async void CreateNewUser()
        {
            User newUser = new User()
            {
                Name = Name,
                Email = Email,
                Gender = Gender,
                Status = "active"
            };
            //var userDetails = WebAPI.PostCall(newUser);
            WebAPI api = new WebAPI();
            var okResult = await api.CreateUser(newUser);
            var value = (okResult as ObjectResult).Value as UserCreateResponse;
            if (value != null && value.code == (int)System.Net.HttpStatusCode.Created)
            {
                ShowPostMessage = newUser.Name + "'s details has successfully been added!";
            }
            else
            {
                ShowPostMessage = "Failed to create" + newUser.Name + "'s details.";
            }
        }


        /// <summary>
        /// Updates user's record
        /// </summary>
        /// <param name="user"></param>
        private async void UpdateUserDetails(User user)
        {
            WebAPI api = new WebAPI();
            var saveResult = await api.UpdateUser(user);
            var value = (saveResult as ObjectResult).Value as UserUpdateResponse;
            if (value != null && value.code == (int)System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = user.Name + "'s details has successfully been updated!";
            }
            else
            {
                ResponseMessage = "Failed to update" + user.Name + "'s details.";
            }
        }

        /// <summary>
        /// Deletes user's record
        /// </summary>
        /// <param name="user"></param>
        private async void DeleteUserDetails(User user)
        {
            WebAPI api = new WebAPI();
            var saveResult = await api.DeleteUser(user.Id);
            var value = (saveResult as ObjectResult).Value as PageResponse;
            if (value.Success)
            {
                ResponseMessage = user.Name + "'s details has successfully been deleted!";
            }
            else
            {
                ResponseMessage = "Failed to delete" + user.Name + "'s details.";
            }
        }
        #endregion
    }
}
