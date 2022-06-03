using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddUser;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Unit_Tests_Kata_Services.CommandsTests;

[TestFixture]
public class AddUserCommandHandlerTest
{
    private AddUserCommandHandler _command;
    private AddUserCommand _addUserCommand;
        
    [SetUp]
    public void Setup()
    {
        var userRepository = new Mock<IUserRepository>();
        
        _command = new AddUserCommandHandler( userRepository.Object);
        
        _addUserCommand = new AddUserCommand
        {
            NewUser = new AddUserViewModel
            {
                Name = "Alice",
                LastName = "Smith",
                Email = "alice@sw.com",
                DisplayName = "Alice"
            }
        };
    }
    
    [Test]
    public void DoesNotThrowException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrowAsync(async ()=>await _command.Handle(_addUserCommand, CancellationToken.None));
    }
    
    [Test]
    public async Task NewUserAdded()
    {
        // 1. Arrange
        
        // 2. Act
        var result = await _command.Handle(_addUserCommand, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }
}