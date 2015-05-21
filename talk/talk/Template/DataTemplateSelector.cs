using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace talk.Template
{
    public abstract class DataTemplateSelector : ContentControl
    {
        //根据newContent的属性，返回所需的DataTemplate
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            //根据newContent的属性，选择对应的DataTemplate
            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}
