using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ChessBoardLayout
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true
                );
        }

        public Hashtable m_PieceImageMapping = new Hashtable();
        public Hashtable PieceImageMapping
        {
            get { return m_PieceImageMapping; }
            set { m_PieceImageMapping = value; Invalidate(); }
        }

        public String m_ChessBoardState;
        public String ChessBoardState
        {
            get { return m_ChessBoardState; }
            set { m_ChessBoardState = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    g.FillRectangle(((x + y) % 2 == 0) ? Brushes.White : Brushes.Black, new Rectangle(x * 80, y * 80, 80, 80));
                }
            }

            for (int i = 0; i < m_ChessBoardState.Length; i++)
            {
                char identifier = ChessBoardState[i];

                if (identifier != 'x')
                {
                    Bitmap bmp = new Bitmap(Application.StartupPath + "/images/" + PieceImageMapping[identifier]);
                    g.DrawImage(bmp, new Point((i % 8) * 80, (i / 8) * 80));
                    base.OnPaint(e);
                }
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }
    }
}