using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;

namespace Tasks.Views
{
    public partial class AboutAndTips : PhoneApplicationPage
    {
        public AboutAndTips()
        {
            InitializeComponent();
            this.DataContext = new AboutAndTipsViewModel();
        }

        private void ContactMe(object sender, RoutedEventArgs e)
        {
            new EmailComposeTask()
            {
                To = "appsforme@outlook.com"
            }.Show();
        }

        private void RateAndReview(object sender, RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }

        public class AboutAndTipsViewModel
        {
            public ObservableCollection<Tip> TipsList { get; private set; }
            public ObservableCollection<ContactMethod> ContactMethodsList { get; private set; }

            public string ApplicationTitleAndVersion { get; set; }
            public string Bio { get; set; }

            public AboutAndTipsViewModel()
            {
                ApplicationTitleAndVersion = "Tasks v1.0.0";
                
                Bio = "Isaac, a student of Computer Engineering at the University of Washington in Seattle, "
                    + "is the one man in the one-man crew that is AppsForMe. He loves the Windows Phone platform, "
                    + "software engineering, and UX design.";

                TipsList = new ObservableCollection<Tip>()
                {
                    new Tip() {
                        Title = "add whole groups at once",
                        Description = "Don't just add one task at a time. When in the inbox, tap the 'group' icon "
                                    + "button, select one or more groups, and then tap the 'ok' icon button. This "
                                    + "will add to the inbox all the tasks in each of those groups. Most excellent!"
                    },

                    new Tip() {
                        Title = "take advantage of cycles",
                        Description = "Ever notice you never buy tortillas without also buying cheese and beans? "
                                    + "Make a group called 'burritos' with tortillas, beans, and cheese, and then "
                                    + "add all three to your inbox at once before you head out to the grocery store!"
                    },

                    //new Tip() {
                    //    Title = "add part of a group",
                    //    Description = "Don't want to add all the tasks in a group? (I need tortillas, but I already "
                    //                + "have cheese.) No problem! When viewing the list of groups, tap one to open it, " 
                    //                + "select only the tasks you want to add, and then tap the 'ok' icon button. How's "
                    //                + "that for efficiency?"
                    //},

                    new Tip() {
                        Title = "put items on hold",
                        Description = "Don't want to see some tasks in your inbox just now? Select them and tap the "
                                    + "'on hold' icon button (it looks like a 'pause' button). Now these tasks will "
                                    + "stay out of your inbox until you mark them as 'active' again!"
                    }
                };

                ContactMethodsList = new ObservableCollection<ContactMethod>()
                {
                    new ContactMethod() {
                        CatchyTitle = "questions or complaints",
                        Description = "If there's something about Tasks that confuses, bothers, stumps you, or if I very " 
                                    + "cleverly included a bug in the latest release (I was testing you to see if you'd  "
                                    + "notice, of course)."
                    },

                    new ContactMethod() {
                        CatchyTitle = "suggestions",
                        Description = "If there's something new you'd love Tasks do or something else you'd like me to build."
                    },

                    new ContactMethod() {
                        CatchyTitle = "how-ya-doin'",
                        Description = "If you'd just like to say Hi!"
                    }
                };
            }

            public class Tip
            {
                public string Title { get; set; }
                public string Description { get; set; }
            }

            public class ContactMethod
            {
                public string CatchyTitle { get; set; }
                public string Description { get; set; }
            }
        }
    }
}