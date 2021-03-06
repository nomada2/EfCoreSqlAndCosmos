﻿// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClassesSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.EfCode.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure
            (EntityTypeBuilder<Book> entity)
        {
            entity.Property(p => p.PublishedOn).HasColumnType("date");
            entity.Property(p => p.OrgPrice).HasColumnType("decimal(9,2)");
            entity.Property(p => p.ActualPrice).HasColumnType("decimal(9,2)");

            entity.Property(x => x.ImageUrl).IsUnicode(false);

            entity.HasIndex(x => x.PublishedOn);
            entity.HasIndex(x => x.ActualPrice);

            //Add indexes for the SqlEvent cached value that is filtered on
            entity.HasIndex(x => x.ReviewsAverageVotes);

            entity.HasQueryFilter(p => !p.SoftDeleted);

            //----------------------------
            //relationships

            entity.HasMany(p => p.Reviews)  
                .WithOne()                     
                .HasForeignKey(p => p.BookId);

            entity.Metadata
                .FindNavigation(nameof(Book.Reviews))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            entity.Metadata
                .FindNavigation(nameof(Book.AuthorsLink))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}