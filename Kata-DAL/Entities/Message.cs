namespace Kata_DAL.Entities;

public class Message
{
    public int MessageId { get; set; }
    public string Content { get; set; }
    public DateTime DateTime { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
    public IEnumerable<User>? Mentions { get; set; }
    
}