﻿using System;
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
            //this.Dock = DockStyle.Fill;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            userControl11.BkColor = new SolidBrush(Color.FromArgb(trackBar1.Value, trackBar1.Value, trackBar1.Value));
            userControl11.Invalidate();
            userControl11.Refresh();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string file = path + @"auto_save.txt";
            string s = Board.BoardState;
            System.Console.WriteLine(s);
            
        }

        private void userControl11_MouseDown(object sender, MouseEventArgs e)
        {
            // checks if ClickableGame is turned on
            if (!Board.ClickableGame) return;

            int step = userControl11.Size.Width / 8;
            int x = e.Location.X / step + 1 ;
            int y = e.Location.Y / step + 1 ;
            
            bool movement_click = (Board.PossibleLocations.Count > 0) ? true : false;

            // player wants to move the piece
            if (movement_click)
            {
                Tuple<int, int> position = new Tuple<int, int>(x, y);

                //check if movement is possible
                if (Board.PossibleLocations.Contains(position))
                {
                    System.Text.StringBuilder temp_stringBuilder = new System.Text.StringBuilder(Board.BoardState);
                    // first element of possible locations is the location of the selected piece.
                    Piece piece = (Piece)Board.PosToPiece[Board.PossibleLocations[0]];
                    // change old location to x
                    temp_stringBuilder[(piece.x - 1) + (piece.y - 1) * 8] = 'x';
                    // and new location to type identifier.
                    temp_stringBuilder[(position.Item1 - 1) + (position.Item2 - 1) * 8] = piece.identifier;
                    // send to analyse data and chage boardstate
                    HandleData.AnalyseData(temp_stringBuilder.ToString());
                    userControl11.ChessBoardState = Board.BoardState;
                    // empty list
                }
                else
                {
                }
                Board.PossibleLocations = new List<Tuple<int, int>>();

            }

            else
            {
                if (authentication.checkPosToPiece(x, y))
                {
                    Piece piece = (Piece)Board.PosToPiece[new Tuple<int, int>(x, y)];
                    Board.PossibleLocations.Add(new Tuple<int, int>(x, y));
                    for (x = 1; x < 9; x++)
                    {
                        for (y = 1; y < 9; y++)
                        {
                            if (piece.check(x, y))
                            {
                                Board.PossibleLocations.Add(new Tuple<int, int>(x, y));
                            }
                        }
                    }
                }
            }
            userControl11.LocationsToColour = Board.PossibleLocations;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "Play ClickableGame")
            {
                button.Text = "Stop ClickableGame";
                Board.ClickableGame = true;
            }
            else 
            {
                button.Text = "Play ClickableGame";
                Board.ClickableGame = false;
            }
        }

    }
}