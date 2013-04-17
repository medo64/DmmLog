using DmmLogDriver;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DmmLog {
    internal partial class SidebarControl : ScrollableControl {

        public SidebarControl() {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.StandardClick, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            Devices.MeasurementUpdate += delegate(object sender, EventArgs e) {
                this.Invalidate();
            };
        }


        private readonly Int32 BaseTitleHeight = 16;
        private readonly Int32 BaseDigitHeight = 80;
        private readonly Int32 BaseInfoHeight = 16;
        private readonly Padding BaseDisplayPadding = new Padding(8);

        protected override void OnResize(EventArgs e) {
            this.DisplayMargin = new Padding(SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight, (int)(SystemInformation.VerticalScrollBarWidth * 1.5), SystemInformation.HorizontalScrollBarHeight);

            using (var g = this.CreateGraphics()) {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                var multiplierX = (g.DpiX / 96.0F);
                var multiplierY = (g.DpiY / 96.0F);

                var titleHeight = (int)(this.BaseTitleHeight * multiplierY);
                var digitHeight = (int)(this.BaseDigitHeight * multiplierY);
                var infoHeight = (int)(this.BaseInfoHeight * multiplierY);

                this.DisplayPadding = new Padding((int)(this.BaseDisplayPadding.Left * multiplierX), (int)(this.BaseDisplayPadding.Top * multiplierY), (int)(this.BaseDisplayPadding.Right * multiplierX), (int)(this.BaseDisplayPadding.Bottom * multiplierY));

                this.DigitIntegralFont = new Font(Settings.SidebarFontName, digitHeight, FontStyle.Bold, GraphicsUnit.Pixel);
                this.DigitFractionalFont = new Font(Settings.SidebarFontName, digitHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                this.DigitUnitFont = new Font(Settings.SidebarFontName, digitHeight / 3, FontStyle.Regular, GraphicsUnit.Pixel);
                this.DigitSize = new Size((int)Math.Ceiling(g.MeasureString("X", this.DigitIntegralFont, 0, StringFormat.GenericTypographic).Width) + SystemInformation.BorderSize.Width * 2, digitHeight);
                var digitsWidth = (1 + Settings.SidebarDigitCount + 1) * this.DigitSize.Width;

                this.TitleFont = new Font(Settings.SidebarFontName, titleHeight, GraphicsUnit.Pixel);
                this.TitleSize = new Size(digitsWidth, titleHeight);

                this.InfoSize = new Size(digitsWidth, infoHeight);

                this.DisplaySize = new Size(digitsWidth, titleHeight + digitHeight + infoHeight);
            }

            this.MinimumSize = new Size(this.DisplayMargin.Horizontal + this.DisplayPadding.Horizontal + this.DisplaySize.Width, this.MinimumSize.Height);
            this.MaximumSize = new Size(this.MinimumSize.Width, this.MaximumSize.Height);
            base.OnResize(e);
        }

        private Padding DisplayMargin;
        private Padding DisplayPadding;
        private Size DisplaySize;

        private Font TitleFont;
        private Size TitleSize;
        private Font DigitIntegralFont;
        private Font DigitFractionalFont;
        private Size DigitSize;
        private Font DigitUnitFont;
        private Size InfoSize;

        private StringFormat SFCenterMiddle = new StringFormat(StringFormat.GenericTypographic) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        private StringFormat SFRightMiddle = new StringFormat(StringFormat.GenericTypographic) { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
        private StringFormat SFLeftTop = new StringFormat(StringFormat.GenericTypographic) { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };



        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.Clear(SystemColors.Window);
            e.Graphics.CompositingQuality = CompositingQuality.Default;
            e.Graphics.InterpolationMode = InterpolationMode.Default;
            e.Graphics.SmoothingMode = SmoothingMode.Default;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var y = 0;
            var x = this.DisplayMargin.Left;
            foreach (var device in Devices.LoadedDevices) {
                y += this.DisplayMargin.Top;

                e.Graphics.FillRectangle(SystemBrushes.Info, x, y, this.DisplaySize.Width + this.DisplayPadding.Horizontal, this.DisplaySize.Height + this.DisplayPadding.Vertical);
                e.Graphics.DrawString(device.DisplayName, this.TitleFont, SystemBrushes.GrayText, new Rectangle(x + this.DisplayPadding.Left, y + this.DisplayPadding.Top, this.TitleSize.Width, this.TitleSize.Height), this.SFLeftTop);
                PaintDigits(e.Graphics, x + this.DisplayPadding.Left, y + this.DisplayPadding.Top + this.TitleSize.Height, device.CurrentMeasurement);

                y += this.DisplayPadding.Vertical + this.DisplaySize.Height;
            }
            y += this.DisplayMargin.Bottom;


            this.AutoScrollMinSize = new Size(0, y);
        }

        private void PaintDigits(Graphics graphics, int x, int y, DmmMeasurement measurement) {
            if (measurement != null) {
                var value = measurement.EngineeringCoefficient;
                int integralDigitCount;
                var chars = GetChars(value, out integralDigitCount);

                if (integralDigitCount < Settings.SidebarDigitCount) { //has dot
                    var dotSize = this.DigitSize.Width / 6;
                    graphics.FillEllipse(SystemBrushes.GrayText, x + (integralDigitCount + 1) * this.DigitSize.Width - dotSize / 2, y + this.DigitSize.Height - dotSize * 2.75F, dotSize, dotSize);
                }
                for (int i = 0; i < (1 + Settings.SidebarDigitCount); i++) {
                    var font = (char.IsDigit(chars[i]) && (i <= integralDigitCount)) ? this.DigitIntegralFont : this.DigitFractionalFont;
                    var brush = (i <= integralDigitCount) ? SystemBrushes.InfoText : SystemBrushes.GrayText;
                    graphics.DrawString(chars[i].ToString(), font, brush, x + this.DigitSize.Width * i + this.DigitSize.Width / 2, y + this.DigitSize.Height / 2, SFCenterMiddle);
                }

                graphics.DrawString(measurement.SIUnit, this.DigitUnitFont, SystemBrushes.InfoText, x + this.DisplaySize.Width, y + this.DigitSize.Height / 2, SFRightMiddle);
            }
        }

        private char[] GetChars(decimal value, out int integeralDigitsCount) {
            var chars = new char[1 + Settings.SidebarDigitCount];

            chars[0] = (value >= 0) ? ' ' : '-';
            value = Math.Abs(value);

            if (Settings.SidebarSlidingDecimalPoint) {
                integeralDigitsCount = 1;
                while ((value > 10) && (integeralDigitsCount < Settings.SidebarDigitCount)) {
                    integeralDigitsCount += 1;
                    value /= 10;
                }
            } else {
                integeralDigitsCount = 3;
                value /= 100;
            }

            if (value >= 10) { //cannot display
                chars[1] = 'O';
                chars[2] = 'L';
            } else {
                for (int i = 0; i < Settings.SidebarDigitCount; i++) {
                    var d = (int)value % 10;
                    chars[1 + i] = (char)(0x30 + d);
                    value *= 10;
                }

                for (int i = 1; i < integeralDigitsCount; i++) { //remove leading zeros
                    if (chars[i] == '0') {
                        chars[i] = ' ';
                        if (Settings.SidebarSlidingMinusSign) {
                            chars[i] = chars[i - 1];
                            chars[i - 1] = ' ';
                        }
                    } else {
                        break;
                    }
                }
            }

            return chars;
        }

    }
}
