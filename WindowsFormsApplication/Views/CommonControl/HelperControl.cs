using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication.Views.CommonControl
{
    public partial class HelperControl : UserControl
    {
        #region イベント定義
        /// <summary>
        /// 割当コントロールの変更イベント
        /// </summary>
        public event EventHandler AssignControlChanged;
        #endregion

        #region メンバー変数
        /// <summary>
        /// 割り当てるコントロール
        /// </summary>
        private Control assignControl;

        /// <summary>
        /// 背景イメージ
        /// </summary>
        private Bitmap backGroundImage;

        /// <summary>
        /// 起動前にアクティブなコントロール
        /// </summary>
        private Control prevActiveControl;

        /// <summary>
        /// 現在表示中のページ番号
        /// </summary>
        private int helpPageNumber;

        /// <summary>
        /// 表示してから移動していない状態
        /// </summary>
        private bool isFirstView;

        /// <summary>
        /// ヘルプの操作説明
        /// </summary>
        private static string operationDescript;

        /// <summary>
        /// ヘルプ情報表示
        /// </summary>
        private List<Tuple<Control, string>> helpGuideList;
        #endregion

        #region プロパティ
        /// <summary>
        /// 割り当てるコントロール
        /// </summary>
        public Control AssignControl
        {
            get
            {
                return this.assignControl;
            }
            set
            {
                if (this.assignControl != value)
                {
                    if (this.assignControl != null)
                    {
                        this.assignControl.SizeChanged -= AssignControl_SizeChanged;
                    }
                    this.assignControl = value;
                    OnAssignControlChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// グレーアウトの色
        /// </summary>
        [DefaultValue(typeof(Color), "16,0,0,0")]
        public Color GrayOutColor
        {
            get;
            set;
        }

        /// <summary>
        /// コントロールの表示設定を行います。
        /// </summary>
        protected new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        /// <summary>
        /// ヘルプ情報表示設定
        /// </summary>
        public List<Tuple<Control, string>> HelpGuide
        {
            get
            {
                return this.helpGuideList;
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HelperControl()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            this.SetStyle(ControlStyles.Opaque, true);

            this.GrayOutColor = Color.FromArgb(16, 0, 0, 0);
            base.Visible = false;

            this.helpPageNumber = 0;
            this.helpGuideList = new List<Tuple<Control, string>>();

            operationDescript = "ヘルプ表示モードになりました。\n\nヘルプモード終了\tESC\n次の手順\t\t左クリック\n前の手順\t\tCtrl+左クリック または、右クリック";

            InitializeComponent();
        }
        #endregion

        #region イベント発行
        /// <summary>
        /// 背景の描画
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
        }

        /// <summary>
        /// 背景の描画
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs pevent)
        {
            using (Bitmap cacheBitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                Graphics graphics = Graphics.FromImage(cacheBitmap);

                if (this.AssignControl != null)
                {
                    if (backGroundImage == null)
                    {
                        backGroundImage = new Bitmap(this.Bounds.Width, this.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    }

                    this.DrawBackControl(this.AssignControl, backGroundImage);
                    // 親コントロールとの間のコントロールを親側から描画
                    for (int i = this.Parent.Controls.Count - 1; i >= 0; i--)
                    {
                        Control c = this.AssignControl.Controls[i];
                        if (c == this)
                        {
                            break;
                        }
                        if (this.Bounds.IntersectsWith(c.Bounds) == false)
                        {
                            continue;
                        }
                        this.DrawBackControl(c, backGroundImage);
                    }

                    graphics.DrawImage(backGroundImage, Point.Empty);
                }

                using (var brush = new SolidBrush(this.GrayOutColor))
                {
                    // グレーアウト
                    graphics.FillRectangle(brush, this.Bounds);
                }

                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                if (0 <= helpPageNumber && helpPageNumber < this.helpGuideList.Count)
                {
                    // ガイド線の描画
                    using (var pen = new Pen(Color.FromArgb(192, Color.Orange), 2))
                    {
                        pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        var drawRect = this.helpGuideList[helpPageNumber].Item1.Bounds;
                        drawRect.Inflate(2, 2);
                        graphics.DrawRectangle(pen, drawRect);
                    }
                }

                // 使用方法を表示
                if (isFirstView == true)
                {
                    // ガイド線の描画
                    SizeF drawStringSize = graphics.MeasureString(operationDescript, this.Font);

                    RectangleF drawRect = this.Bounds;

                    float widthHalf = (drawRect.Width) / 2;
                    drawRect.X = drawRect.Left + (widthHalf - (drawStringSize.Width / 2));
                    drawRect.Width = drawStringSize.Width;

                    drawRect.Y = drawRect.Y + 10;
                    drawRect.Height = drawStringSize.Height;

                    drawRect.Inflate(2, 2);
                    using (var brush = new SolidBrush(Color.FromArgb(64, Color.White)))
                    {
                        graphics.FillRectangle(brush, drawRect);
                    }
                    using (var pen = new Pen(Color.FromArgb(64, Color.Black)))
                    {
                        graphics.DrawRectangle(pen, Rectangle.Round(drawRect));
                    }

                    drawRect.Inflate(-2, -2);
                    drawRect.X += 2;
                    drawRect.Y += 2;
                    using (var brush = new SolidBrush(Color.Black))
                    {
                        graphics.DrawString(operationDescript, this.Font, brush, drawRect);
                    }
                }

                pevent.Graphics.DrawImage(cacheBitmap, Point.Empty);
            }
        }

        /// <summary>
        /// 割当コントロールの変更
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAssignControlChanged(EventArgs e)
        {
            MethodInvoker methodInvoker = delegate()
            {
                if (this.AssignControl == null)
                {
                    this.Visible = false;
                    return;
                }

                this.Bounds = this.AssignControl.Bounds;
                this.AssignControl.SizeChanged += AssignControl_SizeChanged;
            };

            if (this.InvokeRequired == true)
            {
                this.Invoke(methodInvoker);
            }
            else
            {
                methodInvoker();
            }

            if (AssignControlChanged != null)
            {
                AssignControlChanged(this, e);
            }
        }
        #endregion

        #region イベント
        /// <summary>
        /// 割当コントロールのサイズ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssignControl_SizeChanged(object sender, EventArgs e)
        {
            MethodInvoker methodInvoker = delegate()
            {
                if (this.AssignControl == null)
                {
                    this.Visible = false;

                    return;
                }

                if (this.backGroundImage != null)
                {
                    Bitmap bitmap = this.backGroundImage;
                    this.backGroundImage = null;
                    bitmap.Dispose();
                }

                this.Bounds = this.AssignControl.Bounds;
            };

            if (this.InvokeRequired == true)
            {
                this.Invoke(methodInvoker);
            }
            else
            {
                methodInvoker();
            }
        }

        /// <summary>
        /// キーボード押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                // ESCで終了
                this.helpPageNumber = -1;
                SetGuideTooltip();
                this.Visible = false;
                this.helpPageNumber = 0;

                if (prevActiveControl != null)
                {
                    prevActiveControl.Select();
                    prevActiveControl = null;
                }
            }
        }

        /// <summary>
        /// マウスクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperControl_MouseClick(object sender, MouseEventArgs e)
        {
            isFirstView = false;
            if (e.Button == System.Windows.Forms.MouseButtons.Left && (Control.ModifierKeys & Keys.Control) != Keys.Control)
            {
                // 次
                helpPageNumber++;
                SetGuideTooltip();
                if (this.helpGuideList.Count <= helpPageNumber)
                {
                    this.Visible = false;
                    this.helpPageNumber = 0;
                    if (prevActiveControl != null)
                    {
                        prevActiveControl.Select();
                        prevActiveControl = null;
                    }
                    return;
                }
                this.Refresh();
            }
            else if ((e.Button == System.Windows.Forms.MouseButtons.Left && (Control.ModifierKeys & Keys.Control) == Keys.Control)
                || e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // 前
                helpPageNumber = Math.Max(helpPageNumber - 1, 0);
                SetGuideTooltip();
                this.Refresh();
            }
        }
        #endregion

        #region 描画処理
        /// <summary>
        /// コントロールの描画(親コントロールのイメージを描画)
        /// </summary>
        /// <param name="c"></param>
        /// <param name="pevent"></param>
        private void DrawBackControl(Control c, Bitmap bmp)
        {
            try
            {
                c.DrawToBitmap(bmp, c.Bounds);
            }
            catch
            {
            }
        }
        #endregion

        #region 表示処理
        /// <summary>
        /// ヘルプ画面の表示処理
        /// </summary>
        public new void Show()
        {
            Show(null);
        }

        /// <summary>
        /// ヘルプ画面の表示処理
        /// </summary>
        public void Show(Control sender)
        {
            if (this.AssignControl == null || this.helpGuideList.Count <= 0 || this.Visible == true)
            {
                return;
            }

            this.Visible = true;
            this.helpPageNumber = 0;
            if (sender != null)
            {
                for (int helpIndex = 0; helpIndex < this.helpGuideList.Count; helpIndex++)
                {
                    var data = this.helpGuideList[helpIndex];
                    if (data.Item1 == sender)
                    {
                        // 一致するコントロールを見つける
                        this.helpPageNumber = helpIndex;
                        break;
                    }
                }
            }

            // ツールチップの表示
            SetGuideTooltip();

            // 起動したためフラグを立てる
            isFirstView = true;

            // 現在のアクティブコントロールを記憶
            prevActiveControl = sender;
            // 自身をアクティブにする
            this.Select();
        }

        /// <summary>
        /// コントロールユーザから非表示にします。
        /// </summary>
        protected new void Hide()
        {
            base.Hide();
        }
        #endregion

        #region ガイドの表示
        /// <summary>
        /// ガイド表示用ツールチップの設定
        /// </summary>
        private void SetGuideTooltip()
        {
            if (helpPageNumber < 0 || this.helpGuideList.Count <= helpPageNumber)
            {
                this.toolTip.ToolTipTitle = "";
                this.toolTip.Hide(this);

                return;
            }

            var guideData = this.helpGuideList[helpPageNumber];

            this.toolTip.ToolTipTitle = string.Format("手順{0:D2}", helpPageNumber + 1);
            this.toolTip.Show(guideData.Item2, this, guideData.Item1.Bounds.Left, guideData.Item1.Bounds.Bottom + 7);
        }
        #endregion
    }
}
