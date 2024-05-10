using Knights.Challenge.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Knights.Challenge.Domain.DTOs.Response
{
    public class KnightResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public DateTime Birthday { get; set; }
        public List<Weapon> Weapons { get; set; }
        public Attributes Attributes { get; set; }
        public string KeyAttribute { get; set; }
    }
}
