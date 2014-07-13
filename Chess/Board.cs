using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Chess
{
    public class Board
    {
        public static  Hashtable PosToPiece = new Hashtable();
        public static bool ClickableGame = false;
        public static String BoardState;
        public static bool ContinueWithNormalMovement = true;
        public static Piece n_passantPawn = null;
        //public static bool PromotePawnToQueen = false;
        public static bool CastlingMode = false;

        public static List<Tuple<int, int>> PossibleLocations = new List<Tuple<int, int>>();

        public static bool InAnalyseData = false;

        public static void GenerateBoardState()
        {
            StringBuilder temp = new StringBuilder(); ;

            for (int y = 1; y <= 8; y++)
            {
                for (int x = 1; x <= 8; x++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(x, y);
                    if (authentication.checkPosToPiece(x, y))
                    {
                        Piece p = (Piece)Board.PosToPiece[new Tuple<int, int>(x, y)];
                        temp.Append(p.identifier);
                    }
                    else
                    {
                        temp.Append('x');
                    }
                }
            }

            Board.BoardState = temp.ToString();
        }

        private void addToHashTables(Piece piece)
        {
            PosToPiece[new Tuple<int, int>(piece.x, piece.y)] = piece;
        }

        /// <summary>
        /// Identifiers
        /// White are uppercase
        /// WhitePawn -> P
        /// BlackPawn -> p
        /// WhiteRook -> R
        /// BlackRook -> r
        /// WhiteBishop -> B
        /// BlackBishop -> b
        /// WhiteKnight -> N
        /// BlackKnight -> n
        /// WhiteQueen -> Q
        /// BlackQueen -> q
        /// WhiteKing -> K
        /// BlackKing -> k
        /// </summary>

        WhitePawn wp;
        BlackPawn bp;
        WhiteRook wr;
        WhiteBishop wb;
        WhiteKnight wn;
        WhiteKing wk;
        WhiteQueen wq;
        BlackRook br;
        BlackBishop bb;
        BlackKnight bn;
        BlackKing bk;
        BlackQueen bq;

        public Board()
        {

            #region addingPawns
            
            for (int i = 1; i <= 8; i++)
            {
                wp = new WhitePawn(i, 2, 'P');
                addToHashTables(wp);
                bp = new BlackPawn(i, 7, 'p');
                addToHashTables(bp);
            }

            
            #endregion

            #region Adding Pieces manually

            wr = new WhiteRook(1, 1, 'R');
            addToHashTables(wr);

            wb = new WhiteBishop(3, 1, 'B');
            addToHashTables(wb);

            wn = new WhiteKnight(2, 1, 'N');
            addToHashTables(wn);

            wk = new WhiteKing(4, 1, 'K');
            addToHashTables(wk);

            wq = new WhiteQueen(5, 1, 'Q');
            addToHashTables(wq);

            wr = new WhiteRook(8, 1, 'R');
            addToHashTables(wr);

            wb = new WhiteBishop(6, 1, 'B');
            addToHashTables(wb);

            wn = new WhiteKnight(7, 1, 'N');
            addToHashTables(wn);


            br = new BlackRook(1, 8, 'r');
            addToHashTables(br);

            bb = new BlackBishop(3, 8, 'b');
            addToHashTables(bb);

            bn = new BlackKnight(2, 8, 'n');
            addToHashTables(bn);

            bk = new BlackKing(4, 8, 'k');
            addToHashTables(bk);

            bq = new BlackQueen(5, 8, 'q');
            addToHashTables(bq);

            br = new BlackRook(8, 8, 'r');
            addToHashTables(br);

            bb = new BlackBishop(6, 8, 'b');
            addToHashTables(bb);

            bn = new BlackKnight(7, 8, 'n');
            addToHashTables(bn);


            #endregion

            GenerateBoardState();
        }
    }
}