﻿using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using CoreGraphics;
using Pathogen.Views.Partials;
using Pathogen.iOS.Renderers;

[assembly: ExportRenderer(typeof(ChatInputBarView), typeof(ChatEntryRenderer))]
namespace Pathogen.iOS.Renderers
{
    public class ChatEntryRenderer : ViewRenderer //Depending on your situation, you will need to inherit from a different renderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;

        // TODO Modify this for different platforms
        private int AdditionalBottomMargin = 35;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {

            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            CGSize keyboardSize = result.RectangleFValue.Size;
            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height + AdditionalBottomMargin - 15); //push the entry up to keyboard height when keyboard is activated
            }
        }

        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, AdditionalBottomMargin); //set the margins to zero when keyboard is dismissed
            }
        }

        void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }
}