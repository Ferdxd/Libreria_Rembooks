using Libreria_Rembooks.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace Libreria_Rembooks
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using var db = new appDbContext();

                // Crea la BD si no existe y aplica
                // las migraciones pendientes
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"No se pudo preparar la base de datos.\n\n" +
                    $"{ex.Message}",
                    "Error de base de datos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Shutdown();
            }
        }
    }
}