using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using talk.Entity;

namespace talk.Template
{
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate SendTemplate { get; set; }
        public DataTemplate ReceiveTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Dialog table = item as Dialog;
            DataTemplate dt;
            if (table != null)
            {
                if (table.Kind=="send")
                {
                    dt = SendTemplate;
                }
                else
                {
                    dt = ReceiveTemplate;
                }
                return dt;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
