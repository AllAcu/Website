﻿using System.Web.Http;
using Microsoft.Its.Domain;
using Microsoft.Its.Domain.Sql;
using Pocket;

namespace AllAcu.App_Start
{
    public class Domain
    {
        internal void Configure(Configuration config)
        {
            config.UseSqlEventStore();
        }
    }
}
