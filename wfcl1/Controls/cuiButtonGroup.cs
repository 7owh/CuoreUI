﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    [DefaultEvent("Click")]
    public partial class cuiButtonGroup : UserControl
    {
        public cuiButtonGroup()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ForeColor = Color.White;
            Font = new Font("Microsoft Sans Serif", 9.75f);
        }

        private string privateContent = "Your text here!";
        public string Content
        {
            get
            {
                return privateContent;
            }
            set
            {
                privateContent = value;
                Invalidate();
            }
        }

        private Padding privateRounding = new Padding(8, 8, 8, 8);
        public Padding Rounding
        {
            get
            {
                return privateRounding;
            }
            set
            {
                privateRounding = value;
                Invalidate();
            }
        }

        private Color privateNormalBackground = CuoreUI.Drawing.PrimaryColor;
        public Color NormalBackground
        {
            get
            {
                return privateNormalBackground;
            }
            set
            {
                privateNormalBackground = value;
                Invalidate();
            }
        }

        private Color privateHoverBackground = Color.FromArgb(200, 255, 127, 80);
        public Color HoverBackground
        {
            get
            {
                return privateHoverBackground;
            }
            set
            {
                privateHoverBackground = value;
                Invalidate();
            }
        }

        private Color privatePressedBackground = CuoreUI.Drawing.PrimaryColor;
        public Color PressedBackground
        {
            get
            {
                return privatePressedBackground;
            }
            set
            {
                privatePressedBackground = value;
                Invalidate();
            }
        }

        private Color privateNormalOutline = CuoreUI.Drawing.PrimaryColor;
        public Color NormalOutline
        {
            get
            {
                return privateNormalOutline;
            }
            set
            {
                privateNormalOutline = value;
                Invalidate();
            }
        }

        private Color privateHoverOutline = CuoreUI.Drawing.PrimaryColor;
        public Color HoverOutline
        {
            get
            {
                return privateHoverOutline;
            }
            set
            {
                privateHoverOutline = value;
                Invalidate();
            }
        }

        private Color privatePressedOutline = CuoreUI.Drawing.PrimaryColor;
        public Color PressedOutline
        {
            get
            {
                return privatePressedOutline;
            }
            set
            {
                privatePressedOutline = value;
                Invalidate();
            }
        }

        private bool privateChecked = false;
        public bool Checked
        {
            get
            {
                return privateChecked;
            }
            set
            {
                privateChecked = value;
                Invalidate();
            }
        }

        private int state = 1;
        private SolidBrush privateBrush = new SolidBrush(Color.Black);
        private Pen privatePen = new Pen(Color.Black);
        StringFormat stringFormat = new StringFormat();

        private Image privateImage = null;
        public Image Image
        {
            get
            {
                return privateImage;
            }
            set
            {
                privateImage = value;
                Invalidate();
            }
        }


        private Color privateCheckedBackground = CuoreUI.Drawing.PrimaryColor;
        public Color CheckedBackground
        {
            get
            {
                return privateCheckedBackground;
            }
            set
            {
                privateCheckedBackground = value;
                Invalidate();
            }
        }

        private Color privateCheckedOutline = CuoreUI.Drawing.PrimaryColor;
        public Color CheckedOutline
        {
            get
            {
                return privateCheckedOutline;
            }
            set
            {
                privateCheckedOutline = value;
                Invalidate();
            }
        }

        private bool privateImageAutoCenter = true;

        public bool ImageAutoCenter
        {
            get
            {
                return privateImageAutoCenter;
            }
            set
            {
                privateImageAutoCenter = value;
                Invalidate();
            }
        }

        private float privateOutlineThickness = 1.6f;
        public float OutlineThickness
        {
            get
            {
                return privateOutlineThickness;
            }
            set
            {
                privateOutlineThickness = Math.Max(value, 0);
                privatePen.Width = value;
            }
        }

        private Point privateImageExpand = Point.Empty;
        public Point ImageExpand
        {
            get
            {
                return privateImageExpand;
            }
            set
            {
                privateImageExpand = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            stringFormat.Alignment = StringAlignment.Center;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Rectangle modifiedCR = ClientRectangle;
            modifiedCR.Inflate(-1, -1);

            if (Rounding.Left == 0 & Rounding.Bottom == 0)
            {
                modifiedCR.Inflate(-1, 0);
            }

            GraphicsPath roundBackground = Helper.RoundRect(modifiedCR, Rounding);

            Color renderedBackgroundColor;
            Color renderedOutlineColor;
            Color tint = ImageTint;
            switch (state)
            {
                case 2:
                    renderedBackgroundColor = HoverBackground;
                    renderedOutlineColor = HoverOutline;
                    break;

                case 3:
                    renderedBackgroundColor = PressedBackground;
                    renderedOutlineColor = PressedOutline;
                    break;

                case 1:
                    renderedBackgroundColor = NormalBackground;
                    renderedOutlineColor = NormalOutline;
                    break;

                default:
                    renderedBackgroundColor = Color.Black;
                    renderedOutlineColor = Color.Black;
                    break;
            }
            if (Checked)
            {
                tint = CheckedImageTint;
                renderedBackgroundColor = CheckedBackground;
                renderedOutlineColor = CheckedOutline;
            }
            else if (state == 2)
            {
                tint = HoveredImageTint;
            }
            else if (state == 3)
            {
                tint = PressedImageTint;
            }

            privateBrush.Color = renderedBackgroundColor;
            privatePen.Color = renderedOutlineColor;

            e.Graphics.FillPath(privateBrush, roundBackground);

            if (OutlineThickness > 0)
            {
                e.Graphics.DrawPath(privatePen, roundBackground);
            }

            Rectangle textRectangle = ClientRectangle;
            int textY = (Height / 2) - (Font.Height / 2);
            textRectangle.Location = new Point(0, textY);

            Rectangle imageRectangle = textRectangle;
            imageRectangle.Height = Font.Height;
            imageRectangle.Width = imageRectangle.Height;
            imageRectangle.Inflate(ImageExpand.X, ImageExpand.Y);

            if (ImageAutoCenter && privateImage != null)
            {
                imageRectangle.X = (Width / 2) - (imageRectangle.Width / 2);
                int TextOffsetFromImage = (int)e.Graphics.MeasureString(Content, Font, textRectangle.Width, stringFormat).Width;
                imageRectangle.X -= TextOffsetFromImage / 2;
                textRectangle.X += imageRectangle.Width / 2;
            }

            textRectangle.Offset(privateTextOffset);
            imageRectangle.Offset(privateImageOffset);

            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(privateContent, Font, brush, textRectangle, stringFormat);
            }

            if (privateImage != null)
            {
                float tintR = tint.R / 255f;
                float tintG = tint.G / 255f;
                float tintB = tint.B / 255f;
                float tintA = tint.A / 255f;

                // Create a color matrix that will apply the tint color
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] {tintR, 0, 0, 0, 0},
            new float[] {0, tintG, 0, 0, 0},
            new float[] {0, 0, tintB, 0, 0},
            new float[] {0, 0, 0, tintA, 0},
            new float[] {0, 0, 0, 0, 1}
                });

                // Create image attributes and set the color matrix
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);

                // Draw the image with the tint
                e.Graphics.DrawImage(
                    privateImage,
                    imageRectangle,
                    0, 0, privateImage.Width, privateImage.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }

            base.OnPaint(e);
        }

        private Color privateImageTint = Color.White;
        public Color ImageTint
        {
            get
            {
                return privateImageTint;
            }
            set
            {
                privateImageTint = value;
                Invalidate();
            }
        }

        private Color privateHoveredImageTint = Color.White;
        public Color HoveredImageTint
        {
            get
            {
                return privateHoveredImageTint;
            }
            set
            {
                privateHoveredImageTint = value;
                Invalidate();
            }
        }

        private Color privateCheckedImageTint = Color.White;
        public Color CheckedImageTint
        {
            get
            {
                return privateCheckedImageTint;
            }
            set
            {
                privateCheckedImageTint = value;
                Invalidate();
            }
        }

        private Color privatePressedImageTint = Color.White;
        public Color PressedImageTint
        {
            get
            {
                return privatePressedImageTint;
            }
            set
            {
                privatePressedImageTint = value;
                Invalidate();
            }
        }

        private Point privateImageOffset = new Point(0, 0);
        public Point ImageOffset
        {
            get
            {
                return privateImageOffset;
            }
            set
            {
                privateImageOffset = value;
                Invalidate();
            }
        }

        private Point privateTextOffset = new Point(0, 0);
        public Point TextOffset
        {
            get
            {
                return privateTextOffset;
            }
            set
            {
                privateTextOffset = value;
                Invalidate();
            }
        }

        private int privateGroup = 0;

        [Description("The group for this and other cuiButtonGroup controls to uncheck when clicked.")]
        public int Group
        {
            get
            {
                return privateGroup;
            }
            set
            {
                privateGroup = value;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            state = 3;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (ClientRectangle.Contains(e.Location))
            {
                if (state == 3)
                {
                    foreach (Control ctrl in Parent?.Controls)
                    {
                        if (ctrl is cuiButtonGroup cbg)
                        {
                            if (cbg.Group == this.Group && cbg != this)
                            {
                                cbg.Checked = false;
                            }
                        }
                    }
                    Checked = true;
                }

                state = 2;
                Invalidate();
            }
            else
            {
                state = 1;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            state = 1;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            state = 2;
            Invalidate();
        }
    }
}
