using LibrairieClasse;

namespace LibrairieTest
{
    [TestClass]
    public sealed class ValidatorTest
    {
        private const int BoardSize = 4;

        private readonly char[][] _enemyBoard =
        [
            ['-', '-', '-', '-'],
            ['-', '-', '-', '-'],
            ['-', 'M', '-', '-'],
            ['-', '-', '-', '-']
        ];

        // test GetPlayResult()

        [TestMethod]
        public void GetPlayResult_Should_Return_T()
        {
            var validator = new Validator(BoardSize);
            
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);
            int[] play = [0,0];

            var playResult = validator.GetPlayResult(play, 'M');

            Assert.AreEqual(true, playResult);
        }
        
        [TestMethod]
        public void GetPlayResult_Should_Return_T_For_Enemy()
        {
            var validator = new Validator(BoardSize);
            
            int[] messageInit = [0, 0, 0, 1];
            validator.InitEnemyBoat(messageInit);
            validator.InitMyBoat(messageInit);
            int[] play = [0,0];

            var playResult = validator.GetPlayResult(play, 'E');

            Assert.AreEqual(true, playResult);
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_M()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);
            int[] play = [0,2];

            var playResult = validator.GetPlayResult(play, 'M');

            Assert.AreEqual(false, playResult);
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_U_When_Bad_Coord_Format()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);
            int[] play = [10,10];

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_U_When_Play_Done_Before()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);
            int[] play = [1,2];
            
            validator.GetPlayResult(play, 'M');

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_U_When_Play_Out_Of_Range_X()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);
            
            int[] play = [4,2];

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_U_When_Play_Out_Of_Range_Y()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);

            int[] play = [0,7];

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }

        [TestMethod]
        public void GetPlayResult_Should_Return_U_When_Play_Out_Of_Range_XY()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);

            int[] play = [4,4];

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }
        
        [TestMethod]
        public void GetPlayResult_Should_Return_M_When_Play_D4()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];

            validator.InitEnemyBoat(messageInit);

            int[] play = [3,3];

            var playResult = validator.GetPlayResult(play, 'M');

            Assert.AreEqual(false, playResult);
        }
        
        [TestMethod]
        public void GetPlayResult_Should_Throw_Exception_When_Boat_Not_Initiated()
        {
            var validator = new Validator(BoardSize);
            int[] play = [0,0];

            Assert.ThrowsException<InvalidPlayException>(() => validator.GetPlayResult(play, 'M'));
        }

        // InitEnemyBoat()

        [TestMethod]
        public void InitEnemyBoat_Should_Return_True()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [1, 1, 2, 1];

            Assert.IsTrue(validator.InitEnemyBoat(messageInit));
        }

        [TestMethod]
        public void InitEnemyBoat_Should_Return_False_When_Coords_Not_Aside()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [1, 1, 2, 3];

            Assert.IsFalse(validator.InitEnemyBoat(messageInit));
        }

        // ---- Nouveaux tests pour couvrir toute la classe ----

        [TestMethod]
        public void InitMyBoat_Should_Set_Boat_When_Valid()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [1, 1, 2, 1];

            validator.InitMyBoat(messageInit);

            Assert.IsFalse(validator.WinCheck());
        }

        [TestMethod]
        public void InitMyBoat_Should_Do_Nothing_When_Not_Aside()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 2, 2];

            Assert.ThrowsException<InvalidPlayException>(() => validator.InitMyBoat(messageInit));
        }

        [TestMethod]
        public void WinCheck_Should_Return_True_When_Both_My_Cases_Touched()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];
            validator.InitMyBoat(messageInit);
            validator.InitEnemyBoat(messageInit);

            int[] play1 = [0, 0];
            int[] play2 = [0, 1];

            validator.GetPlayResult(play1, 'E');
            validator.GetPlayResult(play2, 'E');

            Assert.IsTrue(validator.WinCheck());
        }

        [TestMethod]
        public void EnemyWinCheck_Should_Return_True_When_Both_Enemy_Cases_Touched()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];
            validator.InitEnemyBoat(messageInit);

            int[] play1 = [0, 0];
            int[] play2 = [0, 1];

            validator.GetPlayResult(play1, 'M');
            validator.GetPlayResult(play2, 'M');

            Assert.IsTrue(validator.EnemyWinCheck());
        }

        [TestMethod]
        public void UpdateBoard_Should_Mark_Enemy_Board_When_Player_M()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];
            validator.InitEnemyBoat(messageInit);

            int[] play = [1, 1];
            validator.GetPlayResult(play, 'M');

            Assert.AreNotEqual('-', GetPrivateBoard(validator, "_actualEnemyBoard")[1][1]);
        }

        [TestMethod]
        public void UpdateBoard_Should_Mark_My_Board_When_Player_E()
        {
            var validator = new Validator(BoardSize);
            int[] messageInit = [0, 0, 0, 1];
            validator.InitMyBoat(messageInit);
            validator.InitEnemyBoat(messageInit);

            int[] play = [1, 1];
            validator.GetPlayResult(play, 'E');

            Assert.AreNotEqual('-', GetPrivateBoard(validator, "_myActualBoard")[1][1]);
        }

        private static char[][] GetPrivateBoard(Validator validator, string fieldName)
        {
            var field = typeof(Validator).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (char[][])field!.GetValue(validator)!;
        }
    }
}
