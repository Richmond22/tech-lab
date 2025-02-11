using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using output_cache.data.entity;

namespace output_cache.data.repository;

public class ArticleRepository(AppDbContext context) : IArticleRepository
{
    public async Task<Article> GetByIdAsync(int id)
    {
        return await context.Articles.FindAsync(id)
            ?? throw new KeyNotFoundException($"Article with id {id} not found");
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await context.Articles.ToListAsync();
    }

    public async Task<Article> AddAsync(Article article)
    {
        await context.Articles.AddAsync(article);
        await context.SaveChangesAsync();
        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        context.Articles.Update(article);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var article = await GetByIdAsync(id);
        context.Articles.Remove(article);
        await context.SaveChangesAsync();
    }
}

public interface IArticleRepository
{
    Task<Article> GetByIdAsync(int id);
    Task<IEnumerable<Article>> GetAllAsync();
    Task<Article> AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task DeleteAsync(int id);
}


