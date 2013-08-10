using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Common
{
    public class OkCancelApplicationBar
    {
        public ApplicationBar ApplicationBar { get; private set; }

        public OkCancelApplicationBar(EventHandler Ok, EventHandler Cancel)
        {
            ApplicationBar = new ApplicationBar();

            var ok = new ApplicationBarIconButton();
            ok.Text = "ok";
            ok.IconUri = new Uri("/Assets/Icons/appbar.check.png", UriKind.RelativeOrAbsolute);
            ok.Click += Ok;
            ApplicationBar.Buttons.Add(ok);

            var cancel = new ApplicationBarIconButton();
            cancel.Text = "cancel";
            cancel.IconUri = new Uri("/Assets/Icons/appbar.cancel.png", UriKind.RelativeOrAbsolute);
            cancel.Click += Cancel;
            ApplicationBar.Buttons.Add(cancel);
        }
    }
}
