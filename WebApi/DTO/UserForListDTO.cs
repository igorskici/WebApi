using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTO
{
    public class UserForListDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public string Gender { get; set; }

    }
}
