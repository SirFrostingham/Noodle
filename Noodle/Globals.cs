using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Noodle.Annotations;
using Noodle.Models;
using Formatting = System.Xml.Formatting;

namespace Noodle
{
    public class Globals : INotifyPropertyChanged
    {
        private static Globals _globals;

        public static Globals Current
        {
            get
            {
                if (_globals == null)
                    _globals = new Globals();

                return _globals;
            }
        }

        private SchedulerModel _currentSchedule;
        public SchedulerModel CurrentSchedule
        {
            get { return _currentSchedule; }
            set
            {
                _currentSchedule = value;
                OnPropertyChanged("CurrentSchedule");
                OnPropertyChanged("Jobs");
                OnPropertyChanged("JobsInProgress");
            }
        }

        private List<Job> _jobs;

        public List<Job> Jobs
        {
            get { return _jobs; }
            set
            {
                _jobs = value;
                OnPropertyChanged("CurrentSchedule");
                OnPropertyChanged("Jobs");
                OnPropertyChanged("JobsInProgress");
            }
        }

        private List<Job> _jobsInProgress;

        public List<Job> JobsInProgress
        {
            get { return _jobsInProgress; }
            set
            {
                _jobsInProgress = value;
                OnPropertyChanged("CurrentSchedule");
                OnPropertyChanged("Jobs");
                OnPropertyChanged("JobsInProgress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            var eventToFire = PropertyChanged;
            if (eventToFire == null)
                return;

            eventToFire(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
