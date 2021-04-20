using System;
using System.Collections.Generic;
using System.Linq;
using Nindo.Mobile.Models;
using Nindo.Net.Models;

namespace Nindo.Mobile.ViewModels.DetailPages
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
                        {
                            var views = ViralEntries.FirstOrDefault(x => "views".Equals(x.Type));
                            var likes = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type));
                            var kommentare = ViralEntries.FirstOrDefault(x => "kommentare".Equals(x.Type));

                            Height = 250;
                            Width = 500;

                            Entries = new List<ExtendedViral>
                            {
                                new ExtendedViral
                                {
                                    ViralTitle = "Views",
                                    ViralEntry = views,
                                    ViralEmbed = $"https://www.youtube.com/embed/{views.PostId}",
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Likes",
                                    ViralEntry = likes,
                                    ViralEmbed = $"https://www.youtube.com/embed/{likes.PostId}",
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Kommentare",
                                    ViralEntry = kommentare,
                                    ViralEmbed = $"https://www.youtube.com/embed/{kommentare.PostId}",
                                }
                            };
                        }
                        break;
                    case "tiktok":
                        {
                            var views = ViralEntries.FirstOrDefault(x => "views".Equals(x.Type));
                            var likes = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type));
                            var kommentare = ViralEntries.FirstOrDefault(x => "kommentare".Equals(x.Type));

                            Height = 800;
                            Width = 500;

                            Entries = new List<ExtendedViral>
                            {
                                new ExtendedViral
                                {
                                    ViralTitle = "Views",
                                    ViralEntry = views,
                                    ViralEmbed = $"https://www.tiktok.com/embed/v2/{views.PostId}?lang=de"
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Likes",
                                    ViralEntry = likes,
                                    ViralEmbed = $"https://www.tiktok.com/embed/v2/{likes.PostId}?lang=de"
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Kommentare",
                                    ViralEntry = kommentare,
                                    ViralEmbed = $"https://www.tiktok.com/embed/v2/{kommentare.PostId}?lang=de"
                                }
                            };
                        }
                        break;
                    case "instagram":
                        {
                            var likes = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type));
                            var kommentare = ViralEntries.FirstOrDefault(x => "kommentare".Equals(x.Type));

                            Height = 750;
                            Width = 500;

                            Entries = new List<ExtendedViral>
                            {
                                new ExtendedViral
                                {
                                    ViralTitle = "Likes",
                                    ViralEntry = likes,
                                    ViralEmbed =
                                        $"https://www.instagram.com/p/{likes.PostId}/embed/?cr=1&v=12&wp=64&rd=https%3A%2F%2Fnindo.de&rp=%2Fviral#%7B%22ci%22%3A0%2C%22os%22%3A1799.8050000001058%2C%22ls%22%3A545.0650000002497%2C%22le%22%3A545.91500000015%7D"
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Kommentare",
                                    ViralEntry = kommentare,
                                    ViralEmbed =
                                        $"https://www.instagram.com/p/{kommentare.PostId}/embed/?cr=1&v=12&wp=64&rd=https%3A%2F%2Fnindo.de&rp=%2Fviral#%7B%22ci%22%3A0%2C%22os%22%3A1799.8050000001058%2C%22ls%22%3A545.0650000002497%2C%22le%22%3A545.91500000015%7D"
                                }
                            };
                        }
                        break;
                    case "twitter":
                        {
                            var likes = ViralEntries.FirstOrDefault(x => "likes".Equals(x.Type));
                            var retweets = ViralEntries.FirstOrDefault(x => "retweets".Equals(x.Type));

                            Height = 1000;
                            Width = 500;

                            Entries = new List<ExtendedViral>
                            {
                                new ExtendedViral
                                {
                                    ViralTitle = "Likes",
                                    ViralEntry = likes,
                                    ViralEmbed = $"https://platform.twitter.com/embed/Tweet.html?dnt=false&embedId=twitter-widget-1&frame=false&hideCard=false&hideThread=false&id={likes.PostId}&lang=de&origin=https%3A%2F%2Fnindo.de%2Fviral&theme=light&widgetsVersion=889aa01%3A1612811843556&width=100px"
                                },
                                new ExtendedViral
                                {
                                    ViralTitle = "Retweets",
                                    ViralEntry = retweets,
                                    ViralEmbed = $"https://platform.twitter.com/embed/Tweet.html?dnt=false&embedId=twitter-widget-1&frame=false&hideCard=false&hideThread=false&id={retweets.PostId}&lang=de&origin=https%3A%2F%2Fnindo.de%2Fviral&theme=light&widgetsVersion=889aa01%3A1612811843556&width=100px"
                                }
                            };
                        }
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