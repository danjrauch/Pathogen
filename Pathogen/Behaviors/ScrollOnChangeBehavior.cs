using System;
using System.Collections.ObjectModel;
using System.Linq;
using Pathogen.Models;
using Xamarin.Forms;

namespace Pathogen.Behaviors
{
    public class ScrollOnChangeBehavior : Behavior<ListView>
    {
        private bool IsScrolled = false;

        protected override void OnAttachedTo(ListView listView)
        {
            listView.ItemSelected += OnSelected;
            listView.Scrolled += OnScrolled;
            base.OnAttachedTo(listView);
        }

        protected override void OnDetachingFrom(ListView listView)
        {
            listView.ItemSelected -= OnSelected;
            listView.Scrolled -= OnScrolled;
            base.OnDetachingFrom(listView);
        }

        void OnSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ListView listView = (ListView)sender;
            if((Message)args.SelectedItem == (listView.ItemsSource as ObservableCollection<Message>).Last())
            {
                listView.ScrollTo(args.SelectedItem, ScrollToPosition.MakeVisible, false);
            }
            listView.SelectedItem = null;
        }

        void OnScrolled(object sender, ScrolledEventArgs args)
        {
            //ListView listView = (ListView)sender;
            IsScrolled = args.ScrollY > 0;
        }
    }
}
