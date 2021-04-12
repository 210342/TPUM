using System.ComponentModel;

public class User : INotifyPropertyChanged
{
    private int userId;
    public int UserId
    {
        get
        {
            return userId;
        }
        set
        {
            userId = value;
            OnPropertyChanged("UserId");
        }
    }

    #region INotifyPropertyChanged Members  

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion

}