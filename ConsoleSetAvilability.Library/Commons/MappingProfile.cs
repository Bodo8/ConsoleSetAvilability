using ConsoleSetAvilability.Library.Infrastracture;
using ConsoleSetAvilability.Library.Models.JsonModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSetAvilability.Library.Commons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ResponseReserveProduct, ResponseReserveDto>();
        }
    }
}
