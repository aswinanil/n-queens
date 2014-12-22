using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Done by: Aswin Anil
/* Outline:
 * A solution to the N-Queens Problem using backtracking algorithm.
 * The board is viewed as having an x and y axis.
 * The origin is at the top left as in the question paper
 * */

namespace N_Queens_Problem
{
    // Queen chess piece
    class Queen : Chess_Piece
    {
        private string[] move_set;

        public Queen()
        {
            type = "Queen";
            move_set = new string[3] { "Horizontal", "Vertical", "Diagonal" };
        }

        public override string get_type()
        {
            return type;
        }

        public string[] get_move_set()
        {
            return move_set;
        }
    }

    // An empty chess piece
    class Chess_Piece
    {
        protected string type;

        public Chess_Piece()
        {
            type = "Empty";
        }

        public virtual string get_type()
        {
            return type;
        }
    }

    // A chess board of dimension size * size
    class Chess_Board
    {
        private int size;
        bool is_solution;                                   // Specifies if the current board with the specified prieces on it is solution to the n-queens problem
        private Chess_Piece[,] board_layout;

        // Constructor
        public Chess_Board(int input_size)
        {
            size = input_size;
            is_solution = false;
            board_layout = new Chess_Piece[size, size];    // Index is from 0 to size-1
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    board_layout[i, j] = new Chess_Piece();
                }
            }
        }

        public int get_size()
        {
            return size;
        }

        public void set_is_solution()                 // Set the current board with the specified prieces as a solution to the n-queens problem
        {
            is_solution = true;
        }

        public bool get_is_solution()
        {
            return is_solution;
        }

        public void insert_queen(int x, int y)
        {
            x -= 1;                                     // To adjust for the index being from 0 to size-1
            y -= 1;                                     // To adjust for the index being from 0 to size-1
            board_layout[x, y] = new Queen();
        }

        public void remove_queen(int x, int y)
        {
            x -= 1;                                     // To adjust for the index being from 0 to size-1
            y -= 1;                                     // To adjust for the index being from 0 to size-1
            if (board_layout[x, y].get_type().Equals("Queen"))
                board_layout[x, y] = new Chess_Piece();
        }

        // Returns whether the coordinate specified, does not result in a queen placed here attacking another queen
        public bool is_valid_position(int x, int y)
        {
            x -= 1;                                     // To adjust for the index being from 0 to size-1
            y -= 1;                                     // To adjust for the index being from 0 to size-1

            // Pieces are placed towards the right
            // Thus only check the left sides
            if (!is_left_clear(x, y))
                return false;
            if (!is_up_left_clear(x, y))
                return false;
            if (!is_down_left_clear(x, y))
                return false;
            return true;
        }

        // Traverse left horizontally to check for chess pieces
        private bool is_left_clear(int x, int y)
        {
            --x;                                        // Do not need to check the initial coordinate
            while (x != -1)
            {
                if (!board_layout[x, y].get_type().Equals("Empty"))
                    return false;
                --x;
            }
            return true;
        }

        // Traverse up-left diagonally to check for chess pieces
        private bool is_up_left_clear(int x, int y)
        {
            --x;                                        // Do not need to check the initial coordinate
            --y;                                        // Do not need to check the initial coordinate
            while (x != -1 && y != -1)
            {
                if (!board_layout[x, y].get_type().Equals("Empty"))
                    return false;
                --x;
                --y;
            }
            return true;
        }

        // Traverse down-left diagonally to check for chess pieces
        private bool is_down_left_clear(int x, int y)
        {
            --x;                                        // Do not need to check the initial coordinate
            ++y;                                        // Do not need to check the initial coordinate
            while (x != -1 && y < size)
            {
                if (!board_layout[x, y].get_type().Equals("Empty"))
                    return false;
                --x;
                ++y;
            }
            return true;
        }

        public void print_board()
        {
            // Print row index
            for (int i = 0; i < size; ++i)
            {
                Console.Write(i);
                if (size > 9 && i < 10)
                    Console.Write(" ");
                Console.Write("|");
            }
            Console.Write(size);
            Console.WriteLine();

            // Print pieces
            for (int i = 0; i < size; ++i)
            {
                // Print column index
                Console.Write(i + 1);
                if (size > 9 && i + 1 < 10)
                    Console.Write(" ");
                Console.Write("|");

                // Print queen positions
                for (int j = 0; j < size - 1; ++j)
                {
                    if (board_layout[j, i].get_type().Equals("Empty"))
                    {
                        Console.Write("O");
                        if (size > 9)
                            Console.Write(" ");
                        Console.Write("|");
                    }

                    if (board_layout[j, i].get_type().Equals("Queen"))
                    {
                        Console.Write("X");
                        if (size > 9)
                            Console.Write(" ");
                        Console.Write("|");
                    }
                }
                // Last column without '|'
                if (board_layout[size - 1, i].get_type().Equals("Empty"))
                    Console.Write("O");
                if (board_layout[size - 1, i].get_type().Equals("Queen"))
                    Console.Write("X");
                Console.WriteLine();
            }
        }
    }

    class N_Queens_Program
    {
        // Accepts a Chess_Board of variable size as input
        // Returns a solution for the n-queens problem if one exists
        static Chess_Board get_solution(Chess_Board my_chess_board)
        {
            bool checking_board = true;                                 // Used to terminate loop
            bool last_check = false;                                    // Set to true when reached last row of first column
            int[] prev_y = new int[my_chess_board.get_size() + 1];      // Used to backtrack to prev row stopped at in prev column
            int x = 1;                                                  // x-coordinate
            int y = 1;                                                  // y-coordinate

            while (checking_board)
            {
                int curr_y = y;
                if (curr_y > my_chess_board.get_size())                 // Backtrack when the whole column has been checked
                {
                    my_chess_board.remove_queen(x - 1, prev_y[x - 1]);
                    y = prev_y[x - 1] + 1;
                    --x;
                    continue;
                }

                if (my_chess_board.is_valid_position(x, y))
                {
                    if (x == my_chess_board.get_size())                 // Inserted into the last column, solution found!
                    {
                        my_chess_board.insert_queen(x, y);
                        my_chess_board.set_is_solution();
                        checking_board = false;
                        continue;
                    }
                    else                                                // Insert queen and move on to next column
                    {
                        my_chess_board.insert_queen(x, y);
                        if (x == 1 && y == my_chess_board.get_size())   // Checking last row of first column, few states left to check
                            last_check = true;
                        prev_y[x] = curr_y;
                        ++x;
                        y = 1;
                    }
                }

                else if (y < my_chess_board.get_size())                 // Traverse column
                {
                    ++y;
                }

                else if (y >= my_chess_board.get_size())                // No place in the whole column to put
                {
                    if (!last_check)                                    // Still have more solutions to check, Backtrack!
                    {
                        my_chess_board.remove_queen(x - 1, prev_y[x - 1]);
                        y = prev_y[x - 1] + 1;
                        --x;
                        continue;
                    }
                    else                                                // Checked last row of first column, no solution found!                                 
                    {
                        checking_board = false;
                        continue;
                    }
                }
            }
            return my_chess_board;
        }

        static void Main(string[] args)
        {
            Console.Write("Enter size of chessboard: ");
            string input = Console.ReadLine();
            int size;

            try
            {
                if (int.TryParse(input, out size))                      // Check if input is an integer
                {
                    if (size < 1)
                        throw new ArgumentException("Size must be more than one");

                    Console.WriteLine("Solution:" + '\n');
                    Chess_Board solution_board = get_solution(new Chess_Board(size));
                    if (solution_board.get_is_solution() == true)
                        solution_board.print_board();
                    else
                        Console.WriteLine("No solution found!");
                }
                else
                    throw new ArgumentException("Input must be an integer");
            }
            catch (ArgumentException argument_exception)
            {
                Console.WriteLine("Exception: {0}", argument_exception);
            }
            finally
            {
                Console.ReadKey();                                      // Stop console from terminating
            }
        }
    }
}
