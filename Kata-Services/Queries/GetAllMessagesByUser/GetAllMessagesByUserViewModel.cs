using Kata_Services.CommonViewModels;

namespace Kata_Services.Queries;

public class GetAllMessagesByUserViewModel
{
    public int MessageId { get; set; }
    public string Content { get; set; }
    public DateTime DateTime { get; set; }
    public IEnumerable<UserInfoViewModel>? Mentions { get; set; }
}