using Microsoft.AspNetCore.Mvc;
using UPSTask;
using UPSTask.TaskObjects;
using Xunit;

namespace TestUPS
{
    public class TestTask
    {
        [Fact]
        public async void GetAllUserList()
        {
            // Arrange
            var api = new WebAPI();
            Filters filters = new Filters();
            // Act
            var okResult = await api.GetUser(filters);
            var value = (okResult as OkObjectResult)?.Value as RestResponse;
            // Assert
            Assert.IsType<User>(value);
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public async void GetSpecificUser()
        {
            // Arrange
            var api = new WebAPI();
            Filters filters = new Filters();
            filters.id = 2164;
            // Act
            var okResult = await api.GetUser(filters);

            var value = (okResult as OkObjectResult)?.Value as RestResponse;
            // Assert
            Assert.IsType<User>(value);
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public async void AddUser()
        {
            // Arrange
            var api = new WebAPI();
            User newUser = new User()
            {
                Name = "test user",
                Email = "test@gmail.com",
                Gender = "female",
                Status = "active"
            };
            // Act
            var okResult = await api.CreateUser(newUser);

            var value = (okResult as OkObjectResult)?.Value as PageResponse;
            // Assert
            Assert.IsType<bool>(value);
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public async void UpdateUser()
        {
            // Arrange
            var api = new WebAPI();
            User newUser = new User()
            {
                Id = 2159,
                Name = "test user",
                Email = "test@gmail.com",
                Gender = "female",
                Status = "active"
            };
            // Act
            var okResult = await api.UpdateUser(newUser);

            var value = (okResult as OkObjectResult)?.Value as PageResponse;
            // Assert
            Assert.IsType<bool>(value);
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Fact]
        public async void DeleteUser()
        {
            // Arrange
            var api = new WebAPI();

            // Act
            var okResult = await api.DeleteUser(2159);

            var value = (okResult as OkObjectResult)?.Value as PageResponse;
            // Assert
            Assert.IsType<bool>(value);
            Assert.IsType<OkObjectResult>(okResult);
        }
    }
}
