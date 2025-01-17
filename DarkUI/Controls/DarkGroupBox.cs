﻿using DarkUI.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkGroupBox : GroupBox
    {
        private Color _borderColor = ThemeProvider.Theme.Colors.DarkBorder;

        [Category("Appearance")]
        [Description("Determines the color of the border.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The rectangle that the title of the group box is drawn to,
        /// for utility purposes.
        /// </summary>
        public Rectangle TitleRect { get; private set; }

        public DarkGroupBox()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.UserPaint, true);

            ResizeRedraw = true;
            DoubleBuffered = true;
            this.ForeColor = ThemeProvider.Theme.Colors.LightText;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            var stringSize = g.MeasureString(Text, Font);

            var textColor = this.ForeColor;
            var fillColor = ThemeProvider.Theme.Colors.GreyBackground;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            using (var p = new Pen(BorderColor, 1))
            {
                var borderRect = new Rectangle(0, (int)stringSize.Height / 2, rect.Width - 1, rect.Height - ((int)stringSize.Height / 2) - 1);
                g.DrawRectangle(p, borderRect);
            }

            var titleRect = new Rectangle(rect.Left + ThemeProvider.Theme.Sizes.Padding,
                    rect.Top,
                    rect.Width - (ThemeProvider.Theme.Sizes.Padding * 2),
                    (int)stringSize.Height);
            TitleRect = new Rectangle(titleRect.Location, new Size((int)stringSize.Width + ThemeProvider.Theme.Sizes.Padding, (int)stringSize.Height));

            using (var b2 = new SolidBrush(fillColor))
            {
                var modRect = new Rectangle(titleRect.Left, titleRect.Top, Math.Min(titleRect.Width, (int)stringSize.Width), titleRect.Height);
                g.FillRectangle(b2, modRect);
            }

            using (var b = new SolidBrush(textColor))
            {
                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(Text, Font, b, titleRect, stringFormat);
            }
        }
    }
}
