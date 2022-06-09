using Kata_DAL.IRepositories;
using Kata_Services.Commands.AddMessageToTimeline;
using Moq;

namespace Unit_Tests_Kata_Services.CommandsTests;

[TestFixture]
public class AddMessageCommandHandlerTest
{
    private AddMessageCommandHandler _command;
    private AddMessageCommand _addMessageCommand;
    
    [SetUp]
    public void Setup()
    {
        var messageRepository = new Mock<IMessageRepository>();
        var userRepository = new Mock<IUserRepository>();
        
        _command = new AddMessageCommandHandler(messageRepository.Object, userRepository.Object);

        _addMessageCommand = new AddMessageCommand
        {
            NewPost = new AddMessageViewModel
            {
                Content = "Some post",
                AuthorId = 0
            }
        };
    }
    
    [Test]
    public void DoesNotThrowException()
    {
        // 1. Arrange
        // 2. Act
        
        // 3. Assert
        Assert.DoesNotThrowAsync(async ()=>await _command.Handle(_addMessageCommand, CancellationToken.None));
    }
    
    [Test]
    public async Task NewMessageAdded()
    {
        // 1. Arrange
        
        // 2. Act
        var result = await _command.Handle(_addMessageCommand, CancellationToken.None);
        
        // 3. Assert
        Assert.That(result, Is.GreaterThanOrEqualTo(0));
    }
}