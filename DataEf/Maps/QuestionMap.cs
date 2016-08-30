﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            ToTable("Question");

            HasKey(x => x.Id);
            HasOptional(x => x.QuestionGroup);
            Property(x => x.Code).HasMaxLength(100);
            Property(x => x.Text).HasMaxLength(300);
        }
    }
}
