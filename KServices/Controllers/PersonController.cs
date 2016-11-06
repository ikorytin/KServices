using System;
using System.Web.Http;
using System.Web.Http.Description;
using KServices.Core.Domain.Core.Exceptions;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Repositories;
using KServices.Core.Domain.Services;

namespace KServices.Controllers
{
    [RoutePrefix("api/person")]
    public class PersonController : BaseApiController
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
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

                Person item = _personRepository.Get(account);

                return Ok(item);
            }
            catch (Exception ex)
            {
                ServerLogger.LogException(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}