﻿using Kata_DAL.Entities;

namespace Kata_DAL.IRepositories;

public interface IMessageRepository
{
    int AddMessage(string content, User author);
    IEnumerable<Message> GetAllMessagesByUserId(int userId);
    IEnumerable<Message> GetSortedByMessageIdFeedForUser(User user);
    int AddMessageWithMentions(string content, User author, IEnumerable<User>? mentions);
}