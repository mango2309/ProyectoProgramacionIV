using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoProgramacionIV.Models
{
    public class Reservacion
    {
        public int IdReservacion { get; set; }
        public int IdCarro { get; set; } 
        public int IdCliente { get; set; } 
        public DateTime FechaReserva { get; set; }

        public string NombreCliente { get; set; }
        public string NombreCarro { get; set; }
    }
}
