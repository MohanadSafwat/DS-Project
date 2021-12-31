using AutoMapper;
using JWTAuthentication.Authentication;

namespace MarketPlace.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<User2,User>();
         
        }
    }
}