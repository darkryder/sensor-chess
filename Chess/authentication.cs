using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    public static class authentication
    {
        public static bool checkPosToPiece(int x, int y)
        {
            Tuple<int, int> temp_coordinates = new Tuple<int, int>(x, y);
            if (Board.PosToPiece.ContainsKey(temp_coordinates)) return true;
            return false;
        }


        public static bool checkBoundaryCondition(int x_old, int y_old, int x_new, int y_new)
        {
            if ((functions.liesBetween(x_old, 1, 8) &&
                functions.liesBetween(y_old, 1, 8) &&
                functions.liesBetween(x_new, 1, 8) &&
                functions.liesBetween(y_new, 1, 8)) == true
                ) return true;
            return false;
        }

        public static Tuple<bool, bool> checkStraightMovement(Piece piece, int x_new, int y_new)
        {
            // returns 2 data in the variables (move_possible, will kill)

            // ensures that it is a straight line
            bool move_possible = true;
            bool will_kill = false;
            
            if ((piece.x != x_new) && (piece.y != y_new))
            {
                move_possible = will_kill = false;
                Tuple<bool, bool> result = new Tuple<bool, bool>(move_possible, will_kill);
                return result;
            }
            
            List<Tuple<int, int >> temp_locations = new List<Tuple<int, int>>();
            #region fill temp_locations
            for (int y = piece.y; y<= y_new; y++)
            {
                Tuple<int, int> temp_coords = new Tuple<int, int>(x_new, y);
                temp_locations.Add(temp_coords);
            }
            for (int y = y_new; y<= piece.y; y++)
            {
                Tuple<int, int> temp_coords = new Tuple<int, int>(x_new, y);
                temp_locations.Add(temp_coords);
            }
            for (int x = piece.x; x<= x_new; x++)
            {
                Tuple<int, int> temp_coords = new Tuple<int, int>(x, y_new);
                temp_locations.Add(temp_coords);
            }
            for (int x = x_new; x <= piece.x; x++)
            {
                Tuple<int, int> temp_coords = new Tuple<int, int>(x, y_new);
                temp_locations.Add(temp_coords);
            }
            #endregion

            foreach (var i in temp_locations)
            {
                if (checkPosToPiece(i.Item1, i.Item2))
                {
                    Piece j = (Piece)Board.PosToPiece[i];
                    
                    if ((j.colour != piece.colour) &&
                        (j.x == x_new) && (j.y == y_new)) will_kill = true;

                    if (
                        ((j.colour == piece.colour) && (j != piece)) ||
                        ((j.colour != piece.colour) && ((j.x != x_new) || (j.y != y_new))))
                    {
                        move_possible = will_kill = false;
                        Tuple<bool, bool> result = new Tuple<bool, bool>(move_possible, will_kill);
                        return result;
                    }
                }
            }
            Tuple<bool, bool> result1 = new Tuple<bool, bool>(move_possible, will_kill);
            return result1;
        }

        public static Tuple<bool, bool> checkDiagonalMovement(Piece piece, int x_new, int y_new)
        {
            if (piece.x == x_new || piece.y == y_new) return new Tuple<bool, bool>(false, false);
            
            if (Math.Abs(((float)piece.x - (float)x_new) / ((float)piece.y - (float)y_new)) != 1)
                return new Tuple<bool, bool>(false, false);
         
            List<Tuple<int, int>> tempLocations = new List<Tuple<int, int>>();

            #region filling tempLocations
            if (x_new > piece.x)
            {
                if (y_new > piece.y)
                {
                    for(int i = 1; i <= x_new - piece.x; i++)
                    {
                        tempLocations.Add(new Tuple<int, int>(piece.x + i, piece.y + i));
                    }
                }
                else
                {
                    for(int i = 1; i <= x_new - piece.x; i++)
                    {
                        tempLocations.Add(new Tuple<int, int>(piece.x + i, piece.y - i));
                    }
                }
            }
            else
            {
                if (y_new > piece.y)
                {
                    for(int i = 1; i <= piece.x - x_new; i++)
                    {
                        tempLocations.Add(new Tuple<int, int>(piece.x - i, piece.y + i));
                    }
                }
                else
                {
                    for(int i = 1; i <= piece.x - x_new; i++)
                    {
                        tempLocations.Add(new Tuple<int, int>(piece.x - i, piece.y - i));
                    }
                }
            }
            #endregion

            bool move_possible = true;
            bool will_kill = false;
            bool early_break = false;
            Tuple<bool, bool> answer;
            foreach(var i in tempLocations)
            {
                if (checkPosToPiece(i.Item1, i.Item2))
                {
                    var j = (Piece)Board.PosToPiece[i];
                    
                    if ((j.colour != piece.colour) &&
                        (j.x == x_new) && (j.y == y_new))
                    {
                        will_kill = true;
                    }

                    else if ((j.colour == piece.colour) ||
                        (j.colour != piece.colour && (j.x != x_new || j.y != y_new))
                        )
                    {
                        early_break = true;
                        break;
                    }
                }
            }
            if (early_break)
            {
                answer =  new Tuple<bool, bool>(false, false);
            }
            else
            {
                answer = new Tuple<bool, bool>(move_possible, will_kill);
            }
            return answer;
        }

        public static bool checkKnightMovement(Piece piece, int x_new, int y_new)
        {
            bool a = (((Math.Abs(piece.x - x_new) == 2) && (Math.Abs(piece.y - y_new) == 1)) ||
                ((Math.Abs(piece.x - x_new) == 1) && (Math.Abs(piece.y - y_new) == 2)));
            bool b = true;
            if (checkPosToPiece(x_new, y_new))
            {
                Piece j = (Piece)Board.PosToPiece[new Tuple<int, int>(x_new, y_new)];
                if (j.colour == piece.colour)
                    b = false;
            }
            return (a && b);
        }

        public static bool checkN_PassantKill(Piece piece)
        {
            if (Board.n_passantPawn == null) return false;
            //if ((Board.n_passantPawn.x == piece.x) && (Board.n_passantPawn.y == piece.y)) return false;
            if (piece is WhitePawn)
            {
                if ((piece.y == (Board.n_passantPawn.y)) &&
                    (Math.Abs(piece.x - Board.n_passantPawn.x) == 1) &&
                    (Board.n_passantPawn.colour == false))
                {
                    return true;
                }
            }
            else if (piece is BlackPawn)
            {
                if ((piece.y == (Board.n_passantPawn.y)) &&
                    (Math.Abs(piece.x - Board.n_passantPawn.x) == 1) &&
                    (Board.n_passantPawn.colour = true)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public static Piece FindKing(char identifier)
        {
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    if (authentication.checkPosToPiece(x, y))
                    {
                        Piece p = (Piece)Board.PosToPiece[new Tuple<int, int>(x, y)];
                        if (p.identifier == identifier)
                        {
                            return p;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// the argument is the colour to check. false argument implies check whether black has been checked.
        /// This is called during the play after an actual move.
        /// </summary>
        public static void InCheckLogic(bool colour)
        {
            Board.InCheck = false;
            Board.KingInCheck = null;
            Board.PiecesCheckingKing = new List<Piece>();
            if ((Board.wking == null) || (Board.bking == null))
            {
                Board.wking = (WhiteKing)FindKing('K');
                Board.bking = (BlackKing)FindKing('k');
            }
            Piece king = (colour == true) ? (Piece)Board.wking : (Piece)Board.bking;

            foreach(Piece piece in Board.PosToPiece.Values)
            {
                if ((piece.colour != colour) && (piece != king))
                {
                    if (piece.check(king.x, king.y))
                    {
                        Board.PiecesCheckingKing.Add(piece);
                        Board.InCheck = true;
                        Board.KingInCheck = king;
                    }
                }
            }
            Console.WriteLine("CHECK : {0}", Board.InCheck);
            foreach (Piece p in Board.PiecesCheckingKing)
            {
                Console.Write("{0}: {1}, {2} || ", p.identifier, p.x, p.y);
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// overloaded InCheckLogic function called by the MateLogic
        /// </summary>
        /// <returns></returns>
        public static bool InCheckLogic()
        {
            Piece king = (Piece) Board.KingInCheck;
            bool colour = king.colour;
            foreach (Piece piece in Board.PosToPiece.Values)
            {
                if ((piece.colour != colour) && (piece != king))
                {
                    if (piece.check(king.x, king.y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// The colour given in the argument specifies the king to check for.
        /// </summary>
        public static bool KingHasLegalMoves(bool colour)
        {
            if ((Board.wking == null) || (Board.bking == null))
            {
                Board.wking = (WhiteKing)FindKing('K');
                Board.bking = (BlackKing)FindKing('k');
            }

            Piece king = colour ? (Piece)Board.wking : (Piece)Board.bking;
            // I have to remove the king because otherwise whole lines (rows, colums, diagonals) 
            //would be marked as safe just because according to rules, they can't jump over king, however king would still be in line of attack.
            Board.PosToPiece.Remove(new Tuple<int, int>(king.x, king.y));
            List<Tuple<int, int>> InvalidPositions = new List<Tuple<int, int>>();
            foreach (Piece piece in Board.PosToPiece.Values)
            {
                if (piece.colour != colour)
                {
                    for (int x = 1; x < 9; x++)
                    {
                        for (int y = 1; y < 9; y++)
                        {
                            if (piece.check(x, y))
                            {
                                InvalidPositions.Add(new Tuple<int, int>(x, y));
                            }
                        }
                    }
                }
            }
            for (int x = 1; x < 9; x++)
            {
                for (int y = 1; y < 9; y++)
                {
                    if (king.check(x, y) && (!InvalidPositions.Contains(new Tuple<int, int>(x, y))))
                    {
                        Board.tempData = "King can escape by running.";
                        //Console.WriteLine("King can escape by running.");
                        Board.PosToPiece[new Tuple<int, int>(king.x, king.y)] = king;
                        return true;
                    }
                }
            }
            Board.tempData = "King can't escape by running";
            //Console.WriteLine("King can't escape by running");
            Board.PosToPiece[new Tuple<int, int>(king.x, king.y)] = king;
            return false;
        }

        public static bool MateLogic(bool colour)
        {
            authentication.InCheckLogic(colour);
            if (!Board.InCheck)
            {
                Board.tempData = "not in check";
                return false;
            }
            if (authentication.KingHasLegalMoves(colour))
            {
                Board.tempData = "king can move";
                return false;
            }
            // I simply can't use the algorithm of finding the temp locations of the piece
            // which is checking the king and see if one of my pieces can reach those locations
            // and hence block, or kill the piece, and then again check for mate, because there might
            // be more than 1 piece checking.
            // So I'll create a list of pieces which are checking the king. Then I'll create their temporary locations,
            // then i'll see if one of my pieces can go to their temporary locations, killing or blocking them, and then again
            // check for InCheck. That way everything will be awesome. I need an overloaded InCheckLogic into which I can send a 
            // changed chessboard. Have to think of an efficient way for that.
            List<Tuple<int, int>> tempLocations = new List<Tuple<int, int>>();

            #region filling tempLocations
            foreach (Piece p in Board.PiecesCheckingKing)
            {
                switch(p.identifier)
                {
                    case 'p':
                    case 'P':
                        // I can only kill pawns, not block them
                        tempLocations.Add(new Tuple<int, int>(p.x, p.y));
                        break;
                    
                    case 'R':
                    case 'r':
                        // This includes their own position as well as the path the rook takes
                        for (int i = Math.Min(p.x, Board.KingInCheck.x); i <= Math.Max(p.x, Board.KingInCheck.x); i++ )
                        {
                            for (int j= Math.Min(p.y, Board.KingInCheck.y); j <= Math.Max(p.y, Board.KingInCheck.y); j++)
                            {
                                tempLocations.Add(new Tuple<int, int>(i, j));
                            }
                        }
                        tempLocations.Remove(new Tuple<int, int>(Board.KingInCheck.x, Board.KingInCheck.y));
                        break;

                    case 'b':
                    case 'B':
                        // fill in the diagonal locations from piece to king
                        #region filling in temp locations
                        int[] x_pos = {0,0,0,0,0,0,0,0};
                        int[] y_pos = {0,0,0,0,0,0,0,0};
                        int x_pos_i = 0;
                        if ( p.x < Board.KingInCheck.x)
                        {
                            for(int i = p.x; i<=Board.KingInCheck.x; i++)
                            {
                                x_pos[x_pos_i] = i;
                                x_pos_i++;
                            }
                        }
                        else
                        {
                            for (int i = p.x; i >= Board.KingInCheck.x; i--)
                            {
                                x_pos[x_pos_i] = i;
                                x_pos_i++;
                            }
                        }
                        int y_pos_i = 0;
                        if ( p.y < Board.KingInCheck.y)
                        {
                            for(int i = p.y; i<=Board.KingInCheck.y; i++)
                            {
                                y_pos[y_pos_i] = i;
                                y_pos_i++;
                            }
                        }
                        else
                        {
                            for (int i = p.y; i >= Board.KingInCheck.y; i--)
                            {
                                y_pos[y_pos_i] = i;
                                y_pos_i++;
                            }
                        }
#endregion
                        for (int i = 0; i < 8; i++ )
                        {
                            if (x_pos[i] == 0) break;
                            tempLocations.Add(new Tuple<int, int>(x_pos[i], y_pos[i]));
                        }
                        tempLocations.Remove(new Tuple<int, int>(Board.KingInCheck.x, Board.KingInCheck.y));
                        break;

                    case 'N':
                    case 'n':
                        // I'll have to kill the knights. Can't block them
                        tempLocations.Add(new Tuple<int, int>(p.x, p.y));
                        break;
                    case 'Q':
                    case 'q':
                        #region Filling in temporary locations for queen
                        // check if straight kill or diagnol kill
                        if ((p.x == Board.KingInCheck.x) || (p.y == Board.KingInCheck.y))
                        {
                            for (int i = Math.Min(p.x, Board.KingInCheck.x); i <= Math.Max(p.x, Board.KingInCheck.x); i++)
                            {
                                for (int j = Math.Min(p.y, Board.KingInCheck.y); j <= Math.Max(p.y, Board.KingInCheck.y); j++)
                                {
                                    tempLocations.Add(new Tuple<int, int>(i, j));
                                }
                            }
                        }
                        else
                        {
                            // diagonal kill
                            int[] xpos = { 0, 0, 0, 0, 0, 0, 0, 0 };
                            int[] ypos = { 0, 0, 0, 0, 0, 0, 0, 0 };
                            int xposi = 0;
                            if (p.x < Board.KingInCheck.x)
                            {
                                for (int i = p.x; i <= Board.KingInCheck.x; i++)
                                {
                                    xpos[xposi] = i;
                                    xposi++;
                                }
                            }
                            else
                            {
                                for (int i = p.x; i >= Board.KingInCheck.x; i--)
                                {
                                    xpos[xposi] = i;
                                    xposi++;
                                }
                            }
                            int yposi = 0;
                            if (p.y < Board.KingInCheck.y)
                            {
                                for (int i = p.y; i <= Board.KingInCheck.y; i++)
                                {
                                    ypos[yposi] = i;
                                    yposi++;
                                }
                            }
                            else
                            {
                                for (int i = p.y; i >= Board.KingInCheck.y; i--)
                                {
                                    ypos[yposi] = i;
                                    yposi++;
                                }
                            }
                            for (int i = 0; i < 8; i++)
                            {
                                if (xpos[i] == 0) break;
                                tempLocations.Add(new Tuple<int, int>(xpos[i], ypos[i]));
                            }
                        }
#endregion
                        tempLocations.Remove(new Tuple<int, int>(Board.KingInCheck.x, Board.KingInCheck.y));
                        break;
                }
            }
            #endregion
            // so that changing PosToPiece does not raise exception
            List<Piece> temp_pieces = Board.PosToPiece.Values.Cast<Piece>().ToList();
            foreach(Piece piece in temp_pieces)
            {
                if (piece.colour == colour)
                {
                    foreach(Tuple<int, int> pos in tempLocations)
                    {
                        if (piece.check(pos.Item1, pos.Item2))
                        {
                            Piece original_piece = null;
                            // checks that its not check till now
                            // creating a new representation doesn't work as still piece.check uses the PosToPiece
                            // also instead of modifying all the piece functions with optional arguments,
                            // I'll modify the PosToPiece itself and correct it before the function returns
                            
                            /*
                            System.Collections.Hashtable tempBoardRepresentation = (System.Collections.Hashtable)Board.PosToPiece.Clone();
                            tempBoardRepresentation.Remove(new Tuple<int, int>(piece.x, piece.y));
                            tempBoardRepresentation[new Tuple<int, int>(pos.Item1, pos.Item2)] = piece;
                            if (authentication.InCheckLogic(tempBoardRepresentation) == false)
                            */
                            Tuple<int, int> actual_coordinates = new Tuple<int, int>(piece.x, piece.y);
                            Board.PosToPiece.Remove(actual_coordinates);

                            // simply doing this would overwrite an already existing piece with a temporary piece,
                            // thus causing a piece to be lost forever. In short -- killing.
                            // Board.PosToPiece[new Tuple<int, int>(pos.Item1, pos.Item2)] = piece;
                            // so
                            if (authentication.checkPosToPiece(pos.Item1, pos.Item2))
                            {
                                if (piece.check(pos.Item1, pos.Item2))
                                {
                                    original_piece = (Piece) Board.PosToPiece[pos];
                                }
                            }

                            Board.PosToPiece[new Tuple<int, int>(pos.Item1, pos.Item2)] = piece;
                            piece.x = pos.Item1;
                            piece.y = pos.Item2;
                            if (authentication.InCheckLogic() == false)
                            {
                                Board.tempData = "SOMEONE CAN BLOCK.!!";
                                //correct the board representation
                                Board.PosToPiece.Remove(new Tuple<int, int>(piece.x, piece.y));
                                Board.PosToPiece[new Tuple<int, int>(actual_coordinates.Item1, actual_coordinates.Item2)] = piece;
                                piece.x = actual_coordinates.Item1;
                                piece.y = actual_coordinates.Item2;
                                //Console.WriteLine("SOMEONE CAN KILL");
                                if (original_piece != null)
                                {
                                    Board.PosToPiece[pos] = original_piece;
                                }
                                return false;
                            }
                            //correct the board representation
                            Board.PosToPiece.Remove(new Tuple<int, int>(piece.x, piece.y));
                            Board.PosToPiece[new Tuple<int, int>(actual_coordinates.Item1, actual_coordinates.Item2)] = piece;
                            piece.x = actual_coordinates.Item1;
                            piece.y = actual_coordinates.Item2;
                            if (original_piece != null)
                            {
                                Board.PosToPiece[pos] = original_piece;
                            }
                        }
                    }
                }
            }
            Board.tempData = "CHECKMATE..!!";
            return true;

        }
    }
}
