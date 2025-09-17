using System.Text.RegularExpressions;

namespace LibrairieClasse
{
    public class Validator()
    {
        private Coordinate _enemyBoatCase1 = null!;
        private Coordinate _enemyBoatCase2 = null!;
        private Coordinate _myBoatCase1 = null!;
        private Coordinate _myBoatCase2 = null!;
        public static string[,] _actualEnemyBoard;
        public static string[,] _myActualBoard;

        public bool InitEnemyBoat(int[] boatPosition) {

            var coord1 = new Coordinate(boatPosition[0], boatPosition[1]);
            var coord2 = new Coordinate(boatPosition[2], boatPosition[3]);

            if (!IsPlayInGrid(coord1) || !IsPlayInGrid(coord2) || !coord1.IsNextTo(coord2)) return false;
            _enemyBoatCase1 = coord1;
            _enemyBoatCase2 = coord2;

            return true;
        }

        public void InitMyBoat(int[] boatPosition)
        {
            var coord1 = new Coordinate(boatPosition[0], boatPosition[1]);
            var coord2 = new Coordinate(boatPosition[2], boatPosition[3]);

            if (!IsPlayInGrid(coord1) || !IsPlayInGrid(coord2) || !coord1.IsNextTo(coord2)) throw new InvalidPlayException("Coordonnée du bateau incorrectes");
            _myBoatCase1 = coord1;
            _myBoatCase2 = coord2;
        }

        private bool IsPlayValid(Coordinate coordinate) 
        {
            if (!IsPlayInGrid(coordinate) || IsPlayDoneBefore(coordinate, _actualEnemyBoard))
            {
                return false;
            }
            
            return true;
        }

        private bool IsPlayInGrid(Coordinate coord)
        {
            if (coord.GetXInt() < 0 || coord.GetXInt() > Interface.Longueur - 1 ||
                coord.Y < 0 || coord.Y > Interface.Hauteur - 1)
            {
                return false;
            }

            return true;
        }

        private bool IsPlayDoneBefore(Coordinate coord, string[,] playBoard)
        {
            switch (playBoard[coord.GetXInt(), coord.Y]) 
            {
                case "-": return false;
                default: return true;
            }
        }

        /**
         *  M = missed
         *  T = touched
         *  U = unknown
         *  
         *  player E = enemy, M = me
        * */
        
        public bool GetPlayResult(int[] missilePosition, char player)
        {
           
            if (_enemyBoatCase1 == null || _enemyBoatCase2 == null)
            {
                throw new InvalidPlayException("Position du bateau pas encore initialisé");
            }

            var playCoord = new Coordinate(missilePosition[1],missilePosition[0]);

            if (!IsPlayValid(playCoord))
            {
                throw new InvalidPlayException("Coup invalide");
            }
            return CheckForTouched(player, playCoord);
            
        }

        private bool CheckForTouched(char player, Coordinate playCoord)
        {
            if(player == 'M')
            {
                if (playCoord.Equals(_enemyBoatCase1) || playCoord.Equals(_enemyBoatCase2))
                {
                    if (playCoord.Equals(_enemyBoatCase1)) _enemyBoatCase1.Touched();
                    else _enemyBoatCase2.Touched();

                    UpdateBoard(playCoord, "T", player);
                    return true;
                }

                UpdateBoard(playCoord, "M", player);
                return false;
            }
            else
            {
                if (playCoord.Equals(_myBoatCase1) || playCoord.Equals(_myBoatCase2))
                {
                    if (playCoord.Equals(_myBoatCase1)) _myBoatCase1.Touched();
                    else _myBoatCase2.Touched();

                    UpdateBoard(playCoord, "T", player);
                    return true;
                }

                UpdateBoard(playCoord, "M", player);
                return false;
            }
            
        }

        public bool WinCheck()
        {
            return _myBoatCase1.IsTouched() && _myBoatCase2.IsTouched();
        }

        public bool EnemyWinCheck()
        {
            return _enemyBoatCase1.IsTouched() && _enemyBoatCase2.IsTouched();
        }

        private void UpdateBoard(Coordinate coord, string result, char player)
        {
            if(player == 'E')
            {
                _myActualBoard[coord.GetXInt(), coord.Y] = result;
                
            }else
            {
                _actualEnemyBoard[coord.GetXInt(), coord.Y] = result;
            }
            
        }
    }
}
