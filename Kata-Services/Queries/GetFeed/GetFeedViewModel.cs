using Kata_Services.CommonViewModels;

namespace Kata_Services.Queries.GetFeed;

public class GetFeedViewModel : GetAllMessagesByUserViewModel
{
    public UserInfoViewModel Author { get; set; }
}