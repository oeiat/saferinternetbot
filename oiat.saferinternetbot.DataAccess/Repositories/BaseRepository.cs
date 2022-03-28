using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using mbit.common.dal;
using mbit.common.dal.Entities;
using mbit.common.dal.Repositories;

namespace oiat.saferinternetbot.DataAccess.Repositories
{
    public class BaseRepository<T> : EfRepositoryBase<T> where T : BaseEntity
    {
        protected string CurrentUserName => Thread.CurrentPrincipal?.Identity?.Name ?? string.Empty;
        public BaseRepository(IContextProvider<DbContext> provider) : base(provider)
        {
        }

        public override void Add(T entity)
        {
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = CurrentUserName;
            base.Add(entity);
        }

        public override void Update(T entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdateUser = CurrentUserName;
            base.Update(entity);
        }
    }
}
