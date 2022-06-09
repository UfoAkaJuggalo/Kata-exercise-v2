﻿using Kata_Services.CommonViewModels;

namespace Kata_Services.Queries.GetFeed;

public class GetFeedViewModel
{
    public int MessageId { get; set; }
    public string Content { get; set; }
    public DateTime DateTime { get; set; }
    public UserInfoViewModel Author { get; set; }
    public IEnumerable<UserInfoViewModel>? Mentions { get; set; }
}