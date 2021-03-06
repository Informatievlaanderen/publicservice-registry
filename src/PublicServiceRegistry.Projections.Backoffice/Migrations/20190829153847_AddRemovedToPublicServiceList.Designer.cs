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
    [Migration("20190829153847_AddRemovedToPublicServiceList")]
    partial class AddRemovedToPublicServiceList
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Be.Vlaanderen.Basisregisters.ProjectionHandling.Runner.ProjectionStates.ProjectionStateItem", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DesiredState");

                    b.Property<DateTimeOffset?>("DesiredStateChangedAt");

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

                    b.Property<int>("LifeCycleStageId");

                    b.Property<int?>("FromAsInt")
                        .HasColumnName("From");

                    b.Property<string>("LifeCycleStageType")
                        .IsRequired();

                    b.Property<int?>("ToAsInt")
                        .HasColumnName("To");

                    b.HasKey("PublicServiceId", "LifeCycleStageId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("PublicServiceLifeCycleList","PublicServiceRegistryBackoffice");
                });

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceList.LifeCycleStageItemForPublicServiceList", b =>
                {
                    b.Property<string>("PublicServiceId");

                    b.Property<int>("LifeCycleStageId");

                    b.Property<int?>("FromAsInt")
                        .HasColumnName("From");

                    b.Property<string>("LifeCycleStageType");

                    b.Property<int?>("ToAsInt")
                        .HasColumnName("To");

                    b.HasKey("PublicServiceId", "LifeCycleStageId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.ToTable("LifeCycleStagesForPublicServiceList","PublicServiceRegistryBackoffice");
                });

            modelBuilder.Entity("PublicServiceRegistry.Projections.Backoffice.PublicServiceList.PublicServiceListItem", b =>
                {
                    b.Property<string>("PublicServiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompetentAuthorityCode");

                    b.Property<string>("CompetentAuthorityName");

                    b.Property<int?>("CurrentLifeCycleStageEndsAtAsInt")
                        .HasColumnName("CurrentLifeCycleStageEndsAt");

                    b.Property<int?>("CurrentLifeCycleStageId");

                    b.Property<string>("CurrentLifeCycleStageType");

                    b.Property<bool>("ExportToOrafin");

                    b.Property<string>("IpdcCode");

                    b.Property<string>("LegislativeDocumentId");

                    b.Property<string>("Name");

                    b.Property<bool>("Removed");

                    b.HasKey("PublicServiceId")
                        .HasAnnotation("SqlServer:Clustered", true);

                    b.HasIndex("Name");

                    b.HasIndex("Removed");

                    b.ToTable("PublicServiceList","PublicServiceRegistryBackoffice");
                });
#pragma warning restore 612, 618
        }
    }
}
