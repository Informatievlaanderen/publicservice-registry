﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PublicServiceRegistry.Projections.Backoffice;
using System;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    [DbContext(typeof(BackofficeContext))]
    [Migration("20180418185341_AddCompetentAuthorityName")]
    partial class AddCompetentAuthorityName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Position");

                    b.HasKey("Name")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("ProjectionStates","PublicServiceRegistryBackoffice");
                });

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceList.PublicServiceIdGeneratorItem", b =>
                {
                    b.Property<int>("PublicServiceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("PublicServiceId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("PublicServiceIdGenerator","PublicServiceRegistry");
                });

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceList.PublicServiceListItem", b =>
                {
                    b.Property<string>("PublicServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompetentAuthorityCode");

                    b.Property<string>("CompetentAuthorityName");

                    b.Property<bool>("ExportToOrafin");

                    b.Property<string>("Name");

                    b.HasKey("PublicServiceId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Name");

                    b.ToTable("PublicServiceList","PublicServiceRegistryBackoffice");
                });
#pragma warning restore 612, 618
        }
    }
}
