using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Repository;
using ProductCatalog.Infrastructure.Persistance;

namespace ProductCatalog.Infrastructure.Tests
{
    public class DepedendencyInjectionTests
    {
        [Fact]
        public void AddInfrastrcuture_Success()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act          
            services.AddInfrastructure();
            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            var dbContext = serviceProvider.GetService<EasyCommerceDbContext>();
            var categoryRepository = serviceProvider.GetService<ICategoryRepository>();
            var productRepository = serviceProvider.GetService<IProductCatalogRepository>();

            //Assert
            dbContext.Should().NotBeNull();
            categoryRepository.Should().NotBeNull();
            productRepository.Should().NotBeNull();
        }
    }
}
