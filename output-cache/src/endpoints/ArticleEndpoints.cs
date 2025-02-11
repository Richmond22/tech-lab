using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using output_cache.data.entity;
using output_cache.data.repository;

namespace output_cache.endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/articles")
            .CacheOutput()
            .WithTags("Articles")
            .WithOpenApi();

        // GET all articles
        group.MapGet("/", async Task<Results<Ok<IEnumerable<Article>>, NotFound>> 
            (IArticleRepository repository) =>
        {
            var articles = await repository.GetAllAsync();
            return TypedResults.Ok(articles);
        })
        .CacheOutput()
        .WithName("GetAllArticles")
        .WithDescription("Gets all articles");

        // GET article by id
        group.MapGet("/{id}", async Task<Results<Ok<Article>, NotFound>> 
            (int id, IArticleRepository repository) =>
        {
            try
            {
                var article = await repository.GetByIdAsync(id);
                return TypedResults.Ok(article);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
        })
        .CacheOutput()
        .WithName("GetArticleById")
        .WithDescription("Gets an article by id");

        // POST new article
        group.MapPost("/", async Task<Results<Created<Article>, BadRequest>> 
            (Article article, IArticleRepository repository) =>
        {
            try
            {
                var created = await repository.AddAsync(article);
                return TypedResults.Created($"/api/articles/{created.Id}", created);
            }
            catch (Exception)
            {
                return TypedResults.BadRequest();
            }
        })
        .WithName("CreateArticle")
        .WithDescription("Creates a new article");

        // PUT update article
        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> 
            (int id, Article article, IArticleRepository repository) =>
        {
            if (id != article.Id)
                return TypedResults.BadRequest();

            try
            {
                await repository.UpdateAsync(article);
                return TypedResults.Ok();
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("UpdateArticle")
        .WithDescription("Updates an existing article");

        // DELETE article
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> 
            (int id, IArticleRepository repository) =>
        {
            try
            {
                await repository.DeleteAsync(id);
                return TypedResults.Ok();
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound();
            }
        })
        .WithName("DeleteArticle")
        .WithDescription("Deletes an article");
    }
} 