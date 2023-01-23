using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JS.Contas.Domain.Models
{
    public enum StatusConta
    {
        Pendente = 1,
        Pago = 2,        
        Atrasado = 3,
        Cancelado = 4
    }
}
