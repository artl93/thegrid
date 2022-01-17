﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 10.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace TheGridTests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public partial class UIMap
    {
        
        /// <summary>
        /// RecordedMethod1
        /// </summary>
        public void RecordedMethod1()
        {
            #region Variable Declarations
            WinClient uIArtsGridClient = this.UIArtsGridWindow.UIArtsGridClient;
            WinButton uICloseButton = this.UIArtsGridWindow.UIArtsGridTitleBar.UICloseButton;
            #endregion

            // Move 'Art's Grid' client from (31, 27) to (17, 373)
            Mouse.StartDragging(uIArtsGridClient, new Point(31, 27));
            Mouse.StopDragging(uIArtsGridClient, -14, 346);

            // Move 'Art's Grid' client from (43, 355) to (344, 111)
            Mouse.StartDragging(uIArtsGridClient, new Point(43, 355));
            Mouse.StopDragging(uIArtsGridClient, 301, -244);

            // Click 'Art's Grid' client
            Mouse.Click(uIArtsGridClient, new Point(33, 383));

            // Move 'Art's Grid' client from (33, 383) to (741, 34)
            Mouse.StartDragging(uIArtsGridClient, new Point(33, 383));
            Mouse.StopDragging(uIArtsGridClient, 708, -349);

            // Click 'Art's Grid' client
            Mouse.Click(uIArtsGridClient, new Point(175, 440));

            // Move 'Art's Grid' client from (44, 359) to (740, 36)
            Mouse.StartDragging(uIArtsGridClient, new Point(44, 359));
            Mouse.StopDragging(uIArtsGridClient, 696, -323);

            // Move 'Art's Grid' client from (367, 166) to (110, 159)
            Mouse.StartDragging(uIArtsGridClient, new Point(367, 166));
            Mouse.StopDragging(uIArtsGridClient, -257, -7);

            // Click 'Art's Grid' client
            Mouse.Click(uIArtsGridClient, new Point(252, 435));

            // Click 'Art's Grid' client
            Mouse.Click(uIArtsGridClient, new Point(252, 435));

            // Move 'Art's Grid' client from (19, 181) to (342, 332)
            Mouse.StartDragging(uIArtsGridClient, new Point(19, 181));
            Mouse.StopDragging(uIArtsGridClient, 323, 151);

            // Move 'Art's Grid' client from (342, 332) to (729, 128)
            Mouse.StartDragging(uIArtsGridClient, new Point(342, 332));
            Mouse.StopDragging(uIArtsGridClient, 387, -204);

            // Click 'Art's Grid' client
            Mouse.Click(uIArtsGridClient, new Point(784, 440));

            // Click 'Close' button
            Mouse.Click(uICloseButton, new Point(30, 11));
        }
        
        #region Properties
        public UIArtsGridWindow UIArtsGridWindow
        {
            get
            {
                if ((this.mUIArtsGridWindow == null))
                {
                    this.mUIArtsGridWindow = new UIArtsGridWindow();
                }
                return this.mUIArtsGridWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIArtsGridWindow mUIArtsGridWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIArtsGridWindow : WinWindow
    {
        
        public UIArtsGridWindow()
        {
            #region Search Criteria
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Art\'s Grid";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("Art\'s Grid");
            #endregion
        }
        
        #region Properties
        public WinClient UIArtsGridClient
        {
            get
            {
                if ((this.mUIArtsGridClient == null))
                {
                    this.mUIArtsGridClient = new WinClient(this);
                    #region Search Criteria
                    this.mUIArtsGridClient.SearchProperties[WinControl.PropertyNames.Name] = "Art\'s Grid";
                    this.mUIArtsGridClient.WindowTitles.Add("Art\'s Grid");
                    #endregion
                }
                return this.mUIArtsGridClient;
            }
        }
        
        public UIArtsGridTitleBar UIArtsGridTitleBar
        {
            get
            {
                if ((this.mUIArtsGridTitleBar == null))
                {
                    this.mUIArtsGridTitleBar = new UIArtsGridTitleBar(this);
                }
                return this.mUIArtsGridTitleBar;
            }
        }
        #endregion
        
        #region Fields
        private WinClient mUIArtsGridClient;
        
        private UIArtsGridTitleBar mUIArtsGridTitleBar;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.40219.1")]
    public class UIArtsGridTitleBar : WinTitleBar
    {
        
        public UIArtsGridTitleBar(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.WindowTitles.Add("Art\'s Grid");
            #endregion
        }
        
        #region Properties
        public WinButton UICloseButton
        {
            get
            {
                if ((this.mUICloseButton == null))
                {
                    this.mUICloseButton = new WinButton(this);
                    #region Search Criteria
                    this.mUICloseButton.SearchProperties[WinButton.PropertyNames.Name] = "Close";
                    this.mUICloseButton.WindowTitles.Add("Art\'s Grid");
                    #endregion
                }
                return this.mUICloseButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUICloseButton;
        #endregion
    }
}
