using System;

using CoreGraphics;
using UIKit;

using Color = BeerDrinkin.Helpers.Colours;
using Strings = BeerDrinkin.Core.Helpers.Strings;

using Splat;

namespace BeerDrinkin.iOS
{
    partial class TabBarController : UITabBarController
    {
        #region Constructor
        public TabBarController(IntPtr handle) : base(handle)
        {
        }

        #endregion

        #region Overrides
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

  
            SetupUI();
            SetupTabChangeAnimation();           
        }

        #endregion

        void SetupUI()
        {
            TabBar.Items[0].Title = Strings.Tabs_MyBeers;
            TabBar.Items[1].Title = Strings.Tabs_WishList;
            TabBar.Items[2].Title = Strings.Tabs_Search;
            TabBar.Items[3].Title = Client.Instance.BeerDrinkinClient.CurrentAccount == null ? Strings.Tabs_Profile : Client.Instance.BeerDrinkinClient.CurrentAccount.FirstName;

            TabBar.SelectedImageTintColor = Color.Blue.ToNative();
        }

        void SetupTabChangeAnimation()
        {
            ShouldSelectViewController = (tabController, controller) =>
            {
                if (SelectedViewController == null || controller == SelectedViewController)
                    return true;

                var fromView = SelectedViewController.View;
                var toView = controller.View;

                var destFrame = fromView.Frame;
                const float offset = 25;

                //Position toView off screen
                fromView.Superview.AddSubview(toView);
                toView.Frame = new CGRect(offset, destFrame.Y, destFrame.Width, destFrame.Height);

                UIView.Animate(0.1,
                    () =>
                    {
                        toView.Frame = new CGRect(0, destFrame.Y, destFrame.Width, destFrame.Height);
                    }, () =>
                    {
                        //Completion handler. Remove old view
                        fromView.RemoveFromSuperview();
                        SelectedViewController = controller;
                    });
                return true;
            };
         }
    }
}