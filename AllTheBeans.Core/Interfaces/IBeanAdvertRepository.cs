using AllTheBeans.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Interfaces
{
    public interface IBeanAdvertRepository
    {
        Task<BeanDTO> GetAdvert(DateTime date);
        Task AddAdvert(BeanCreateAdvertDTO advert);
        Task ClearAdverts();
    }
}
