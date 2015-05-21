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
using System.Windows.Media;
using System.IO.IsolatedStorage;
using talk.Common;
using Microsoft.Phone.Tasks;

namespace talk
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;


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
                scroll();
                return;
            }
          
            datalist.Add(new Dialog { Kind = "send", Word = txt });
            if (txt.Equals("李斌"))
            {
                datalist.Add(new Dialog { Kind = "receive", Word = "大帅哥一枚" });
                scroll();
                sendTxt.Text = "";
                return;
            }
            if (txt.Equals("黄世杰") || txt.Equals("曹峥"))
            {
                datalist.Add(new Dialog { Kind = "receive", Word = "呵呵哒，不解释" });
                scroll();
                sendTxt.Text = "";
                return;
            }
            string url = "http://www.tuling123.com/openapi/api?key=c5f6ea756045a4ab86befb688cde2faf&info=" + txt;
            AsyncGetWithWebRequest(url);

            sendTxt.Text = "";


            scroll();
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
                        this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = tl.Text }); scroll(); });
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
                        this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = "人家困了，明天再来吧，喵~" }); scroll(); });
                        break;
                    case "308000":
                        this.Dispatcher.BeginInvoke(() =>
                        {
                            Common.CommPara.url = tl.List[0].Detailurl;
                            this.NavigationService.Navigate(new Uri("/webpage.xaml", UriKind.Relative));
                        });

                        break;
                    default:

                        this.Dispatcher.BeginInvoke(() => { datalist.Add(new Dialog { Kind = "receive", Word = "喵喵喵~" }); scroll(); });
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



        // 获取子类型
        private T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private void scroll()
        {
            LongListSelector sv = FindChildOfType<LongListSelector>(llscontent);
            sv.ScrollTo(sv.ItemsSource[sv.ItemsSource.Count - 1]);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //OpenTitleStoryboard.Begin();
            //return;
            //邀请评价
            if (!settings.Contains("hasReviewed") && CommPara.pastFive)
            {
                OpenTitleStoryboard.Begin();
            }
            
        }

        /// <summary>
        /// 去评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!settings.Contains("hasReviewed"))
            {
                settings.Add("hasReviewed", true);
            }
            else
            {
                settings["hasReviewed"] = true;
            }
            settings.Save();
            MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        /// <summary>
        /// 拒绝评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CloseTitleStoryboard.Begin();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (TopPopGrid.Visibility == Visibility.Visible)
            {
                TopPopGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}