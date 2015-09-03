using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderActionResponseDto
    {
        public OrderDetailDto OrderDetail { get; set; }
        public bool Success { get; set; }   // indicates if the action was successful
        public string ServerMessage { get; set; }   // message to display to the user

    }
}
