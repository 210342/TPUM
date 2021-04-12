using System.Windows.Input;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

class UserViewModel
{
    private List<User> _UsersList;

    public UserViewModel()
    {
        _UsersList = new List<User>
            {
                new User{UserId = 1},
                new User{UserId = 2},
            };
    }

    public List<User> Users
    {
        get { return _UsersList; }
        set { _UsersList = value; }
    }

    //private icommand mupdater;
    //public icommand updatecommand
    //{
    //    get
    //    {
    //        if (mupdater == null)
    //            mupdater = new updater();
    //        return mupdater;
    //    }
    //    set
    //    {
    //        mupdater = value;
    //    }
    //}

    //private class updater : icommand
    //{
    //    #region icommand members  

    //    public bool canexecute(object parameter)
    //    {
    //        return true;
    //    }

    //    public event eventhandler canexecutechanged;

    //    public void execute(object parameter)
    //    {

    //    }

    //    #endregion
    //}
}