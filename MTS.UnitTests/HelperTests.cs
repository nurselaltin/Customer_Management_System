using Core.Helper;

namespace MTS.UnitTests
{
  public class HelperTests
  {
    [Test]
    public void ComputeHash_GivenEmptyPass_GiveEmptyVal()
    {
      //Arrange
      string pass = "";

      //Action
      var res = Helper.ComputeHash(pass);

      //Assert

       Assert.AreEqual(res,"");
    }

  }
}