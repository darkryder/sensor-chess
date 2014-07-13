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

    }
}
