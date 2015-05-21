using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using talk.Resources;
using talk.Entity;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace talk
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();


        }


        private ObservableCollection<Dialog> datalist = new ObservableCollection<Dialog>();

        public ObservableCollection<Dialog> Datalist
        {
            get { return datalist; }
            set
            {
                if (datalist != value)
                {
                    datalist = value;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("手机没有联网");
                return;
            }
            string txt = sendTxt.Text;
            if (string.IsNullOrEmpty(txt))
            {
                datalist.Add(new Dialog { Kind = "receive", Word = "你倒是说话呀，喵~" });
                return;
            }
            datalist.Add(new Dialog { Kind = "send", Word = txt });

            string url = "http://www.tuling123.com/openapi/api?key=c5f6ea756045a4ab86befb688cde2faf&info=" + txt;
            AsyncGetWithWebRequest(url);

            sendTxt.Text = "";
        }

        public void AsyncGetWithWebRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var resultString = streamReader.ReadToEnd();
                tlfirst tl = JsonConvert.DeserializeObject<tlfirst>(resultString);
                switch (tl.Code)
                {
                    case "100000":
                        this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = tl.Text }); });
                        break;
                    case "200000":
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            Common.CommPara.url = tl.Url;
                            this.NavigationService.Navigate(new Uri("/webpage.xaml", UriKind.Relative));
                        });
                        break;
                    case "40004":
                    case "40005":
                    case "40006":
                         this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = "人家困了，明天再来吧，喵~"}); });
                        break;
                    case "308000":
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            Common.CommPara.url = tl.List[0].Detailurl;
                            this.NavigationService.Navigate(new Uri("/webpage.xaml", UriKind.Relative));
                        });
                        
                        break;
                    default:
                        this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = "喵喵喵~" }); });
                        break;
                }

                //int m=resultString.IndexOf(':');
                //int d=resultString.IndexOf(',');
                //string code = resultString.Substring(m+1, d - m);
                //switch (code)
                //{
                //    case "100000"://文本

                //    default:
                //        break;
                //}

            }
        }



    }
}