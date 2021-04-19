using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Nindo.Mobile.Models;
using Nindo.Net.Models;

namespace Nindo.Mobile.ViewModels
{
    public class ViralDetailPageViewModel : ViewModelBase
    {
        public ViralDetailPageViewModel(IList<Viral> viral)
        {
            Title = $"Viral Hits";

            GetFirstDayOfCurrentMonth();
            ViralEntries = viral.ToList();
        }

        public void SetupViewModel()
        {
            try
            {
                IsBusy = true;

                switch (ViralEntries.First().Platform)
                {
                    case "youtube":
                        Entries = new List<ExtendedViral>
                        {
                            new ExtendedViral
                            {
                                ViralTitle = "Views",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "views".Equals(x.Type))
                            },
                            new ExtendedViral
                            {
                                ViralTitle = "Likes",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type))
                            },
                            new ExtendedViral
                            {
                                ViralTitle = "Kommentare",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "kommentare".Equals(x.Type))
                            }
                        };
                        break;
                    case "tiktok":
                        Entries = new List<ExtendedViral>
                        {
                            new ExtendedViral
                            {
                                ViralTitle = "Views",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "views".Equals(x.Type))
                            },
                            new ExtendedViral
                            {
                                ViralTitle = "Likes",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type))
                            },
                            new ExtendedViral
                            {
                                ViralTitle = "Kommentare",
                                ViralEntry = ViralEntries.FirstOrDefault(x => "kommentare".Equals(x.Type))
                            }
                        };
                        break;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void GetFirstDayOfCurrentMonth()
        {
            var dateTimeNow = DateTime.Now;
            FirstDayOfMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month, 1).ToString("dd.MM");
        }

        private List<Viral> _viralEntries;

        public List<Viral> ViralEntries
        {
            get => _viralEntries;
            set
            {
                _viralEntries = value;
                OnPropertyChanged();
            }
        }

        private IList<ExtendedViral> _entries;

        public IList<ExtendedViral> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        private string _firstDayOfMonth;

        public string FirstDayOfMonth
        {
            get => _firstDayOfMonth;
            set
            {
                _firstDayOfMonth = value;
                OnPropertyChanged();
            }
        }

        private string _contentUrl;

        public string ContentUrl
        {
            get => _contentUrl;
            set
            {
                _contentUrl = value;
                OnPropertyChanged();
            }
        }


        private int _height;

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
    }
}