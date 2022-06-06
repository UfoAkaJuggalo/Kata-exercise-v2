namespace Kata_DAL.Entities;

public class User
{
    public int UserId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public ICollection<Message> Timeline { get; set; }
    public ICollection<User> Subscriptions { get; set; }
    public ICollection<User> Followers { get; set; }
}