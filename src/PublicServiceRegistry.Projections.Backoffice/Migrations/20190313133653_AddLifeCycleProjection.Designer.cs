﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublicServiceRegistry.Projections.Backoffice;

namespace PublicServiceRegistry.Projections.Backoffice.Migrations
{
    [DbContext(typeof(BackofficeContext))]
    [Migration("20190313133653_AddLifeCycleProjection")]
    partial class AddLifeCycleProjection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
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

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceLabelList.PublicServiceLabelListItem", b =>
                {
                    b.Property<string>("PublicServiceId");

                    b.Property<string>("LabelType");

                    b.Property<string>("LabelValue");

                    b.HasKey("PublicServiceId", "LabelType")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("PublicServiceLabelList","PublicServiceRegistryBackoffice");
                });

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceLifeCycle.PublicServiceLifeCycleItem", b =>
                {
                    b.Property<string>("PublicServiceId");

                    b.Property<string>("LifeCycleStage");

                    b.Property<DateTimeOffset?>("From");

                    b.Property<DateTimeOffset?>("To");

                    b.HasKey("PublicServiceId", "LifeCycleStage")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("PublicServiceLifeCycleList","PublicServiceRegistryBackoffice");
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
