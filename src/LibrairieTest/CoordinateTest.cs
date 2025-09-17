using LibrairieClasse;

namespace LibrairieTest;

[TestClass]
public class CoordinateTest
{
    [TestMethod]
    public void IsNextTo_Should_Return_True_Y_Plus()
    {
        var coord1 = new Coordinate('A', 1);
        var coord2 = new Coordinate('A', 2);

        Assert.IsTrue(coord1.IsNextTo(coord2));
    }

    [TestMethod]
    public void IsNextTo_Should_Return_True_X_Plus()
    {
        var coord1 = new Coordinate('A', 1);
        var coord2 = new Coordinate('B', 1);

        Assert.IsTrue(coord1.IsNextTo(coord2));
    }

    [TestMethod]
    public void IsNextTo_Should_Return_True_X_Minus()
    {
        var coord1 = new Coordinate('C', 1);
        var coord2 = new Coordinate('B', 1);

        Assert.IsTrue(coord1.IsNextTo(coord2));
    }

    [TestMethod]
    public void IsNextTo_Should_Return_True_Y_Minus()
    {
        var coord1 = new Coordinate('A', 3);
        var coord2 = new Coordinate('A', 2);

        Assert.IsTrue(coord1.IsNextTo(coord2));
    }

    [TestMethod]
    public void IsNextTo_Should_Return_False()
    {
        var coord1 = new Coordinate('A', 1);
        var coord2 = new Coordinate('D', 2);

        Assert.IsFalse(coord1.IsNextTo(coord2));
    }
    
    
    
    [TestMethod]
    public void GetXInt_Should_Return_X_In_Int()
    {
        var coord1 = new Coordinate('B', 1);

        Assert.AreEqual(2, coord1.GetXInt());
    }
    
    [TestMethod]
    public void YIsInitiated()
    {
        var coord1 = new Coordinate('B', 1);

        Assert.AreEqual('B', coord1.GetXChar());
    }
    
    // Equals() et Hashcode()
    
    [TestMethod]
    public void Should_Return_False_When_Not_Equal_Coordinate()
    {
        var coord1 = new Coordinate('B', 1);
        var coord2 = "C1";

        Assert.IsFalse(coord1.Equals(coord2));
    }
    
    [TestMethod]
    public void GetHashCode_Should_Be_Equal_For_Equal_Objects()
    {
        var coord1 = new Coordinate('B', 1);
        var coord2 = new Coordinate('B', 1);

        Assert.AreEqual(coord1, coord2); // Vérifie Equals
        Assert.AreEqual(coord1.GetHashCode(), coord2.GetHashCode()); // Vérifie HashCode
    }

    [TestMethod]
    public void GetHashCode_Should_Be_Stable()
    {
        var coord = new Coordinate('C', 3);

        var hash1 = coord.GetHashCode();
        var hash2 = coord.GetHashCode();

        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void GetHashCode_Can_Differ_For_Different_Objects()
    {
        var coord1 = new Coordinate('A', 1);
        var coord2 = new Coordinate('B', 1);

        // Pas obligatoire qu'ils soient différents, mais dans ton implémentation ils devraient l’être
        Assert.AreNotEqual(coord1.GetHashCode(), coord2.GetHashCode());
    }
}
