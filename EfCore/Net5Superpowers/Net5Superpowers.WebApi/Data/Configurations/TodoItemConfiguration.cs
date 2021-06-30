using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net5Superpowers.WebApi.Models;

namespace Net5Superpowers.WebApi.Data.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(e => e.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
