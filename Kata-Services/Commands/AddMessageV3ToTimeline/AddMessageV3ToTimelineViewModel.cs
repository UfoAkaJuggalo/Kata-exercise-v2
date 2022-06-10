using Kata_Services.CommonViewModels;

namespace Kata_Services.Commands.AddMessageV3ToTimeline;

public class AddMessageV3ToTimelineViewModel
{
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public IEnumerable<UserInfoMentionsViewModel>? Mentions { get; set; }
    public IEnumerable<LinksViewModel>? Links { get; set; }
}