using LegacyApp;

namespace LegacyAppTest;


public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Retur_FAlse_When_Email_Without_At_And_Dot()
    {
        //Arrange
        string firstName = "Jhon";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe";

        var service = new UserService();

        //Act
        bool res = service.AddUser(firstName, lastName, email, birthDate, clientId);

        //Assert
        Assert.Equal(false, res);
    }
}