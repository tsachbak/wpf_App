using GalaSoft.MvvmLight.Command;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using System.Security;
using System.Net;
using System.Windows.Controls;

namespace WpfApp1
{
    public class Login : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private DataBaseHandler _dataBaseHandler;
        UsersMgr m_usersMgr = UsersMgr.GetInstance();

        public Login()
        {
            _dataBaseHandler = new DataBaseHandler();
            AgeOptions = Enumerable.Range(18, 102).ToList();

            SubmitUserLoginInfo = new RelayCommand(submitUserLoginInfo);
            SignIn = new RelayCommand(signIn);
        }

        #region Send Commands

        internal void signIn()
        {
            User? user = m_usersMgr.GetUserByMail(ExistingUserMail);
            if (user == null)
            {
                MessageBox.Show("User is not exist! no such email in the data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.PWD != ExistingUserPwd) 
            {
                MessageBox.Show("Wrong Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RaiseLoginSuccess();
        }

        public event EventHandler LoginSuccess;
        private void RaiseLoginSuccess()
        {
            LoginSuccess?.Invoke(this, EventArgs.Empty);
        }

        internal void submitUserLoginInfo()
        {
            var user = new User
            {
                Mail = UserMail,
                PWD = UserPwd,
                Name = UserName,
                Age = SelectedAge,
                Phone = UserPhone
            };

            if (IsUserValid(user))
            {
                if (IsUserExist(user))
                {
                    MessageBox.Show("User is already exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _dataBaseHandler.InsertUser(user);
                    m_usersMgr.AddUser(user);
                    MessageBox.Show("User has Register Successfuly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private bool IsUserExist(User user)
        {
            foreach (User user1 in  m_usersMgr.GetUsers()) 
            { 
                if (user1.Mail == user.Mail)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Properties

        private List<int> m_ageOptions;
        public List<int> AgeOptions
        {
            get { return m_ageOptions; }
            set
            {
                m_ageOptions = value;
                OnPropertyChanged("AgeOptions");
            }
        }

        private int m_selectedAge;
        public int SelectedAge
        {
            get { return m_selectedAge; }
            set
            {
                m_selectedAge = value;
                OnPropertyChanged("SelectedAge");
            }
        }

        private string m_userName;
        public string UserName
        {
            get { return m_userName; }
            set
            {
                m_userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string m_userPWD;
        public string UserPwd
        {
            get { return m_userPWD; }
            set
            {
                m_userPWD = value;
                OnPropertyChanged("UserPwd");
            }
        }

        private string m_userMail;
        public string UserMail
        {
            get { return m_userMail; }
            set
            {
                if (IsValidMail(value))
                {
                    m_userMail = value;
                    OnPropertyChanged("UserMail");
                }
                else
                {
                    MessageBox.Show("Invalid Mail address. Please enter a valid email address", "Error", 
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string m_userPhone;
        public string UserPhone
        {
            get { return m_userPhone; }
            set
            {
                m_userPhone = value;
                OnPropertyChanged("UserPhone");
            }
        }

        private RelayCommand m_submitUserLoginInfo;
        public RelayCommand SubmitUserLoginInfo
        {
            get { return m_submitUserLoginInfo; }
            set { m_submitUserLoginInfo = value; }
        }

        private string m_existingUserMail;
        public string ExistingUserMail
        {
            get { return m_existingUserMail; }
            set
            {
                if (IsValidMail(value))
                {
                    m_existingUserMail = value;
                    OnPropertyChanged("ExistingUserMail");
                }
                else
                {
                    MessageBox.Show("Invalid Mail address. Please enter a valid email address", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private RelayCommand m_signIn;
        public RelayCommand SignIn
        {
            get { return m_signIn; }
            set { m_signIn = value; }
        }

        private string m_existingUserPwd;
        public string ExistingUserPwd
        {
            get { return m_existingUserPwd; }
            set
            {
                m_existingUserPwd = value;
                OnPropertyChanged("ExistingUserPwd");
            }
        }

        #endregion

        #region Validation

        internal bool IsValidMail(string mail)
        {
            string pattern = @"^[a-zA-Z0-9._%+=]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            
            return Regex.IsMatch(mail, pattern);
        }

        internal bool IsUserValid(User user)
        {
            if (user.Name == null || user.Phone == null)
            {
                MessageBox.Show("You must fill all the feilds!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.Age == 0)
            {
                MessageBox.Show("You must select age!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!IsValidPwd())
            {
                MessageBox.Show("Invalid Password. Password has to be 4 to 6 digits", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool IsValidPwd()
        {
            if (UserPwd == null || UserPwd.Length < 4 || UserPwd.Length > 6)
            {
                return false;
            }
            return true;
        }

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
