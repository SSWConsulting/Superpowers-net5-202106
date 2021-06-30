using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Net5Superpowers.WebUI.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Superpowers.WebUI.Models
{
    public class CreateTodoListVm
    {
        public string Title { get; set; }
    }

    public class CreateTodoListVmValidator : AbstractValidator<CreateTodoListVm>
    {
        private readonly ApplicationDbContext _context;

        public CreateTodoListVmValidator(
            ApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .MaximumLength(200)
                .NotEmpty()
                .MustAsync(HaveUniqueTitle).WithMessage("It already exists");
        }

        private async Task<bool> HaveUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.TodoLists.AllAsync(l => l.Title != title, cancellationToken);
        }
    }
}

