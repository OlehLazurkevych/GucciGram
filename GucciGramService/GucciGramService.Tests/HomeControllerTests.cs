using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GucciGramService.Controllers;
using GucciGramService.Models;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GucciGramService.Tests
{
    public class HomeControllerTests
    {
        IdentityDbContext identityDbContext;
        GeneralDbContext generalDbContext;
        CommentDbContext commentDbContext;
        SearchDbContext searchDbContext;
        LikeDbContext likeDbContext;

        public HomeControllerTests()
        {
            DbContextOptionsBuilder<IdentityDbContext> IdentityDbContextBuilder = new DbContextOptionsBuilder<IdentityDbContext>(new DbContextOptions<IdentityDbContext>());
            DbContextOptionsBuilder<GeneralDbContext> GeneralDbContextBuilder = new DbContextOptionsBuilder<GeneralDbContext>(new DbContextOptions<GeneralDbContext>());
            DbContextOptionsBuilder<CommentDbContext> CommentDbContextBuilder = new DbContextOptionsBuilder<CommentDbContext>(new DbContextOptions<CommentDbContext>());
            DbContextOptionsBuilder<SearchDbContext> SearchDbContextBuilder = new DbContextOptionsBuilder<SearchDbContext>(new DbContextOptions<SearchDbContext>());
            DbContextOptionsBuilder<LikeDbContext> LikeDbContextBuilder = new DbContextOptionsBuilder<LikeDbContext>(new DbContextOptions<LikeDbContext>());
            
            identityDbContext = new IdentityDbContext(IdentityDbContextBuilder.UseSqlServer("Server=(local)\\GUCCIGRAMSERVER;Database=GucciGram_Database;Trusted_Connection=True;MultipleActiveResultSets=true").Options);
            generalDbContext = new GeneralDbContext(GeneralDbContextBuilder.UseSqlServer("Server=(local)\\GUCCIGRAMSERVER;Database=GucciGram_Database;Trusted_Connection=True;MultipleActiveResultSets=true").Options); ;
            commentDbContext = new CommentDbContext(CommentDbContextBuilder.UseSqlServer("Server=(local)\\GUCCIGRAMSERVER;Database=GucciGram_Database;Trusted_Connection=True;MultipleActiveResultSets=true").Options); ;
            searchDbContext = new SearchDbContext(SearchDbContextBuilder.UseSqlServer("Server=(local)\\GUCCIGRAMSERVER;Database=GucciGram_Database;Trusted_Connection=True;MultipleActiveResultSets=true").Options); ;
            likeDbContext = new LikeDbContext(LikeDbContextBuilder.UseSqlServer("Server=(local)\\GUCCIGRAMSERVER;Database=GucciGram_Database;Trusted_Connection=True;MultipleActiveResultSets=true").Options); ;
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfPosts()
        {
            HomeController home = new HomeController(null, generalDbContext, likeDbContext, commentDbContext);

            // Act
            var result = home.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            //var model = Assert.IsAssignableFrom<List<PostViewModel>>(viewResult.ViewData.Model);
            //Assert.Equal(2, model.Count());
        }
    }
}
