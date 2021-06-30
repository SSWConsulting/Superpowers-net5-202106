﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net5Superpowers.WebUI.Models;

namespace Net5Superpowers.WebUI.Data.Configurations
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Property(e => e.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
