using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EsyaSoft.Adapter.API.EFModels;

public partial class AdapterContext : DbContext
{
    public AdapterContext()
    {
    }

    public AdapterContext(DbContextOptions<AdapterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<FlexSyncAccount> FlexSyncAccounts { get; set; }

    public virtual DbSet<FlexSyncAccountServicePointAssociation> FlexSyncAccountServicePointAssociations { get; set; }

    public virtual DbSet<FlexSyncConsumer> FlexSyncConsumers { get; set; }

    public virtual DbSet<FlexSyncConsumerAccountAssociation> FlexSyncConsumerAccountAssociations { get; set; }

    public virtual DbSet<FlexSyncConsumerAddress> FlexSyncConsumerAddresses { get; set; }

    public virtual DbSet<FlexSyncConsumerConsumerAddressAssociation> FlexSyncConsumerConsumerAddressAssociations { get; set; }

    public virtual DbSet<FlexSyncHeader> FlexSyncHeaders { get; set; }

    public virtual DbSet<FlexSyncParameter> FlexSyncParameters { get; set; }

    public virtual DbSet<FlexSyncServicePoint> FlexSyncServicePoints { get; set; }

    public virtual DbSet<FlexSyncServicePointGroup> FlexSyncServicePointGroups { get; set; }

    public virtual DbSet<FlexSyncServicePointServicePointGroupAssociation> FlexSyncServicePointServicePointGroupAssociations { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobParameter> JobParameters { get; set; }

    public virtual DbSet<JobQueue> JobQueues { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<MdrcancelDetail> MdrcancelDetails { get; set; }

    public virtual DbSet<MdrcancelHeader> MdrcancelHeaders { get; set; }

    public virtual DbSet<MdrconfirmationDetail> MdrconfirmationDetails { get; set; }

    public virtual DbSet<MdrconfirmationHeader> MdrconfirmationHeaders { get; set; }

    public virtual DbSet<Mdrdetail> Mdrdetails { get; set; }

    public virtual DbSet<Mdrheader> Mdrheaders { get; set; }

    public virtual DbSet<MdrresultDetail> MdrresultDetails { get; set; }

    public virtual DbSet<MdrresultHeader> MdrresultHeaders { get; set; }

    public virtual DbSet<MeasurementTaskRequestDetail> MeasurementTaskRequestDetails { get; set; }

    public virtual DbSet<MeasurementTaskRequestHeader> MeasurementTaskRequestHeaders { get; set; }

    public virtual DbSet<MeterChangeConfirmationDetail> MeterChangeConfirmationDetails { get; set; }

    public virtual DbSet<MeterChangeConfirmationHeader> MeterChangeConfirmationHeaders { get; set; }

    public virtual DbSet<MeterChangeRequestDetail> MeterChangeRequestDetails { get; set; }

    public virtual DbSet<MeterChangeRequestHeader> MeterChangeRequestHeaders { get; set; }

    public virtual DbSet<MeterConnectionStatusSingle> MeterConnectionStatusSingles { get; set; }

    public virtual DbSet<MeterCreateChangeRegisterDetail> MeterCreateChangeRegisterDetails { get; set; }

    public virtual DbSet<MeterCreateConfirmationDetail> MeterCreateConfirmationDetails { get; set; }

    public virtual DbSet<MeterCreateConfirmationHeader> MeterCreateConfirmationHeaders { get; set; }

    public virtual DbSet<MeterCreateRequestDetail> MeterCreateRequestDetails { get; set; }

    public virtual DbSet<MeterCreateRequestHeader> MeterCreateRequestHeaders { get; set; }

    public virtual DbSet<MeterLocationDetail> MeterLocationDetails { get; set; }

    public virtual DbSet<MeterLocationHeader> MeterLocationHeaders { get; set; }

    public virtual DbSet<MeterRegisterCreateConfirmationDetail> MeterRegisterCreateConfirmationDetails { get; set; }

    public virtual DbSet<MeterRegisterCreateConfirmationHeader> MeterRegisterCreateConfirmationHeaders { get; set; }

    public virtual DbSet<MeterRegisterCreateDetail> MeterRegisterCreateDetails { get; set; }

    public virtual DbSet<MeterRegisterCreateHeader> MeterRegisterCreateHeaders { get; set; }

    public virtual DbSet<MeterRegisterDetail> MeterRegisterDetails { get; set; }

    public virtual DbSet<MeterRegisterHeader> MeterRegisterHeaders { get; set; }

    public virtual DbSet<MeterReplicationDetail> MeterReplicationDetails { get; set; }

    public virtual DbSet<MeterReplicationHeader> MeterReplicationHeaders { get; set; }

    public virtual DbSet<RegisterSpecificDetail> RegisterSpecificDetails { get; set; }

    public virtual DbSet<ReplicationRegisterDetail> ReplicationRegisterDetails { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<ServiceCallLog> ServiceCallLogs { get; set; }

    public virtual DbSet<ServiceDirectionType> ServiceDirectionTypes { get; set; }

    public virtual DbSet<ServiceMaster> ServiceMasters { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<State> States { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=ARUN-DELL2-2022;Initial Catalog=ADAPTER;User ID=sa;Password=pa55w0rd;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AggregatedCounter>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

            entity.ToTable("AggregatedCounter", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

            entity.ToTable("Counter", "HangFire");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<FlexSyncAccount>(entity =>
        {
            entity.HasKey(e => e.FlexSyncAccountAltId).HasName("PK__FlexSync__EF730397BE3B84C4");

            entity.ToTable("FlexSyncAccount");

            entity.Property(e => e.FlexSyncAccountAltId).ValueGeneratedNever();
            entity.Property(e => e.AccountClass).HasMaxLength(20);
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.AccountType).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncAccountId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncAccountServicePointAssociation>(entity =>
        {
            entity.HasKey(e => e.FlexSyncAccountServicePointAssociationAltId).HasName("PK__FlexSync__BDA26397E0F97B21");

            entity.ToTable("FlexSyncAccountServicePointAssociation");

            entity.Property(e => e.FlexSyncAccountServicePointAssociationAltId).ValueGeneratedNever();
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.AccountType).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FlexSyncAccountServicePointAssociationId).ValueGeneratedOnAdd();
            entity.Property(e => e.ServicePointId).HasMaxLength(50);
            entity.Property(e => e.ServicePointType).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncConsumer>(entity =>
        {
            entity.HasKey(e => e.FlexSyncConsumerAltId).HasName("PK__FlexSync__BF9DD91E0CA79624");

            entity.ToTable("FlexSyncConsumer");

            entity.Property(e => e.FlexSyncConsumerAltId).ValueGeneratedNever();
            entity.Property(e => e.ConsumerId).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.FlexSyncConsumerId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncConsumerAccountAssociation>(entity =>
        {
            entity.HasKey(e => e.FlexSyncConsumerAccountAssociationAltId).HasName("PK__FlexSync__BA73072300AAA01E");

            entity.ToTable("FlexSyncConsumerAccountAssociation");

            entity.Property(e => e.FlexSyncConsumerAccountAssociationAltId).ValueGeneratedNever();
            entity.Property(e => e.AccountDescription).HasMaxLength(50);
            entity.Property(e => e.AccountId).HasMaxLength(50);
            entity.Property(e => e.ConsumerDescription).HasMaxLength(50);
            entity.Property(e => e.ConsumerId).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncConsumerAccountAssociationId).ValueGeneratedOnAdd();
            entity.Property(e => e.RelType).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncConsumerAddress>(entity =>
        {
            entity.HasKey(e => e.FlexSyncConsumerAddressAltId).HasName("PK__FlexSync__0037174DC7DEFAB5");

            entity.ToTable("FlexSyncConsumerAddress");

            entity.Property(e => e.FlexSyncConsumerAddressAltId).ValueGeneratedNever();
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ConsumerAddressId).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.FlexSyncConsumerAddressId).ValueGeneratedOnAdd();
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.StateOrProvince).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Timezone).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncConsumerConsumerAddressAssociation>(entity =>
        {
            entity.HasKey(e => e.FlexSyncConsumerConsumerAddressAssociationAltId).HasName("PK__FlexSync__9731C6843EA76303");

            entity.ToTable("FlexSyncConsumerConsumerAddressAssociation");

            entity.Property(e => e.FlexSyncConsumerConsumerAddressAssociationAltId).ValueGeneratedNever();
            entity.Property(e => e.AddressRole).HasMaxLength(50);
            entity.Property(e => e.ConsumerAddressId).HasMaxLength(50);
            entity.Property(e => e.ConsumerAddressTypeDesc).HasMaxLength(50);
            entity.Property(e => e.ConsumerId).HasMaxLength(50);
            entity.Property(e => e.ConsumerTypeDesc).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncConsumerConsumerAddressAssociationId).ValueGeneratedOnAdd();
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncHeader>(entity =>
        {
            entity.HasKey(e => e.FlexSyncHeaderAltId).HasName("PK__FlexSync__65A3A458DF8CD5EB");

            entity.ToTable("FlexSyncHeader");

            entity.Property(e => e.FlexSyncHeaderAltId).ValueGeneratedNever();
            entity.Property(e => e.AsyncReplyTo).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.HeaderDateTime).HasColumnType("datetime");
            entity.Property(e => e.HeaderId).ValueGeneratedOnAdd();
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsProcessed).HasColumnName("isProcessed");
            entity.Property(e => e.MessageId)
                .HasMaxLength(100)
                .HasColumnName("MessageID");
            entity.Property(e => e.Noun).HasMaxLength(50);
            entity.Property(e => e.Source).HasMaxLength(50);
            entity.Property(e => e.SyncMode).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Verb).HasMaxLength(50);
        });

        modelBuilder.Entity<FlexSyncParameter>(entity =>
        {
            entity.HasKey(e => e.FlexSyncParameterAltId).HasName("PK__FlexSync__01BF50CE8DE8E806");

            entity.ToTable("FlexSyncParameter");

            entity.Property(e => e.FlexSyncParameterAltId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncParameterId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ParentType).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasMaxLength(100);
        });

        modelBuilder.Entity<FlexSyncServicePoint>(entity =>
        {
            entity.HasKey(e => e.FlexSyncServicePointAltId).HasName("PK__FlexSync__61C68425CBF7726E");

            entity.ToTable("FlexSyncServicePoint");

            entity.Property(e => e.FlexSyncServicePointAltId).ValueGeneratedNever();
            entity.Property(e => e.ClassName).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncServicePointId).ValueGeneratedOnAdd();
            entity.Property(e => e.LocInfo).HasMaxLength(50);
            entity.Property(e => e.PremiseDescription).HasMaxLength(100);
            entity.Property(e => e.PremiseMrid)
                .HasMaxLength(50)
                .HasColumnName("PremiseMRID");
            entity.Property(e => e.ServicePointId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncServicePointGroup>(entity =>
        {
            entity.HasKey(e => e.FlexSyncServicePointGroupAltId).HasName("PK__FlexSync__4F90E06814D25890");

            entity.ToTable("FlexSyncServicePointGroup");

            entity.Property(e => e.FlexSyncServicePointGroupAltId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncServicePointGroupId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RouteSubType).HasMaxLength(50);
            entity.Property(e => e.RouteType).HasMaxLength(50);
            entity.Property(e => e.ServicePointGroupId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FlexSyncServicePointServicePointGroupAssociation>(entity =>
        {
            entity.HasKey(e => e.FlexSyncServicePointServicePointGroupAssociationAltId).HasName("PK__FlexSync__C403FFBF73445F68");

            entity.ToTable("FlexSyncServicePointServicePointGroupAssociation");

            entity.Property(e => e.FlexSyncServicePointServicePointGroupAssociationAltId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FlexSyncServicePointServicePointGroupAssociationId).ValueGeneratedOnAdd();
            entity.Property(e => e.ServicePointGroupId).HasMaxLength(50);
            entity.Property(e => e.ServicePointGroupType).HasMaxLength(50);
            entity.Property(e => e.ServicePointId).HasMaxLength(50);
            entity.Property(e => e.ServicePointType).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

            entity.ToTable("Hash", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Field).HasMaxLength(100);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

            entity.ToTable("Job", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName").HasFilter("([StateName] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.StateName).HasMaxLength(20);
        });

        modelBuilder.Entity<JobParameter>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Name }).HasName("PK_HangFire_JobParameter");

            entity.ToTable("JobParameter", "HangFire");

            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Job).WithMany(p => p.JobParameters)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_JobParameter_Job");
        });

        modelBuilder.Entity<JobQueue>(entity =>
        {
            entity.HasKey(e => new { e.Queue, e.Id }).HasName("PK_HangFire_JobQueue");

            entity.ToTable("JobQueue", "HangFire");

            entity.Property(e => e.Queue).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FetchedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_List");

            entity.ToTable("List", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MdrcancelDetail>(entity =>
        {
            entity.ToTable("MDRCancelDetail");

            entity.Property(e => e.MdrcancelDetailId).HasColumnName("MDRCancelDetailID");
            entity.Property(e => e.CancelDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("CancelDetailMHCreationTime");
            entity.Property(e => e.CancelDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CancelDetailMHSenderBusinessSystemId");
            entity.Property(e => e.CancelDetailUuid).HasColumnName("CancelDetailUUID");
            entity.Property(e => e.IsCancellationConfirmationSentToSap).HasColumnName("IsCancellationConfirmationSentToSAP");
            entity.Property(e => e.IsCancellationProcessedByMdm).HasColumnName("IsCancellationProcessedByMDM");
            entity.Property(e => e.IsMdminvokedCancellation).HasColumnName("IsMDMInvokedCancellation");
            entity.Property(e => e.MdrcancelDetailAltId).HasColumnName("MDRCancelDetailAltID");
            entity.Property(e => e.MdrcancelHeaderAltId).HasColumnName("MDRCancelHeaderAltID");
            entity.Property(e => e.MdrcancelHeaderId).HasColumnName("MDRCancelHeaderID");
            entity.Property(e => e.MeterReadingDocumentId)
                .HasMaxLength(20)
                .HasColumnName("MeterReadingDocumentID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
        });

        modelBuilder.Entity<MdrcancelHeader>(entity =>
        {
            entity.ToTable("MDRCancelHeader");

            entity.Property(e => e.MdrcancelHeaderId).HasColumnName("MDRCancelHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsCancellationConfirmationSentToSap).HasColumnName("IsCancellationConfirmationSentToSAP");
            entity.Property(e => e.IsCancellationProcessedByMdm).HasColumnName("IsCancellationProcessedByMDM");
            entity.Property(e => e.IsMdminvokedCancellation).HasColumnName("IsMDMInvokedCancellation");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MccreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MCCreationDatetime");
            entity.Property(e => e.McsenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MCSenderSystemBusinessID");
            entity.Property(e => e.Mcuuid).HasColumnName("MCUUID");
            entity.Property(e => e.MdrcancelHeaderAltId).HasColumnName("MDRCancelHeaderAltID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MdrconfirmationDetail>(entity =>
        {
            entity.ToTable("MDRConfirmationDetail");

            entity.Property(e => e.MdrconfirmationDetailId).HasColumnName("MDRConfirmationDetailID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MdrconfirmationDetailAltId).HasColumnName("MDRConfirmationDetailAltID");
            entity.Property(e => e.MdrconfirmationDetailCreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MDRConfirmationDetailCreationDatetime");
            entity.Property(e => e.MdrconfirmationDetailLogBusinessDocumentProcessingResultCode).HasColumnName("MDRConfirmationDetailLogBusinessDocumentProcessingResultCode");
            entity.Property(e => e.MdrconfirmationDetailLogItemNote)
                .HasMaxLength(50)
                .HasColumnName("MDRConfirmationDetailLogItemNote");
            entity.Property(e => e.MdrconfirmationDetailLogItemSeverityCode).HasColumnName("MDRConfirmationDetailLogItemSeverityCode");
            entity.Property(e => e.MdrconfirmationDetailLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRConfirmationDetailLogItemTypeId");
            entity.Property(e => e.MdrconfirmationDetailLogMaximumLogItemSeverityCode).HasColumnName("MDRConfirmationDetailLogMaximumLogItemSeverityCode");
            entity.Property(e => e.MdrconfirmationDetailMeterReadingDocumentId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRConfirmationDetailMeterReadingDocumentID");
            entity.Property(e => e.MdrconfirmationDetailRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRConfirmationDetailRecipientSystemBusinessID");
            entity.Property(e => e.MdrconfirmationDetailRefUuid).HasColumnName("MDRConfirmationDetailRefUUID");
            entity.Property(e => e.MdrconfirmationDetailUuid).HasColumnName("MDRConfirmationDetailUUID");
            entity.Property(e => e.MdrconfirmationHeaderAltId).HasColumnName("MDRConfirmationHeaderAltID");
            entity.Property(e => e.MdrconfirmationHeaderId).HasColumnName("MDRConfirmationHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MdrconfirmationHeader>(entity =>
        {
            entity.ToTable("MDRConfirmationHeader");

            entity.Property(e => e.MdrconfirmationHeaderId).HasColumnName("MDRConfirmationHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MdrconfirmationHeaderAltId).HasColumnName("MDRConfirmationHeaderAltID");
            entity.Property(e => e.MdrhconfirmationCreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MDRHConfirmationCreationDatetime");
            entity.Property(e => e.MdrhconfirmationLogBusinessDocumentProcessingResultCode).HasColumnName("MDRHConfirmationLogBusinessDocumentProcessingResultCode");
            entity.Property(e => e.MdrhconfirmationLogItemNote)
                .HasMaxLength(50)
                .HasColumnName("MDRHConfirmationLogItemNote");
            entity.Property(e => e.MdrhconfirmationLogItemSeverityCode).HasColumnName("MDRHConfirmationLogItemSeverityCode");
            entity.Property(e => e.MdrhconfirmationLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRHConfirmationLogItemTypeId");
            entity.Property(e => e.MdrhconfirmationLogMaximumLogItemSeverityCode).HasColumnName("MDRHConfirmationLogMaximumLogItemSeverityCode");
            entity.Property(e => e.MdrhconfirmationRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRHConfirmationRecipientSystemBusinessID");
            entity.Property(e => e.MdrhconfirmationRefUuid).HasColumnName("MDRHConfirmationRefUUID");
            entity.Property(e => e.MdrhconfirmationUuid).HasColumnName("MDRHConfirmationUUID");
            entity.Property(e => e.MdrheaderAltId).HasColumnName("MDRHeaderAltID");
            entity.Property(e => e.MdrheaderId).HasColumnName("MDRHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Mdrdetail>(entity =>
        {
            entity.ToTable("MDRDetail");

            entity.Property(e => e.MdrdetailId).HasColumnName("MDRDetailID");
            entity.Property(e => e.DetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("DetailMHCreationTime");
            entity.Property(e => e.DetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("DetailMHSenderBusinessSystemId");
            entity.Property(e => e.DetailMhuuid).HasColumnName("DetailMHUUID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MdrdetailAltId).HasColumnName("MDRDetailAltID");
            entity.Property(e => e.MdrheaderAltId).HasColumnName("MDRHeaderAltID");
            entity.Property(e => e.MdrheaderId).HasColumnName("MDRHeaderID");
            entity.Property(e => e.MeterReadingDocumentId)
                .HasMaxLength(20)
                .HasColumnName("MeterReadingDocumentID");
            entity.Property(e => e.ScheduledMeterReadingDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(50)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
        });

        modelBuilder.Entity<Mdrheader>(entity =>
        {
            entity.ToTable("MDRHeader");

            entity.Property(e => e.MdrheaderId).HasColumnName("MDRHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MdrheaderAltId).HasColumnName("MDRHeaderAltID");
            entity.Property(e => e.MhcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MHCreationDatetime");
            entity.Property(e => e.Mhuuid).HasColumnName("MHUUID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("SenderSystemBusinessID");
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MdrresultDetail>(entity =>
        {
            entity.ToTable("MDRResultDetail");

            entity.Property(e => e.MdrresultDetailId).HasColumnName("MDRResultDetailID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsPickedByAdapter).HasDefaultValue(false);
            entity.Property(e => e.MdrresultDetailActualMeterReadingDate)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailActualMeterReadingDate");
            entity.Property(e => e.MdrresultDetailActualMeterReadingTime)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailActualMeterReadingTime");
            entity.Property(e => e.MdrresultDetailAltId).HasColumnName("MDRResultDetailAltID");
            entity.Property(e => e.MdrresultDetailCreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MDRResultDetailCreationDatetime");
            entity.Property(e => e.MdrresultDetailMeterReadingDocumentId).HasColumnName("MDRResultDetailMeterReadingDocumentID");
            entity.Property(e => e.MdrresultDetailMeterReadingReasonCode).HasColumnName("MDRResultDetailMeterReadingReasonCode");
            entity.Property(e => e.MdrresultDetailMeterReadingResultValue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MDRResultDetailMeterReadingResultValue");
            entity.Property(e => e.MdrresultDetailMeterReadingTypeCode).HasColumnName("MDRResultDetailMeterReadingTypeCode");
            entity.Property(e => e.MdrresultDetailRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailRecipientSystemBusinessID");
            entity.Property(e => e.MdrresultDetailRefUuid).HasColumnName("MDRResultDetailRefUUID");
            entity.Property(e => e.MdrresultDetailScheduledMeterReadingDate)
                .HasColumnType("datetime")
                .HasColumnName("MDRResultDetailScheduledMeterReadingDate");
            entity.Property(e => e.MdrresultDetailUtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailUtilitiesMeasurementTaskID");
            entity.Property(e => e.MdrresultDetailUtilitiesObjectIdentificationSystemCodeText)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailUtilitiesObjectIdentificationSystemCodeText");
            entity.Property(e => e.MdrresultDetailUtilitiesUtilitiesDeviceId)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("MDRResultDetailUtilitiesUtilitiesDeviceID");
            entity.Property(e => e.MdrresultDetailUuid).HasColumnName("MDRResultDetailUUID");
            entity.Property(e => e.MdrresultHeaderAltId).HasColumnName("MDRResultHeaderAltID");
            entity.Property(e => e.MdrresultHeaderId).HasColumnName("MDRResultHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MdrresultHeader>(entity =>
        {
            entity.ToTable("MDRResultHeader");

            entity.Property(e => e.MdrresultHeaderId).HasColumnName("MDRResultHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MdrheaderAltId).HasColumnName("MDRHeaderAltID");
            entity.Property(e => e.MdrheaderId).HasColumnName("MDRHeaderID");
            entity.Property(e => e.MdrresultHeaderAltId).HasColumnName("MDRResultHeaderAltID");
            entity.Property(e => e.MdrresultHeaderCreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MDRResultHeaderCreationDatetime");
            entity.Property(e => e.MdrresultHeaderRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MDRResultHeaderRecipientSystemBusinessID");
            entity.Property(e => e.MdrresultHeaderRefUuid).HasColumnName("MDRResultHeaderRefUUID");
            entity.Property(e => e.MdrresultHeaderUuid).HasColumnName("MDRResultHeaderUUID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeasurementTaskRequestDetail>(entity =>
        {
            entity.ToTable("MeasurementTaskRequestDetail");

            entity.Property(e => e.MeasurementTaskRequestDetailId).HasColumnName("MeasurementTaskRequestDetailID");
            entity.Property(e => e.AssignedUtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("AssignedUtilitiesDeviceID");
            entity.Property(e => e.DeviceAssignmentListCompleteTransmissionIndicator).HasColumnName("deviceAssignmentListCompleteTransmissionIndicator");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeasurementTaskRequestDetailAltId).HasColumnName("MeasurementTaskRequestDetailAltID");
            entity.Property(e => e.MeasurementTaskRequestDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeasurementTaskRequestDetailMHCreationTime");
            entity.Property(e => e.MeasurementTaskRequestDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeasurementTaskRequestDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeasurementTaskRequestDetailMhuuid).HasColumnName("MeasurementTaskRequestDetailMHUUID");
            entity.Property(e => e.MeasurementTaskRequestHeaderAltId).HasColumnName("MeasurementTaskRequestHeaderAltID");
            entity.Property(e => e.MeasurementTaskRequestHeaderId).HasColumnName("MeasurementTaskRequestHeaderID");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
            entity.Property(e => e.UtilitiesQuantityAdjustmentFactorValue).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<MeasurementTaskRequestHeader>(entity =>
        {
            entity.ToTable("MeasurementTaskRequestHeader");

            entity.Property(e => e.MeasurementTaskRequestHeaderId).HasColumnName("MeasurementTaskRequestHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MeasurementTaskRequestHeaderAltId).HasColumnName("MeasurementTaskRequestHeaderAltID");
            entity.Property(e => e.MtcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MTCreationDatetime");
            entity.Property(e => e.MtmessageUuid).HasColumnName("MTMessageUUID");
            entity.Property(e => e.MtsenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MTSenderSystemBusinessID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterChangeConfirmationDetail>(entity =>
        {
            entity.ToTable("MeterChangeConfirmationDetail");

            entity.Property(e => e.MeterChangeConfirmationDetailId).HasColumnName("MeterChangeConfirmationDetailID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterChangeConfirmationDetailAltId).HasColumnName("MeterChangeConfirmationDetailAltID");
            entity.Property(e => e.MeterChangeConfirmationDetailCreationDatetime).HasColumnType("datetime");
            entity.Property(e => e.MeterChangeConfirmationDetailLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterChangeConfirmationDetailLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterChangeConfirmationDetailRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterChangeConfirmationDetailRecipientSystemBusinessID");
            entity.Property(e => e.MeterChangeConfirmationDetailRefUuid).HasColumnName("MeterChangeConfirmationDetailRefUUID");
            entity.Property(e => e.MeterChangeConfirmationDetailUtilitiesDeviceId)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("MeterChangeConfirmationDetailUtilitiesDeviceID");
            entity.Property(e => e.MeterChangeConfirmationDetailUuid).HasColumnName("MeterChangeConfirmationDetailUUID");
            entity.Property(e => e.MeterChangeConfirmationHeaderAltId).HasColumnName("MeterChangeConfirmationHeaderAltID");
            entity.Property(e => e.MeterChangeConfirmationHeaderId).HasColumnName("MeterChangeConfirmationHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterChangeConfirmationHeader>(entity =>
        {
            entity.ToTable("MeterChangeConfirmationHeader");

            entity.Property(e => e.MeterChangeConfirmationHeaderId).HasColumnName("MeterChangeConfirmationHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterChangeConfirmationHeaderAltId).HasColumnName("MeterChangeConfirmationHeaderAltID");
            entity.Property(e => e.MeterChangeConfirmationMainLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterChangeConfirmationMainLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterChangeConfirmationMhcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MeterChangeConfirmationMHCreationDatetime");
            entity.Property(e => e.MeterChangeConfirmationMhrecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterChangeConfirmationMHRecipientSystemBusinessID");
            entity.Property(e => e.MeterChangeConfirmationMhrefUuid).HasColumnName("MeterChangeConfirmationMHRefUUID");
            entity.Property(e => e.MeterChangeConfirmationMhuuid).HasColumnName("MeterChangeConfirmationMHUUID");
            entity.Property(e => e.MeterChangeRequestHeaderAltId).HasColumnName("MeterChangeRequestHeaderAltID");
            entity.Property(e => e.MeterChangeRequestHeaderId).HasColumnName("MeterChangeRequestHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterChangeRequestDetail>(entity =>
        {
            entity.ToTable("MeterChangeRequestDetail");

            entity.Property(e => e.MeterChangeRequestDetailId).HasColumnName("MeterChangeRequestDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeterChangeRequestDetailAltId).HasColumnName("MeterChangeRequestDetailAltID");
            entity.Property(e => e.MeterChangeRequestDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterChangeRequestDetailMHCreationTime");
            entity.Property(e => e.MeterChangeRequestDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterChangeRequestDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterChangeRequestDetailMhuuid).HasColumnName("MeterChangeRequestDetailMHUUID");
            entity.Property(e => e.MeterChangeRequestHeaderAltId).HasColumnName("MeterChangeRequestHeaderAltID");
            entity.Property(e => e.MeterChangeRequestHeaderId).HasColumnName("MeterChangeRequestHeaderID");
            entity.Property(e => e.PartyInternalId)
                .HasMaxLength(30)
                .HasColumnName("PartyInternalID");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesDeviceMaterialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceMaterialID");
            entity.Property(e => e.UtilitiesDeviceSerialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceSerialID");
        });

        modelBuilder.Entity<MeterChangeRequestHeader>(entity =>
        {
            entity.ToTable("MeterChangeRequestHeader");

            entity.Property(e => e.MeterChangeRequestHeaderId).HasColumnName("MeterChangeRequestHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MchangeRcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MChangeRCreationDatetime");
            entity.Property(e => e.MchangeRmessageUuid).HasColumnName("MChangeRMessageUUID");
            entity.Property(e => e.MchangeRsenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MChangeRSenderSystemBusinessID");
            entity.Property(e => e.MeterChangeRequestHeaderAltId).HasColumnName("MeterChangeRequestHeaderAltID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterConnectionStatusSingle>(entity =>
        {
            entity.ToTable("MeterConnectionStatusSingle");

            entity.Property(e => e.MeterConnectionStatusSingleId).HasColumnName("MeterConnectionStatusSingleID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsCompletedByMdm).HasColumnName("IsCompletedByMDM");
            entity.Property(e => e.IsSentToMdm).HasColumnName("IsSentToMDM");
            entity.Property(e => e.McschangeRequestCategoryCode).HasColumnName("MCSChangeRequestCategoryCode");
            entity.Property(e => e.McschangeRequestId)
                .HasMaxLength(20)
                .HasColumnName("MCSChangeRequestId");
            entity.Property(e => e.McscreationDatetimeByMdm)
                .HasColumnType("datetime")
                .HasColumnName("MCSCreationDatetimeByMDM");
            entity.Property(e => e.McscreationDatetimeBySap)
                .HasColumnType("datetime")
                .HasColumnName("MCSCreationDatetimeBySAP");
            entity.Property(e => e.McslogBusinessDocumentProcessingResultCodeByMdm).HasColumnName("MCSLogBusinessDocumentProcessingResultCodeByMDM");
            entity.Property(e => e.McslogItemNoteByMdm)
                .HasMaxLength(50)
                .HasColumnName("MCSLogItemNoteByMDM");
            entity.Property(e => e.McslogItemSeverityCodeByMdm).HasColumnName("MCSLogItemSeverityCodeByMDM");
            entity.Property(e => e.McslogItemTypeIdByMdm)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MCSLogItemTypeIdByMDM");
            entity.Property(e => e.McslogMaximumLogItemSeverityCodeByMdm).HasColumnName("MCSLogMaximumLogItemSeverityCodeByMDM");
            entity.Property(e => e.McsplannedProcessingDateTimeBySap)
                .HasColumnType("datetime")
                .HasColumnName("MCSPlannedProcessingDateTimeBySAP");
            entity.Property(e => e.McsprocessingDateTimeByMdm)
                .HasColumnType("datetime")
                .HasColumnName("MCSProcessingDateTimeByMDM");
            entity.Property(e => e.McsrecipientSystemBusinessIdbyMdm)
                .HasMaxLength(10)
                .HasColumnName("MCSRecipientSystemBusinessIDByMDM");
            entity.Property(e => e.McsrefUuidbyMdm).HasColumnName("MCSRefUUIDByMDM");
            entity.Property(e => e.McssenderBusinessSystemIdbySap)
                .HasMaxLength(10)
                .HasColumnName("MCSSenderBusinessSystemIDBySAP");
            entity.Property(e => e.McsutilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .HasColumnName("MCSUtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.McsutilitiesDeviceConnectionStatusProcessingResultCodeByMdm).HasColumnName("MCSUtilitiesDeviceConnectionStatusProcessingResultCodeByMDM");
            entity.Property(e => e.McsutilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("MCSUtilitiesDeviceID");
            entity.Property(e => e.Mcsuuid).HasColumnName("MCSUUID");
            entity.Property(e => e.MeterConnectionStatusSingleAltId).HasColumnName("MeterConnectionStatusSingleAltID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterCreateChangeRegisterDetail>(entity =>
        {
            entity.ToTable("MeterCreateChangeRegisterDetail");

            entity.Property(e => e.MeterCreateChangeRegisterDetailId).HasColumnName("MeterCreateChangeRegisterDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeterCreateChangeRegisterDetailAltId).HasColumnName("MeterCreateChangeRegisterDetailAltID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReferenceDetailAltId).HasColumnName("ReferenceDetailAltID");
            entity.Property(e => e.ReferenceDetailId).HasColumnName("ReferenceDetailID");
            entity.Property(e => e.RegisterSourceType)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.SpecMeasureUnitCode).HasMaxLength(20);
            entity.Property(e => e.SpecMeterReadingResultAdjustmentFactorValue).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TimeZoneCode).HasMaxLength(20);
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
        });

        modelBuilder.Entity<MeterCreateConfirmationDetail>(entity =>
        {
            entity.ToTable("MeterCreateConfirmationDetail");

            entity.Property(e => e.MeterCreateConfirmationDetailId).HasColumnName("MeterCreateConfirmationDetailID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterCreateConfirmationConfirmationHeaderAltId).HasColumnName("MeterCreateConfirmationConfirmationHeaderAltID");
            entity.Property(e => e.MeterCreateConfirmationDetailAltId).HasColumnName("MeterCreateConfirmationDetailAltID");
            entity.Property(e => e.MeterCreateConfirmationDetailCreationDatetime).HasColumnType("datetime");
            entity.Property(e => e.MeterCreateConfirmationDetailLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterCreateConfirmationDetailLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterCreateConfirmationDetailRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterCreateConfirmationDetailRecipientSystemBusinessID");
            entity.Property(e => e.MeterCreateConfirmationDetailRefUuid).HasColumnName("MeterCreateConfirmationDetailRefUUID");
            entity.Property(e => e.MeterCreateConfirmationDetailUtilitiesDeviceId)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("MeterCreateConfirmationDetailUtilitiesDeviceID");
            entity.Property(e => e.MeterCreateConfirmationDetailUuid).HasColumnName("MeterCreateConfirmationDetailUUID");
            entity.Property(e => e.MeterCreateConfirmationHeaderId).HasColumnName("MeterCreateConfirmationHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterCreateConfirmationHeader>(entity =>
        {
            entity.ToTable("MeterCreateConfirmationHeader");

            entity.Property(e => e.MeterCreateConfirmationHeaderId).HasColumnName("MeterCreateConfirmationHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterCreateConfirmationHeaderAltId).HasColumnName("MeterCreateConfirmationHeaderAltID");
            entity.Property(e => e.MeterCreateConfirmationMainLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterCreateConfirmationMainLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterCreateConfirmationMhcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MeterCreateConfirmationMHCreationDatetime");
            entity.Property(e => e.MeterCreateConfirmationMhrecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterCreateConfirmationMHRecipientSystemBusinessID");
            entity.Property(e => e.MeterCreateConfirmationMhrefUuid).HasColumnName("MeterCreateConfirmationMHRefUUID");
            entity.Property(e => e.MeterCreateConfirmationMhuuid).HasColumnName("MeterCreateConfirmationMHUUID");
            entity.Property(e => e.MeterCreateRequestHeaderAltId).HasColumnName("MeterCreateRequestHeaderAltID");
            entity.Property(e => e.MeterCreateRequestHeaderId).HasColumnName("MeterCreateRequestHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterCreateRequestDetail>(entity =>
        {
            entity.ToTable("MeterCreateRequestDetail");

            entity.Property(e => e.MeterCreateRequestDetailId).HasColumnName("MeterCreateRequestDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeterCreateRequestDetailAltId).HasColumnName("MeterCreateRequestDetailAltID");
            entity.Property(e => e.MeterCreateRequestDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterCreateRequestDetailMHCreationTime");
            entity.Property(e => e.MeterCreateRequestDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterCreateRequestDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterCreateRequestDetailMhuuid).HasColumnName("MeterCreateRequestDetailMHUUID");
            entity.Property(e => e.MeterCreateRequestHeaderAltId).HasColumnName("MeterCreateRequestHeaderAltID");
            entity.Property(e => e.MeterCreateRequestHeaderId).HasColumnName("MeterCreateRequestHeaderID");
            entity.Property(e => e.PartyInternalId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PartyInternalID");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(10)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesDeviceMaterialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceMaterialID");
            entity.Property(e => e.UtilitiesDeviceSerialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceSerialID");
        });

        modelBuilder.Entity<MeterCreateRequestHeader>(entity =>
        {
            entity.ToTable("MeterCreateRequestHeader");

            entity.Property(e => e.MeterCreateRequestHeaderId).HasColumnName("MeterCreateRequestHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.McrcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MCRCreationDatetime");
            entity.Property(e => e.McrmessageUuid).HasColumnName("MCRMessageUUID");
            entity.Property(e => e.McrsenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MCRSenderSystemBusinessID");
            entity.Property(e => e.MeterCreateRequestHeaderAltId).HasColumnName("MeterCreateRequestHeaderAltID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterLocationDetail>(entity =>
        {
            entity.ToTable("MeterLocationDetail");

            entity.Property(e => e.MeterLocationDetailId).HasColumnName("MeterLocationDetailID");
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.CountryCode).HasMaxLength(5);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.InstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("InstallationPointID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.LocationListCompleteTransmissionIndicator).HasColumnName("locationListCompleteTransmissionIndicator");
            entity.Property(e => e.LogicalInstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("LogicalInstallationPointID");
            entity.Property(e => e.LogicalLocationListCompleteTransmissionIndicator).HasColumnName("logicalLocationListCompleteTransmissionIndicator");
            entity.Property(e => e.MeterLocationDetailAltId).HasColumnName("MeterLocationDetailAltID");
            entity.Property(e => e.MeterLocationDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterLocationDetailMHCreationTime");
            entity.Property(e => e.MeterLocationDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterLocationDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterLocationDetailMhuuid).HasColumnName("MeterLocationDetailMHUUID");
            entity.Property(e => e.MeterLocationHeaderAltId).HasColumnName("MeterLocationHeaderAltID");
            entity.Property(e => e.MeterLocationHeaderId).HasColumnName("MeterLocationHeaderID");
            entity.Property(e => e.ModificationTimeZoneCode).HasMaxLength(30);
            entity.Property(e => e.ParentInstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("ParentInstallationPointID");
            entity.Property(e => e.RegionCode).HasMaxLength(5);
            entity.Property(e => e.StreetName).HasMaxLength(150);
            entity.Property(e => e.StreetPostalCode).HasMaxLength(10);
            entity.Property(e => e.TimeZoneCode).HasMaxLength(30);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(30)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
        });

        modelBuilder.Entity<MeterLocationHeader>(entity =>
        {
            entity.ToTable("MeterLocationHeader");

            entity.Property(e => e.MeterLocationHeaderId).HasColumnName("MeterLocationHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MeterLocationHeaderAltId).HasColumnName("MeterLocationHeaderAltID");
            entity.Property(e => e.MlcreationDatetime)
                .HasColumnType("datetime")
                .HasColumnName("MLCreationDatetime");
            entity.Property(e => e.MlmessageUuid).HasColumnName("MLMessageUUID");
            entity.Property(e => e.MlsenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MLSenderSystemBusinessID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterRegisterCreateConfirmationDetail>(entity =>
        {
            entity.ToTable("MeterRegisterCreateConfirmationDetail");

            entity.Property(e => e.MeterRegisterCreateConfirmationDetailId).HasColumnName("MeterRegisterCreateConfirmationDetailID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailAltId).HasColumnName("MeterRegisterCreateConfirmationDetailAltID");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailCreationDatetime).HasColumnType("datetime");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailRecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterCreateConfirmationDetailRecipientSystemBusinessID");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailRefUuid).HasColumnName("MeterRegisterCreateConfirmationDetailRefUUID");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailUtilitiesDeviceId)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("MeterRegisterCreateConfirmationDetailUtilitiesDeviceID");
            entity.Property(e => e.MeterRegisterCreateConfirmationDetailUuid).HasColumnName("MeterRegisterCreateConfirmationDetailUUID");
            entity.Property(e => e.MeterRegisterCreateConfirmationHeaderAltId).HasColumnName("MeterRegisterCreateConfirmationHeaderAltID");
            entity.Property(e => e.MeterRegisterCreateConfirmationHeaderId).HasColumnName("MeterRegisterCreateConfirmationHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterRegisterCreateConfirmationHeader>(entity =>
        {
            entity.ToTable("MeterRegisterCreateConfirmationHeader");

            entity.Property(e => e.MeterRegisterCreateConfirmationHeaderId).HasColumnName("MeterRegisterCreateConfirmationHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterRegisterCreateConfirmationHeaderAltId).HasColumnName("MeterRegisterCreateConfirmationHeaderAltID");
            entity.Property(e => e.MeterRegisterCreateConfirmationMainLogItemNote).HasMaxLength(50);
            entity.Property(e => e.MeterRegisterCreateConfirmationMainLogItemTypeId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MeterRegisterCreateConfirmationMhcreationDatetime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterRegisterCreateConfirmationMHCreationDatetime");
            entity.Property(e => e.MeterRegisterCreateConfirmationMhrecipientSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterCreateConfirmationMHRecipientSystemBusinessID");
            entity.Property(e => e.MeterRegisterCreateConfirmationMhrefUuid).HasColumnName("MeterRegisterCreateConfirmationMHRefUUID");
            entity.Property(e => e.MeterRegisterCreateConfirmationMhuuid).HasColumnName("MeterRegisterCreateConfirmationMHUUID");
            entity.Property(e => e.MeterRegisterCreateHeaderAltId).HasColumnName("MeterRegisterCreateHeaderAltID");
            entity.Property(e => e.MeterRegisterCreateHeaderId).HasColumnName("MeterRegisterCreateHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterRegisterCreateDetail>(entity =>
        {
            entity.ToTable("MeterRegisterCreateDetail");

            entity.Property(e => e.MeterRegisterCreateDetailId).HasColumnName("MeterRegisterCreateDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeterRegisterCreateDetailAltId).HasColumnName("MeterRegisterCreateDetailAltID");
            entity.Property(e => e.MeterRegisterCreateDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterRegisterCreateDetailMHCreationTime");
            entity.Property(e => e.MeterRegisterCreateDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterCreateDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterRegisterCreateDetailMhuuid).HasColumnName("MeterRegisterCreateDetailMHUUID");
            entity.Property(e => e.MeterRegisterCreateHeaderAltId).HasColumnName("MeterRegisterCreateHeaderAltID");
            entity.Property(e => e.MeterRegisterCreateHeaderId).HasColumnName("MeterRegisterCreateHeaderID");
            entity.Property(e => e.SpecMeasureUnitCode).HasMaxLength(20);
            entity.Property(e => e.SpecMeterReadingResultAdjustmentFactorValue).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TimeZoneCode).HasMaxLength(20);
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(30)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
        });

        modelBuilder.Entity<MeterRegisterCreateHeader>(entity =>
        {
            entity.ToTable("MeterRegisterCreateHeader");

            entity.Property(e => e.MeterRegisterCreateHeaderId).HasColumnName("MeterRegisterCreateHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MeterRegisterCreateCreationDatetime).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterRegisterCreateHeaderAltId).HasColumnName("MeterRegisterCreateHeaderAltID");
            entity.Property(e => e.MeterRegisterCreateMessageSenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterCreateMessageSenderSystemBusinessID");
            entity.Property(e => e.MeterRegisterCreateMessageUuid).HasColumnName("MeterRegisterCreateMessageUUID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterRegisterDetail>(entity =>
        {
            entity.ToTable("MeterRegisterDetail");

            entity.Property(e => e.MeterRegisterDetailId).HasColumnName("MeterRegisterDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.MeterRegisterDetailAltId).HasColumnName("MeterRegisterDetailAltID");
            entity.Property(e => e.MeterRegisterDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterRegisterDetailMHCreationTime");
            entity.Property(e => e.MeterRegisterDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterRegisterDetailMhuuid).HasColumnName("MeterRegisterDetailMHUUID");
            entity.Property(e => e.MeterRegisterHeaderAltId).HasColumnName("MeterRegisterHeaderAltID");
            entity.Property(e => e.MeterRegisterHeaderId).HasColumnName("MeterRegisterHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.RegisterType)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(30)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
        });

        modelBuilder.Entity<MeterRegisterHeader>(entity =>
        {
            entity.ToTable("MeterRegisterHeader");

            entity.Property(e => e.MeterRegisterHeaderId).HasColumnName("MeterRegisterHeaderID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MeterRegisterHeaderAltId).HasColumnName("MeterRegisterHeaderAltID");
            entity.Property(e => e.MeterRegisterHeaderCreationDatetime).HasColumnType("smalldatetime");
            entity.Property(e => e.MeterRegisterHeaderMessageSenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterRegisterHeaderMessageSenderSystemBusinessID");
            entity.Property(e => e.MeterRegisterHeaderMessageUuid).HasColumnName("MeterRegisterHeaderMessageUUID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.RegisterType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<MeterReplicationDetail>(entity =>
        {
            entity.ToTable("MeterReplicationDetail");

            entity.Property(e => e.MeterReplicationDetailId).HasColumnName("MeterReplicationDetailID");
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.CountryCode).HasMaxLength(5);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.InstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("InstallationPointID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.LocationListCompleteTransmissionIndicator).HasColumnName("locationListCompleteTransmissionIndicator");
            entity.Property(e => e.LogicalInstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("LogicalInstallationPointID");
            entity.Property(e => e.LogicalLocationListCompleteTransmissionIndicator).HasColumnName("logicalLocationListCompleteTransmissionIndicator");
            entity.Property(e => e.MeterReplicationDetailAltId).HasColumnName("MeterReplicationDetailAltID");
            entity.Property(e => e.MeterReplicationDetailMhcreationTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("MeterReplicationDetailMHCreationTime");
            entity.Property(e => e.MeterReplicationDetailMhsenderBusinessSystemId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MeterReplicationDetailMHSenderBusinessSystemId");
            entity.Property(e => e.MeterReplicationDetailMhuuid).HasColumnName("MeterReplicationDetailMHUUID");
            entity.Property(e => e.MeterReplicationHeaderAltId).HasColumnName("MeterReplicationHeaderAltID");
            entity.Property(e => e.MeterReplicationHeaderId).HasColumnName("MeterReplicationHeaderID");
            entity.Property(e => e.ParentInstallationPointId)
                .HasMaxLength(30)
                .HasColumnName("ParentInstallationPointID");
            entity.Property(e => e.RegionCode).HasMaxLength(5);
            entity.Property(e => e.RegisterListCompleteTransmissionIndicator).HasColumnName("registerListCompleteTransmissionIndicator");
            entity.Property(e => e.RelationshipListCompleteTransmissionIndicator).HasColumnName("relationshipListCompleteTransmissionIndicator");
            entity.Property(e => e.StreetName).HasMaxLength(150);
            entity.Property(e => e.StreetPostalCode).HasMaxLength(10);
            entity.Property(e => e.TimeZoneCode).HasMaxLength(30);
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.UtilitiesAdvancedMeteringSystemId)
                .HasMaxLength(30)
                .HasColumnName("UtilitiesAdvancedMeteringSystemID");
            entity.Property(e => e.UtilitiesDeviceId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceID");
            entity.Property(e => e.UtilitiesDeviceMaterialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceMaterialID");
            entity.Property(e => e.UtilitiesDeviceSerialId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesDeviceSerialID");
        });

        modelBuilder.Entity<MeterReplicationHeader>(entity =>
        {
            entity.HasKey(e => e.MeterReplicationHeaderAltId);

            entity.ToTable("MeterReplicationHeader");

            entity.Property(e => e.MeterReplicationHeaderAltId)
                .ValueGeneratedNever()
                .HasColumnName("MeterReplicationHeaderAltID");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsMdminvoked).HasColumnName("IsMDMInvoked");
            entity.Property(e => e.IsProcessCompleted).HasColumnName("isProcessCompleted");
            entity.Property(e => e.MeterReplicationHeaderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MeterReplicationHeaderID");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReplicationCreationDatetime).HasColumnType("datetime");
            entity.Property(e => e.ReplicationMessageUuid).HasColumnName("ReplicationMessageUUID");
            entity.Property(e => e.ReplicationSenderSystemBusinessId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("ReplicationSenderSystemBusinessID");
            entity.Property(e => e.UpdatedBy).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<RegisterSpecificDetail>(entity =>
        {
            entity.ToTable("RegisterSpecificDetail");

            entity.Property(e => e.RegisterSpecificDetailId).HasColumnName("RegisterSpecificDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReferenceDetailAltId).HasColumnName("ReferenceDetailAltID");
            entity.Property(e => e.ReferenceDetailId).HasColumnName("ReferenceDetailID");
            entity.Property(e => e.RegisterSourceType)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.RegisterSpecificDetailAltId).HasColumnName("RegisterSpecificDetailAltID");
            entity.Property(e => e.SpecMeasureUnitCode).HasMaxLength(20);
            entity.Property(e => e.SpecMeterReadingResultAdjustmentFactorValue).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TimeZoneCode).HasMaxLength(20);
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
        });

        modelBuilder.Entity<ReplicationRegisterDetail>(entity =>
        {
            entity.ToTable("ReplicationRegisterDetail");

            entity.Property(e => e.ReplicationRegisterDetailId).HasColumnName("ReplicationRegisterDetailID");
            entity.Property(e => e.IsConfirmationSent).HasColumnName("isConfirmationSent");
            entity.Property(e => e.IsMdminvoked).HasColumnName("isMDMInvoked");
            entity.Property(e => e.PayloadType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ReferenceDetailAltId).HasColumnName("ReferenceDetailAltID");
            entity.Property(e => e.ReferenceDetailId).HasColumnName("ReferenceDetailID");
            entity.Property(e => e.RegisterSourceType)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ReplicationRegisterDetailAltId).HasColumnName("ReplicationRegisterDetailAltID");
            entity.Property(e => e.SpecMeasureUnitCode).HasMaxLength(20);
            entity.Property(e => e.SpecMeterReadingResultAdjustmentFactorValue).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TimeZoneCode).HasMaxLength(20);
            entity.Property(e => e.UtilitiesMeasurementTaskId)
                .HasMaxLength(20)
                .HasColumnName("UtilitiesMeasurementTaskID");
            entity.Property(e => e.UtilitiesObjectIdentificationSystemCodeText).HasMaxLength(20);
        });

        modelBuilder.Entity<Schema>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("PK_HangFire_Schema");

            entity.ToTable("Schema", "HangFire");

            entity.Property(e => e.Version).ValueGeneratedNever();
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Server");

            entity.ToTable("Server", "HangFire");

            entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

            entity.Property(e => e.Id).HasMaxLength(200);
            entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
        });

        modelBuilder.Entity<ServiceCallLog>(entity =>
        {
            entity.HasKey(e => e.EntryId);

            entity.ToTable("ServiceCallLog");

            entity.Property(e => e.CallTimings).HasColumnType("datetime");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.ServiceParamJson).HasColumnName("ServiceParamJSON");
        });

        modelBuilder.Entity<ServiceDirectionType>(entity =>
        {
            entity.ToTable("ServiceDirectionType");

            entity.Property(e => e.ServiceDirectionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ServiceDirectionTypeID");
            entity.Property(e => e.ServiceDirectionTypeDescription).HasMaxLength(100);
            entity.Property(e => e.ServiceDirectionTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<ServiceMaster>(entity =>
        {
            entity.HasKey(e => e.ServiceId);

            entity.ToTable("ServiceMaster");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.DeployedBy).HasMaxLength(25);
            entity.Property(e => e.DeploymentDate).HasColumnType("smalldatetime");
            entity.Property(e => e.MessageDesc).HasMaxLength(100);
            entity.Property(e => e.ServiceBusinessName).HasMaxLength(100);
            entity.Property(e => e.ServiceDescription).HasMaxLength(500);
            entity.Property(e => e.ServiceEndPointName).HasMaxLength(75);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.WsdlservicePayLoad).HasColumnName("WSDLServicePayLoad");
            entity.Property(e => e.Xmlpayload).HasColumnName("XMLPayload");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Value }).HasName("PK_HangFire_Set");

            entity.ToTable("Set", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

            entity.ToTable("State", "HangFire");

            entity.HasIndex(e => e.CreatedAt, "IX_HangFire_State_CreatedAt");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Reason).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.States)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_State_Job");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
