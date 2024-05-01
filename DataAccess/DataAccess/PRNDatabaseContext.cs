using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess;

public partial class PRNDatabaseContext : DbContext
{
    public PRNDatabaseContext()
    {
    }

    public PRNDatabaseContext(DbContextOptions<PRNDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }


    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<ImportReceipt> ImportReceipts { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAddress> OrderAddresses { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ReceiptProduct> ReceiptProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=CAMCAM\\NAMPLT;Initial Catalog=PRNDatabase;User ID=sa;Password=123123;TrustServerCertificate=True; Max pool size =1000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__AAB3216F16C1CE1D");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.BrandLogo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Brand_Logo");
            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Brand_Name");
            entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.ProId, e.Username});
            entity
                .ToTable("Cart");

            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");
            entity.Property(e => e.ProName)
                .HasMaxLength(50)
                .HasColumnName("pro_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Pro).WithMany()
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCart367601");

            entity.HasOne(d => d.UsernameNavigation).WithMany()
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCart485431");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__Category__26E351206EF2FBF9");

            entity.ToTable("Category");

            entity.Property(e => e.CatId).HasColumnName("Cat_ID");
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Cat_Name");
            entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");
            entity.Property(e => e.Keyword)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("keyword");
        });


        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__Customer__F3DBC5735862BA29");

            entity.ToTable("Customer");

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_Num");
        });

        modelBuilder.Entity<DeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity
                .ToTable("Delivery_Address");

            entity.Property(e => e.AddressInformation)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Address_Information");
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_Num");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.Property(e => e.ID)
                .HasColumnName("ID");

            entity.HasOne(d => d.UsernameNavigation).WithMany()
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDelivery_A639869");
        });

        modelBuilder.Entity<ImportReceipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptId).HasName("PK__Import_R__38B787CCE38578BB");

            entity.ToTable("Import_Receipt");

            entity.Property(e => e.ReceiptId).HasColumnName("Receipt_ID");
            entity.Property(e => e.DateImport)
                .HasColumnType("datetime")
                .HasColumnName("Date_Import");
            entity.Property(e => e.Payment).HasColumnType("money");
            entity.Property(e => e.PersonInCharge)
                .HasMaxLength(100)
                .HasColumnName("Person_In_Charge");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manager__3214EC273A5EF113");

            entity.ToTable("Manager");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.LivingAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Living_Address");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_Num");
            entity.Property(e => e.Ssn)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("SSN");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__F1E4639B828A84AA");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Order_ID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("End_date");
            entity.Property(e => e.StaffId).HasColumnName("Staff_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("Start_date");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("money")
                .HasColumnName("Total_Price");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FKOrder657292");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Username)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrder846319");
        });

        modelBuilder.Entity<OrderAddress>(entity =>
        {
            entity.HasKey(e => new { e.OrderId });
            entity
                .ToTable("Order_Address");

            entity.Property(e => e.AddressInformation)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Address_Information");
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.OrderId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Order_ID");
            entity.Property(e => e.PhoneNum)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_Num");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrder_Addr788059");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProId });
            entity
                .ToTable("Order_Details");

            entity.Property(e => e.OrderId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Order_ID");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");
            entity.Property(e => e.ProName)
                .HasMaxLength(50)
                .HasColumnName("pro_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrder_Deta343235");

            entity.HasOne(d => d.Pro).WithMany()
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKOrder_Deta184552");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Product__335E4CA6F16431EB");

            entity.ToTable("Product");

            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");
            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.CatId).HasColumnName("Cat_ID");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");
            entity.Property(e => e.ProDes)
                .HasMaxLength(1000)
                .HasColumnName("pro_des");
            entity.Property(e => e.ProName)
                .HasMaxLength(50)
                .HasColumnName("pro_name");
            entity.Property(e => e.ProPrice)
                .HasColumnType("money")
                .HasColumnName("pro_price");
            entity.Property(e => e.ProQuan).HasColumnName("pro_quan");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct140662");

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct936418");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
           
            entity
                .ToTable("Product_Attribute");
            entity.HasKey(e => new { e.ProId, e.Feature });


            entity.Property(e => e.Des)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Feature)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");

            entity.HasOne(d => d.Pro).WithMany()
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct_At137892");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => new { e.ProId, e.ProductImage1 });
            entity
                .ToTable("Product_Image");
            

            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");
            entity.Property(e => e.ProductImage1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Product_Image");


            entity.HasOne(d => d.Pro).WithMany()
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKProduct_Im545081");
        });

        modelBuilder.Entity<ReceiptProduct>(entity =>
        {
            entity.HasKey(e => new { e.ProId, e.ReceiptId });
            entity
                .ToTable("Receipt_Product");

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("pro_id");
            entity.Property(e => e.ProName)
                .HasMaxLength(50)
                .HasColumnName("pro_name");
            entity.Property(e => e.ReceiptId).HasColumnName("Receipt_ID");

            entity.HasOne(d => d.Pro).WithMany()
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKReceipt_Pr358049");

            entity.HasOne(d => d.Receipt).WithMany()
                .HasForeignKey(d => d.ReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKReceipt_Pr300114");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
