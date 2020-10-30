using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clouding
{
    public class StackWidgetItem: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string packageName { get; set; }
        public string speed { get; set; }
        public string timeLeft { get; set; }
        public string state { get; set; }
        public int progressValue { get; set; }

        static int num;
        public string imageSrc
        {
            get
            {
                if(useTestCase)
                {
                    num++;
                    if (num % 4 == 0)
                        return "/Assets/bell.png";
                    else if (num % 4 == 1)
                        return "/Assets/apps.png";
                    else if (num % 4 == 2)
                        return "/Assets/cartcolor.png";
                    return "/Assets/computer.png";
                }
                else
                    return "/test;component/Assets/bell.png";
            }
        }

        public bool useTestCase;
        public StackWidgetItem(string timeLeft, string speed, string name, int progressValue)
        {
            useTestCase = true;
            this.timeLeft = timeLeft;
            this.packageName = name;
            this.speed = speed;
            this.progressValue = progressValue;
            this.state = "暂停中";
        }

        public int progressValue_
        {
            get { return progressValue; }
            set
            {
                progressValue_ = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("progressValue_"));
            }
        }
        public string state_
        {
            get { return state; }
            set
            {
                state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("state_"));
            }
        }

        public string timeLeft_
        {
            get { return timeLeft; }
            set
            {
                timeLeft = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("timeLeft_"));
            }
        }

        public string speed_
        {
            get { return speed; }
            set
            {
                speed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("speed_"));
            }
        }

    }
}
