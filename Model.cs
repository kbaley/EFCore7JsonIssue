using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer($"Server=localhost;Database=Moo;Trusted_Connection=True;Encrypt=false");
    
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<Blog>().OwnsOne(
            post => post.Comment, navigationBuilder =>
            {
                navigationBuilder.ToJson();
            }
        );
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public Comment Comment { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}

public class Comment
{
    public string Text { get; set; }
    public string Author { get; set; }
}