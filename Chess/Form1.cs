using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private String[] data;
        private int i = 0;

        private void button1_Click(object sender, EventArgs e)
        {
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HandleData.AnalyseData(data[i]);
                i++;
                userControl11.ChessBoardState = Board.BoardState;
            }
            catch(Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data = HandleData.GetData();
        }

        private void button_DrawBmp_Click(object sender, EventArgs e)
        {
            /*
            Bitmap bmpback = new Bitmap(Application.StartupPath + "/images/background.png");
            Graphics g = Graphics.FromImage(bmpback);

            Bitmap bmp = new Bitmap(Application.StartupPath + "/images/bishop.png");

            g.DrawImage( bmp, new Point(0, 320));

            bmp.Save("hello.png");
            
            panel1.BackgroundImage = bmp;

            userControl11.ControlString = "932865923865";
            userControl11.Invalidate();
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Board chessboard = new Board();
            userControl11.ChessBoardState = Board.BoardState;
            System.Collections.Hashtable mapping = new System.Collections.Hashtable();
            mapping['R'] = "rook.png";
            mapping['N'] = "knight.png";
            mapping['B'] = "bishop.png";
            mapping['K'] = "king.png";
            mapping['Q'] = "queen.png";
            mapping['P'] = "pawn.png";
            mapping['r'] = "rook_b.png";
            mapping['n'] = "knight_b.png";
            mapping['b'] = "bishop_b.png";
            mapping['k'] = "king_b.png";
            mapping['q'] = "queen_b.png";
            mapping['p'] = "pawn_b.png";
            userControl11.PieceImageMapping = mapping;
        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }

    }
}
