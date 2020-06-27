using AllTheBeans.Core.DTOs;
using AllTheBeans.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllTheBeans.Core.Interfaces
{
    public interface IBeanRepository
    {
        Task<List<Bean>> GetAllBeans();
        void AddBean(BeanCreateDTO bean);
        Task<BeanDTO> GetBeanById(int id);
    }
}
