using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk.Entity
{
    public class Dialog : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string word;
        public string Word 
        {
            get { return word; }
            set {
                if (word!=value)
                {
                    NotifyPropertyChanging("Word");
                    word = value;
                    NotifyPropertyChanged("Word");
                }
            }
        }

        private string kind;

        public string Kind
        {
            get { return kind; }
            set {
                NotifyPropertyChanging("Kind");
                kind = value;
                NotifyPropertyChanged("Kind");
            }
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        //用来通知页面表的字段数据产生了改变
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // 用来通知数据上下文表的字段数据将要产生改变
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion
    }
}
