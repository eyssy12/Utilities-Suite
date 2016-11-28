namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Extensions;
    using Organiser.Library;

    public abstract class DefaultEntityBase<TEntity> : IDefaultEntity<TEntity>
        where TEntity : class
    {
        protected readonly IEnumerable<TEntity> Entities;

        private readonly TEntity defaultEntity;

        protected DefaultEntityBase(IEnumerable<TEntity> entities)
        {
            if (entities.IsEmpty())
            {
                throw new ArgumentNullException(nameof(entities), "no entities provided");
            }

            IEnumerable<TEntity> temp = entities.Where(v => v.GetType().GetCustomAttribute<DefaultEntityAttribute>() != null).ToArray();

            if (temp.Count() == 0)
            {
                throw new ArgumentException("No default entity provided.");
            }

            if (temp.Count() > 1)
            {
                throw new ArgumentException("More than one default entity provided.");
            }

            this.Entities = entities;
            this.defaultEntity = temp.Single();
        }

        public TEntity DefaultEntity
        {
            get { return this.defaultEntity; }
        }
    }
}