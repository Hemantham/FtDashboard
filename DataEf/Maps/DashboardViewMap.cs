﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class DashboardViewMap : EntityTypeConfiguration<DashboardView>
    {
        public DashboardViewMap()
        {
            ToTable("DashboardView");

            HasKey(x => x.Id);
            HasMany(x => x.FieldOfInterest).WithMany().Map(x => 
                            x.ToTable("DashboardViewFieldsOfInterest")
                            .MapLeftKey("DashboardId")
                            .MapRightKey("QuestionId")
                            );
            Property(x => x.Name).HasMaxLength(300).IsRequired();
            Property(x => x.ChartRanges).HasMaxLength(200);
            Property(x => x.Code).HasMaxLength(200).IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Code") { IsUnique = true }));
            HasOptional(x => x.Parent).WithMany(x=> x.ChildrenViews)
                .Map(x=> x.MapKey("ParentId"));
           
        }
    }
}
