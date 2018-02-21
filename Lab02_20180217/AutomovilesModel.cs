namespace Lab02_20180217
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AutomovilesModel : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'AutomovilesModel' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'Lab02_20180217.AutomovilesModel' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'AutomovilesModel'  en el archivo de configuración de la aplicación.
        public AutomovilesModel()
            : base("name=AutomovilesConnectionString")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Automovil>()
                .HasKey<int>(auto => auto.Id)
                .Property<int>(auto => auto.Id)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Automovil> Automoviles { get; set; }
    }

    public class Automovil
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Puertas { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}