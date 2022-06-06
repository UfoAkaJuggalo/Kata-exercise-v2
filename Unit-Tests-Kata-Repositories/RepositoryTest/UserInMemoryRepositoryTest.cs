using Kata_DAL.Entities;
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
    
    [Test]
    public void SubscribeToUserTimelineDoesNotThrowsException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrow(
            () => _userRepository.SubscribeToUserTimeline(0, 1));
    }    
    [Test]
    public void SubscribeToUserTimelineAddUserToFolowersAndSubscribtions()
    {
        // 1. Arrange
        var followerId = _userRepository.AddUser("Alice", "Alice", "Smith", "Alice@smith.com");
        var targetUserId = _userRepository.AddUser("Bob", "Bob", "Wesson", "Bob@smith.com");
        
        // 2. Act
        _userRepository.SubscribeToUserTimeline(followerId, targetUserId);
        var follower = _userRepository.GetUserById(followerId);
        var targetUser = _userRepository.GetUserById(targetUserId);
        
        // 3. Assert
        Assert.That(follower.Subscriptions, Has.Exactly(1).Items);
        Assert.That(targetUser.Followers, Has.Exactly(1).Items);
        Assert.That(follower.Subscriptions, Has.Exactly(1).Matches<User>(u=>u.UserId==targetUserId));
        Assert.That(targetUser.Followers, Has.Exactly(1).Matches<User>(u=>u.UserId==followerId));
    }
}