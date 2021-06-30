using System;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Net5Superpowers.WebUI.Data;
using Net5Superpowers.WebUI.Models;
using Xunit;

namespace Net5Superpowers.WebUI.UnitTests.Models
{
    public class CreateTodoListVmTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

        private readonly CreateTodoListVmValidator _validator;

        public CreateTodoListVmTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _validator = new CreateTodoListVmValidator(_context);
        }

        [Fact]
        public void ShouldHaveErrorWhenTitleIsNull()
        {
            // Arrange
            var vm = new CreateTodoListVm
            {
                Title = ""
            };

            // Act
            var result = _validator.TestValidate(vm);

            // Assert
            result.ShouldHaveValidationErrorFor(vm => vm.Title)
                .WithErrorMessage("'Title' must not be empty.");
        }

        [Fact]
        public void ShouldHaveErrorWhenTitleIsTooLong()
        {
            var vm = new CreateTodoListVm
            {
                Title = "This title is much, much, much longer than most titles in that it " +
                        "has exceeded 200 characters and in fact it is not a valid title" +
                        "since the maximum length that a title can have in this system is " +
                        "200 characters."
            };

            var result = _validator.TestValidate(vm);

            result.ShouldHaveValidationErrorFor(vm => vm.Title);
        }

        [Fact]
        public void ShouldHaveErrorWhenTitleIsNotUnique()
        {
            _context.Add(new TodoList { Title = "Todo List" });
            _context.SaveChanges();

            var vm = new CreateTodoListVm
            {
                Title = "Todo List"
            };

            var result = _validator.TestValidate(vm);

            result.ShouldHaveValidationErrorFor(vm => vm.Title);
        }

        [Fact]
        public void ShouldNotHaveErrorWhenValidTitleIsSpecified()
        {
            var vm = new CreateTodoListVm
            {
                Title = "MIT"
            };

            var result = _validator.TestValidate(vm);

            result.ShouldNotHaveAnyValidationErrors();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
