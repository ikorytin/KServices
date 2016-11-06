using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using KServices.Core.Domain.Core.Exceptions;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Repositories;
using KServices.Core.Domain.Services;

namespace KServices.Controllers
{
    [RoutePrefix("api/entity")]
    public class EntityController : BaseApiController
    {
        private readonly IEntityRepository _entityRepository;

        public EntityController(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        [ResponseType(typeof(Person))]
        public object Get(string account)
        {
            try
            {
                if (string.IsNullOrEmpty(account))
                {
                    throw new BadParameterException("Username is required.");
                }

                List<Entity> items = _entityRepository.Get(account);

                return Ok(items);
            }
            catch (Exception ex)
            {
                ServerLogger.LogException(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}