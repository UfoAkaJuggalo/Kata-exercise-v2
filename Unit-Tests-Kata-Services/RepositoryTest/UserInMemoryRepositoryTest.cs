using Kata_DAL.InMemoryRepositories;
using Kata_DAL.IRepositories;
using NUnit.Framework;

namespace Unit_Tests_Kata_Services.RepositoryTest;

[TestFixture]
public class UserInMemoryRepositoryTest
{
    private IUserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        _userRepository = new UserInMemoryRepository();
    }
    
    [Test]
    public void AddNewUserDoesNotThrowException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrow(
             () =>  _userRepository.AddUser("Alice", "Alice", "Smith", "Alice@smith.com"));
    }
    
    [Test]
    public void NewUserAddedSuccess()
    {
        // 1. Arrange
        
        // 2. Act
        var result = _userRepository.AddUser("Alice", "Alice", "Smith", "Alice@smith.com");
        
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }
    
    [Test]
    public void UserNotFoundThrowsException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.Throws<ArgumentOutOfRangeException>(
             () => _userRepository.GetUserById(-1));
    }
    
    [Test]
    public void FindUserById()
    {
        // 1. Arrange
        var displayName = "Alice";
        var newUserId = _userRepository.AddUser(displayName, "Alice", "Smith", "Alice@smith.com");
        
        // 2. Act
        var result = _userRepository.GetUserById(newUserId);

        // 3. Assert
        Assert.That(newUserId, Is.EqualTo(result.UserId));
        Assert.That(displayName, Is.EqualTo(result.DisplayName));
    }
}