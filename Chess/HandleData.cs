using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chess
{
    static class HandleData
    {
        public static String[] GetData()
        {
            String[] lines = System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + "\\TestWithCastling.txt");
            return lines;
        }

        #region PawnPromotion
        public static void PawnPromotion(String data, int[] old_positions, int[] new_positions)
        {
            Tuple<int, int> old_coordinate = new Tuple<int, int>(((new_positions[0] % 8) + 1), (new_positions[0] / 8 + 1));
            Piece piece;
            switch (data[new_positions[0]])
            {
                case 'q':
                case 'p':
                    piece = (BlackQueen)new BlackQueen(old_coordinate.Item1, old_coordinate.Item2, 'q');
                    break;
                case 'Q':
                case 'P':
                    piece = (WhiteQueen)new WhiteQueen(old_coordinate.Item1, old_coordinate.Item2, 'Q');
                    break;
                case 'R':
                    piece = (WhiteRook)new WhiteRook(old_coordinate.Item1, old_coordinate.Item2, 'R');
                    break;
                case 'r':
                    piece = (BlackRook)new BlackRook(old_coordinate.Item1, old_coordinate.Item2, 'r');
                    break;
                case 'N':
                    piece = (WhiteKnight)new WhiteKnight(old_coordinate.Item1, old_coordinate.Item2, 'N');
                    break;
                case 'n':
                    piece = (BlackKnight)new BlackKnight(old_coordinate.Item1, old_coordinate.Item2, 'n');
                    break;
                case 'B':
                    piece = (WhiteBishop)new WhiteBishop(old_coordinate.Item1, old_coordinate.Item2, 'B');
                    break;
                case 'b':
                    piece = (BlackBishop)new BlackBishop(old_coordinate.Item1, old_coordinate.Item2, 'b');
                    break;
                default:
                    piece = null;
                    break;
            }
            Board.PosToPiece.Remove(new Tuple<int, int>((old_positions[0] % 8) + 1, (old_positions[0] / 8) + 1));
            Board.PosToPiece[old_coordinate] = piece;
            Board.GenerateBoardState();
            Board.n_passantPawn = null;
        }
        #endregion

        public static void AnalyseData(String data)
        {
            Board.ContinueWithNormalMovement = true;
            Board.InAnalyseData = true;
            // String.Compare is not useful as I'll have to run the check again if it comes out to be false;
            bool move_occured = false;
            int[] old_positions = {-1, -1};
            int[] new_positions = {-1, -1};
            for (int i = 0, j = 0, k =0; i < data.Length; i++)
            {
                if (data[i] != Board.BoardState[i])
                {
                    if (data[i] == 'x')
                    {
                        // This would obviously be the old position of the moved piece
                        move_occured = true;
                        old_positions[j] = i;
                        j++;
                    }
                    else
                    {
                        // This would be the new position. It could have been an empty place or a kill might have occured.
                        move_occured = true;
                        new_positions[k] = i;
                        k++;
                    }
                }
            }


            if (move_occured)
            {
                #region castling
                if (Board.CastlingMode) 
                {
                    Tuple<int, int> old_coordinate = new Tuple<int, int>(((old_positions[0] % 8) + 1), (old_positions[0] / 8 + 1));
                    WhiteRook piece = (WhiteRook)Board.PosToPiece[old_coordinate];
                    int extent = Math.Abs(old_coordinate.Item1 - 4) - 1;
                    if (piece.castlingPossible && piece.identifier == 'R')
                    {
                        // check till the edge of king. That means that all the places are empty..!!
                        if (piece.check((piece.x == 1) ? 3: 5, 1))
                        {
                            // handling the rook
                            Board.PosToPiece.Remove(old_coordinate);
                            piece.x = (extent==2) ? 3 : 5;
                            piece.y = 1;
                            piece.castlingPossible = false;
                            Board.PosToPiece[new Tuple<int, int> (piece.x, piece.y)] = piece;
                            WhiteKing KingPiece = (WhiteKing)Board.PosToPiece[new Tuple<int, int>(4, 1)];
                            KingPiece.x = (extent == 2) ? 2 : 6;
                            KingPiece.castlingPossible = false;
                            Board.PosToPiece.Remove(new Tuple<int, int>(4, 1));
                            Board.PosToPiece[new Tuple<int, int> (KingPiece.x, KingPiece.y)] = KingPiece;
                            Board.BoardState = data;
                        }

                    }
                    else if (piece.castlingPossible && piece.identifier == 'r')
                    {
                        // check till the edge of king. That means that all the places are empty..!!
                        if (piece.check((piece.x == 1) ? 3 : 5, 1))
                        {
                            // handling the rook
                            Board.PosToPiece.Remove(old_coordinate);
                            piece.x = (extent == 2) ? 3 : 5;
                            piece.y = 8;
                            piece.castlingPossible = false;
                            Board.PosToPiece[new Tuple<int, int>(piece.x, piece.y)] = piece;
                            BlackKing KingPiece = (BlackKing)Board.PosToPiece[new Tuple<int, int>(4, 8)];
                            KingPiece.x = (extent == 2) ? 2 : 6;
                            KingPiece.castlingPossible = false;
                            Board.PosToPiece.Remove(new Tuple<int, int>(4, 8));
                            Board.PosToPiece[new Tuple<int, int>(KingPiece.x, KingPiece.y)] = KingPiece;
                            Board.BoardState = data;
                        }
                    }
                    Board.n_passantPawn = null;
                    Board.CastlingMode = false;
                }
                #endregion

                if (Board.ClickableGame)
                {
                    Tuple<int, int> old_coordinates = new Tuple<int, int>((old_positions[0] % 8) + 1, (old_positions[0] / 8) + 1);
                    Piece piece = (Piece)Board.PosToPiece[old_coordinates];
                    if (authentication.checkN_PassantKill(piece))
                    {
                        Board.PosToPiece.Remove(new Tuple<int, int>(Board.n_passantPawn.x, Board.n_passantPawn.y));
                        Board.PosToPiece.Remove(old_coordinates);
                        piece.x = (new_positions[0] % 8) + 1;
                        piece.y = (new_positions[0] / 8) + 1;
                        Board.PosToPiece[new Tuple<int, int>(piece.x, piece.y)] = piece;
                        Board.GenerateBoardState();
                        Board.ContinueWithNormalMovement = false;
                        Board.n_passantPawn = null;
                    }
                }

                #region NormalMovement
                // normal movement or killing move
                if (((old_positions[1] == -1 && new_positions[1] == -1) == true) && Board.ContinueWithNormalMovement)
                {
                    Board.n_passantPawn = null;
                    Tuple<int, int> old_coordinate = new Tuple<int, int>(((old_positions[0] % 8) + 1), ((old_positions[0] / 8) + 1));
                    Piece piece = (Piece)Board.PosToPiece[old_coordinate];

                    if (((piece is WhitePawn) && (piece.y == 7)) ||
                        ((piece is BlackPawn) && (piece.y == 2)))
                    {
                        PawnPromotion(data, old_positions, new_positions);
                    }

                    else if (piece.check((new_positions[0] % 8) + 1, (new_positions[0] / 8) + 1))
                    {
                        Board.BoardState = data;
                        Board.PosToPiece.Remove(old_coordinate);
                        piece.x = (new_positions[0] % 8) + 1;
                        piece.y = (new_positions[0] / 8) + 1;
                        Board.PosToPiece[new Tuple<int, int>(piece.x, piece.y)] = piece;
                    }
                    
                }
                #endregion

                #region N_Passant Movement
                //N_passantMovement
                else if ((new_positions[1] == -1 && old_positions[1] != -1))
                {
                    Tuple<int, int> old_coordinates;
                    if (Board.n_passantPawn == null) return;
                    if (
                        (((old_positions[0] % 8) + 1) == Board.n_passantPawn.x) &&
                        (((old_positions[0] / 8) + 1) == Board.n_passantPawn.y))
                    {
                        old_coordinates = new Tuple<int, int>((old_positions[1] % 8) + 1, old_positions[1] / 8 + 1);
                    }
                    else
                    {
                        old_coordinates = new Tuple<int, int>((old_positions[0] % 8) + 1, old_positions[0] / 8 + 1);
                    }
                    Piece piece = (Piece)Board.PosToPiece[old_coordinates];
                    if (authentication.checkN_PassantKill(piece))
                    {
                        Board.BoardState = data;
                        Board.PosToPiece.Remove(new Tuple<int, int> (Board.n_passantPawn.x, Board.n_passantPawn.y));
                        Board.PosToPiece.Remove(old_coordinates);
                        piece.x = (new_positions[0] % 8) + 1;
                        piece.y = (new_positions[0] / 8) + 1;
                        Board.PosToPiece[new Tuple<int, int>(piece.x, piece.y)] = piece;
                    }
                }

                

                #endregion
            }
            Board.InAnalyseData = false;
            //don't keep it hardcoded.
            authentication.MateLogic(false);
//            authentication.InCheckLogic(false);
//            if (Board.InCheck)
            {
 //               if (authentication.KingHasLegalMoves(false));
            }
        }
    }
}
