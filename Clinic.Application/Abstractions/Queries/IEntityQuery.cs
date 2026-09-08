using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Abstractions.Queries
{
    public interface IEntityQuery<TEntity>
    {
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}
