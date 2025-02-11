using System;

namespace output_cache.data.entity;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;
}