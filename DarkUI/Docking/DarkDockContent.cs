﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
    [ToolboxItem(false)]
    public class DarkDockContent : UserControl
    {
        #region Event Handler Region

        public event EventHandler DockTextChanged;

        //Event for when the content is closed.
        public delegate void OnContentClosedHandler();
        public event OnContentClosedHandler OnContentClosed;

        //Event for when the dock panel on this content is changed.
        public delegate void OnDockPanelChangedHandler(DarkDockPanel panel);
        public event OnDockPanelChangedHandler OnDockPanelChanged;

        #endregion

        #region Field Region

        private string _dockText;
        private Image _icon;

        #endregion

        #region Property Region

        [Category("Appearance")]
        [Description("Determines the text that will appear in the content tabs and headers.")]
        public string DockText
        {
            get { return _dockText; }
            set
            {
                var oldText = _dockText;

                _dockText = value;

                if (DockTextChanged != null)
                    DockTextChanged(this, null);

                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("Determines the icon that will appear in the content tabs and headers.")]
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                Invalidate();
            }
        }

        [Category("Layout")]
        [Description("Determines the default area of the dock panel this content will be added to.")]
        [DefaultValue(DarkDockArea.Document)]
        public DarkDockArea DefaultDockArea { get; set; }

        [Category("Behavior")]
        [Description("Determines the key used by this content in the dock serialization.")]
        public string SerializationKey { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockPanel DockPanel 
        { 
            get { return dockPanel; }
            internal set
            {
                dockPanel = value;
                OnDockPanelChanged?.Invoke(value);
            }
        }
        private DarkDockPanel dockPanel;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockRegion DockRegion { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockGroup DockGroup { get; internal set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DarkDockArea DockArea { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Order { get; set; }

        //Whether we're currently closing or not.
        public bool Closing
        {
            get { return closing; }
            set
            {
                closing = value;
                if (value)
                    Close();
            }
        }
        bool closing = false;

        #endregion

        #region Constructor Region

        public DarkDockContent()
        {
            BackColor = System.Drawing.Color.Transparent;// ThemeProvider.Theme.Colors.GreyBackground;
        }

        #endregion

        #region Method Region

        public virtual void Close()
        {
            //Call closing method.
            closing = true;
            ContentClosing();

            //Remove ourselves from the dock panel.
            if (DockPanel != null && Closing)
            {
                DockPanel.RemoveContent(this);
                OnContentClosed?.Invoke();
            }
        }

        public virtual void ContentClosing() { }

        #endregion

        #region Event Handler Region

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (DockPanel == null)
                return;

            DockPanel.ActiveContent = this;
        }

        #endregion
    }
}
