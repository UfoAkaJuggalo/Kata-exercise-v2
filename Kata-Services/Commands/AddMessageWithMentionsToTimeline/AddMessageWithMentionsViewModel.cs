using Kata_Services.CommonViewModels;

namespace Kata_Services.Commands.AddMessageWithMentionsToTimeline;

public class AddMessageWithMentionsViewModel
{
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public IEnumerable<UserInfoMentionsViewModel>? Mentions { get; set; }
}