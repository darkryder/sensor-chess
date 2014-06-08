using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess
{

    public class Pieces
    {
        virtual public bool check(int x, int y)
        {
            return false;
        }
        public bool colour;
        public char type, identifier;
        public int x, y;
        public PictureBox image;
        public Pieces(bool _colour = false, char _type = '0', int _x = 0, int _y = 0, char _identifier = '0')
        {
            colour = _colour;
            type = _type;
            x = _x;
            y = _y;
            identifier = _identifier;
        }
    }

    public class WhitePawn: Pieces
    {
        public WhitePawn(int x, int y, char identifier)
            : base(_colour: true, _type: 'p', _x: x, _y: y, _identifier: identifier)
        {
        }
        
        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (y_new <= this.y || (y_new - this.y > 2)) return false;
            if ((y_new - this.y == 2) && (this.y != 2)) return false;

            Tuple<bool, bool> temp = authentication.checkStraightMovement(this, x_new, y_new);
            // for n_passant
            if (temp.Item1 == true && this.y == 2 && y_new == 4) Board.n_passantPawn = (WhitePawn)this;
            
            if (temp.Item1 == true && temp.Item2 == false) return true;

            if ((Math.Abs(this.x - x_new) == 1) && (y_new - this.y == 1))
            {
                temp = authentication.checkDiagonalMovement(this, x_new, y_new);
                if (temp.Item2 == temp.Item1 == true) return true;
            }
            return false;
        }
        
    }

    public class BlackPawn : Pieces
    {
        public BlackPawn(int x, int y, char identifier)
            : base(_colour: false, _type: 'p', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (y_new >= this.y || (this.y - y_new > 2)) return false;
            if ((this.y - y_new == 2) && (this.y != 7)) return false;

            Tuple<bool, bool> temp = authentication.checkStraightMovement(this, x_new, y_new);

            // for n_passant
            if (temp.Item1 == true && this.y == 7 && y_new == 5) Board.n_passantPawn = (BlackPawn)this;

            if (temp.Item1 == true && temp.Item2 == false) return true;

            if ((Math.Abs(this.x - x_new) == 1) && (this.y - y_new == 1))
            {
                temp = authentication.checkDiagonalMovement(this, x_new, y_new);
                if (temp.Item2 == temp.Item1 == true) return true;
            }
            return false;
        }

    }

    public class WhiteBishop : Pieces
    {
        public WhiteBishop(int x, int y, char identifier)
            : base(_colour: true, _type: 'b', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class BlackBishop : Pieces
    {
        public BlackBishop(int x, int y, char identifier)
            : base(_colour: false, _type: 'b', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class WhiteRook: Pieces
    {
        public WhiteRook(int x, int y, char identifier)
            : base(_colour: true, _type: 'r', _x: x, _y: y, _identifier: identifier)
        {
        }

        public bool castlingPossible = true;

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class BlackRook : Pieces
    {
        public BlackRook(int x, int y, char identifier)
            : base(_colour: false, _type: 'r', _x: x, _y: y, _identifier: identifier)
        {
        }

        public bool castlingPossible = true;

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class WhiteKnight : Pieces
    {
        public WhiteKnight(int x, int y, char identifier)
            : base(_colour: true, _type: 'n', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkKnightMovement(this, x_new, y_new) == true) return true;
            return false;
        }

    }

    public class BlackKnight: Pieces
    {
        public BlackKnight(int x, int y, char identifier)
            : base(_colour: false, _type: 'r', _x: x, _y: y, _identifier: identifier)
        {
        }

        
        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkKnightMovement(this, x_new, y_new)) return true;
            return false;
        }
    }

    public class WhiteQueen : Pieces
    {
        public WhiteQueen(int x, int y, char identifier)
            : base(_colour: true, _type: 'q', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class BlackQueen : Pieces
    {
        public BlackQueen(int x, int y, char identifier)
            : base(_colour: false, _type: 'q', _x: x, _y: y, _identifier: identifier)
        {
        }

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class WhiteKing : Pieces
    {
        public WhiteKing(int x, int y, char identifier)
            : base(_colour: true, _type: 'k', _x: x, _y: y, _identifier: identifier)
        {
        }

        public bool castlingPossible = true;

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;
            if (this.castlingPossible && (this.y == y_new) && (Math.Abs(this.x - x_new) == 2))
            {
                Board.CastlingMode = true;
                return false;
            }
            if (!(Math.Abs(this.x - x_new) <= 1) && (Math.Abs(this.y - y_new) <= 1)) return false;

            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

    public class BlackKing : Pieces
    {
        public BlackKing(int x, int y, char identifier)
            : base(_colour: false, _type: 'k', _x: x, _y: y, _identifier: identifier)
        {
        }

        public bool castlingPossible = true;

        public override bool check(int x_new, int y_new)
        {
            if (!authentication.checkBoundaryCondition(this.x, this.y, x_new, y_new)) return false;

            if (this.castlingPossible && (this.y == y_new) && (Math.Abs(this.x - x_new) == 2))
            {
                Board.CastlingMode = true;
                return false;
            }

            if (!(Math.Abs(this.x - x_new) <= 1) && (Math.Abs(this.y - y_new) <= 1)) return false;

            if (authentication.checkStraightMovement(this, x_new, y_new).Item1 == true) return true;
            if (authentication.checkDiagonalMovement(this, x_new, y_new).Item1 == true) return true;
            return false;
        }

    }

}
